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
    public class JobTitlesController : Controller
    {
        private readonly HRDSContext _context;

        public JobTitlesController(HRDSContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var isEn = CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToLower() == "en";

            ViewBag.JobGroups = await _context.JobGroups
                .Where(x => !x.IsDeleted && x.IsActive)
                .Select(x => new {
                    x.JobGroupId,
                    Name = isEn && !string.IsNullOrEmpty(x.JobGroupNameEn) ? x.JobGroupNameEn : x.JobGroupNameAr
                })
                .ToListAsync();

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetJson()
        {
            var isEn = CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToLower() == "en";

            var list = await _context.JobTitles
                .Include(x => x.JobGroup)
                .Where(x => !x.IsDeleted)
                .Select(x => new JobTitleViewModel
                {
                    JobTitleId = x.JobTitleId,
                    JobGroupId = x.JobGroupId,
                    JobGroupNameAr = x.JobGroup.JobGroupNameAr,
                    JobGroupNameEn = x.JobGroup.JobGroupNameEn,
                    DisplayJobGroupName = isEn && !string.IsNullOrEmpty(x.JobGroup.JobGroupNameEn) ? x.JobGroup.JobGroupNameEn : x.JobGroup.JobGroupNameAr,
                    JobTitleCode = x.JobTitleCode,
                    JobTitleNameAr = x.JobTitleNameAr,
                    JobTitleNameEn = x.JobTitleNameEn,
                    DisplayJobTitleName = isEn && !string.IsNullOrEmpty(x.JobTitleNameEn) ? x.JobTitleNameEn : x.JobTitleNameAr,
                    IsActive = x.IsActive
                })
                .ToListAsync();

            return Json(new { data = list });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var entity = await _context.JobTitles.FirstOrDefaultAsync(x => x.JobTitleId == id && !x.IsDeleted);
            if (entity == null) return Json(null);

            var model = new JobTitleViewModel
            {
                JobTitleId = entity.JobTitleId,
                JobGroupId = entity.JobGroupId,
                JobTitleCode = entity.JobTitleCode,
                JobTitleNameAr = entity.JobTitleNameAr,
                JobTitleNameEn = entity.JobTitleNameEn,
                IsActive = entity.IsActive
            };

            return Json(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save([FromBody] JobTitleViewModel model)
        {
            if (!ModelState.IsValid) return Json(new { success = false, message = "Invalid Data" });

            if (model.JobTitleId == 0)
            {
                var entity = new JobTitle
                {
                    JobGroupId = model.JobGroupId,
                    JobTitleCode = model.JobTitleCode.ToUpper().Trim(),
                    JobTitleNameAr = model.JobTitleNameAr.Trim(),
                    JobTitleNameEn = model.JobTitleNameEn?.Trim(),
                    IsActive = model.IsActive,
                    CreatedAt = DateTime.Now,
                    IsDeleted = false
                };
                _context.JobTitles.Add(entity);
            }
            else
            {
                var entity = await _context.JobTitles.FirstOrDefaultAsync(x => x.JobTitleId == model.JobTitleId && !x.IsDeleted);
                if (entity == null) return Json(new { success = false, message = "Record Not Found" });

                entity.JobGroupId = model.JobGroupId;
                entity.JobTitleCode = model.JobTitleCode.ToUpper().Trim();
                entity.JobTitleNameAr = model.JobTitleNameAr.Trim();
                entity.JobTitleNameEn = model.JobTitleNameEn?.Trim();
                entity.IsActive = model.IsActive;
                entity.UpdatedAt = DateTime.Now;
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Saved Successfully" });
        }
    }
}