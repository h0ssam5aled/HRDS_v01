using System.Globalization;
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
    public class RequestStatusesController : Controller
    {
        private readonly HRDSContext _context;

        public RequestStatusesController(HRDSContext context)
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
            var isEn = CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToLower() == "en";

            var list = await _context.RequestStatuses
                .Select(x => new RequestStatusViewModel
                {
                    StatusId = x.StatusId,
                    StatusCode = x.StatusCode,
                    StatusNameAr = x.StatusNameAr,
                    StatusNameEn = x.StatusNameEn,
                    DisplayStatusName = isEn && !string.IsNullOrEmpty(x.StatusNameEn) ? x.StatusNameEn : x.StatusNameAr,
                    BadgeClass = x.BadgeClass,
                    IsFinal = x.IsFinal,
                    IsActive = x.IsActive
                })
                .ToListAsync();

            return Json(new { data = list });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var entity = await _context.RequestStatuses.FirstOrDefaultAsync(x => x.StatusId == id);
            if (entity == null) return Json(null);

            var model = new RequestStatusViewModel
            {
                StatusId = entity.StatusId,
                StatusCode = entity.StatusCode,
                StatusNameAr = entity.StatusNameAr,
                StatusNameEn = entity.StatusNameEn,
                BadgeClass = entity.BadgeClass,
                IsFinal = entity.IsFinal,
                IsActive = entity.IsActive
            };

            return Json(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save([FromBody] RequestStatusViewModel model)
        {
            if (!ModelState.IsValid) return Json(new { success = false, message = "Invalid Data" });

            if (model.StatusId == 0)
            {
                var entity = new RequestStatus
                {
                    StatusCode = model.StatusCode.ToUpper().Trim(),
                    StatusNameAr = model.StatusNameAr.Trim(),
                    StatusNameEn = model.StatusNameEn?.Trim(),
                    BadgeClass = model.BadgeClass?.Trim(),
                    IsFinal = model.IsFinal,
                    IsActive = model.IsActive
                };
                _context.RequestStatuses.Add(entity);
            }
            else
            {
                var entity = await _context.RequestStatuses.FirstOrDefaultAsync(x => x.StatusId == model.StatusId);
                if (entity == null) return Json(new { success = false, message = "Record Not Found" });

                entity.StatusCode = model.StatusCode.ToUpper().Trim();
                entity.StatusNameAr = model.StatusNameAr.Trim();
                entity.StatusNameEn = model.StatusNameEn?.Trim();
                entity.BadgeClass = model.BadgeClass?.Trim();
                entity.IsFinal = model.IsFinal;
                entity.IsActive = model.IsActive;
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Saved Successfully" });
        }
    }
}