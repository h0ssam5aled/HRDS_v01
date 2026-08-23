using HRDS.Web.Areas.HRConfig.ViewModels;
using HRDS.Web.Models;
using HRDS.Web.Models.Entities;
using HRDS.Web.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace HRDS.Web.Areas.HRConfig.Controllers
{
    [Authorize]
    [Area("HRConfig")]
    [HasModuleAccess("HRConfig")]
    public class SectionsController : Controller
    {
        private readonly HRDSContext _context;

        public SectionsController(HRDSContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var currentCulture = CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToLower();

            var departments = await _context.Departments
                .Where(d => !d.IsDeleted && d.IsActive)
                .Select(d => new SelectListItem
                {
                    Value = d.DepartmentId.ToString(),
                    // عرض الاسم الإنجليزي إذا كانت اللغة إنجليزي، وإلا العربي
                    Text = currentCulture == "en" && !string.IsNullOrEmpty(d.DepartmentNameEn)
                           ? d.DepartmentNameEn
                           : d.DepartmentNameAr
                })
                .ToListAsync();

            ViewBag.Departments = departments;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetJson()
        {
            var list = await _context.Sections
                .Include(s => s.Department)
                .Where(x => !x.IsDeleted)
                .Select(x => new SectionViewModel
                {
                    SectionId = x.SectionId,
                    SectionCode = x.SectionCode,
                    SectionNameAr = x.SectionNameAr,
                    SectionNameEn = x.SectionNameEn,
                    DepartmentId = x.DepartmentId,
                    DepartmentNameAr = x.Department != null ? x.Department.DepartmentNameAr : "-",
                    DepartmentNameEn = x.Department != null ? x.Department.DepartmentNameEn : "-", // أضف هذا السطر
                    IsActive = x.IsActive
                })
                .ToListAsync();

            return Json(new { data = list });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var entity = await _context.Sections.FirstOrDefaultAsync(x => x.SectionId == id && !x.IsDeleted);
            if (entity == null) return Json(null);

            var model = new SectionViewModel
            {
                SectionId = entity.SectionId,
                SectionCode = entity.SectionCode,
                SectionNameAr = entity.SectionNameAr,
                SectionNameEn = entity.SectionNameEn,
                DepartmentId = entity.DepartmentId,
                IsActive = entity.IsActive
            };

            return Json(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save([FromBody] SectionViewModel model)
        {
            if (!ModelState.IsValid) return Json(new { success = false, message = "بيانات غير صالحة" });

            if (model.SectionId == 0)
            {
                var entity = new Section
                {
                    SectionCode = model.SectionCode.ToUpper().Trim(),
                    SectionNameAr = model.SectionNameAr.Trim(),
                    SectionNameEn = model.SectionNameEn?.Trim(),
                    DepartmentId = model.DepartmentId,
                    IsActive = model.IsActive,
                    CreatedAt = DateTime.Now,
                    IsDeleted = false
                };
                _context.Sections.Add(entity);
            }
            else
            {
                var entity = await _context.Sections.FirstOrDefaultAsync(x => x.SectionId == model.SectionId && !x.IsDeleted);
                if (entity == null) return Json(new { success = false, message = "السجل غير موجود" });

                entity.SectionCode = model.SectionCode.ToUpper().Trim();
                entity.SectionNameAr = model.SectionNameAr.Trim();
                entity.SectionNameEn = model.SectionNameEn?.Trim();
                entity.DepartmentId = model.DepartmentId;
                entity.IsActive = model.IsActive;
                entity.UpdatedAt = DateTime.Now;
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "تم الحفظ بنجاح" });
        }
    }
}