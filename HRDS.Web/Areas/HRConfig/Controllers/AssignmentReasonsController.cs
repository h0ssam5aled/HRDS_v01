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
    public class AssignmentReasonsController : Controller
    {
        private readonly HRDSContext _context;

        public AssignmentReasonsController(HRDSContext context)
        {
            _context = context;
        }

        public IActionResult Index() => View();

        [HttpGet]
        public async Task<IActionResult> GetJson()
        {
            var list = await _context.AssignmentReasons
                .Where(x => !x.IsDeleted)
                .Select(x => new AssignmentReasonViewModel
                {
                    AssignmentReasonId = x.AssignmentReasonId,
                    AssignmentReasonCode = x.AssignmentReasonCode,
                    AssignmentReasonNameAr = x.AssignmentReasonNameAr,
                    AssignmentReasonNameEn = x.AssignmentReasonNameEn,
                    Description = x.Description,
                    IsActive = x.IsActive
                })
                .ToListAsync();

            return Json(new { data = list });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var entity = await _context.AssignmentReasons.FirstOrDefaultAsync(x => x.AssignmentReasonId == id && !x.IsDeleted);
            if (entity == null) return Json(null);

            var model = new AssignmentReasonViewModel
            {
                AssignmentReasonId = entity.AssignmentReasonId,
                AssignmentReasonCode = entity.AssignmentReasonCode,
                AssignmentReasonNameAr = entity.AssignmentReasonNameAr,
                AssignmentReasonNameEn = entity.AssignmentReasonNameEn,
                Description = entity.Description,
                IsActive = entity.IsActive
            };

            return Json(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save([FromBody] AssignmentReasonViewModel model)
        {
            if (!ModelState.IsValid) return Json(new { success = false, message = "بيانات غير صالحة" });

            if (model.AssignmentReasonId == 0)
            {
                var entity = new AssignmentReason
                {
                    AssignmentReasonCode = model.AssignmentReasonCode.ToUpper().Trim(),
                    AssignmentReasonNameAr = model.AssignmentReasonNameAr.Trim(),
                    AssignmentReasonNameEn = model.AssignmentReasonNameEn?.Trim(),
                    Description = model.Description?.Trim(),
                    IsActive = model.IsActive,
                    CreatedAt = DateTime.Now,
                    IsDeleted = false
                };
                _context.AssignmentReasons.Add(entity);
            }
            else
            {
                var entity = await _context.AssignmentReasons.FirstOrDefaultAsync(x => x.AssignmentReasonId == model.AssignmentReasonId && !x.IsDeleted);
                if (entity == null) return Json(new { success = false, message = "السجل غير موجود" });

                entity.AssignmentReasonCode = model.AssignmentReasonCode.ToUpper().Trim();
                entity.AssignmentReasonNameAr = model.AssignmentReasonNameAr.Trim();
                entity.AssignmentReasonNameEn = model.AssignmentReasonNameEn?.Trim();
                entity.Description = model.Description?.Trim();
                entity.IsActive = model.IsActive;
                entity.UpdatedAt = DateTime.Now;
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "تم الحفظ بنجاح" });
        }
    }
}