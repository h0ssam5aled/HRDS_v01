using HRDS.Web.Models;
using HRDS.Web.Models.Entities;
using HRDS.Web.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRDS.Web.Areas.Security.Controllers
{
    [Area("Security")]
    [Authorize]
    [HasModuleAccess("SECURITY")]
    public class RolesController : Controller
    {
        private readonly HRDSContext _context;
        private readonly ILogger<RolesController> _logger;

        public RolesController(HRDSContext context, ILogger<RolesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetRolesJson()
        {
            var rolesList = await _context.Roles
                .AsNoTracking()
                .Select(r => new RoleViewModel
                {
                    RoleId = r.RoleId,
                    RoleCode = r.RoleCode,
                    RoleNameAr = r.RoleNameAr,
                    RoleNameEn = r.RoleNameEn,
                    Description = r.Description,
                    IsActive = r.IsActive
                })
                .OrderBy(r => r.RoleId)
                .ToListAsync();

            return Json(new { data = rolesList });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null) return NotFound();

            var vm = new RoleViewModel
            {
                RoleId = role.RoleId,
                RoleCode = role.RoleCode,
                RoleNameAr = role.RoleNameAr,
                RoleNameEn = role.RoleNameEn,
                Description = role.Description,
                IsActive = role.IsActive
            };

            return Json(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save([FromBody] RoleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return Json(new { success = false, message = string.Join("<br/>", errors) });
            }

            try
            {
                var codeExists = await _context.Roles
                    .AnyAsync(r => r.RoleCode.ToLower() == model.RoleCode.ToLower() && r.RoleId != model.RoleId);

                if (codeExists)
                {
                    return Json(new { success = false, message = "كود الدور مستخدم بالفعل!" });
                }

                if (model.RoleId == 0)
                {
                    var newRole = new Role
                    {
                        RoleCode = model.RoleCode.ToUpper().Trim(),
                        RoleNameAr = model.RoleNameAr.Trim(),
                        RoleNameEn = model.RoleNameEn.Trim(),
                        Description = model.Description?.Trim(),
                        IsActive = model.IsActive
                    };
                    _context.Roles.Add(newRole);
                }
                else
                {
                    var existingRole = await _context.Roles.FindAsync(model.RoleId);
                    if (existingRole == null) return Json(new { success = false, message = "الدور غير موجود!" });

                    existingRole.RoleCode = model.RoleCode.ToUpper().Trim();
                    existingRole.RoleNameAr = model.RoleNameAr.Trim();
                    existingRole.RoleNameEn = model.RoleNameEn.Trim();
                    existingRole.Description = model.Description?.Trim();
                    existingRole.IsActive = model.IsActive;
                }

                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "تم حفظ البيانات بنجاح" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "حدث خطأ أثناء حفظ الدور");
                return Json(new { success = false, message = "حدث خطأ غير متوقع أثناء الحفظ" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null) return Json(new { success = false, message = "الدور غير موجود" });

            role.IsActive = !role.IsActive;
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "تم تغيير حالة التفعيل بنجاح" });
        }
    }
}