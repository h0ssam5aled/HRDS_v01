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
    public class EmploymentTypesController : Controller
    {
        private readonly HRDSContext _context;

        public EmploymentTypesController(HRDSContext context)
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
            var list = await _context.EmploymentTypes
                .Select(x => new EmploymentTypeViewModel
                {
                    EmploymentTypeId = x.EmploymentTypeId,
                    EmploymentTypeCode = x.EmploymentTypeCode,
                    EmploymentTypeNameAr = x.EmploymentTypeNameAr,
                    EmploymentTypeNameEn = x.EmploymentTypeNameEn,
                    Description = x.Description,
                    DefaultWorkingHours = x.DefaultWorkingHours,
                    IsOvertimeAllowed = x.IsOvertimeAllowed,
                    IsLeaveEligible = x.IsLeaveEligible
                })
                .ToListAsync();

            return Json(new { data = list });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var entity = await _context.EmploymentTypes.FindAsync(id);
            if (entity == null)
                return Json(null);

            var model = new EmploymentTypeViewModel
            {
                EmploymentTypeId = entity.EmploymentTypeId,
                EmploymentTypeCode = entity.EmploymentTypeCode,
                EmploymentTypeNameAr = entity.EmploymentTypeNameAr,
                EmploymentTypeNameEn = entity.EmploymentTypeNameEn,
                Description = entity.Description,
                DefaultWorkingHours = entity.DefaultWorkingHours,
                IsOvertimeAllowed = entity.IsOvertimeAllowed,
                IsLeaveEligible = entity.IsLeaveEligible
            };

            return Json(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save([FromBody] EmploymentTypeViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, message = "بيانات غير صالحة" });

            if (model.EmploymentTypeId == 0)
            {
                var entity = new EmploymentType
                {
                    EmploymentTypeCode = model.EmploymentTypeCode.ToUpper().Trim(),
                    EmploymentTypeNameAr = model.EmploymentTypeNameAr.Trim(),
                    EmploymentTypeNameEn = model.EmploymentTypeNameEn?.Trim(),
                    Description = model.Description?.Trim(),
                    DefaultWorkingHours = model.DefaultWorkingHours,
                    IsOvertimeAllowed = model.IsOvertimeAllowed,
                    IsLeaveEligible = model.IsLeaveEligible
                };
                _context.EmploymentTypes.Add(entity);
            }
            else
            {
                var entity = await _context.EmploymentTypes.FindAsync(model.EmploymentTypeId);
                if (entity == null)
                    return Json(new { success = false, message = "السجل غير موجود" });

                entity.EmploymentTypeCode = model.EmploymentTypeCode.ToUpper().Trim();
                entity.EmploymentTypeNameAr = model.EmploymentTypeNameAr.Trim();
                entity.EmploymentTypeNameEn = model.EmploymentTypeNameEn?.Trim();
                entity.Description = model.Description?.Trim();
                entity.DefaultWorkingHours = model.DefaultWorkingHours;
                entity.IsOvertimeAllowed = model.IsOvertimeAllowed;
                entity.IsLeaveEligible = model.IsLeaveEligible;
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "تم الحفظ بنجاح" });
        }
    }
}