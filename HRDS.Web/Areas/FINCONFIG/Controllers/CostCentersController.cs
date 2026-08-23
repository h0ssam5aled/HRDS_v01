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
    public class CostCentersController : Controller
    {
        private readonly HRDSContext _context;

        public CostCentersController(HRDSContext context)
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

            var rawData = await _context.CostCenters
                .Include(x => x.ParentCostCenter)
                .OrderBy(x => x.CostCenterCode)
                .ToListAsync();

            var list = rawData.Select(x => new CostCenterViewModel
            {
                CostCenterId = x.CostCenterId,
                ParentCostCenterId = x.ParentCostCenterId,
                ParentCostCenterName = x.ParentCostCenter != null
                    ? (isEn && !string.IsNullOrEmpty(x.ParentCostCenter.CostCenterNameEn) ? x.ParentCostCenter.CostCenterNameEn : x.ParentCostCenter.CostCenterNameAr)
                    : null,
                CostCenterCode = x.CostCenterCode,
                CostCenterNameAr = x.CostCenterNameAr,
                CostCenterNameEn = x.CostCenterNameEn,
                DisplayCostCenterName = isEn && !string.IsNullOrEmpty(x.CostCenterNameEn) ? x.CostCenterNameEn : x.CostCenterNameAr,
                CompanyId = x.CompanyId,
                CompanyBranchId = x.CompanyBranchId,
                CostCenterLevel = x.CostCenterLevel,
                IsLeaf = x.IsLeaf,
                HierarchyPath = x.HierarchyPath,
                IsActive = x.IsActive
            }).ToList();

            return Json(new { data = list });
        }

        [HttpGet]
        public async Task<IActionResult> GetParentDropdown()
        {
            var isEn = CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToLower() == "en";

            var parents = await _context.CostCenters
                .Where(x => x.IsActive)
                .OrderBy(x => x.CostCenterCode)
                .Select(x => new
                {
                    id = x.CostCenterId,
                    name = x.CostCenterCode + " - " + (isEn && !string.IsNullOrEmpty(x.CostCenterNameEn) ? x.CostCenterNameEn : x.CostCenterNameAr)
                })
                .ToListAsync();

            return Json(parents);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var entity = await _context.CostCenters.FirstOrDefaultAsync(x => x.CostCenterId == id);
            if (entity == null) return Json(null);

            var model = new CostCenterViewModel
            {
                CostCenterId = entity.CostCenterId,
                ParentCostCenterId = entity.ParentCostCenterId,
                CostCenterCode = entity.CostCenterCode,
                CostCenterNameAr = entity.CostCenterNameAr,
                CostCenterNameEn = entity.CostCenterNameEn,
                CompanyId = entity.CompanyId,
                CompanyBranchId = entity.CompanyBranchId,
                CostCenterLevel = entity.CostCenterLevel,
                IsLeaf = entity.IsLeaf,
                HierarchyPath = entity.HierarchyPath,
                IsActive = entity.IsActive
            };

            return Json(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save([FromBody] CostCenterViewModel model)
        {
            if (!ModelState.IsValid) return Json(new { success = false, message = "Invalid Data" });

            CostCenter? parent = null;
            if (model.ParentCostCenterId.HasValue && model.ParentCostCenterId.Value > 0)
            {
                parent = await _context.CostCenters.FirstOrDefaultAsync(x => x.CostCenterId == model.ParentCostCenterId.Value);
            }

            byte level = (byte)(parent != null ? parent.CostCenterLevel + 1 : 1);

            if (model.CostCenterId == 0)
            {
                var entity = new CostCenter
                {
                    ParentCostCenterId = parent?.CostCenterId,
                    CostCenterCode = model.CostCenterCode.ToUpper().Trim(),
                    CostCenterNameAr = model.CostCenterNameAr.Trim(),
                    CostCenterNameEn = model.CostCenterNameEn?.Trim(),
                    CompanyId = model.CompanyId,
                    CompanyBranchId = model.CompanyBranchId,
                    CostCenterLevel = level,
                    IsLeaf = true, // افتراضياً يكون Leaf عند الإنشاء
                    IsActive = model.IsActive,
                    CreatedDate = DateTime.Now
                };

                _context.CostCenters.Add(entity);
                await _context.SaveChangesAsync();

                // تحديث الـ HierarchyPath والـ Parent IsLeaf بعد توليد الـ Primary Key
                entity.HierarchyPath = parent != null ? $"{parent.HierarchyPath}/{entity.CostCenterId}" : $"{entity.CostCenterId}";
                if (parent != null && parent.IsLeaf)
                {
                    parent.IsLeaf = false;
                }
            }
            else
            {
                var entity = await _context.CostCenters.FirstOrDefaultAsync(x => x.CostCenterId == model.CostCenterId);
                if (entity == null) return Json(new { success = false, message = "Record Not Found" });

                entity.ParentCostCenterId = parent?.CostCenterId;
                entity.CostCenterCode = model.CostCenterCode.ToUpper().Trim();
                entity.CostCenterNameAr = model.CostCenterNameAr.Trim();
                entity.CostCenterNameEn = model.CostCenterNameEn?.Trim();
                entity.CompanyId = model.CompanyId;
                entity.CompanyBranchId = model.CompanyBranchId;
                entity.CostCenterLevel = level;
                entity.HierarchyPath = parent != null ? $"{parent.HierarchyPath}/{entity.CostCenterId}" : $"{entity.CostCenterId}";
                entity.IsActive = model.IsActive;
                entity.ModifiedDate = DateTime.Now;

                if (parent != null && parent.IsLeaf)
                {
                    parent.IsLeaf = false;
                }
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Saved Successfully" });
        }
    }
}