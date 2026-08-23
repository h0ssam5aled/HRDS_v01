using HRDS.Web.Areas.HRConfig.ViewModels;
using HRDS.Web.Models;
using HRDS.Web.Models.Entities;
using HRDS.Web.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRDS.Web.Areas.HRConfig.Controllers
{
    [Authorize]
    [Area("HRConfig")]
    [HasModuleAccess("HRConfig")]
    public class DepartmentsController : Controller
    {
        private readonly HRDSContext _context;

        public DepartmentsController(HRDSContext context)
        {
            _context = context;
        }

        public IActionResult Index() => View();

        [HttpGet]
        public async Task<IActionResult> GetJson()
        {
            var list = await _context.Departments
                .Where(x => !x.IsDeleted)
                .Select(x => new DepartmentViewModel
                {
                    DepartmentId = x.DepartmentId,
                    DepartmentCode = x.DepartmentCode,
                    DepartmentNameAr = x.DepartmentNameAr,
                    DepartmentNameEn = x.DepartmentNameEn,
                    IsActive = x.IsActive
                })
                .ToListAsync();

            return Json(new { data = list });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var entity = await _context.Departments.FirstOrDefaultAsync(x => x.DepartmentId == id && !x.IsDeleted);
            if (entity == null) return Json(null);

            var model = new DepartmentViewModel
            {
                DepartmentId = entity.DepartmentId,
                DepartmentCode = entity.DepartmentCode,
                DepartmentNameAr = entity.DepartmentNameAr,
                DepartmentNameEn = entity.DepartmentNameEn,
                CompanyId = entity.CompanyId,
                CompanyBranchId = entity.CompanyBranchId,
                IsActive = entity.IsActive
            };

            return Json(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save([FromBody] DepartmentViewModel model)
        {
            if (!ModelState.IsValid) return Json(new { success = false, message = "بيانات غير صالحة" });

            if (model.DepartmentId == 0)
            {
                var entity = new Department
                {
                    DepartmentCode = model.DepartmentCode.ToUpper().Trim(),
                    DepartmentNameAr = model.DepartmentNameAr.Trim(),
                    DepartmentNameEn = model.DepartmentNameEn?.Trim(),
                    CompanyId = model.CompanyId,
                    CompanyBranchId = model.CompanyBranchId,
                    IsActive = model.IsActive,
                    CreatedAt = DateTime.Now,
                    IsDeleted = false
                };
                _context.Departments.Add(entity);
            }
            else
            {
                var entity = await _context.Departments.FirstOrDefaultAsync(x => x.DepartmentId == model.DepartmentId && !x.IsDeleted);
                if (entity == null) return Json(new { success = false, message = "السجل غير موجود" });

                entity.DepartmentCode = model.DepartmentCode.ToUpper().Trim();
                entity.DepartmentNameAr = model.DepartmentNameAr.Trim();
                entity.DepartmentNameEn = model.DepartmentNameEn?.Trim();
                entity.CompanyId = model.CompanyId;
                entity.CompanyBranchId = model.CompanyBranchId;
                entity.IsActive = model.IsActive;
                entity.UpdatedAt = DateTime.Now;
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "تم الحفظ بنجاح" });
        }
    }
}