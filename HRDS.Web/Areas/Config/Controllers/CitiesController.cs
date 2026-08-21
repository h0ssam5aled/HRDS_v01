using HRDS.Web.Areas.Config.Models;
using HRDS.Web.Models.Entities;
using HRDS.Web.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRDS.Web.Areas.Config.Controllers
{
    [Area("Config")]
    [HasModuleAccess("Config")]
    public class CitiesController : Controller
    {
        private readonly HRDSContext _context;

        public CitiesController(HRDSContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetCitiesJson()
        {
            var cities = await _context.Cities
                .Include(c => c.Governorate)
                .Select(c => new CityViewModel
                {
                    CityId = c.CityId,
                    GovernorateId = c.GovernorateId,
                    CityCode = c.CityCode,
                    CityNameAr = c.CityNameAr,
                    CityNameEn = c.CityNameEn,
                    GovernorateNameAr = c.Governorate.GovernorateNameAr,
                    GovernorateNameEn = c.Governorate.GovernorateNameEn,
                    IsActive = c.IsActive,
                    SortOrder = c.SortOrder
                })
                .OrderBy(c => c.SortOrder)
                .ToListAsync();

            return Json(new { data = cities });
        }

        [HttpGet]
        public async Task<IActionResult> GetGovernoratesLookup()
        {
            var governorates = await _context.Governorates
                .Where(g => g.IsActive)
                .OrderBy(g => g.SortOrder)
                .Select(g => new GovernorateLookupDto
                {
                    GovernorateId = g.GovernorateId,
                    GovernorateNameAr = g.GovernorateNameAr,
                    GovernorateNameEn = g.GovernorateNameEn
                })
                .ToListAsync();

            return Json(new { success = true, data = governorates });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _context.Cities.FindAsync(id);
            if (item == null)
            {
                return Json(new { success = false, message = "المدينة غير موجودة" });
            }

            var dto = new CityViewModel
            {
                CityId = item.CityId,
                GovernorateId = item.GovernorateId,
                CityCode = item.CityCode,
                CityNameAr = item.CityNameAr,
                CityNameEn = item.CityNameEn,
                IsActive = item.IsActive,
                SortOrder = item.SortOrder
            };

            return Json(new { success = true, data = dto });
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] CityViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "البيانات المدخلة غير صحيحة" });
            }

            if (model.CityId == 0)
            {
                var entity = new City
                {
                    GovernorateId = model.GovernorateId,
                    CityCode = model.CityCode,
                    CityNameAr = model.CityNameAr,
                    CityNameEn = model.CityNameEn,
                    IsActive = model.IsActive,
                    SortOrder = model.SortOrder
                };

                _context.Cities.Add(entity);
            }
            else
            {
                var entity = await _context.Cities.FindAsync(model.CityId);
                if (entity == null)
                {
                    return Json(new { success = false, message = "المدينة غير موجودة" });
                }

                entity.GovernorateId = model.GovernorateId;
                entity.CityCode = model.CityCode;
                entity.CityNameAr = model.CityNameAr;
                entity.CityNameEn = model.CityNameEn;
                entity.IsActive = model.IsActive;
                entity.SortOrder = model.SortOrder;

                _context.Cities.Update(entity);
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "تم الحفظ بنجاح" });
        }
    }
}