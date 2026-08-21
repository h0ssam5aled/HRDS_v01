using HRDS.Web.Models;
using HRDS.Web.Models.Entities;
using HRDS.Web.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HRDS.Web.Areas.CoreData.Controllers
{
    [Area("CoreData")]
    [HasModuleAccess("Core")]
    public class CompaniesController : Controller
    {
        private readonly HRDSContext _context;

        public CompaniesController(HRDSContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var companies = await _context.Companies.ToListAsync();
            return View(companies);
        }

        public IActionResult Create()
        {
            var isArabic = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "ar";

            ViewBag.Countries = new SelectList(
                _context.Countries,
                "CountryId",
                isArabic ? "CountryNameAr" : "CountryNameEn"
            );
            ViewBag.Governorates = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.Cities = new SelectList(Enumerable.Empty<SelectListItem>());
            return View();
        }

        [HttpGet]
        public IActionResult GetGovernorates(int countryId)
        {
            var isArabic = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "ar";

            var governorates = _context.Governorates
                .Where(g => g.CountryId == countryId)
                .Select(g => new {
                    value = g.GovernorateId,
                    text = isArabic ? g.GovernorateNameAr : g.GovernorateNameEn
                })
                .ToList();

            return Json(governorates);
        }

        [HttpGet]
        public IActionResult GetCities(int governorateId)
        {
            var isArabic = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "ar";

            var cities = _context.Cities
                .Where(c => c.GovernorateId == governorateId)
                .Select(c => new {
                    value = c.CityId,
                    text = isArabic ? c.CityNameAr : c.CityNameEn
                })
                .ToList();

            return Json(cities);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Company model)
        {
            // إزالة التحقق من علاقات الكيانات لأن القيمة القادمة من الفورم هي المعرفات فقط
            ModelState.Remove("Country");
            ModelState.Remove("Governorate");
            ModelState.Remove("City");

            if (ModelState.IsValid)
            {
                _context.Companies.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // إعادة تحميل القوائم في حال وجود خطأ آخر
            var isArabic = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "ar";
            ViewBag.Countries = new SelectList(_context.Countries, "CountryId", isArabic ? "CountryNameAr" : "CountryNameEn", model.CountryId);
            ViewBag.Governorates = new SelectList(_context.Governorates.Where(g => g.CountryId == model.CountryId), "GovernorateId", isArabic ? "GovernorateNameAr" : "GovernorateNameEn", model.GovernorateId);
            ViewBag.Cities = new SelectList(_context.Cities.Where(c => c.GovernorateId == model.GovernorateId), "CityId", isArabic ? "CityNameAr" : "CityNameEn", model.CityId);

            return View(model);
        }

        // GET: Companies/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var company = await _context.Companies.FindAsync(id);
            if (company == null) return NotFound();

            var isArabic = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "ar";

            ViewBag.Countries = new SelectList(_context.Countries, "CountryId", isArabic ? "CountryNameAr" : "CountryNameEn", company.CountryId);
            ViewBag.Governorates = new SelectList(_context.Governorates.Where(g => g.CountryId == company.CountryId), "GovernorateId", isArabic ? "GovernorateNameAr" : "GovernorateNameEn", company.GovernorateId);
            ViewBag.Cities = new SelectList(_context.Cities.Where(c => c.GovernorateId == company.GovernorateId), "CityId", isArabic ? "CityNameAr" : "CityNameEn", company.CityId);

            return View(company);
        }

        // POST: Companies/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Company model)
        {
            if (id != model.CompanyId) return NotFound();

            // إزالة التحقق من علاقات الكيانات لتجنب خطأ الـ Null
            ModelState.Remove("Country");
            ModelState.Remove("Governorate");
            ModelState.Remove("City");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(model);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Companies.Any(e => e.CompanyId == model.CompanyId))
                        return NotFound();
                    else
                        throw;
                }
            }

            var isArabic = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "ar";

            ViewBag.Countries = new SelectList(_context.Countries, "CountryId", isArabic ? "CountryNameAr" : "CountryNameEn", model.CountryId);
            ViewBag.Governorates = new SelectList(_context.Governorates.Where(g => g.CountryId == model.CountryId), "GovernorateId", isArabic ? "GovernorateNameAr" : "GovernorateNameEn", model.GovernorateId);
            ViewBag.Cities = new SelectList(_context.Cities.Where(c => c.GovernorateId == model.GovernorateId), "CityId", isArabic ? "CityNameAr" : "CityNameEn", model.CityId);

            return View(model);
        }
    }
}