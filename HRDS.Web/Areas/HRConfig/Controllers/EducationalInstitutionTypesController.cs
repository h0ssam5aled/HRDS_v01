using HRDS.Web.Areas.HR.ViewModels;
using HRDS.Web.Models;
using HRDS.Web.Models.Entities;
using HRDS.Web.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRDS.Web.Controllers
{
    [Authorize]
    [Area("HRConfig")]
    [HasModuleAccess("HRConfig")]
    public class EducationalInstitutionTypesController : Controller
    {
        private readonly HRDSContext _context;
        private readonly ILogger<EducationalInstitutionTypesController> _logger;

        public EducationalInstitutionTypesController(HRDSContext context, ILogger<EducationalInstitutionTypesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index() => View();

        [HttpGet]
        public async Task<IActionResult> GetJson()
        {
            var list = await _context.EducationalInstitutionTypes
                .Select(x => new EducationalInstitutionTypeViewModel
                {
                    InstitutionTypeId = x.InstitutionTypeId,
                    InstitutionTypeCode = x.InstitutionTypeCode,
                    InstitutionTypeNameAr = x.InstitutionTypeNameAr,
                    InstitutionTypeNameEn = x.InstitutionTypeNameEn
                })
                .OrderBy(x => x.InstitutionTypeId)
                .ToListAsync();

            return Json(new { data = list });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save([FromBody] EducationalInstitutionTypeViewModel model)
        {
            if (!ModelState.IsValid) return Json(new { success = false, message = "بيانات غير صالحة" });

            if (model.InstitutionTypeId == 0)
            {
                var entity = new EducationalInstitutionType
                {
                    InstitutionTypeCode = model.InstitutionTypeCode.ToUpper().Trim(),
                    InstitutionTypeNameAr = model.InstitutionTypeNameAr.Trim(),
                    InstitutionTypeNameEn = model.InstitutionTypeNameEn?.Trim()
                };
                _context.EducationalInstitutionTypes.Add(entity);
            }
            else
            {
                var entity = await _context.EducationalInstitutionTypes.FindAsync(model.InstitutionTypeId);
                if (entity == null) return Json(new { success = false, message = "السجل غير موجود" });

                entity.InstitutionTypeCode = model.InstitutionTypeCode.ToUpper().Trim();
                entity.InstitutionTypeNameAr = model.InstitutionTypeNameAr.Trim();
                entity.InstitutionTypeNameEn = model.InstitutionTypeNameEn?.Trim();
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "تم الحفظ بنجاح" });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var entity = await _context.EducationalInstitutionTypes.FindAsync(id);
            if (entity == null)
                return Json(null);

            var model = new EducationalInstitutionTypeViewModel
            {
                InstitutionTypeId = entity.InstitutionTypeId,
                InstitutionTypeCode = entity.InstitutionTypeCode,
                InstitutionTypeNameAr = entity.InstitutionTypeNameAr,
                InstitutionTypeNameEn = entity.InstitutionTypeNameEn
            };

            return Json(model);
        }
    }
}