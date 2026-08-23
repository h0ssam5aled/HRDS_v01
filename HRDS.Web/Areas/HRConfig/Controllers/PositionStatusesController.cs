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
    public class PositionStatusesController : Controller
    {
        private readonly HRDSContext _context;

        public PositionStatusesController(HRDSContext context)
        {
            _context = context;
        }

        public IActionResult Index() => View();

        [HttpGet]
        public async Task<IActionResult> GetJson()
        {
            var list = await _context.PositionStatuses
                .Where(x => !x.IsDeleted)
                .Select(x => new PositionStatusViewModel
                {
                    PositionStatusId = x.PositionStatusId,
                    PositionStatusCode = x.PositionStatusCode,
                    PositionStatusNameAr = x.PositionStatusNameAr,
                    PositionStatusNameEn = x.PositionStatusNameEn,
                    Description = x.Description,
                    IsActive = x.IsActive
                })
                .ToListAsync();

            return Json(new { data = list });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var entity = await _context.PositionStatuses.FirstOrDefaultAsync(x => x.PositionStatusId == id && !x.IsDeleted);
            if (entity == null) return Json(null);

            var model = new PositionStatusViewModel
            {
                PositionStatusId = entity.PositionStatusId,
                PositionStatusCode = entity.PositionStatusCode,
                PositionStatusNameAr = entity.PositionStatusNameAr,
                PositionStatusNameEn = entity.PositionStatusNameEn,
                Description = entity.Description,
                IsActive = entity.IsActive
            };

            return Json(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save([FromBody] PositionStatusViewModel model)
        {
            if (!ModelState.IsValid) return Json(new { success = false, message = "بيانات غير صالحة" });

            if (model.PositionStatusId == 0)
            {
                var entity = new PositionStatus
                {
                    PositionStatusCode = model.PositionStatusCode.ToUpper().Trim(),
                    PositionStatusNameAr = model.PositionStatusNameAr.Trim(),
                    PositionStatusNameEn = model.PositionStatusNameEn?.Trim(),
                    Description = model.Description?.Trim(),
                    IsActive = model.IsActive,
                    CreatedAt = DateTime.Now,
                    IsDeleted = false
                };
                _context.PositionStatuses.Add(entity);
            }
            else
            {
                var entity = await _context.PositionStatuses.FirstOrDefaultAsync(x => x.PositionStatusId == model.PositionStatusId && !x.IsDeleted);
                if (entity == null) return Json(new { success = false, message = "السجل غير موجود" });

                entity.PositionStatusCode = model.PositionStatusCode.ToUpper().Trim();
                entity.PositionStatusNameAr = model.PositionStatusNameAr.Trim();
                entity.PositionStatusNameEn = model.PositionStatusNameEn?.Trim();
                entity.Description = model.Description?.Trim();
                entity.IsActive = model.IsActive;
                entity.UpdatedAt = DateTime.Now;
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "تم الحفظ بنجاح" });
        }
    }
}