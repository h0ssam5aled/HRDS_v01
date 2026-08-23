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
    public class JobLevelsController : Controller
    {
        private readonly HRDSContext _context;

        public JobLevelsController(HRDSContext context)
        {
            _context = context;
        }

        public IActionResult Index() => View();

        [HttpGet]
        public async Task<IActionResult> GetJson()
        {
            var list = await _context.JobLevels
                .Where(x => !x.IsDeleted)
                .Select(x => new JobLevelViewModel
                {
                    JobLevelId = x.JobLevelId,
                    JobLevelCode = x.JobLevelCode,
                    JobLevelNameAr = x.JobLevelNameAr,
                    JobLevelNameEn = x.JobLevelNameEn,
                    IsActive = x.IsActive
                })
                .ToListAsync();

            return Json(new { data = list });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var entity = await _context.JobLevels.FirstOrDefaultAsync(x => x.JobLevelId == id && !x.IsDeleted);
            if (entity == null) return Json(null);

            var model = new JobLevelViewModel
            {
                JobLevelId = entity.JobLevelId,
                JobLevelCode = entity.JobLevelCode,
                JobLevelNameAr = entity.JobLevelNameAr,
                JobLevelNameEn = entity.JobLevelNameEn,
                IsActive = entity.IsActive
            };

            return Json(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save([FromBody] JobLevelViewModel model)
        {
            if (!ModelState.IsValid) return Json(new { success = false, message = "بيانات غير صالحة" });

            if (model.JobLevelId == 0)
            {
                var entity = new JobLevel
                {
                    JobLevelCode = model.JobLevelCode.ToUpper().Trim(),
                    JobLevelNameAr = model.JobLevelNameAr.Trim(),
                    JobLevelNameEn = model.JobLevelNameEn?.Trim(),
                    IsActive = model.IsActive,
                    CreatedAt = DateTime.Now,
                    IsDeleted = false
                };
                _context.JobLevels.Add(entity);
            }
            else
            {
                var entity = await _context.JobLevels.FirstOrDefaultAsync(x => x.JobLevelId == model.JobLevelId && !x.IsDeleted);
                if (entity == null) return Json(new { success = false, message = "السجل غير موجود" });

                entity.JobLevelCode = model.JobLevelCode.ToUpper().Trim();
                entity.JobLevelNameAr = model.JobLevelNameAr.Trim();
                entity.JobLevelNameEn = model.JobLevelNameEn?.Trim();
                entity.IsActive = model.IsActive;
                entity.UpdatedAt = DateTime.Now;
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "تم الحفظ بنجاح" });
        }
    }
}