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
    public class BankBranchesController : Controller
    {
        private readonly HRDSContext _context;

        public BankBranchesController(HRDSContext context)
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

            var rawData = await _context.BankBranches
                .Include(x => x.Bank)
                .OrderByDescending(x => x.BranchId)
                .ToListAsync();

            var list = rawData.Select(x => new BankBranchViewModel
            {
                BranchId = x.BranchId,
                BankId = x.BankId,
                BankName = isEn && !string.IsNullOrEmpty(x.Bank?.BankNameEn) ? x.Bank.BankNameEn : x.Bank?.BankNameAr,
                BankBranchCode = x.BankBranchCode,
                BankBranchNameAr = x.BankBranchNameAr,
                BankBranchNameEn = x.BankBranchNameEn,
                BankBranchAddress = x.BankBranchAddress,
                BankBranchPhone = x.BankBranchPhone,
                Description = x.Description,
                IsActive = x.IsActive
            }).ToList();

            return Json(new { data = list });
        }

        [HttpGet]
        public async Task<IActionResult> GetBanksDropdown()
        {
            var isEn = CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToLower() == "en";

            var banks = await _context.Banks
                .Where(x => x.IsActive)
                .Select(x => new
                {
                    id = x.BankId,
                    text = isEn && !string.IsNullOrEmpty(x.BankNameEn)
                        ? $"{x.BankNameEn} ({x.BankCode})"
                        : $"{x.BankNameAr} ({x.BankCode})"
                })
                .ToListAsync();

            return Json(banks);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var branch = await _context.BankBranches.FindAsync(id);
            if (branch == null)
            {
                return Json(new { success = false, message = "Record not found" });
            }

            var model = new BankBranchViewModel
            {
                BranchId = branch.BranchId,
                BankId = branch.BankId,
                BankBranchCode = branch.BankBranchCode,
                BankBranchNameAr = branch.BankBranchNameAr,
                BankBranchNameEn = branch.BankBranchNameEn,
                BankBranchAddress = branch.BankBranchAddress,
                BankBranchPhone = branch.BankBranchPhone,
                Description = branch.Description,
                IsActive = branch.IsActive
            };

            return Json(new { success = true, data = model });
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] BankBranchViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return Json(new { success = false, message = string.Join("<br/>", errors) });
            }

            // التحقق من عدم تكرار كود الفرع داخل نفس البنك
            var codeExists = await _context.BankBranches
                .AnyAsync(x => x.BankId == model.BankId && x.BankBranchCode == model.BankBranchCode && x.BranchId != model.BranchId);

            if (codeExists)
            {
                return Json(new { success = false, message = "Branch Code already exists for this bank." });
            }

            if (model.BranchId == 0)
            {
                var entity = new BankBranch
                {
                    BankId = model.BankId,
                    BankBranchCode = model.BankBranchCode,
                    BankBranchNameAr = model.BankBranchNameAr,
                    BankBranchNameEn = model.BankBranchNameEn,
                    BankBranchAddress = model.BankBranchAddress,
                    BankBranchPhone = model.BankBranchPhone,
                    Description = model.Description,
                    IsActive = model.IsActive,
                    CreatedDate = DateTime.Now
                };

                _context.BankBranches.Add(entity);
            }
            else
            {
                var entity = await _context.BankBranches.FindAsync(model.BranchId);
                if (entity == null)
                {
                    return Json(new { success = false, message = "Record not found." });
                }

                entity.BankId = model.BankId;
                entity.BankBranchCode = model.BankBranchCode;
                entity.BankBranchNameAr = model.BankBranchNameAr;
                entity.BankBranchNameEn = model.BankBranchNameEn;
                entity.BankBranchAddress = model.BankBranchAddress;
                entity.BankBranchPhone = model.BankBranchPhone;
                entity.Description = model.Description;
                entity.IsActive = model.IsActive;
                entity.ModifiedDate = DateTime.Now;

                _context.BankBranches.Update(entity);
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Saved successfully." });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _context.BankBranches.FindAsync(id);
            if (entity == null)
            {
                return Json(new { success = false, message = "Record not found." });
            }

            _context.BankBranches.Remove(entity);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Deleted successfully." });
        }
    }
}