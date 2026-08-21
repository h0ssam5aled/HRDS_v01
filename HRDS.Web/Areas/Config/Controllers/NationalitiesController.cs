using HRDS.Web.Areas.Config.Models;
using HRDS.Web.Models.Entities;
using HRDS.Web.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRDS.Web.Areas.Config.Controllers
{
    [Area("Config")]
    [HasModuleAccess("Config")]
    public class NationalitiesController : Controller
    {
        private readonly HRDSContext _context;

        public NationalitiesController(HRDSContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetNationalitiesJson()
        {
            var data = await _context.Nationalities
                .Select(n => new NationalityViewModel
                {
                    NationalityId = n.NationalityId,
                    NationalityCode = n.NationalityCode,
                    NationalityNameAr = n.NationalityNameAr,
                    NationalityNameEn = n.NationalityNameEn,
                    IsActive = n.IsActive,
                    SortOrder = n.SortOrder
                })
                .OrderBy(n => n.SortOrder)
                .ToListAsync();

            return Json(new { data });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _context.Nationalities.FindAsync(id);
            if (item == null)
            {
                return Json(new { success = false, message = "الجنسية غير موجودة" });
            }

            var dto = new NationalityViewModel
            {
                NationalityId = item.NationalityId,
                NationalityCode = item.NationalityCode,
                NationalityNameAr = item.NationalityNameAr,
                NationalityNameEn = item.NationalityNameEn,
                IsActive = item.IsActive,
                SortOrder = item.SortOrder
            };

            return Json(new { success = true, data = dto });
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] NationalityViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "البيانات المدخلة غير صحيحة" });
            }

            if (model.NationalityId == 0)
            {
                var entity = new Nationality
                {
                    NationalityCode = model.NationalityCode,
                    NationalityNameAr = model.NationalityNameAr,
                    NationalityNameEn = model.NationalityNameEn,
                    IsActive = model.IsActive,
                    SortOrder = model.SortOrder
                };

                _context.Nationalities.Add(entity);
            }
            else
            {
                var entity = await _context.Nationalities.FindAsync(model.NationalityId);
                if (entity == null)
                {
                    return Json(new { success = false, message = "الجنسية غير موجودة" });
                }

                entity.NationalityCode = model.NationalityCode;
                entity.NationalityNameAr = model.NationalityNameAr;
                entity.NationalityNameEn = model.NationalityNameEn;
                entity.IsActive = model.IsActive;
                entity.SortOrder = model.SortOrder;

                _context.Nationalities.Update(entity);
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "تم الحفظ بنجاح" });
        }
    }
}