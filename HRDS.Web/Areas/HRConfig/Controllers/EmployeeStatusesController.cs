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
    public class EmployeeStatusesController : Controller
    {
        private readonly HRDSContext _context;

        public EmployeeStatusesController(HRDSContext context)
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
            var list = await _context.EmployeeStatuses
                .Select(x => new EmployeeStatusViewModel
                {
                    EmployeeStatusId = x.EmployeeStatusId,
                    EmployeeStatusCode = x.EmployeeStatusCode,
                    EmployeeStatusNameAr = x.EmployeeStatusNameAr,
                    EmployeeStatusNameEn = x.EmployeeStatusNameEn,
                    Description = x.Description
                })
                .ToListAsync();

            return Json(new { data = list });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var entity = await _context.EmployeeStatuses.FindAsync(id);
            if (entity == null)
                return Json(null);

            var model = new EmployeeStatusViewModel
            {
                EmployeeStatusId = entity.EmployeeStatusId,
                EmployeeStatusCode = entity.EmployeeStatusCode,
                EmployeeStatusNameAr = entity.EmployeeStatusNameAr,
                EmployeeStatusNameEn = entity.EmployeeStatusNameEn,
                Description = entity.Description
            };

            return Json(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save([FromBody] EmployeeStatusViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, message = "بيانات غير صالحة" });

            if (model.EmployeeStatusId == 0)
            {
                var entity = new EmployeeStatus
                {
                    EmployeeStatusCode = model.EmployeeStatusCode.ToUpper().Trim(),
                    EmployeeStatusNameAr = model.EmployeeStatusNameAr.Trim(),
                    EmployeeStatusNameEn = model.EmployeeStatusNameEn?.Trim(),
                    Description = model.Description?.Trim()
                };
                _context.EmployeeStatuses.Add(entity);
            }
            else
            {
                var entity = await _context.EmployeeStatuses.FindAsync(model.EmployeeStatusId);
                if (entity == null)
                    return Json(new { success = false, message = "السجل غير موجود" });

                entity.EmployeeStatusCode = model.EmployeeStatusCode.ToUpper().Trim();
                entity.EmployeeStatusNameAr = model.EmployeeStatusNameAr.Trim();
                entity.EmployeeStatusNameEn = model.EmployeeStatusNameEn?.Trim();
                entity.Description = model.Description?.Trim();
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "تم الحفظ بنجاح" });
        }
    }
}