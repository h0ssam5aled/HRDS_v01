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
    public class AccountsController : Controller
    {
        private readonly HRDSContext _context;

        public AccountsController(HRDSContext context)
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

            var rawData = await _context.Accounts
                .Include(x => x.AccountType)
                .Include(x => x.ParentAccount)
                .Include(x => x.Currency)
                .OrderBy(x => x.AccountCode)
                .ToListAsync();

            var list = rawData.Select(x => new AccountViewModel
            {
                AccountId = x.AccountId,
                AccountTypeId = x.AccountTypeId,
                AccountTypeName = isEn && !string.IsNullOrEmpty(x.AccountType?.AccountTypeNameEn)
                    ? x.AccountType.AccountTypeNameEn
                    : x.AccountType?.AccountTypeNameAr,
                ParentAccountId = x.ParentAccountId,
                ParentAccountName = x.ParentAccount != null
                    ? (isEn && !string.IsNullOrEmpty(x.ParentAccount.AccountNameEn) ? x.ParentAccount.AccountNameEn : x.ParentAccount.AccountNameAr)
                    : null,
                CurrencyId = x.CurrencyId,
                CurrencyName = isEn && !string.IsNullOrEmpty(x.Currency?.CurrencyNameEn)
                    ? x.Currency.CurrencyNameEn
                    : x.Currency?.CurrencyNameAr,
                AccountCode = x.AccountCode,
                AccountNameAr = x.AccountNameAr,
                AccountNameEn = x.AccountNameEn,
                AccountLevel = x.AccountLevel,
                IsLeaf = x.IsLeaf,
                AccountNature = x.AccountNature,
                HierarchyPath = x.HierarchyPath,
                Description = x.Description,
                IsActive = x.IsActive
            }).ToList();

            return Json(new { data = list });
        }

        [HttpGet]
        public async Task<IActionResult> GetParentAccountsDropdown()
        {
            var isEn = CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToLower() == "en";

            var accounts = await _context.Accounts
                .Where(x => x.IsActive)
                .Select(x => new
                {
                    id = x.AccountId,
                    text = isEn && !string.IsNullOrEmpty(x.AccountNameEn)
                        ? $"{x.AccountCode} - {x.AccountNameEn}"
                        : $"{x.AccountCode} - {x.AccountNameAr}",
                    level = x.AccountLevel,
                    nature = x.AccountNature,
                    typeId = x.AccountTypeId
                })
                .ToListAsync();

            return Json(accounts);
        }

        [HttpGet]
        public async Task<IActionResult> GetAccountTypesDropdown()
        {
            var isEn = CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToLower() == "en";

            var types = await _context.AccountTypes
                .Where(x => x.IsActive)
                .Select(x => new
                {
                    id = x.AccountTypeId,
                    text = isEn && !string.IsNullOrEmpty(x.AccountTypeNameEn) ? x.AccountTypeNameEn : x.AccountTypeNameAr
                })
                .ToListAsync();

            return Json(types);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                return Json(new { success = false, message = "Record not found" });
            }

            var model = new AccountViewModel
            {
                AccountId = account.AccountId,
                AccountTypeId = account.AccountTypeId,
                ParentAccountId = account.ParentAccountId,
                CurrencyId = account.CurrencyId,
                AccountCode = account.AccountCode,
                AccountNameAr = account.AccountNameAr,
                AccountNameEn = account.AccountNameEn,
                AccountLevel = account.AccountLevel,
                IsLeaf = account.IsLeaf,
                AccountNature = account.AccountNature,
                HierarchyPath = account.HierarchyPath,
                Description = account.Description,
                IsActive = account.IsActive
            };

            return Json(new { success = true, data = model });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save([FromBody] AccountViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return Json(new { success = false, message = string.Join("<br/>", errors) });
            }

            // منع التكرار في كود الحساب
            var codeExists = await _context.Accounts
                .AnyAsync(x => x.AccountCode == model.AccountCode && x.AccountId != model.AccountId);

            if (codeExists)
            {
                return Json(new { success = false, message = "Account Code already exists." });
            }

            // احتساب المستوى وتحديث الحساب الأب
            byte level = 1;
            string path = model.AccountCode;

            if (model.ParentAccountId.HasValue && model.ParentAccountId > 0)
            {
                var parent = await _context.Accounts.FindAsync(model.ParentAccountId.Value);
                if (parent != null)
                {
                    level = (byte)(parent.AccountLevel + 1);
                    path = $"{parent.HierarchyPath}/{model.AccountCode}";

                    // تحويل الحساب الأب إلى رئيسي (IsLeaf = false)
                    if (parent.IsLeaf)
                    {
                        parent.IsLeaf = false;
                        _context.Accounts.Update(parent);
                    }
                }
            }

            if (model.AccountId == 0)
            {
                var entity = new Account
                {
                    AccountTypeId = model.AccountTypeId,
                    ParentAccountId = model.ParentAccountId > 0 ? model.ParentAccountId : null,
                    CurrencyId = model.CurrencyId > 0 ? model.CurrencyId : null,
                    AccountCode = model.AccountCode,
                    AccountNameAr = model.AccountNameAr,
                    AccountNameEn = model.AccountNameEn,
                    AccountLevel = level,
                    IsLeaf = model.IsLeaf,
                    AccountNature = model.AccountNature,
                    HierarchyPath = path,
                    Description = model.Description,
                    IsActive = model.IsActive,
                    CreatedDate = DateTime.Now
                };

                _context.Accounts.Add(entity);
            }
            else
            {
                var entity = await _context.Accounts.FindAsync(model.AccountId);
                if (entity == null)
                {
                    return Json(new { success = false, message = "Record not found." });
                }

                entity.AccountTypeId = model.AccountTypeId;
                entity.ParentAccountId = model.ParentAccountId > 0 ? model.ParentAccountId : null;
                entity.CurrencyId = model.CurrencyId > 0 ? model.CurrencyId : null;
                entity.AccountCode = model.AccountCode;
                entity.AccountNameAr = model.AccountNameAr;
                entity.AccountNameEn = model.AccountNameEn;
                entity.AccountLevel = level;
                entity.IsLeaf = model.IsLeaf;
                entity.AccountNature = model.AccountNature;
                entity.HierarchyPath = path;
                entity.Description = model.Description;
                entity.IsActive = model.IsActive;
                entity.ModifiedDate = DateTime.Now;

                _context.Accounts.Update(entity);
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Saved successfully." });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _context.Accounts
                .Include(x => x.InverseParentAccount)
                .FirstOrDefaultAsync(x => x.AccountId == id);

            if (entity == null)
            {
                return Json(new { success = false, message = "Record not found." });
            }

            if (entity.InverseParentAccount.Any())
            {
                return Json(new { success = false, message = "Cannot delete account with sub-accounts." });
            }

            _context.Accounts.Remove(entity);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Deleted successfully." });
        }
    }
}