using HRDS.Web.Areas.HR.ViewModels;
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
    [Authorize]
    [Area("HRConfig")]
    [HasModuleAccess("HRConfig")]
    public class EducationalInstitutionsController : Controller
    {
        private readonly HRDSContext _context;

        public EducationalInstitutionsController(HRDSContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var isArabic = CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToLower() == "ar";
            ViewBag.InstitutionTypes = new SelectList(
                await _context.EducationalInstitutionTypes.ToListAsync(),
                "InstitutionTypeId",
                isArabic ? "InstitutionTypeNameAr" : "InstitutionTypeNameEn"
            );
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetJson()
        {
            var list = await _context.EducationalInstitutions
                .Include(x => x.InstitutionType)
                .Select(x => new EducationalInstitutionViewModel
                {
                    InstitutionId = x.InstitutionId,
                    InstitutionTypeId = x.InstitutionTypeId,
                    InstitutionTypeNameAr = x.InstitutionType.InstitutionTypeNameAr,
                    InstitutionTypeNameEn = x.InstitutionType.InstitutionTypeNameEn,
                    InstitutionCode = x.InstitutionCode,
                    InstitutionNameAr = x.InstitutionNameAr,
                    InstitutionNameEn = x.InstitutionNameEn
                })
                .ToListAsync();

            return Json(new { data = list });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save([FromBody] EducationalInstitutionViewModel model)
        {
            if (!ModelState.IsValid) return Json(new { success = false, message = "بيانات غير صالحة" });

            if (model.InstitutionId == 0)
            {
                var entity = new EducationalInstitution
                {
                    InstitutionTypeId = model.InstitutionTypeId,
                    InstitutionCode = model.InstitutionCode.ToUpper().Trim(),
                    InstitutionNameAr = model.InstitutionNameAr.Trim(),
                    InstitutionNameEn = model.InstitutionNameEn?.Trim()
                };
                _context.EducationalInstitutions.Add(entity);
            }
            else
            {
                var entity = await _context.EducationalInstitutions.FindAsync(model.InstitutionId);
                if (entity == null) return Json(new { success = false, message = "السجل غير موجود" });

                entity.InstitutionTypeId = model.InstitutionTypeId;
                entity.InstitutionCode = model.InstitutionCode.ToUpper().Trim();
                entity.InstitutionNameAr = model.InstitutionNameAr.Trim();
                entity.InstitutionNameEn = model.InstitutionNameEn?.Trim();
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "تم الحفظ بنجاح" });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var entity = await _context.EducationalInstitutions.FindAsync(id);
            if (entity == null)
                return Json(null);

            var model = new EducationalInstitutionViewModel
            {
                InstitutionId = entity.InstitutionId,
                InstitutionTypeId = entity.InstitutionTypeId,
                InstitutionCode = entity.InstitutionCode,
                InstitutionNameAr = entity.InstitutionNameAr,
                InstitutionNameEn = entity.InstitutionNameEn
            };

            return Json(model);
        }
    }
}