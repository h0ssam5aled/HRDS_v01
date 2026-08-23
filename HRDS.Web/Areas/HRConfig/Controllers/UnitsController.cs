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
    public class UnitsController : Controller
    {
        private readonly HRDSContext _context;

        public UnitsController(HRDSContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var currentCulture = CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToLower();

            ViewBag.Sections = new SelectList(
                await _context.Sections.Where(s => !s.IsDeleted && s.IsActive).Select(s => new {
                    s.SectionId,
                    SectionName = currentCulture == "en" && !string.IsNullOrEmpty(s.SectionNameEn)
                                  ? s.SectionNameEn
                                  : s.SectionNameAr
                }).ToListAsync(),
                "SectionId",
                "SectionName"
            );
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetJson()
        {
            var units = await _context.Units
                .Include(u => u.Section)
                .Where(u => !u.IsDeleted)
                .Select(u => new
                {
                    u.UnitId,
                    u.UnitCode,
                    u.UnitNameAr,
                    u.UnitNameEn,
                    u.DisplayOrder,
                    u.IsActive,
                    SectionNameAr = u.Section != null ? u.Section.SectionNameAr : "",
                    SectionNameEn = u.Section != null ? u.Section.SectionNameEn : ""
                })
                .ToListAsync();

            return Json(new { data = units });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var entity = await _context.Units.FirstOrDefaultAsync(x => x.UnitId == id && !x.IsDeleted);
            if (entity == null) return Json(null);

            var model = new UnitViewModel
            {
                UnitId = entity.UnitId,
                SectionId = entity.SectionId,
                UnitCode = entity.UnitCode,
                UnitNameAr = entity.UnitNameAr,
                UnitNameEn = entity.UnitNameEn,
                DisplayOrder = entity.DisplayOrder,
                Description = entity.Description,
                IsActive = entity.IsActive
            };

            return Json(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save([FromBody] UnitViewModel model)
        {
            if (!ModelState.IsValid) return Json(new { success = false, message = "بيانات غير صالحة" });

            if (model.UnitId == 0)
            {
                var entity = new Unit
                {
                    SectionId = model.SectionId,
                    UnitCode = model.UnitCode.ToUpper().Trim(),
                    UnitNameAr = model.UnitNameAr.Trim(),
                    UnitNameEn = model.UnitNameEn?.Trim(),
                    DisplayOrder = model.DisplayOrder,
                    Description = model.Description?.Trim(),
                    IsActive = model.IsActive,
                    CreatedAt = DateTime.Now,
                    IsDeleted = false
                };
                _context.Units.Add(entity);
            }
            else
            {
                var entity = await _context.Units.FirstOrDefaultAsync(x => x.UnitId == model.UnitId && !x.IsDeleted);
                if (entity == null) return Json(new { success = false, message = "السجل غير موجود" });

                entity.SectionId = model.SectionId;
                entity.UnitCode = model.UnitCode.ToUpper().Trim();
                entity.UnitNameAr = model.UnitNameAr.Trim();
                entity.UnitNameEn = model.UnitNameEn?.Trim();
                entity.DisplayOrder = model.DisplayOrder;
                entity.Description = model.Description?.Trim();
                entity.IsActive = model.IsActive;
                entity.UpdatedAt = DateTime.Now;
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "تم الحفظ بنجاح" });
        }
    }
}