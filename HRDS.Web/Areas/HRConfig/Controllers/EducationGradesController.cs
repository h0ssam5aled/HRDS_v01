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
    public class EducationGradesController : Controller
    {
        private readonly HRDSContext _context;

        public EducationGradesController(HRDSContext context)
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
            var list = await _context.EducationGrades
                .Select(x => new EducationGradeViewModel
                {
                    GradeId = x.GradeId,
                    GradeCode = x.GradeCode,
                    GradeNameAr = x.GradeNameAr,
                    GradeNameEn = x.GradeNameEn
                })
                .ToListAsync();

            return Json(new { data = list });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(decimal id)
        {
            var entity = await _context.EducationGrades.FindAsync(id);
            if (entity == null)
                return Json(null);

            var model = new EducationGradeViewModel
            {
                GradeId = entity.GradeId,
                GradeCode = entity.GradeCode,
                GradeNameAr = entity.GradeNameAr,
                GradeNameEn = entity.GradeNameEn
            };

            return Json(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save([FromBody] EducationGradeViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, message = "بيانات غير صالحة" });

            if (model.GradeId == 0)
            {
                var entity = new EducationGrade
                {
                    GradeCode = model.GradeCode.ToUpper().Trim(),
                    GradeNameAr = model.GradeNameAr.Trim(),
                    GradeNameEn = model.GradeNameEn?.Trim()
                };
                _context.EducationGrades.Add(entity);
            }
            else
            {
                var entity = await _context.EducationGrades.FindAsync(model.GradeId);
                if (entity == null)
                    return Json(new { success = false, message = "السجل غير موجود" });

                entity.GradeCode = model.GradeCode.ToUpper().Trim();
                entity.GradeNameAr = model.GradeNameAr.Trim();
                entity.GradeNameEn = model.GradeNameEn?.Trim();
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "تم الحفظ بنجاح" });
        }
    }
}