using HRDS.Web.Models;
using HRDS.Web.Models.Entities;
using HRDS.Web.Security; // إضافة النيم سبيس الخاص بالصلاحيات
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRDS.Web.Controllers
{
    [Area("Security")]
    [Authorize]
    [HasModuleAccess("SECURITY")] // حماية الشاشة بالكامل بهذا السطر فقط
    public class ModulesController : Controller
    {
        private readonly HRDSContext _context;
        private readonly ILogger<ModulesController> _logger;

        public ModulesController(HRDSContext context, ILogger<ModulesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // 1. عرض الصفحة الرئيسية
        public IActionResult Index()
        {
            return View();
        }

        // 2. جلب البيانات لجدول DataTables عبر AJAX
        [HttpGet]
        public async Task<IActionResult> GetModulesJson()
        {
            var modules = await _context.Modules
                .Select(m => new ModuleViewModel
                {
                    ModuleId = m.ModuleId,
                    ModuleCode = m.ModuleCode,
                    ModuleNameAr = m.ModuleNameAr,
                    ModuleNameEn = m.ModuleNameEn,
                    Description = m.Description,
                    IsActive = m.IsActive
                })
                .OrderBy(m => m.ModuleId)
                .ToListAsync();

            return Json(new { data = modules });
        }

        // 3. جلب بيانات موديول معين للعرض أو التعديل
        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var module = await _context.Modules.FindAsync(id);
            if (module == null) return NotFound();

            var vm = new ModuleViewModel
            {
                ModuleId = module.ModuleId,
                ModuleCode = module.ModuleCode,
                ModuleNameAr = module.ModuleNameAr,
                ModuleNameEn = module.ModuleNameEn,
                Description = module.Description,
                IsActive = module.IsActive
            };

            return Json(vm);
        }

        // 4. حفظ الموديول (إضافة أو تعديل)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save([FromBody] ModuleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return Json(new { success = false, message = string.Join("<br/>", errors) });
            }

            try
            {
                var codeExists = await _context.Modules
                    .AnyAsync(m => m.ModuleCode.ToLower() == model.ModuleCode.ToLower() && m.ModuleId != model.ModuleId);

                if (codeExists)
                {
                    return Json(new { success = false, message = "رمز الموديول (Module Code) مستخدم بالفعل!" });
                }

                if (model.ModuleId == 0)
                {
                    var newModule = new Module
                    {
                        ModuleCode = model.ModuleCode.ToUpper().Trim(),
                        ModuleNameAr = model.ModuleNameAr.Trim(),
                        ModuleNameEn = model.ModuleNameEn.Trim(),
                        Description = model.Description?.Trim(),
                        IsActive = model.IsActive
                    };
                    _context.Modules.Add(newModule);
                }
                else
                {
                    var existingModule = await _context.Modules.FindAsync(model.ModuleId);
                    if (existingModule == null)
                        return Json(new { success = false, message = "الموديول غير موجود!" });

                    existingModule.ModuleCode = model.ModuleCode.ToUpper().Trim();
                    existingModule.ModuleNameAr = model.ModuleNameAr.Trim();
                    existingModule.ModuleNameEn = model.ModuleNameEn.Trim();
                    existingModule.Description = model.Description?.Trim();
                    existingModule.IsActive = model.IsActive;
                }

                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "تم حفظ البيانات بنجاح" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "حدث خطأ أثناء حفظ الموديول");
                return Json(new { success = false, message = "حدث خطأ غير متوقع أثناء الحفظ" });
            }
        }

        // 5. تغيير حالة التفعيل (Toggle Active)
        [HttpPost]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var module = await _context.Modules.FindAsync(id);
            if (module == null) return Json(new { success = false, message = "الموديول غير موجود" });

            module.IsActive = !module.IsActive;
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "تم تغيير حالة التفعيل بنجاح" });
        }
    }
}