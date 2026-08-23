using HRDS.Web.Areas.HR.ViewModels;
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
    public class AcademicMajorsController : Controller
    {
        private readonly HRDSContext _context;

        public AcademicMajorsController(HRDSContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var isArabic = CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToLower() == "ar";
            ViewBag.Faculties = new SelectList(
                await _context.AcademicFaculties.ToListAsync(),
                "FacultyId",
                isArabic ? "FacultyNameAr" : "FacultyNameEn"
            );
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetJson()
        {
            var list = await _context.AcademicMajors
                .Include(x => x.Faculty)
                .Select(x => new AcademicMajorViewModel
                {
                    MajorId = x.MajorId,
                    FacultyId = x.FacultyId,
                    FacultyNameAr = x.Faculty.FacultyNameAr,
                    FacultyNameEn = x.Faculty.FacultyNameEn,
                    MajorCode = x.MajorCode,
                    MajorNameAr = x.MajorNameAr,
                    MajorNameEn = x.MajorNameEn
                })
                .ToListAsync();

            return Json(new { data = list });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save([FromBody] AcademicMajorViewModel model)
        {
            if (!ModelState.IsValid) return Json(new { success = false, message = "بيانات غير صالحة" });

            if (model.MajorId == 0)
            {
                var entity = new AcademicMajor
                {
                    FacultyId = model.FacultyId,
                    MajorCode = model.MajorCode.ToUpper().Trim(),
                    MajorNameAr = model.MajorNameAr.Trim(),
                    MajorNameEn = model.MajorNameEn?.Trim()
                };
                _context.AcademicMajors.Add(entity);
            }
            else
            {
                var entity = await _context.AcademicMajors.FindAsync(model.MajorId);
                if (entity == null) return Json(new { success = false, message = "السجل غير موجود" });

                entity.FacultyId = model.FacultyId;
                entity.MajorCode = model.MajorCode.ToUpper().Trim();
                entity.MajorNameAr = model.MajorNameAr.Trim();
                entity.MajorNameEn = model.MajorNameEn?.Trim();
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "تم الحفظ بنجاح" });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var entity = await _context.AcademicMajors.FindAsync(id);
            if (entity == null)
                return Json(null);

            var model = new AcademicMajorViewModel
            {
                MajorId = entity.MajorId,
                FacultyId = entity.FacultyId, // ضروري لتحديد الكلية في المودال
                MajorCode = entity.MajorCode,
                MajorNameAr = entity.MajorNameAr,
                MajorNameEn = entity.MajorNameEn
            };

            return Json(model);
        }
    }
}