using HRDS.Web.Areas.HR.ViewModels;
using HRDS.Web.Models;
using HRDS.Web.Models.Entities;
using HRDS.Web.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRDS.Web.Controllers
{
    [Authorize]
    [Area("HRConfig")]
    [HasModuleAccess("HRConfig")]
    public class AssetTypesController : Controller
    {
        private readonly HRDSContext _context;

        public AssetTypesController(HRDSContext context)
        {
            _context = context;
        }

        public IActionResult Index() => View();

        [HttpGet]
        public async Task<IActionResult> GetJson()
        {
            var list = await _context.AssetTypes
                .Select(x => new AssetTypeViewModel
                {
                    AssetTypeId = x.AssetTypeId,
                    AssetTypeCode = x.AssetTypeCode,
                    AssetTypeNameAr = x.AssetTypeNameAr,
                    AssetTypeNameEn = x.AssetTypeNameEn,
                    IsActive = x.IsActive
                })
                .ToListAsync();

            return Json(new { data = list });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var entity = await _context.AssetTypes.FindAsync(id);
            if (entity == null) return Json(null);

            var model = new AssetTypeViewModel
            {
                AssetTypeId = entity.AssetTypeId,
                AssetTypeCode = entity.AssetTypeCode,
                AssetTypeNameAr = entity.AssetTypeNameAr,
                AssetTypeNameEn = entity.AssetTypeNameEn,
                IsActive = entity.IsActive
            };

            return Json(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save([FromBody] AssetTypeViewModel model)
        {
            if (!ModelState.IsValid) return Json(new { success = false, message = "بيانات غير صالحة" });

            if (model.AssetTypeId == 0)
            {
                var entity = new AssetType
                {
                    AssetTypeCode = model.AssetTypeCode.ToUpper().Trim(),
                    AssetTypeNameAr = model.AssetTypeNameAr.Trim(),
                    AssetTypeNameEn = model.AssetTypeNameEn?.Trim(),
                    IsActive = model.IsActive
                };
                _context.AssetTypes.Add(entity);
            }
            else
            {
                var entity = await _context.AssetTypes.FindAsync(model.AssetTypeId);
                if (entity == null) return Json(new { success = false, message = "السجل غير موجود" });

                entity.AssetTypeCode = model.AssetTypeCode.ToUpper().Trim();
                entity.AssetTypeNameAr = model.AssetTypeNameAr.Trim();
                entity.AssetTypeNameEn = model.AssetTypeNameEn?.Trim();
                entity.IsActive = model.IsActive;
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "تم الحفظ بنجاح" });
        }
    }
}