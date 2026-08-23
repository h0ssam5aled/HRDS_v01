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
    public class ProbationStatusesController : Controller
    {
        private readonly HRDSContext _context;

        public ProbationStatusesController(HRDSContext context)
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
            var list = await _context.ProbationStatuses
                .Select(x => new ProbationStatusViewModel
                {
                    ProbationStatusId = x.ProbationStatusId,
                    StatusCode = x.StatusCode,
                    StatusNameAr = x.StatusNameAr,
                    StatusNameEn = x.StatusNameEn,
                    Description = x.Description
                })
                .ToListAsync();

            return Json(new { data = list });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var entity = await _context.ProbationStatuses.FindAsync(id);
            if (entity == null)
                return Json(null);

            var model = new ProbationStatusViewModel
            {
                ProbationStatusId = entity.ProbationStatusId,
                StatusCode = entity.StatusCode,
                StatusNameAr = entity.StatusNameAr,
                StatusNameEn = entity.StatusNameEn,
                Description = entity.Description
            };

            return Json(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save([FromBody] ProbationStatusViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, message = "بيانات غير صالحة" });

            if (model.ProbationStatusId == 0)
            {
                var entity = new ProbationStatus
                {
                    StatusCode = model.StatusCode.ToUpper().Trim(),
                    StatusNameAr = model.StatusNameAr.Trim(),
                    StatusNameEn = model.StatusNameEn?.Trim(),
                    Description = model.Description?.Trim()
                };
                _context.ProbationStatuses.Add(entity);
            }
            else
            {
                var entity = await _context.ProbationStatuses.FindAsync(model.ProbationStatusId);
                if (entity == null)
                    return Json(new { success = false, message = "السجل غير موجود" });

                entity.StatusCode = model.StatusCode.ToUpper().Trim();
                entity.StatusNameAr = model.StatusNameAr.Trim();
                entity.StatusNameEn = model.StatusNameEn?.Trim();
                entity.Description = model.Description?.Trim();
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "تم الحفظ بنجاح" });
        }
    }
}