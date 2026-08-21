using HRDS.Web.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;

namespace HRDS.Web.Security
{
    public class ModuleAccessHandler : AuthorizationHandler<ModuleAccessRequirement>
    {
        private readonly HRDSContext _context;
        private readonly IConfiguration _configuration;

        public ModuleAccessHandler(HRDSContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            ModuleAccessRequirement requirement)
        {
            var user = context.User;

            // 1. إذا كان المستخدم غير مسجل دخول
            if (user?.Identity?.IsAuthenticated != true)
            {
                return;
            }

            // 2. قراءة اسم مستخدم الـ SuperAdmin المعرف في appsettings.json
            var superAdminUsername = _configuration["SuperAdmin:Username"];
            var currentUsername = user.Identity.Name ?? user.FindFirst(ClaimTypes.Name)?.Value;

            // 3. التجاوز المباشر إذا كان هو الـ SuperAdmin المذكور في appsettings أو يملك دور Admin
            if ((!string.IsNullOrEmpty(superAdminUsername) && string.Equals(currentUsername, superAdminUsername, StringComparison.OrdinalIgnoreCase)) ||
                user.IsInRole("Admin") ||
                user.HasClaim("Permission", "System.Admin"))
            {
                context.Succeed(requirement);
                return;
            }

            // 4. استخراج ID المستخدم لفحص قاعدة البيانات
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out var userId))
            {
                return;
            }

            // 5. التحقق من صلاحيات باقي المستخدمين العاديين من جدول قاعدة البيانات
            bool hasAccess = await _context.UserAccesses
                .Where(ua => ua.UserId == userId && ua.IsActive).SelectMany(ua => ua.UserRoles)
                .Where(ur => ur.IsActive).Select(ur => ur.Role)
                .Where(r => r.IsActive).SelectMany(r => r.RolePermissions)
                .Where(rp => rp.IsActive).Select(rp => rp.Permission)
                .Where(p => p.IsActive).Select(p => p.Model)
                .Where(m => m.IsActive).Select(m => m.Module)
                .AnyAsync(m => m.IsActive && m.ModuleCode.ToUpper() == requirement.ModuleCode.ToUpper());

            if (hasAccess)
            {
                context.Succeed(requirement);
            }
        }
    }
}