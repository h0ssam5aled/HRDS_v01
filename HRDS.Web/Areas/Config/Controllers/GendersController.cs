using HRDS.Web.Areas.Config.Models;
using HRDS.Web.Models.Entities;
using HRDS.Web.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRDS.Web.Areas.Config.Controllers
{
    [Area("Config")]
    [HasModuleAccess("Config")]
    public class GendersController : Controller
    {
        private readonly HRDSContext _context;

        public GendersController(HRDSContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetGendersJson()
        {
            var data = await _context.Genders
                .Select(g => new GenderViewModel
                {
                    GenderId = g.GenderId,
                    GenderCode = g.GenderCode,
                    GenderNameAr = g.GenderNameAr,
                    GenderNameEn = g.GenderNameEn,
                    IsActive = g.IsActive,
                    SortOrder = g.SortOrder
                })
                .OrderBy(g => g.SortOrder)
                .ToListAsync();

            return Json(new { data });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _context.Genders.FindAsync(id);
            if (item == null)
            {
                return Json(new { success = false, message = "النوع غير موجود" });
            }

            var dto = new GenderViewModel
            {
                GenderId = item.GenderId,
                GenderCode = item.GenderCode,
                GenderNameAr = item.GenderNameAr,
                GenderNameEn = item.GenderNameEn,
                IsActive = item.IsActive,
                SortOrder = item.SortOrder
            };

            return Json(new { success = true, data = dto });
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] GenderViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "البيانات المدخلة غير صحيحة" });
            }

            if (model.GenderId == 0)
            {
                var entity = new Gender
                {
                    GenderCode = model.GenderCode,
                    GenderNameAr = model.GenderNameAr,
                    GenderNameEn = model.GenderNameEn,
                    IsActive = model.IsActive,
                    SortOrder = model.SortOrder
                };

                _context.Genders.Add(entity);
            }
            else
            {
                var entity = await _context.Genders.FindAsync(model.GenderId);
                if (entity == null)
                {
                    return Json(new { success = false, message = "النوع غير موجود" });
                }

                entity.GenderCode = model.GenderCode;
                entity.GenderNameAr = model.GenderNameAr;
                entity.GenderNameEn = model.GenderNameEn;
                entity.IsActive = model.IsActive;
                entity.SortOrder = model.SortOrder;

                _context.Genders.Update(entity);
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "تم الحفظ بنجاح" });
        }
    }
}