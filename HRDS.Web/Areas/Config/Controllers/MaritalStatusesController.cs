using HRDS.Web.Areas.Config.Models;
using HRDS.Web.Models.Entities;
using HRDS.Web.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRDS.Web.Areas.Config.Controllers
{
    [Area("Config")]
    [HasModuleAccess("Config")]
    public class MaritalStatusesController : Controller
    {
        private readonly HRDSContext _context;

        public MaritalStatusesController(HRDSContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetMaritalStatusesJson()
        {
            var data = await _context.MaritalStatuses
                .Select(m => new MaritalStatusViewModel
                {
                    MaritalStatusId = m.MaritalStatusId,
                    MaritalStatusCode = m.MaritalStatusCode,
                    MaritalStatusNameAr = m.MaritalStatusNameAr,
                    MaritalStatusNameEn = m.MaritalStatusNameEn,
                    IsActive = m.IsActive,
                    SortOrder = m.SortOrder
                })
                .OrderBy(m => m.SortOrder)
                .ToListAsync();

            return Json(new { data });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _context.MaritalStatuses.FindAsync(id);
            if (item == null)
            {
                return Json(new { success = false, message = "الحالة الاجتماعية غير موجودة" });
            }

            var dto = new MaritalStatusViewModel
            {
                MaritalStatusId = item.MaritalStatusId,
                MaritalStatusCode = item.MaritalStatusCode,
                MaritalStatusNameAr = item.MaritalStatusNameAr,
                MaritalStatusNameEn = item.MaritalStatusNameEn,
                IsActive = item.IsActive,
                SortOrder = item.SortOrder
            };

            return Json(new { success = true, data = dto });
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] MaritalStatusViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "البيانات المدخلة غير صحيحة" });
            }

            if (model.MaritalStatusId == 0)
            {
                var entity = new MaritalStatus
                {
                    MaritalStatusCode = model.MaritalStatusCode,
                    MaritalStatusNameAr = model.MaritalStatusNameAr,
                    MaritalStatusNameEn = model.MaritalStatusNameEn,
                    IsActive = model.IsActive,
                    SortOrder = model.SortOrder
                };

                _context.MaritalStatuses.Add(entity);
            }
            else
            {
                var entity = await _context.MaritalStatuses.FindAsync(model.MaritalStatusId);
                if (entity == null)
                {
                    return Json(new { success = false, message = "الحالة الاجتماعية غير موجودة" });
                }

                entity.MaritalStatusCode = model.MaritalStatusCode;
                entity.MaritalStatusNameAr = model.MaritalStatusNameAr;
                entity.MaritalStatusNameEn = model.MaritalStatusNameEn;
                entity.IsActive = model.IsActive;
                entity.SortOrder = model.SortOrder;

                _context.MaritalStatuses.Update(entity);
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "تم الحفظ بنجاح" });
        }
    }
}