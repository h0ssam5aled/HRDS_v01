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
    public class BankAccountTypesController : Controller
    {
        private readonly HRDSContext _context;

        public BankAccountTypesController(HRDSContext context)
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

            var rawData = await _context.BankAccountTypes
                .OrderByDescending(x => x.BankAccountTypeId)
                .ToListAsync();

            var list = rawData.Select(x => new BankAccountTypeViewModel
            {
                BankAccountTypeId = x.BankAccountTypeId,
                BankAccountTypeCode = x.BankAccountTypeCode,
                BankAccountTypeNameAr = x.BankAccountTypeNameAr,
                BankAccountTypeNameEn = x.BankAccountTypeNameEn,
                DisplayBankAccountTypeName = isEn && !string.IsNullOrEmpty(x.BankAccountTypeNameEn) ? x.BankAccountTypeNameEn : x.BankAccountTypeNameAr,
                Description = x.Description,
                IsActive = x.IsActive
            }).ToList();

            return Json(new { data = list });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var entity = await _context.BankAccountTypes.FirstOrDefaultAsync(x => x.BankAccountTypeId == id);
            if (entity == null) return Json(null);

            var model = new BankAccountTypeViewModel
            {
                BankAccountTypeId = entity.BankAccountTypeId,
                BankAccountTypeCode = entity.BankAccountTypeCode,
                BankAccountTypeNameAr = entity.BankAccountTypeNameAr,
                BankAccountTypeNameEn = entity.BankAccountTypeNameEn,
                Description = entity.Description,
                IsActive = entity.IsActive
            };

            return Json(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save([FromBody] BankAccountTypeViewModel model)
        {
            if (!ModelState.IsValid) return Json(new { success = false, message = "Invalid Data" });

            if (model.BankAccountTypeId == 0)
            {
                var entity = new BankAccountType
                {
                    BankAccountTypeCode = model.BankAccountTypeCode.ToUpper().Trim(),
                    BankAccountTypeNameAr = model.BankAccountTypeNameAr.Trim(),
                    BankAccountTypeNameEn = model.BankAccountTypeNameEn?.Trim(),
                    Description = model.Description?.Trim(),
                    IsActive = model.IsActive,
                    CreatedDate = DateTime.Now
                };
                _context.BankAccountTypes.Add(entity);
            }
            else
            {
                var entity = await _context.BankAccountTypes.FirstOrDefaultAsync(x => x.BankAccountTypeId == model.BankAccountTypeId);
                if (entity == null) return Json(new { success = false, message = "Record Not Found" });

                entity.BankAccountTypeCode = model.BankAccountTypeCode.ToUpper().Trim();
                entity.BankAccountTypeNameAr = model.BankAccountTypeNameAr.Trim();
                entity.BankAccountTypeNameEn = model.BankAccountTypeNameEn?.Trim();
                entity.Description = model.Description?.Trim();
                entity.IsActive = model.IsActive;
                entity.ModifiedDate = DateTime.Now;
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Saved Successfully" });
        }
    }
}