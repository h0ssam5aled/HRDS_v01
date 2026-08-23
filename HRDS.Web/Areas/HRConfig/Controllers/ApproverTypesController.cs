using System.Globalization;
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
    public class ApproverTypesController : Controller
    {
        private readonly HRDSContext _context;

        public ApproverTypesController(HRDSContext context)
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

            var list = await _context.ApproverTypes
                .Select(x => new ApproverTypeViewModel
                {
                    ApproverTypeId = x.ApproverTypeId,
                    ApproverTypeCode = x.ApproverTypeCode,
                    ApproverTypeNameAr = x.ApproverTypeNameAr,
                    ApproverTypeNameEn = x.ApproverTypeNameEn,
                    DisplayApproverTypeName = isEn && !string.IsNullOrEmpty(x.ApproverTypeNameEn) ? x.ApproverTypeNameEn : x.ApproverTypeNameAr,
                    RequiresJobTitle = x.RequiresJobTitle,
                    RequiresEmployee = x.RequiresEmployee,
                    IsActive = x.IsActive
                })
                .ToListAsync();

            return Json(new { data = list });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var entity = await _context.ApproverTypes.FirstOrDefaultAsync(x => x.ApproverTypeId == id);
            if (entity == null) return Json(null);

            var model = new ApproverTypeViewModel
            {
                ApproverTypeId = entity.ApproverTypeId,
                ApproverTypeCode = entity.ApproverTypeCode,
                ApproverTypeNameAr = entity.ApproverTypeNameAr,
                ApproverTypeNameEn = entity.ApproverTypeNameEn,
                RequiresJobTitle = entity.RequiresJobTitle,
                RequiresEmployee = entity.RequiresEmployee,
                IsActive = entity.IsActive
            };

            return Json(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save([FromBody] ApproverTypeViewModel model)
        {
            if (!ModelState.IsValid) return Json(new { success = false, message = "Invalid Data" });

            if (model.ApproverTypeId == 0)
            {
                var entity = new ApproverType
                {
                    ApproverTypeCode = model.ApproverTypeCode.ToUpper().Trim(),
                    ApproverTypeNameAr = model.ApproverTypeNameAr.Trim(),
                    ApproverTypeNameEn = model.ApproverTypeNameEn?.Trim(),
                    RequiresJobTitle = model.RequiresJobTitle,
                    RequiresEmployee = model.RequiresEmployee,
                    IsActive = model.IsActive
                };
                _context.ApproverTypes.Add(entity);
            }
            else
            {
                var entity = await _context.ApproverTypes.FirstOrDefaultAsync(x => x.ApproverTypeId == model.ApproverTypeId);
                if (entity == null) return Json(new { success = false, message = "Record Not Found" });

                entity.ApproverTypeCode = model.ApproverTypeCode.ToUpper().Trim();
                entity.ApproverTypeNameAr = model.ApproverTypeNameAr.Trim();
                entity.ApproverTypeNameEn = model.ApproverTypeNameEn?.Trim();
                entity.RequiresJobTitle = model.RequiresJobTitle;
                entity.RequiresEmployee = model.RequiresEmployee;
                entity.IsActive = model.IsActive;
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Saved Successfully" });
        }
    }
}