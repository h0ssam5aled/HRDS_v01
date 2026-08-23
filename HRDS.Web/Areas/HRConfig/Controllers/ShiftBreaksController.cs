using HRDS.Web.Areas.HR.ViewModels;
using HRDS.Web.Areas.HRConfig.ViewModels;
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
    public class ShiftBreaksController : Controller
    {
        private readonly HRDSContext _context;

        public ShiftBreaksController(HRDSContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Shifts = new SelectList(await _context.Shifts
                .Where(s => s.IsActive)
                .Select(s => new { s.ShiftId, Name = s.ShiftNameAr + " (" + s.ShiftCode + ")" })
                .ToListAsync(), "ShiftId", "Name");

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetJson()
        {
            var data = await _context.ShiftBreaks
                .Include(b => b.Shift)
                .Select(b => new ShiftBreakViewModel
                {
                    BreakId = b.BreakId,
                    ShiftId = b.ShiftId,
                    ShiftNameAr = b.Shift.ShiftNameAr,
                    BreakCode = b.BreakCode,
                    BreakNameAr = b.BreakNameAr,
                    BreakNameEn = b.BreakNameEn,
                    FromTime = b.FromTime.ToString("HH:mm:ss"),
                    ToTime = b.ToTime.ToString("HH:mm:ss"),
                    IsPaidBreak = b.IsPaidBreak,
                    Description = b.Description,
                    IsActive = b.IsActive
                }).ToListAsync();

            return Json(new { data });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var b = await _context.ShiftBreaks.FindAsync(id);
            if (b == null) return NotFound();

            var model = new ShiftBreakViewModel
            {
                BreakId = b.BreakId,
                ShiftId = b.ShiftId,
                BreakCode = b.BreakCode,
                BreakNameAr = b.BreakNameAr,
                BreakNameEn = b.BreakNameEn,
                FromTime = b.FromTime.ToString("HH:mm:ss"),
                ToTime = b.ToTime.ToString("HH:mm:ss"),
                IsPaidBreak = b.IsPaidBreak,
                Description = b.Description,
                IsActive = b.IsActive
            };

            return Json(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save([FromBody] ShiftBreakViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, message = "بيانات غير صالحة" });

            if (!TimeOnly.TryParse(model.FromTime, out var fromTime) || !TimeOnly.TryParse(model.ToTime, out var toTime))
                return Json(new { success = false, message = "صيغة الوقت غير صحيحة" });

            if (model.BreakId == 0)
            {
                var entity = new ShiftBreak
                {
                    ShiftId = model.ShiftId,
                    BreakCode = model.BreakCode,
                    BreakNameAr = model.BreakNameAr,
                    BreakNameEn = model.BreakNameEn,
                    FromTime = fromTime,
                    ToTime = toTime,
                    IsPaidBreak = model.IsPaidBreak,
                    Description = model.Description,
                    IsActive = model.IsActive
                };
                _context.ShiftBreaks.Add(entity);
            }
            else
            {
                var entity = await _context.ShiftBreaks.FindAsync(model.BreakId);
                if (entity == null) return Json(new { success = false, message = "العنصر غير موجود" });

                entity.ShiftId = model.ShiftId;
                entity.BreakCode = model.BreakCode;
                entity.BreakNameAr = model.BreakNameAr;
                entity.BreakNameEn = model.BreakNameEn;
                entity.FromTime = fromTime;
                entity.ToTime = toTime;
                entity.IsPaidBreak = model.IsPaidBreak;
                entity.Description = model.Description;
                entity.IsActive = model.IsActive;
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }
    }
}