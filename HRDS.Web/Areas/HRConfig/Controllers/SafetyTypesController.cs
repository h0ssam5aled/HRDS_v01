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
    public class SafetyTypesController : Controller
    {
        private readonly HRDSContext _context;

        public SafetyTypesController(HRDSContext context)
        {
            _context = context;
        }

        public IActionResult Index() => View();

        [HttpGet]
        public async Task<IActionResult> GetJson()
        {
            var list = await _context.SafetyTypes
                .Select(x => new SafetyTypeViewModel
                {
                    SafetyTypeId = x.SafetyTypeId,
                    SafetyTypeCode = x.SafetyTypeCode,
                    SafetyTypeNameAr = x.SafetyTypeNameAr,
                    SafetyTypeNameEn = x.SafetyTypeNameEn,
                    SeverityLevel = x.SeverityLevel,
                    IsActive = x.IsActive
                })
                .ToListAsync();

            return Json(new { data = list });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var entity = await _context.SafetyTypes.FindAsync(id);
            if (entity == null) return Json(null);

            var model = new SafetyTypeViewModel
            {
                SafetyTypeId = entity.SafetyTypeId,
                SafetyTypeCode = entity.SafetyTypeCode,
                SafetyTypeNameAr = entity.SafetyTypeNameAr,
                SafetyTypeNameEn = entity.SafetyTypeNameEn,
                SeverityLevel = entity.SeverityLevel,
                IsActive = entity.IsActive
            };

            return Json(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save([FromBody] SafetyTypeViewModel model)
        {
            if (!ModelState.IsValid) return Json(new { success = false, message = "بيانات غير صالحة" });

            if (model.SafetyTypeId == 0)
            {
                var entity = new SafetyType
                {
                    SafetyTypeCode = model.SafetyTypeCode.ToUpper().Trim(),
                    SafetyTypeNameAr = model.SafetyTypeNameAr.Trim(),
                    SafetyTypeNameEn = model.SafetyTypeNameEn?.Trim(),
                    SeverityLevel = model.SeverityLevel,
                    IsActive = model.IsActive
                };
                _context.SafetyTypes.Add(entity);
            }
            else
            {
                var entity = await _context.SafetyTypes.FindAsync(model.SafetyTypeId);
                if (entity == null) return Json(new { success = false, message = "السجل غير موجود" });

                entity.SafetyTypeCode = model.SafetyTypeCode.ToUpper().Trim();
                entity.SafetyTypeNameAr = model.SafetyTypeNameAr.Trim();
                entity.SafetyTypeNameEn = model.SafetyTypeNameEn?.Trim();
                entity.SeverityLevel = model.SeverityLevel;
                entity.IsActive = model.IsActive;
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "تم الحفظ بنجاح" });
        }
    }
}