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
    public class JobGroupsController : Controller
    {
        private readonly HRDSContext _context;

        public JobGroupsController(HRDSContext context)
        {
            _context = context;
        }

        public IActionResult Index() => View();

        [HttpGet]
        public async Task<IActionResult> GetJson()
        {
            var list = await _context.JobGroups
                .Where(x => !x.IsDeleted)
                .Select(x => new JobGroupViewModel
                {
                    JobGroupId = x.JobGroupId,
                    JobGroupCode = x.JobGroupCode,
                    JobGroupNameAr = x.JobGroupNameAr,
                    JobGroupNameEn = x.JobGroupNameEn,
                    IsActive = x.IsActive
                })
                .ToListAsync();

            return Json(new { data = list });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var entity = await _context.JobGroups.FirstOrDefaultAsync(x => x.JobGroupId == id && !x.IsDeleted);
            if (entity == null) return Json(null);

            var model = new JobGroupViewModel
            {
                JobGroupId = entity.JobGroupId,
                JobGroupCode = entity.JobGroupCode,
                JobGroupNameAr = entity.JobGroupNameAr,
                JobGroupNameEn = entity.JobGroupNameEn,
                IsActive = entity.IsActive
            };

            return Json(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save([FromBody] JobGroupViewModel model)
        {
            if (!ModelState.IsValid) return Json(new { success = false, message = "بيانات غير صالحة" });

            if (model.JobGroupId == 0)
            {
                var entity = new JobGroup
                {
                    JobGroupCode = model.JobGroupCode.ToUpper().Trim(),
                    JobGroupNameAr = model.JobGroupNameAr.Trim(),
                    JobGroupNameEn = model.JobGroupNameEn?.Trim(),
                    IsActive = model.IsActive,
                    CreatedAt = DateTime.Now,
                    IsDeleted = false
                };
                _context.JobGroups.Add(entity);
            }
            else
            {
                var entity = await _context.JobGroups.FirstOrDefaultAsync(x => x.JobGroupId == model.JobGroupId && !x.IsDeleted);
                if (entity == null) return Json(new { success = false, message = "السجل غير موجود" });

                entity.JobGroupCode = model.JobGroupCode.ToUpper().Trim();
                entity.JobGroupNameAr = model.JobGroupNameAr.Trim();
                entity.JobGroupNameEn = model.JobGroupNameEn?.Trim();
                entity.IsActive = model.IsActive;
                entity.UpdatedAt = DateTime.Now;
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "تم الحفظ بنجاح" });
        }
    }
}