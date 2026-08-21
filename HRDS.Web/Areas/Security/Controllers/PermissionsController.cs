using HRDS.Web.Models;
using HRDS.Web.Models.Entities;
using HRDS.Web.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace HRDS.Web.Areas.Security.Controllers
{
    [Area("Security")]
    [Authorize]
    [HasModuleAccess("SECURITY")]
    public class PermissionsController : Controller
    {
        private readonly HRDSContext _context;
        private readonly ILogger<PermissionsController> _logger;

        public PermissionsController(HRDSContext context, ILogger<PermissionsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            // الحصول على رمز اللغة الحالية (مثلاً "ar" أو "en")
            var isArabic = CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToLower() == "ar";

            // تحديد اسم الخاصية بناءً على اللغة
            var modelTextField = isArabic ? "ModelNameAr" : "ModelNameEn";
            var actionTextField = isArabic ? "ActionNameAr" : "ActionNameEn";

            ViewBag.Models = new SelectList(
                await _context.Models.Where(m => m.IsActive).ToListAsync(),
                "ModelId",
                modelTextField
            );

            ViewBag.Actions = new SelectList(
                await _context.Actions.Where(a => a.IsActive).ToListAsync(),
                "ActionId",
                actionTextField
            );

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetPermissionsJson()
        {
            var permissionsList = await _context.Permissions
                .Include(p => p.Model)
                .Include(p => p.Action)
                .AsNoTracking()
                .Select(p => new PermissionViewModel
                {
                    PermissionId = p.PermissionId,
                    PermissionCode = p.PermissionCode,
                    PermissionNameAr = p.PermissionNameAr,
                    PermissionNameEn = p.PermissionNameEn,
                    ModelId = p.ModelId,
                    ModelNameAr = p.Model.ModelNameAr,
                    ModelNameEn = p.Model.ModelNameEn,
                    ActionId = p.ActionId,
                    ActionNameAr = p.Action.ActionNameAr,
                    ActionNameEn = p.Action.ActionNameEn,
                    Description = p.Description,
                    IsActive = p.IsActive
                })
                .OrderBy(p => p.PermissionId)
                .ToListAsync();

            return Json(new { data = permissionsList });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var entity = await _context.Permissions.FindAsync(id);
            if (entity == null) return NotFound();

            var vm = new PermissionViewModel
            {
                PermissionId = entity.PermissionId,
                ModelId = entity.ModelId,
                ActionId = entity.ActionId,
                PermissionCode = entity.PermissionCode,
                PermissionNameAr = entity.PermissionNameAr,
                PermissionNameEn = entity.PermissionNameEn,
                Description = entity.Description,
                IsActive = entity.IsActive
            };

            return Json(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save([FromBody] PermissionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return Json(new { success = false, message = string.Join("<br/>", errors) });
            }

            try
            {
                var codeExists = await _context.Permissions
                    .AnyAsync(p => p.PermissionCode.ToLower() == model.PermissionCode.ToLower() && p.PermissionId != model.PermissionId);

                if (codeExists)
                {
                    return Json(new { success = false, message = "كود الصلاحية مستخدم بالفعل!" });
                }

                if (model.PermissionId == 0)
                {
                    var newPermission = new Permission
                    {
                        ModelId = model.ModelId,
                        ActionId = model.ActionId,
                        PermissionCode = model.PermissionCode.ToUpper().Trim(),
                        PermissionNameAr = model.PermissionNameAr.Trim(),
                        PermissionNameEn = model.PermissionNameEn.Trim(),
                        Description = model.Description?.Trim(),
                        IsActive = model.IsActive
                    };
                    _context.Permissions.Add(newPermission);
                }
                else
                {
                    var existing = await _context.Permissions.FindAsync(model.PermissionId);
                    if (existing == null) return Json(new { success = false, message = "الصلاحية غير موجودة!" });

                    existing.ModelId = model.ModelId;
                    existing.ActionId = model.ActionId;
                    existing.PermissionCode = model.PermissionCode.ToUpper().Trim();
                    existing.PermissionNameAr = model.PermissionNameAr.Trim();
                    existing.PermissionNameEn = model.PermissionNameEn.Trim();
                    existing.Description = model.Description?.Trim();
                    existing.IsActive = model.IsActive;
                }

                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "تم حفظ بيانات الصلاحية بنجاح" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "حدث خطأ أثناء حفظ الصلاحية");
                return Json(new { success = false, message = "حدث خطأ غير متوقع أثناء الحفظ" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var entity = await _context.Permissions.FindAsync(id);
            if (entity == null) return Json(new { success = false, message = "الصلاحية غير موجودة" });

            entity.IsActive = !entity.IsActive;
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "تم تغيير حالة التفعيل بنجاح" });
        }
    }
}