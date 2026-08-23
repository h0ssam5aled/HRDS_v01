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
    public class AcademicFacultiesController : Controller
    {
        private readonly HRDSContext _context;

        public AcademicFacultiesController(HRDSContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var isArabic = CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToLower() == "ar";
            ViewBag.Institutions = new SelectList(
                await _context.EducationalInstitutions.ToListAsync(),
                "InstitutionId",
                isArabic ? "InstitutionNameAr" : "InstitutionNameEn"
            );
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetJson()
        {
            var list = await _context.AcademicFaculties
                .Include(x => x.Institution)
                .Select(x => new AcademicFacultyViewModel
                {
                    FacultyId = x.FacultyId,
                    InstitutionId = x.InstitutionId,
                    InstitutionNameAr = x.Institution.InstitutionNameAr,
                    InstitutionNameEn = x.Institution.InstitutionNameEn,
                    FacultyCode = x.FacultyCode,
                    FacultyNameAr = x.FacultyNameAr,
                    FacultyNameEn = x.FacultyNameEn
                })
                .ToListAsync();

            return Json(new { data = list });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save([FromBody] AcademicFacultyViewModel model)
        {
            if (!ModelState.IsValid) return Json(new { success = false, message = "بيانات غير صالحة" });

            if (model.FacultyId == 0)
            {
                var entity = new AcademicFaculty
                {
                    InstitutionId = model.InstitutionId,
                    FacultyCode = model.FacultyCode.ToUpper().Trim(),
                    FacultyNameAr = model.FacultyNameAr.Trim(),
                    FacultyNameEn = model.FacultyNameEn?.Trim()
                };
                _context.AcademicFaculties.Add(entity);
            }
            else
            {
                var entity = await _context.AcademicFaculties.FindAsync(model.FacultyId);
                if (entity == null) return Json(new { success = false, message = "السجل غير موجود" });

                entity.InstitutionId = model.InstitutionId;
                entity.FacultyCode = model.FacultyCode.ToUpper().Trim();
                entity.FacultyNameAr = model.FacultyNameAr.Trim();
                entity.FacultyNameEn = model.FacultyNameEn?.Trim();
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "تم الحفظ بنجاح" });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var entity = await _context.AcademicFaculties.FindAsync(id);
            if (entity == null)
                return Json(null);

            var model = new AcademicFacultyViewModel
            {
                FacultyId = entity.FacultyId,
                InstitutionId = entity.InstitutionId, // تأكد من وجود هذا السطر
                FacultyCode = entity.FacultyCode,
                FacultyNameAr = entity.FacultyNameAr,
                FacultyNameEn = entity.FacultyNameEn
            };

            return Json(model);
        }
    }
}