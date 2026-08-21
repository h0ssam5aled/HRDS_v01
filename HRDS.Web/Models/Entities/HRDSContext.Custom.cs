using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HRDS.Web.Models.Entities
{
    // استخدام partial لتوسيع الكلاس الأساسي بدون تعديل الملف المولد تلقائياً
    public partial class HRDSContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        // Constructor إضافي لحقن IHttpContextAccessor
        public HRDSContext(DbContextOptions<HRDSContext> options, IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplyAuditInfo();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            ApplyAuditInfo();
            return base.SaveChanges();
        }

        private void ApplyAuditInfo()
        {
            var userIdClaim = _httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int? currentUserId = int.TryParse(userIdClaim, out var id) ? id : null;

            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Added)
                {
                    if (entry.Properties.Any(p => p.Metadata.Name == "CreatedBy"))
                        entry.Property("CreatedBy").CurrentValue = currentUserId;

                    if (entry.Properties.Any(p => p.Metadata.Name == "CreatedAt"))
                        entry.Property("CreatedAt").CurrentValue = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    if (entry.Properties.Any(p => p.Metadata.Name == "ModifiedBy"))
                        entry.Property("ModifiedBy").CurrentValue = currentUserId;

                    if (entry.Properties.Any(p => p.Metadata.Name == "ModifiedAt"))
                        entry.Property("ModifiedAt").CurrentValue = DateTime.UtcNow;
                }
            }
        }
    }
}