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
    public class AllowanceTypesController : Controller
    {
        private readonly HRDSContext _context;

        public AllowanceTypesController(HRDSContext context)
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
            var list = await _context.AllowanceTypes
                .Select(x => new AllowanceTypeViewModel
                {
                    AllowanceTypeId = x.AllowanceTypeId,
                    AllowanceTypeCode = x.AllowanceTypeCode,
                    AllowanceTypeNameAr = x.AllowanceTypeNameAr,
                    AllowanceTypeNameEn = x.AllowanceTypeNameEn,
                    Description = x.Description,
                    IsActive = x.IsActive
                })
                .ToListAsync();

            return Json(new { data = list });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var entity = await _context.AllowanceTypes.FindAsync(id);
            if (entity == null) return Json(null);

            var model = new AllowanceTypeViewModel
            {
                AllowanceTypeId = entity.AllowanceTypeId,
                AllowanceTypeCode = entity.AllowanceTypeCode,
                AllowanceTypeNameAr = entity.AllowanceTypeNameAr,
                AllowanceTypeNameEn = entity.AllowanceTypeNameEn,
                Description = entity.Description,
                IsActive = entity.IsActive
            };

            return Json(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save([FromBody] AllowanceTypeViewModel model)
        {
            if (!ModelState.IsValid) return Json(new { success = false, message = "بيانات غير صالحة" });

            if (model.AllowanceTypeId == 0)
            {
                var entity = new AllowanceType
                {
                    AllowanceTypeCode = model.AllowanceTypeCode.ToUpper().Trim(),
                    AllowanceTypeNameAr = model.AllowanceTypeNameAr.Trim(),
                    AllowanceTypeNameEn = model.AllowanceTypeNameEn?.Trim(),
                    Description = model.Description?.Trim(),
                    IsActive = model.IsActive
                };
                _context.AllowanceTypes.Add(entity);
            }
            else
            {
                var entity = await _context.AllowanceTypes.FindAsync(model.AllowanceTypeId);
                if (entity == null) return Json(new { success = false, message = "السجل غير موجود" });

                entity.AllowanceTypeCode = model.AllowanceTypeCode.ToUpper().Trim();
                entity.AllowanceTypeNameAr = model.AllowanceTypeNameAr.Trim();
                entity.AllowanceTypeNameEn = model.AllowanceTypeNameEn?.Trim();
                entity.Description = model.Description?.Trim();
                entity.IsActive = model.IsActive;
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "تم الحفظ بنجاح" });
        }
    }
}