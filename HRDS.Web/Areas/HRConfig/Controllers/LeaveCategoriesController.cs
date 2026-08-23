using HRDS.Web.Models.Entities;
using HRDS.Web.Security;
using HRDS.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRDS.Web.Controllers
{
    [Authorize]
    [Area("HRConfig")]
    [HasModuleAccess("HRConfig")]
    public class LeaveCategoriesController : Controller
    {
        private readonly HRDSContext _context;

        public LeaveCategoriesController(HRDSContext context)
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
            var data = await _context.LeaveCategories
                .Where(c => !c.IsDeleted)
                .Select(c => new LeaveCategoryViewModel
                {
                    LeaveCategoryId = c.LeaveCategoryId,
                    LeaveCategoryCode = c.LeaveCategoryCode,
                    LeaveCategoryNameAr = c.LeaveCategoryNameAr,
                    LeaveCategoryNameEn = c.LeaveCategoryNameEn,
                    Description = c.Description,
                    IsActive = c.IsActive
                })
                .ToListAsync();

            return Json(new { data });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _context.LeaveCategories
                .Where(c => c.LeaveCategoryId == id && !c.IsDeleted)
                .Select(c => new LeaveCategoryViewModel
                {
                    LeaveCategoryId = c.LeaveCategoryId,
                    LeaveCategoryCode = c.LeaveCategoryCode,
                    LeaveCategoryNameAr = c.LeaveCategoryNameAr,
                    LeaveCategoryNameEn = c.LeaveCategoryNameEn,
                    Description = c.Description,
                    IsActive = c.IsActive
                })
                .FirstOrDefaultAsync();

            if (item == null) return NotFound();
            return Json(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save([FromBody] LeaveCategoryViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, message = "بيانات غير صالحة" });

            if (model.LeaveCategoryId == 0)
            {
                var entity = new LeaveCategory
                {
                    LeaveCategoryCode = model.LeaveCategoryCode,
                    LeaveCategoryNameAr = model.LeaveCategoryNameAr,
                    LeaveCategoryNameEn = model.LeaveCategoryNameEn,
                    Description = model.Description,
                    IsActive = model.IsActive,
                    CreatedAt = DateTime.Now,
                    IsDeleted = false
                };

                _context.LeaveCategories.Add(entity);
            }
            else
            {
                var existing = await _context.LeaveCategories.FindAsync(model.LeaveCategoryId);
                if (existing == null || existing.IsDeleted)
                    return Json(new { success = false, message = "غير موجود" });

                existing.LeaveCategoryCode = model.LeaveCategoryCode;
                existing.LeaveCategoryNameAr = model.LeaveCategoryNameAr;
                existing.LeaveCategoryNameEn = model.LeaveCategoryNameEn;
                existing.Description = model.Description;
                existing.IsActive = model.IsActive;
                existing.UpdatedAt = DateTime.Now;
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }
    }
}