using HRDS.Web.Areas.Config.ViewModels;
using HRDS.Web.Models.Entities;
using HRDS.Web.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRDS.Web.Areas.Config.Controllers
{
    [Area("Config")]
    [HasModuleAccess("Config")]
    public class CountriesController : Controller
    {
        private readonly HRDSContext _context;

        public CountriesController(HRDSContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetCountriesJson()
        {
            var countriesList = await _context.Countries
                .AsNoTracking()
                .Select(c => new CountriesViewModel
                {
                    CountryId = c.CountryId,
                    CountryCode2 = c.CountryCode2,
                    CountryCode3 = c.CountryCode3,
                    CountryNameAr = c.CountryNameAr,
                    CountryNameEn = c.CountryNameEn,
                    IsActive = c.IsActive,
                    SortOrder = c.SortOrder
                })
                .OrderBy(c => c.SortOrder)
                .ToListAsync();

            return Json(new { data = countriesList });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var country = await _context.Countries.FindAsync(id);
            if (country == null)
                return NotFound(new { success = false, message = "الدولة غير موجودة" });

            var viewModel = new CountriesViewModel
            {
                CountryId = country.CountryId,
                CountryCode2 = country.CountryCode2,
                CountryCode3 = country.CountryCode3,
                CountryNameAr = country.CountryNameAr,
                CountryNameEn = country.CountryNameEn,
                IsActive = country.IsActive,
                SortOrder = country.SortOrder
            };

            return Json(new { success = true, data = viewModel });
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] CountriesViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, message = "برجاء التأكد من إدخال البيانات بشكل صحيح." });

            if (model.CountryId == 0) // إضافة جديد
            {
                var country = new Country
                {
                    CountryCode2 = model.CountryCode2,
                    CountryCode3 = model.CountryCode3,
                    CountryNameAr = model.CountryNameAr,
                    CountryNameEn = model.CountryNameEn,
                    IsActive = model.IsActive,
                    SortOrder = model.SortOrder
                };

                _context.Countries.Add(country);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "تم إضاقة الدولة بنجاح!" });
            }
            else // تعديل
            {
                var country = await _context.Countries.FindAsync(model.CountryId);
                if (country == null)
                    return Json(new { success = false, message = "عفواً، لم يتم العثور على الدولة!" });

                country.CountryCode2 = model.CountryCode2;
                country.CountryCode3 = model.CountryCode3;
                country.CountryNameAr = model.CountryNameAr;
                country.CountryNameEn = model.CountryNameEn;
                country.IsActive = model.IsActive;
                country.SortOrder = model.SortOrder;

                _context.Countries.Update(country);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "تم تعديل بيانات الدولة بنجاح!" });
            }
        }
    }
}