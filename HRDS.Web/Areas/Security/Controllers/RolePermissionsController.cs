using HRDS.Web.Models;
using HRDS.Web.Models.Entities;
using HRDS.Web.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRDS.Web.Areas.Security.Controllers
{
    [Area("Security")]
    [HasModuleAccess("SECURITY")]
    public class RolePermissionsController : Controller
    {
        private readonly HRDSContext _context;

        public RolePermissionsController(HRDSContext context)
        {
            _context = context;
        }

        // عرض واجهة المصفوفة لدور محدد
        public async Task<IActionResult> Matrix(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null) return NotFound();

            // جلب الصلاحيات الممنوحة حالياً لهذا الدور
            var grantedPermissionIds = await _context.RolePermissions
                .Where(rp => rp.RoleId == id && rp.IsActive)
                .Select(rp => rp.PermissionId)
                .ToListAsync();

            // جلب الموديولات والموديولات الفرعية (Models) مع الأفعال (Actions) المربوطة بالـ Permissions
            var modulesData = await _context.Modules
                .Include(m => m.Models)
                    .ThenInclude(md => md.Permissions)
                        .ThenInclude(p => p.Action)
                .Where(m => m.IsActive)
                .AsNoTracking()
                .ToListAsync();

            var viewModel = new RolePermissionMatrixViewModel
            {
                RoleId = role.RoleId,
                RoleNameAr = role.RoleNameAr,
                RoleNameEn = role.RoleNameEn,
                Modules = modulesData.Select(m => new ModulePermissionsGroupViewModel
                {
                    ModuleId = m.ModuleId,
                    ModuleNameAr = m.ModuleNameAr,
                    Models = m.Models.Where(md => md.IsActive).Select(md => new ModelPermissionsGroupViewModel
                    {
                        ModelId = md.ModelId,
                        ModelNameAr = md.ModelNameAr,
                        Actions = md.Permissions.Where(p => p.IsActive).Select(p => new ActionPermissionItemViewModel
                        {
                            PermissionId = p.PermissionId,
                            ActionId = p.ActionId,
                            ActionNameAr = p.Action.ActionNameAr,
                            IsGranted = grantedPermissionIds.Contains(p.PermissionId)
                        }).ToList()
                    }).ToList()
                }).ToList()
            };

            return View(viewModel);
        }

        // حفظ الصلاحيات المحددة عبر AJAX
        [HttpPost]
        public async Task<IActionResult> SavePermissions([FromBody] SaveRolePermissionsRequest request)
        {
            if (request == null || request.RoleId <= 0)
                return Json(new { success = false, message = "بيانات غير صالحة" });

            // حذف الصلاحيات القديمة للدور
            var existingPermissions = await _context.RolePermissions
                .Where(rp => rp.RoleId == request.RoleId)
                .ToListAsync();

            _context.RolePermissions.RemoveRange(existingPermissions);

            // إضافة الصلاحيات المحددة الجديدة
            if (request.SelectedPermissionIds != null && request.SelectedPermissionIds.Any())
            {
                var newPermissions = request.SelectedPermissionIds.Select(pId => new RolePermission
                {
                    RoleId = request.RoleId,
                    PermissionId = pId,
                    IsActive = true
                });

                await _context.RolePermissions.AddRangeAsync(newPermissions);
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "تم حفظ الصلاحيات بنجاح" });
        }
    }
}