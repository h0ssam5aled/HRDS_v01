using HRDS.Web.Models;
using HRDS.Web.Models.Entities;
using HRDS.Web.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace HRDS.Web.Controllers
{
    [Area("Security")]
    [Authorize]
    [HasModuleAccess("SECURITY")]
    public class ModelsController : Controller
    {
        private readonly HRDSContext _context;
        private readonly ILogger<ModelsController> _logger;

        public ModelsController(HRDSContext context, ILogger<ModelsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // 1. عرض الصفحة الرئيسية
        public async Task<IActionResult> Index()
        {
            var isArabic = CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToLower() == "ar";
            var moduleTextField = isArabic ? "ModuleNameAr" : "ModuleNameEn";

            ViewBag.Modules = new SelectList(
                await _context.Modules.Where(m => m.IsActive).ToListAsync(),
                "ModuleId",
                moduleTextField
            );

            return View();
        }

        // 2. جلب كافة الشاشات لجدول DataTables عبر AJAX
        [HttpGet]
        public async Task<IActionResult> GetModelsJson()
        {
            var modelsList = await _context.Models
                .Include(m => m.Module)
                .Select(m => new ModelViewModel
                {
                    ModelId = m.ModelId,
                    ModuleId = m.ModuleId,
                    ModuleNameAr = m.Module.ModuleNameAr,
                    ModuleNameEn = m.Module.ModuleNameEn,
                    ModelCode = m.ModelCode,
                    ModelNameAr = m.ModelNameAr,
                    ModelNameEn = m.ModelNameEn,
                    Description = m.Description,
                    IsActive = m.IsActive
                })
                .OrderBy(m => m.ModuleId)
                .ThenBy(m => m.ModelId)
                .ToListAsync();

            return Json(new { data = modelsList });
        }

        // 3. جلب بيانات شاشة معينة للتعديل
        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var modelEntity = await _context.Models.FindAsync(id);
            if (modelEntity == null) return NotFound();

            var vm = new ModelViewModel
            {
                ModelId = modelEntity.ModelId,
                ModuleId = modelEntity.ModuleId,
                ModelCode = modelEntity.ModelCode,
                ModelNameAr = modelEntity.ModelNameAr,
                ModelNameEn = modelEntity.ModelNameEn,
                Description = modelEntity.Description,
                IsActive = modelEntity.IsActive
            };

            return Json(vm);
        }

        // 4. حفظ (إضافة أو تعديل)
        // 4. حفظ (إضافة أو تعديل)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save([FromBody] ModelViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return Json(new { success = false, message = string.Join("<br/>", errors) });
            }

            try
            {
                var codeExists = await _context.Models
                    .AnyAsync(m => m.ModelCode.ToLower() == model.ModelCode.ToLower() && m.ModelId != model.ModelId);

                if (codeExists)
                {
                    return Json(new { success = false, message = "رمز الشاشة (Model Code) مستخدم بالفعل!" });
                }

                if (model.ModelId == 0)
                {
                    var newModel = new Model
                    {
                        ModuleId = model.ModuleId,
                        ModelCode = model.ModelCode.ToUpper().Trim(),
                        ModelNameAr = model.ModelNameAr.Trim(),
                        ModelNameEn = model.ModelNameEn.Trim(),
                        Description = model.Description?.Trim(),
                        IsActive = model.IsActive
                    };
                    _context.Models.Add(newModel);
                }
                else
                {
                    var existingModel = await _context.Models.FindAsync(model.ModelId);
                    if (existingModel == null)
                        return Json(new { success = false, message = "الشاشة غير موجودة!" });

                    existingModel.ModuleId = model.ModuleId;
                    existingModel.ModelCode = model.ModelCode.ToUpper().Trim();
                    existingModel.ModelNameAr = model.ModelNameAr.Trim();
                    existingModel.ModelNameEn = model.ModelNameEn.Trim();
                    existingModel.Description = model.Description?.Trim();
                    existingModel.IsActive = model.IsActive;
                }

                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "تم حفظ بيانات الشاشة بنجاح" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "حدث خطأ أثناء حفظ الشاشة");
                return Json(new { success = false, message = "حدث خطأ غير متوقع أثناء الحفظ" });
            }
        }

        // 5. تغيير حالة التفعيل
        [HttpPost]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var modelEntity = await _context.Models.FindAsync(id);
            if (modelEntity == null) return Json(new { success = false, message = "الشاشة غير موجودة" });

            modelEntity.IsActive = !modelEntity.IsActive;
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "تم تغيير حالة التفعيل بنجاح" });
        }
    }
}