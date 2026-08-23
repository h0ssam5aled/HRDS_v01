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
    public class BanksController : Controller
    {
        private readonly HRDSContext _context;

        public BanksController(HRDSContext context)
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

            var rawData = await _context.Banks
                .OrderByDescending(x => x.BankId)
                .ToListAsync();

            var list = rawData.Select(x => new BankViewModel
            {
                BankId = x.BankId,
                BankCode = x.BankCode,
                BankNameAr = x.BankNameAr,
                BankNameEn = x.BankNameEn,
                DisplayBankName = isEn && !string.IsNullOrEmpty(x.BankNameEn) ? x.BankNameEn : x.BankNameAr,
                SwiftCode = x.SwiftCode,
                Description = x.Description,
                IsActive = x.IsActive
            }).ToList();

            return Json(new { data = list });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var entity = await _context.Banks.FirstOrDefaultAsync(x => x.BankId == id);
            if (entity == null) return Json(null);

            var model = new BankViewModel
            {
                BankId = entity.BankId,
                BankCode = entity.BankCode,
                BankNameAr = entity.BankNameAr,
                BankNameEn = entity.BankNameEn,
                SwiftCode = entity.SwiftCode,
                Description = entity.Description,
                IsActive = entity.IsActive
            };

            return Json(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save([FromBody] BankViewModel model)
        {
            if (!ModelState.IsValid) return Json(new { success = false, message = "Invalid Data" });

            if (model.BankId == 0)
            {
                var entity = new Bank
                {
                    BankCode = model.BankCode.ToUpper().Trim(),
                    BankNameAr = model.BankNameAr.Trim(),
                    BankNameEn = model.BankNameEn?.Trim(),
                    SwiftCode = model.SwiftCode?.Trim(),
                    Description = model.Description?.Trim(),
                    IsActive = model.IsActive,
                    CreatedDate = DateTime.Now
                };
                _context.Banks.Add(entity);
            }
            else
            {
                var entity = await _context.Banks.FirstOrDefaultAsync(x => x.BankId == model.BankId);
                if (entity == null) return Json(new { success = false, message = "Record Not Found" });

                entity.BankCode = model.BankCode.ToUpper().Trim();
                entity.BankNameAr = model.BankNameAr.Trim();
                entity.BankNameEn = model.BankNameEn?.Trim();
                entity.SwiftCode = model.SwiftCode?.Trim();
                entity.Description = model.Description?.Trim();
                entity.IsActive = model.IsActive;
                entity.ModifiedDate = DateTime.Now;
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Saved Successfully" });
        }
    }
}