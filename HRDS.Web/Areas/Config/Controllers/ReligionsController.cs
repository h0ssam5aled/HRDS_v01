using HRDS.Web.Areas.Config.Models;
using HRDS.Web.Models.Entities;
using HRDS.Web.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRDS.Web.Areas.Config.Controllers
{
    [Area("Config")]
    [HasModuleAccess("Config")]
    public class ReligionsController : Controller
    {
        private readonly HRDSContext _context;

        public ReligionsController(HRDSContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetReligionsJson()
        {
            var data = await _context.Religions
                .Select(r => new ReligionViewModel
                {
                    ReligionId = r.ReligionId,
                    ReligionCode = r.ReligionCode,
                    ReligionNameAr = r.ReligionNameAr,
                    ReligionNameEn = r.ReligionNameEn,
                    IsActive = r.IsActive,
                    SortOrder = r.SortOrder
                })
                .OrderBy(r => r.SortOrder)
                .ToListAsync();

            return Json(new { data });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _context.Religions.FindAsync(id);
            if (item == null)
            {
                return Json(new { success = false, message = "الديانة غير موجودة" });
            }

            var dto = new ReligionViewModel
            {
                ReligionId = item.ReligionId,
                ReligionCode = item.ReligionCode,
                ReligionNameAr = item.ReligionNameAr,
                ReligionNameEn = item.ReligionNameEn,
                IsActive = item.IsActive,
                SortOrder = item.SortOrder
            };

            return Json(new { success = true, data = dto });
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] ReligionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "البيانات المدخلة غير صحيحة" });
            }

            if (model.ReligionId == 0)
            {
                var entity = new Religion
                {
                    ReligionCode = model.ReligionCode,
                    ReligionNameAr = model.ReligionNameAr,
                    ReligionNameEn = model.ReligionNameEn,
                    IsActive = model.IsActive,
                    SortOrder = model.SortOrder
                };

                _context.Religions.Add(entity);
            }
            else
            {
                var entity = await _context.Religions.FindAsync(model.ReligionId);
                if (entity == null)
                {
                    return Json(new { success = false, message = "الديانة غير موجودة" });
                }

                entity.ReligionCode = model.ReligionCode;
                entity.ReligionNameAr = model.ReligionNameAr;
                entity.ReligionNameEn = model.ReligionNameEn;
                entity.IsActive = model.IsActive;
                entity.SortOrder = model.SortOrder;

                _context.Religions.Update(entity);
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "تم الحفظ بنجاح" });
        }
    }
}