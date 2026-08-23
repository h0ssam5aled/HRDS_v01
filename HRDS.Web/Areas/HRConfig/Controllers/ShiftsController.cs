using HRDS.Web.Areas.HR.ViewModels;
using HRDS.Web.Models;
using HRDS.Web.Models.Entities;
using HRDS.Web.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HRDS.Web.Controllers
{
    [Authorize]
    [Area("HRConfig")]
    [HasModuleAccess("HRConfig")]
    public class ShiftsController : Controller
    {
        private readonly HRDSContext _context;

        public ShiftsController(HRDSContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.ShiftTypes = new SelectList(
                await _context.ShiftTypes.Where(x => x.IsActive).ToListAsync(),
                "ShiftTypeId",
                "ShiftTypeNameAr"
            );
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetJson()
        {
            var list = await _context.Shifts
                .Include(x => x.ShiftType)
                .Select(x => new ShiftViewModel
                {
                    ShiftId = x.ShiftId,
                    ShiftTypeId = x.ShiftTypeId,
                    ShiftTypeNameAr = x.ShiftType.ShiftTypeNameAr,
                    ShiftCode = x.ShiftCode,
                    ShiftNameAr = x.ShiftNameAr,
                    ShiftNameEn = x.ShiftNameEn,
                    StartTime =  x.StartTime,
                    EndTime = x.EndTime,
                    CrossMidnight = x.CrossMidnight,
                    GraceInMinutes = x.GraceInMinutes,
                    GraceOutMinutes = x.GraceOutMinutes,
                    AllowOvertime = x.AllowOvertime,
                    MinimumOvertimeMinutes = x.MinimumOvertimeMinutes,
                    AllowLateDeduction = x.AllowLateDeduction,
                    AutoCloseAttendance = x.AutoCloseAttendance,
                    StandardHours = x.StandardHours,
                    IsActive = x.IsActive
                })
                .ToListAsync();

            return Json(new { data = list });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var entity = await _context.Shifts.FindAsync(id);
            if (entity == null) return Json(null);

            var model = new ShiftViewModel
            {
                ShiftId = entity.ShiftId,
                ShiftTypeId = entity.ShiftTypeId,
                ShiftCode = entity.ShiftCode,
                ShiftNameAr = entity.ShiftNameAr,
                ShiftNameEn = entity.ShiftNameEn,
                StartTime = entity.StartTime,
                EndTime = entity.EndTime,
                CrossMidnight = entity.CrossMidnight,
                GraceInMinutes = entity.GraceInMinutes,
                GraceOutMinutes = entity.GraceOutMinutes,
                AllowOvertime = entity.AllowOvertime,
                MinimumOvertimeMinutes = entity.MinimumOvertimeMinutes,
                AllowLateDeduction = entity.AllowLateDeduction,
                AutoCloseAttendance = entity.AutoCloseAttendance,
                StandardHours = entity.StandardHours,
                IsActive = entity.IsActive
            };

            return Json(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save([FromBody] ShiftViewModel model)
        {
            if (!ModelState.IsValid) return Json(new { success = false, message = "بيانات غير صالحة" });

            if (model.ShiftId == 0)
            {
                var entity = new Shift
                {
                    ShiftTypeId = model.ShiftTypeId,
                    ShiftCode = model.ShiftCode.ToUpper().Trim(),
                    ShiftNameAr = model.ShiftNameAr.Trim(),
                    ShiftNameEn = model.ShiftNameEn?.Trim(),
                    StartTime = model.StartTime,
                    EndTime = model.EndTime,
                    CrossMidnight = model.CrossMidnight,
                    GraceInMinutes = model.GraceInMinutes,
                    GraceOutMinutes = model.GraceOutMinutes,
                    AllowOvertime = model.AllowOvertime,
                    MinimumOvertimeMinutes = model.MinimumOvertimeMinutes,
                    AllowLateDeduction = model.AllowLateDeduction,
                    AutoCloseAttendance = model.AutoCloseAttendance,
                    StandardHours = model.StandardHours,
                    IsActive = model.IsActive
                };
                _context.Shifts.Add(entity);
            }
            else
            {
                var entity = await _context.Shifts.FindAsync(model.ShiftId);
                if (entity == null) return Json(new { success = false, message = "السجل غير موجود" });

                entity.ShiftTypeId = model.ShiftTypeId;
                entity.ShiftCode = model.ShiftCode.ToUpper().Trim();
                entity.ShiftNameAr = model.ShiftNameAr.Trim();
                entity.ShiftNameEn = model.ShiftNameEn?.Trim();
                entity.StartTime = model.StartTime;
                entity.EndTime = model.EndTime;
                entity.CrossMidnight = model.CrossMidnight;
                entity.GraceInMinutes = model.GraceInMinutes;
                entity.GraceOutMinutes = model.GraceOutMinutes;
                entity.AllowOvertime = model.AllowOvertime;
                entity.MinimumOvertimeMinutes = model.MinimumOvertimeMinutes;
                entity.AllowLateDeduction = model.AllowLateDeduction;
                entity.AutoCloseAttendance = model.AutoCloseAttendance;
                entity.StandardHours = model.StandardHours;
                entity.IsActive = model.IsActive;
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "تم الحفظ بنجاح" });
        }
    }
}