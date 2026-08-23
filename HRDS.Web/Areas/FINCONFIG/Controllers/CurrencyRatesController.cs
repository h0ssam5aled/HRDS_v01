using System.Globalization;
using System.Net.Http.Json;
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
    public class CurrencyRatesController : Controller
    {
        private readonly HRDSContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<CurrencyRatesController> _logger; // أضف هذا السطر

        public CurrencyRatesController(
         HRDSContext context,
         IHttpClientFactory httpClientFactory,
         ILogger<CurrencyRatesController> logger) // حقن الـ Logger هنا
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetJson()
        {
            var isEn = CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToLower() == "en";

            // 1. إجراء التحديث التلقائي لأسعار اليوم في الخلفية
            await AutoSyncTodayRatesAsync();

            // 2. استرجاع البيانات للـ DataTables
            var rawData = await _context.CurrencyRates
                .Include(x => x.Currency)
                .Include(x => x.BaseCurrency)
                .OrderByDescending(x => x.RateDate)
                .ThenByDescending(x => x.CurrencyRateId)
                .ToListAsync();

            var list = rawData.Select(x => new CurrencyRateViewModel
            {
                CurrencyRateId = x.CurrencyRateId,
                CurrencyId = x.CurrencyId,
                CurrencyName = isEn && !string.IsNullOrEmpty(x.Currency?.CurrencyNameEn) ? x.Currency.CurrencyNameEn : x.Currency?.CurrencyNameAr,
                BaseCurrencyId = x.BaseCurrencyId,
                BaseCurrencyName = isEn && !string.IsNullOrEmpty(x.BaseCurrency?.CurrencyNameEn) ? x.BaseCurrency.CurrencyNameEn : x.BaseCurrency?.CurrencyNameAr,
                ExchangeRate = x.ExchangeRate,
                RateDate = x.RateDate.ToString("yyyy-MM-dd"),
                IsActive = x.IsActive
            }).ToList();

            return Json(new { data = list });
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrenciesDropdown()
        {
            var isEn = CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToLower() == "en";

            var currencies = await _context.Currencies
                .Where(x => x.IsActive)
                .Select(x => new
                {
                    id = x.CurrencyId,
                    text = isEn && !string.IsNullOrEmpty(x.CurrencyNameEn)
                        ? $"{x.CurrencyNameEn} ({x.CurrencyCode})"
                        : $"{x.CurrencyNameAr} ({x.CurrencyCode})"
                })
                .ToListAsync();

            return Json(currencies);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var rate = await _context.CurrencyRates.FindAsync(id);
            if (rate == null)
            {
                return Json(new { success = false, message = "Record not found" });
            }

            var model = new CurrencyRateViewModel
            {
                CurrencyRateId = rate.CurrencyRateId,
                CurrencyId = rate.CurrencyId,
                BaseCurrencyId = rate.BaseCurrencyId,
                ExchangeRate = rate.ExchangeRate,
                RateDate = rate.RateDate.ToString("yyyy-MM-dd"),
                IsActive = rate.IsActive
            };

            return Json(new { success = true, data = model });
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] CurrencyRateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return Json(new { success = false, message = string.Join("<br/>", errors) });
            }

            if (model.CurrencyId == model.BaseCurrencyId)
            {
                return Json(new { success = false, message = "Currency and Base Currency cannot be the same." });
            }

            if (!DateTime.TryParse(model.RateDate, out DateTime parsedDate))
            {
                return Json(new { success = false, message = "Invalid Rate Date format." });
            }

            if (model.CurrencyRateId == 0)
            {
                // إضافة سجل جديد
                var exists = await _context.CurrencyRates
                    .AnyAsync(x => x.CurrencyId == model.CurrencyId && x.BaseCurrencyId == model.BaseCurrencyId && x.RateDate.Date == parsedDate.Date);

                if (exists)
                {
                    return Json(new { success = false, message = "A rate for this currency pair and date already exists." });
                }

                var entity = new CurrencyRate
                {
                    CurrencyId = model.CurrencyId,
                    BaseCurrencyId = model.BaseCurrencyId,
                    ExchangeRate = model.ExchangeRate,
                    RateDate = parsedDate,
                    IsActive = model.IsActive,
                    CreatedDate = DateTime.Now
                };

                _context.CurrencyRates.Add(entity);
            }
            else
            {
                // تعديل سجل قائم
                var entity = await _context.CurrencyRates.FindAsync(model.CurrencyRateId);
                if (entity == null)
                {
                    return Json(new { success = false, message = "Record not found." });
                }

                entity.CurrencyId = model.CurrencyId;
                entity.BaseCurrencyId = model.BaseCurrencyId;
                entity.ExchangeRate = model.ExchangeRate;
                entity.RateDate = parsedDate;
                entity.IsActive = model.IsActive;
                entity.ModifiedDate = DateTime.Now;

                _context.CurrencyRates.Update(entity);
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Saved successfully." });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _context.CurrencyRates.FindAsync(id);
            if (entity == null)
            {
                return Json(new { success = false, message = "Record not found." });
            }

            _context.CurrencyRates.Remove(entity);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Deleted successfully." });
        }

        private async Task AutoSyncTodayRatesAsync()
        {
            try
            {
                var today = DateTime.Today;

                // 1. تحديد العملة الأساسية (EGP)
                var baseCurrency = await _context.Currencies.FirstOrDefaultAsync(x => x.IsBaseCurrency && x.IsActive)
                                   ?? await _context.Currencies.FirstOrDefaultAsync(x => x.CurrencyCode == "EGP" && x.IsActive);

                if (baseCurrency == null) return;

                // 2. جلب جميع العملات النشطة المخزنة في النظام (مثل USD, EUR, GBP)
                var activeCurrencies = await _context.Currencies
                    .Where(x => x.IsActive && x.CurrencyId != baseCurrency.CurrencyId)
                    .ToListAsync();

                if (!activeCurrencies.Any()) return;

                // 3. تحديد العملات التي ليس لها سعر مضاف/محدث بتاريخ اليوم
                var syncedCurrencyIdsToday = await _context.CurrencyRates
                    .Where(x => x.BaseCurrencyId == baseCurrency.CurrencyId && x.RateDate.Date == today)
                    .Select(x => x.CurrencyId)
                    .ToListAsync();

                // العملات المتبقية التي تحتاج تحديث اليوم (مثلاً GBP فقط)
                var currenciesToSync = activeCurrencies
                    .Where(c => !syncedCurrencyIdsToday.Contains(c.CurrencyId))
                    .ToList();

                // إذا كانت كل العملات محينة بالفعل لليوم، اخرج فوراً
                if (!currenciesToSync.Any()) return;

                // 4. الاتصال بالـ API لجلب الأسعار للعملات الناقصة فقط
                var client = _httpClientFactory.CreateClient();
                client.Timeout = TimeSpan.FromSeconds(5);

                var url = $"https://open.er-api.com/v6/latest/{baseCurrency.CurrencyCode}";
                var response = await client.GetFromJsonAsync<ExchangeRateApiResponse>(url);

                if (response != null && response.Rates != null)
                {
                    foreach (var targetCurrency in currenciesToSync)
                    {
                        if (response.Rates.TryGetValue(targetCurrency.CurrencyCode, out var latestRate) && latestRate > 0)
                        {
                            decimal calculatedRate = (decimal)(1.0 / latestRate);

                            _context.CurrencyRates.Add(new CurrencyRate
                            {
                                CurrencyId = targetCurrency.CurrencyId,
                                BaseCurrencyId = baseCurrency.CurrencyId,
                                ExchangeRate = calculatedRate,
                                RateDate = today,
                                IsActive = true,
                                CreatedDate = DateTime.Now,
                                ModifiedDate = DateTime.Now
                            });
                        }
                    }

                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to auto-sync exchange rates from external API.");
                _context.ChangeTracker.Clear();
            }
        }
    }

    public class ExchangeRateApiResponse
    {
        public string Result { get; set; } = null!;
        public Dictionary<string, double> Rates { get; set; } = new();
    }
}