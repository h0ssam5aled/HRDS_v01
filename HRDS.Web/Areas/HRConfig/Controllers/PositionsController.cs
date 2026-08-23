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
    public class PositionsController : Controller
    {
        private readonly HRDSContext _context;

        public PositionsController(HRDSContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var isEn = CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToLower() == "en";

            ViewBag.JobTitles = await _context.JobTitles
                .Where(x => !x.IsDeleted && x.IsActive)
                .Select(x => new { x.JobTitleId, Name = isEn && !string.IsNullOrEmpty(x.JobTitleNameEn) ? x.JobTitleNameEn : x.JobTitleNameAr })
                .ToListAsync();

            ViewBag.JobLevels = await _context.JobLevels
                .Where(x => !x.IsDeleted && x.IsActive)
                .Select(x => new { x.JobLevelId, Name = isEn && !string.IsNullOrEmpty(x.JobLevelNameEn) ? x.JobLevelNameEn : x.JobLevelNameAr })
                .ToListAsync();

            ViewBag.PositionStatuses = await _context.PositionStatuses
                .Where(x => !x.IsDeleted && x.IsActive)
                .Select(x => new { x.PositionStatusId, Name = isEn && !string.IsNullOrEmpty(x.PositionStatusNameEn) ? x.PositionStatusNameEn : x.PositionStatusNameAr })
                .ToListAsync();

            ViewBag.Units = await _context.Units
                .Where(x => !x.IsDeleted && x.IsActive)
                .Select(x => new { x.UnitId, Name = isEn && !string.IsNullOrEmpty(x.UnitNameEn) ? x.UnitNameEn : x.UnitNameAr })
                .ToListAsync();

            ViewBag.Positions = await _context.Positions
                .Where(x => !x.IsDeleted && x.IsActive)
                .Select(x => new { x.PositionId, Name = isEn && !string.IsNullOrEmpty(x.PositionNameEn) ? x.PositionNameEn : x.PositionNameAr })
                .ToListAsync();

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetJson()
        {
            var isEn = CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToLower() == "en";

            var list = await _context.Positions
                .Include(x => x.JobTitle)
                .Include(x => x.JobLevel)
                .Include(x => x.PositionStatus)
                .Include(x => x.Unit)
                .Include(x => x.ReportsToPosition)
                .Where(x => !x.IsDeleted)
                .Select(x => new PositionViewModel
                {
                    PositionId = x.PositionId,
                    UnitId = x.UnitId,
                    UnitName = x.Unit != null ? (isEn && !string.IsNullOrEmpty(x.Unit.UnitNameEn) ? x.Unit.UnitNameEn : x.Unit.UnitNameAr) : "-",
                    ReportsToPositionId = x.ReportsToPositionId,
                    ReportsToPositionName = x.ReportsToPosition != null ? (isEn && !string.IsNullOrEmpty(x.ReportsToPosition.PositionNameEn) ? x.ReportsToPosition.PositionNameEn : x.ReportsToPosition.PositionNameAr) : "-",
                    JobTitleId = x.JobTitleId,
                    DisplayJobTitle = isEn && !string.IsNullOrEmpty(x.JobTitle.JobTitleNameEn) ? x.JobTitle.JobTitleNameEn : x.JobTitle.JobTitleNameAr,
                    JobLevelId = x.JobLevelId,
                    DisplayJobLevel = x.JobLevel != null ? (isEn && !string.IsNullOrEmpty(x.JobLevel.JobLevelNameEn) ? x.JobLevel.JobLevelNameEn : x.JobLevel.JobLevelNameAr) : "-",
                    PositionStatusId = x.PositionStatusId,
                    DisplayPositionStatus = isEn && !string.IsNullOrEmpty(x.PositionStatus.PositionStatusNameEn) ? x.PositionStatus.PositionStatusNameEn : x.PositionStatus.PositionStatusNameAr,
                    PositionCode = x.PositionCode,
                    PositionNameAr = x.PositionNameAr,
                    PositionNameEn = x.PositionNameEn,
                    DisplayPositionName = isEn && !string.IsNullOrEmpty(x.PositionNameEn) ? x.PositionNameEn : x.PositionNameAr,
                    HeadCount = x.HeadCount,
                    IsManagerial = x.IsManagerial,
                    EffectiveFrom = x.EffectiveFrom.HasValue ? x.EffectiveFrom.Value.ToString("yyyy-MM-dd") : null,
                    EffectiveTo = x.EffectiveTo.HasValue ? x.EffectiveTo.Value.ToString("yyyy-MM-dd") : null,
                    Remarks = x.Remarks,
                    IsActive = x.IsActive
                })
                .ToListAsync();

            return Json(new { data = list });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var entity = await _context.Positions.FirstOrDefaultAsync(x => x.PositionId == id && !x.IsDeleted);
            if (entity == null) return Json(null);

            var model = new PositionViewModel
            {
                PositionId = entity.PositionId,
                UnitId = entity.UnitId,
                ReportsToPositionId = entity.ReportsToPositionId,
                JobTitleId = entity.JobTitleId,
                JobLevelId = entity.JobLevelId,
                PositionStatusId = entity.PositionStatusId,
                PositionCode = entity.PositionCode,
                PositionNameAr = entity.PositionNameAr,
                PositionNameEn = entity.PositionNameEn,
                HeadCount = entity.HeadCount,
                IsManagerial = entity.IsManagerial,
                EffectiveFrom = entity.EffectiveFrom?.ToString("yyyy-MM-dd"),
                EffectiveTo = entity.EffectiveTo?.ToString("yyyy-MM-dd"),
                Remarks = entity.Remarks,
                IsActive = entity.IsActive
            };

            return Json(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save([FromBody] PositionViewModel model)
        {
            if (!ModelState.IsValid) return Json(new { success = false, message = "Invalid Data" });

            DateOnly? effFrom = string.IsNullOrEmpty(model.EffectiveFrom) ? null : DateOnly.Parse(model.EffectiveFrom);
            DateOnly? effTo = string.IsNullOrEmpty(model.EffectiveTo) ? null : DateOnly.Parse(model.EffectiveTo);

            if (model.PositionId == 0)
            {
                var entity = new Position
                {
                    UnitId = model.UnitId,
                    ReportsToPositionId = model.ReportsToPositionId,
                    JobTitleId = model.JobTitleId,
                    JobLevelId = model.JobLevelId,
                    PositionStatusId = model.PositionStatusId,
                    PositionCode = model.PositionCode.ToUpper().Trim(),
                    PositionNameAr = model.PositionNameAr.Trim(),
                    PositionNameEn = model.PositionNameEn?.Trim(),
                    HeadCount = model.HeadCount,
                    IsManagerial = model.IsManagerial,
                    EffectiveFrom = effFrom,
                    EffectiveTo = effTo,
                    Remarks = model.Remarks?.Trim(),
                    IsActive = model.IsActive,
                    CreatedAt = DateTime.Now,
                    IsDeleted = false
                };
                _context.Positions.Add(entity);
            }
            else
            {
                var entity = await _context.Positions.FirstOrDefaultAsync(x => x.PositionId == model.PositionId && !x.IsDeleted);
                if (entity == null) return Json(new { success = false, message = "Record Not Found" });

                entity.UnitId = model.UnitId;
                entity.ReportsToPositionId = model.ReportsToPositionId;
                entity.JobTitleId = model.JobTitleId;
                entity.JobLevelId = model.JobLevelId;
                entity.PositionStatusId = model.PositionStatusId;
                entity.PositionCode = model.PositionCode.ToUpper().Trim();
                entity.PositionNameAr = model.PositionNameAr.Trim();
                entity.PositionNameEn = model.PositionNameEn?.Trim();
                entity.HeadCount = model.HeadCount;
                entity.IsManagerial = model.IsManagerial;
                entity.EffectiveFrom = effFrom;
                entity.EffectiveTo = effTo;
                entity.Remarks = model.Remarks?.Trim();
                entity.IsActive = model.IsActive;
                entity.UpdatedAt = DateTime.Now;
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Saved Successfully" });
        }
    }
}