using HRDS.Web.Areas.Config.Models; // استبدل بـ namespace المباشر للـ ViewModel والـ DBContext الخاص بك
using HRDS.Web.Models.Entities;
using HRDS.Web.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRDS.Web.Areas.Config.Controllers
{
    [Area("Config")]
    [HasModuleAccess("Config")]
    public class GovernoratesController : Controller
    {
        private readonly HRDSContext _context; // استبدل باسم الـ DbContext لديك

        public GovernoratesController(HRDSContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetGovernoratesJson()
        {
            var governorates = await _context.Governorates
                .Include(g => g.Country)
                .Select(g => new GovernorateViewModel
                {
                    GovernorateId = g.GovernorateId,
                    CountryId = g.CountryId,
                    GovernorateCode = g.GovernorateCode,
                    GovernorateNameAr = g.GovernorateNameAr,
                    GovernorateNameEn = g.GovernorateNameEn,
                    CountryNameAr = g.Country.CountryNameAr,
                    CountryNameEn = g.Country.CountryNameEn,
                    IsActive = g.IsActive,
                    SortOrder = g.SortOrder
                })
                .OrderBy(g => g.SortOrder)
                .ToListAsync();

            return Json(new { data = governorates });
        }

        [HttpGet]
        public async Task<IActionResult> GetCountriesLookup()
        {
            var countries = await _context.Countries
                .Where(c => c.IsActive)
                .OrderBy(c => c.SortOrder)
                .Select(c => new CountryLookupDto
                {
                    CountryId = c.CountryId,
                    CountryNameAr = c.CountryNameAr,
                    CountryNameEn = c.CountryNameEn
                })
                .ToListAsync();

            return Json(new { success = true, data = countries });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _context.Governorates.FindAsync(id);
            if (item == null)
            {
                return Json(new { success = false, message = "المحافظة غير موجودة" });
            }

            var dto = new GovernorateViewModel
            {
                GovernorateId = item.GovernorateId,
                CountryId = item.CountryId,
                GovernorateCode = item.GovernorateCode,
                GovernorateNameAr = item.GovernorateNameAr,
                GovernorateNameEn = item.GovernorateNameEn,
                IsActive = item.IsActive,
                SortOrder = item.SortOrder
            };

            return Json(new { success = true, data = dto });
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] GovernorateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "البيانات المدخلة غير صحيحة" });
            }

            if (model.GovernorateId == 0)
            {
                // Add New Governorate
                var entity = new Governorate // استبدل بـ Entity الخاصة بجدول المحافظات
                {
                    CountryId = model.CountryId,
                    GovernorateCode = model.GovernorateCode,
                    GovernorateNameAr = model.GovernorateNameAr,
                    GovernorateNameEn = model.GovernorateNameEn,
                    IsActive = model.IsActive,
                    SortOrder = model.SortOrder
                };

                _context.Governorates.Add(entity);
            }
            else
            {
                // Update Existing Governorate
                var entity = await _context.Governorates.FindAsync(model.GovernorateId);
                if (entity == null)
                {
                    return Json(new { success = false, message = "المحافظة غير موجودة" });
                }

                entity.CountryId = model.CountryId;
                entity.GovernorateCode = model.GovernorateCode;
                entity.GovernorateNameAr = model.GovernorateNameAr;
                entity.GovernorateNameEn = model.GovernorateNameEn;
                entity.IsActive = model.IsActive;
                entity.SortOrder = model.SortOrder;

                _context.Governorates.Update(entity);
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "تم الحفظ بنجاح" });
        }
    }
}