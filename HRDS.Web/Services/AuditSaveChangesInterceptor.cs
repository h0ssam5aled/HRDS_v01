using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Security.Claims;

namespace HRDS.Web.Services
{
    public class AuditSaveChangesInterceptor : SaveChangesInterceptor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuditSaveChangesInterceptor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            if (eventData.Context != null)
            {
                ApplyAuditInfo(eventData.Context);
            }
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        public override InterceptionResult<int> SavingChanges(
            DbContextEventData eventData,
            InterceptionResult<int> result)
        {
            if (eventData.Context != null)
            {
                ApplyAuditInfo(eventData.Context);
            }
            return base.SavingChanges(eventData, result);
        }

        private void ApplyAuditInfo(DbContext context)
        {
            var userIdClaim = _httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int? currentUserId = int.TryParse(userIdClaim, out var id) ? id : null;

            foreach (var entry in context.ChangeTracker.Entries())
            {
                // في حالة الإضافة Insert
                if (entry.State == EntityState.Added)
                {
                    if (entry.Properties.Any(p => p.Metadata.Name == "CreatedBy"))
                        entry.Property("CreatedBy").CurrentValue = currentUserId;

                    if (entry.Properties.Any(p => p.Metadata.Name == "CreatedAt"))
                        entry.Property("CreatedAt").CurrentValue = DateTime.UtcNow;
                }
                // في حالة التعديل Update
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