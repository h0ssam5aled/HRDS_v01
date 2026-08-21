using HRDS.Web.Areas.Config.Models;
using HRDS.Web.Models.Entities;
using HRDS.Web.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRDS.Web.Areas.Config.Controllers
{
    [Area("Config")]
    [HasModuleAccess("Config")]
    public class MilitaryStatusesController : Controller
    {
        private readonly HRDSContext _context;

        public MilitaryStatusesController(HRDSContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetMilitaryStatusesJson()
        {
            var data = await _context.MilitaryStatuses
                .Select(m => new MilitaryStatusViewModel
                {
                    MilitaryStatusId = m.MilitaryStatusId,
                    MilitaryStatusCode = m.MilitaryStatusCode,
                    MilitaryStatusNameAr = m.MilitaryStatusNameAr,
                    MilitaryStatusNameEn = m.MilitaryStatusNameEn,
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
            var item = await _context.MilitaryStatuses.FindAsync(id);
            if (item == null)
            {
                return Json(new { success = false, message = "الحالة العسكرية غير موجودة" });
            }

            var dto = new MilitaryStatusViewModel
            {
                MilitaryStatusId = item.MilitaryStatusId,
                MilitaryStatusCode = item.MilitaryStatusCode,
                MilitaryStatusNameAr = item.MilitaryStatusNameAr,
                MilitaryStatusNameEn = item.MilitaryStatusNameEn,
                IsActive = item.IsActive,
                SortOrder = item.SortOrder
            };

            return Json(new { success = true, data = dto });
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] MilitaryStatusViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "البيانات المدخلة غير صحيحة" });
            }

            if (model.MilitaryStatusId == 0)
            {
                var entity = new MilitaryStatus
                {
                    MilitaryStatusCode = model.MilitaryStatusCode,
                    MilitaryStatusNameAr = model.MilitaryStatusNameAr,
                    MilitaryStatusNameEn = model.MilitaryStatusNameEn,
                    IsActive = model.IsActive,
                    SortOrder = model.SortOrder
                };

                _context.MilitaryStatuses.Add(entity);
            }
            else
            {
                var entity = await _context.MilitaryStatuses.FindAsync(model.MilitaryStatusId);
                if (entity == null)
                {
                    return Json(new { success = false, message = "الحالة العسكرية غير موجودة" });
                }

                entity.MilitaryStatusCode = model.MilitaryStatusCode;
                entity.MilitaryStatusNameAr = model.MilitaryStatusNameAr;
                entity.MilitaryStatusNameEn = model.MilitaryStatusNameEn;
                entity.IsActive = model.IsActive;
                entity.SortOrder = model.SortOrder;

                _context.MilitaryStatuses.Update(entity);
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "تم الحفظ بنجاح" });
        }
    }
}