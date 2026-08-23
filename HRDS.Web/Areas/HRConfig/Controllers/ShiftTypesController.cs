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
    public class ShiftTypesController : Controller
    {
        private readonly HRDSContext _context;

        public ShiftTypesController(HRDSContext context)
        {
            _context = context;
        }

        public IActionResult Index() => View();

        [HttpGet]
        public async Task<IActionResult> GetJson()
        {
            var list = await _context.ShiftTypes
                .Select(x => new ShiftTypeViewModel
                {
                    ShiftTypeId = x.ShiftTypeId,
                    ShiftTypeCode = x.ShiftTypeCode,
                    ShiftTypeNameAr = x.ShiftTypeNameAr,
                    ShiftTypeNameEn = x.ShiftTypeNameEn,
                    Description = x.Description,
                    IsActive = x.IsActive
                })
                .ToListAsync();

            return Json(new { data = list });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var entity = await _context.ShiftTypes.FindAsync(id);
            if (entity == null) return Json(null);

            var model = new ShiftTypeViewModel
            {
                ShiftTypeId = entity.ShiftTypeId,
                ShiftTypeCode = entity.ShiftTypeCode,
                ShiftTypeNameAr = entity.ShiftTypeNameAr,
                ShiftTypeNameEn = entity.ShiftTypeNameEn,
                Description = entity.Description,
                IsActive = entity.IsActive
            };

            return Json(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save([FromBody] ShiftTypeViewModel model)
        {
            if (!ModelState.IsValid) return Json(new { success = false, message = "بيانات غير صالحة" });

            if (model.ShiftTypeId == 0)
            {
                var entity = new ShiftType
                {
                    ShiftTypeCode = model.ShiftTypeCode.ToUpper().Trim(),
                    ShiftTypeNameAr = model.ShiftTypeNameAr.Trim(),
                    ShiftTypeNameEn = model.ShiftTypeNameEn?.Trim(),
                    Description = model.Description?.Trim(),
                    IsActive = model.IsActive
                };
                _context.ShiftTypes.Add(entity);
            }
            else
            {
                var entity = await _context.ShiftTypes.FindAsync(model.ShiftTypeId);
                if (entity == null) return Json(new { success = false, message = "السجل غير موجود" });

                entity.ShiftTypeCode = model.ShiftTypeCode.ToUpper().Trim();
                entity.ShiftTypeNameAr = model.ShiftTypeNameAr.Trim();
                entity.ShiftTypeNameEn = model.ShiftTypeNameEn?.Trim();
                entity.Description = model.Description?.Trim();
                entity.IsActive = model.IsActive;
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "تم الحفظ بنجاح" });
        }
    }
}