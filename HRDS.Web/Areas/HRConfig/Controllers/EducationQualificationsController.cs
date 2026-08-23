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
    public class EducationQualificationsController : Controller
    {
        private readonly HRDSContext _context;

        public EducationQualificationsController(HRDSContext context)
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
            var list = await _context.EducationQualifications
                .Select(x => new EducationQualificationViewModel
                {
                    QualificationId = x.QualificationId,
                    QualificationCode = x.QualificationCode,
                    QualificationNameAr = x.QualificationNameAr,
                    QualificationNameEn = x.QualificationNameEn
                })
                .ToListAsync();

            return Json(new { data = list });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var entity = await _context.EducationQualifications.FindAsync(id);
            if (entity == null)
                return Json(null);

            var model = new EducationQualificationViewModel
            {
                QualificationId = entity.QualificationId,
                QualificationCode = entity.QualificationCode,
                QualificationNameAr = entity.QualificationNameAr,
                QualificationNameEn = entity.QualificationNameEn
            };

            return Json(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save([FromBody] EducationQualificationViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, message = "بيانات غير صالحة" });

            if (model.QualificationId == 0)
            {
                var entity = new EducationQualification
                {
                    QualificationCode = model.QualificationCode.ToUpper().Trim(),
                    QualificationNameAr = model.QualificationNameAr.Trim(),
                    QualificationNameEn = model.QualificationNameEn?.Trim()
                };
                _context.EducationQualifications.Add(entity);
            }
            else
            {
                var entity = await _context.EducationQualifications.FindAsync(model.QualificationId);
                if (entity == null)
                    return Json(new { success = false, message = "السجل غير موجود" });

                entity.QualificationCode = model.QualificationCode.ToUpper().Trim();
                entity.QualificationNameAr = model.QualificationNameAr.Trim();
                entity.QualificationNameEn = model.QualificationNameEn?.Trim();
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "تم الحفظ بنجاح" });
        }
    }
}