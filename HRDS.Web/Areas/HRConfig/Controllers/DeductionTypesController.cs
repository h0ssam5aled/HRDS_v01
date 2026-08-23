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
    public class DeductionTypesController : Controller
    {
        private readonly HRDSContext _context;

        public DeductionTypesController(HRDSContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetJson()
        {
            var list = await _context.DeductionTypes
                .Select(x => new DeductionTypeViewModel
                {
                    DeductionTypeId = x.DeductionTypeId,
                    DeductionTypeCode = x.DeductionTypeCode,
                    DeductionTypeNameAr = x.DeductionTypeNameAr,
                    DeductionTypeNameEn = x.DeductionTypeNameEn,
                    Description = x.Description,
                    IsActive = x.IsActive
                })
                .ToListAsync();

            return Json(new { data = list });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var entity = await _context.DeductionTypes.FindAsync(id);
            if (entity == null) return Json(null);

            var model = new DeductionTypeViewModel
            {
                DeductionTypeId = entity.DeductionTypeId,
                DeductionTypeCode = entity.DeductionTypeCode,
                DeductionTypeNameAr = entity.DeductionTypeNameAr,
                DeductionTypeNameEn = entity.DeductionTypeNameEn,
                Description = entity.Description,
                IsActive = entity.IsActive
            };

            return Json(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save([FromBody] DeductionTypeViewModel model)
        {
            if (!ModelState.IsValid) return Json(new { success = false, message = "بيانات غير صالحة" });

            if (model.DeductionTypeId == 0)
            {
                var entity = new DeductionType
                {
                    DeductionTypeCode = model.DeductionTypeCode.ToUpper().Trim(),
                    DeductionTypeNameAr = model.DeductionTypeNameAr.Trim(),
                    DeductionTypeNameEn = model.DeductionTypeNameEn?.Trim(),
                    Description = model.Description?.Trim(),
                    IsActive = model.IsActive
                };
                _context.DeductionTypes.Add(entity);
            }
            else
            {
                var entity = await _context.DeductionTypes.FindAsync(model.DeductionTypeId);
                if (entity == null) return Json(new { success = false, message = "السجل غير موجود" });

                entity.DeductionTypeCode = model.DeductionTypeCode.ToUpper().Trim();
                entity.DeductionTypeNameAr = model.DeductionTypeNameAr.Trim();
                entity.DeductionTypeNameEn = model.DeductionTypeNameEn?.Trim();
                entity.Description = model.Description?.Trim();
                entity.IsActive = model.IsActive;
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "تم الحفظ بنجاح" });
        }
    }
}