using System.Globalization;
using HRDS.Web.Areas.Finance.ViewModels;
using HRDS.Web.Models;
using HRDS.Web.Models.Entities;
using HRDS.Web.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRDS.Web.Controllers
{
    [Authorize]
    [Area("FINCONFIG")]
    [HasModuleAccess("FINCONFIG")]
    public class CurrenciesController : Controller
    {
        private readonly HRDSContext _context;

        public CurrenciesController(HRDSContext context)
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
            var isEn = CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToLower() == "en";

            var rawData = await _context.Currencies
                .OrderByDescending(x => x.CurrencyId)
                .ToListAsync();

            var list = rawData.Select(x => new CurrencyViewModel
            {
                CurrencyId = x.CurrencyId,
                CurrencyCode = x.CurrencyCode,
                CurrencyNameAr = x.CurrencyNameAr,
                CurrencyNameEn = x.CurrencyNameEn,
                DisplayCurrencyName = isEn && !string.IsNullOrEmpty(x.CurrencyNameEn) ? x.CurrencyNameEn : x.CurrencyNameAr,
                Symbol = x.Symbol,
                Description = x.Description,
                IsBaseCurrency = x.IsBaseCurrency,
                IsActive = x.IsActive
            }).ToList();

            return Json(new { data = list });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var entity = await _context.Currencies.FirstOrDefaultAsync(x => x.CurrencyId == id);
            if (entity == null) return Json(null);

            var model = new CurrencyViewModel
            {
                CurrencyId = entity.CurrencyId,
                CurrencyCode = entity.CurrencyCode,
                CurrencyNameAr = entity.CurrencyNameAr,
                CurrencyNameEn = entity.CurrencyNameEn,
                Symbol = entity.Symbol,
                Description = entity.Description,
                IsBaseCurrency = entity.IsBaseCurrency,
                IsActive = entity.IsActive
            };

            return Json(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save([FromBody] CurrencyViewModel model)
        {
            if (!ModelState.IsValid) return Json(new { success = false, message = "Invalid Data" });

            // إذا تم اختيار هذه العملة كعملة أساسية، يتم إلغاء تعيين أي عملة أساسية أخرى
            if (model.IsBaseCurrency)
            {
                var existingBaseCurrencies = await _context.Currencies
                    .Where(x => x.IsBaseCurrency && x.CurrencyId != model.CurrencyId)
                    .ToListAsync();

                foreach (var baseCurr in existingBaseCurrencies)
                {
                    baseCurr.IsBaseCurrency = false;
                }
            }

            if (model.CurrencyId == 0)
            {
                var entity = new Currency
                {
                    CurrencyCode = model.CurrencyCode.ToUpper().Trim(),
                    CurrencyNameAr = model.CurrencyNameAr.Trim(),
                    CurrencyNameEn = model.CurrencyNameEn?.Trim(),
                    Symbol = model.Symbol?.Trim(),
                    Description = model.Description?.Trim(),
                    IsBaseCurrency = model.IsBaseCurrency,
                    IsActive = model.IsActive,
                    CreatedDate = DateTime.Now
                };
                _context.Currencies.Add(entity);
            }
            else
            {
                var entity = await _context.Currencies.FirstOrDefaultAsync(x => x.CurrencyId == model.CurrencyId);
                if (entity == null) return Json(new { success = false, message = "Record Not Found" });

                entity.CurrencyCode = model.CurrencyCode.ToUpper().Trim();
                entity.CurrencyNameAr = model.CurrencyNameAr.Trim();
                entity.CurrencyNameEn = model.CurrencyNameEn?.Trim();
                entity.Symbol = model.Symbol?.Trim();
                entity.Description = model.Description?.Trim();
                entity.IsBaseCurrency = model.IsBaseCurrency;
                entity.IsActive = model.IsActive;
                entity.ModifiedDate = DateTime.Now;
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Saved Successfully" });
        }
    }
}