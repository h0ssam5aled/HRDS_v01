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
    public class HolidayCalendarsController : Controller
    {
        private readonly HRDSContext _context;

        public HolidayCalendarsController(HRDSContext context)
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

            var rawData = await _context.HolidayCalendars
                .OrderByDescending(x => x.StartDate)
                .ToListAsync();

            var list = rawData.Select(x => new HolidayCalendarViewModel
            {
                HolidayId = x.HolidayId,
                HolidayNameAr = x.HolidayNameAr,
                HolidayNameEn = x.HolidayNameEn,
                DisplayHolidayName = isEn && !string.IsNullOrEmpty(x.HolidayNameEn) ? x.HolidayNameEn : x.HolidayNameAr,
                StartDate = x.StartDate.ToString("yyyy-MM-dd"),
                EndDate = x.EndDate.ToString("yyyy-MM-dd"),
                TotalDays = x.TotalDays,
                Year = x.Year,
                IsRecurring = x.IsRecurring,
                IsActive = x.IsActive,
                CompanyId = x.CompanyId
            }).ToList();

            return Json(new { data = list });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var entity = await _context.HolidayCalendars.FirstOrDefaultAsync(x => x.HolidayId == id);
            if (entity == null) return Json(null);

            var model = new HolidayCalendarViewModel
            {
                HolidayId = entity.HolidayId,
                HolidayNameAr = entity.HolidayNameAr,
                HolidayNameEn = entity.HolidayNameEn,
                StartDate = entity.StartDate.ToString("yyyy-MM-dd"),
                EndDate = entity.EndDate.ToString("yyyy-MM-dd"),
                TotalDays = entity.TotalDays,
                Year = entity.Year,
                IsRecurring = entity.IsRecurring,
                IsActive = entity.IsActive,
                CompanyId = entity.CompanyId
            };

            return Json(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save([FromBody] HolidayCalendarViewModel model)
        {
            if (!ModelState.IsValid) return Json(new { success = false, message = "Invalid Data" });

            if (!DateOnly.TryParse(model.StartDate, out var start) || !DateOnly.TryParse(model.EndDate, out var end))
            {
                return Json(new { success = false, message = "Invalid Date Format" });
            }

            if (end < start)
            {
                return Json(new { success = false, message = "End Date must be after Start Date" });
            }

            short calculatedDays = (short)((end.ToDateTime(TimeOnly.MinValue) - start.ToDateTime(TimeOnly.MinValue)).Days + 1);

            if (model.HolidayId == 0)
            {
                var entity = new HolidayCalendar
                {
                    HolidayNameAr = model.HolidayNameAr.Trim(),
                    HolidayNameEn = model.HolidayNameEn?.Trim(),
                    StartDate = start,
                    EndDate = end,
                    TotalDays = calculatedDays,
                    Year = start.Year,
                    IsRecurring = model.IsRecurring,
                    IsActive = model.IsActive,
                    CompanyId = model.CompanyId
                };
                _context.HolidayCalendars.Add(entity);
            }
            else
            {
                var entity = await _context.HolidayCalendars.FirstOrDefaultAsync(x => x.HolidayId == model.HolidayId);
                if (entity == null) return Json(new { success = false, message = "Record Not Found" });

                entity.HolidayNameAr = model.HolidayNameAr.Trim();
                entity.HolidayNameEn = model.HolidayNameEn?.Trim();
                entity.StartDate = start;
                entity.EndDate = end;
                entity.TotalDays = calculatedDays;
                entity.Year = start.Year;
                entity.IsRecurring = model.IsRecurring;
                entity.IsActive = model.IsActive;
                entity.CompanyId = model.CompanyId;
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Saved Successfully" });
        }
    }
}