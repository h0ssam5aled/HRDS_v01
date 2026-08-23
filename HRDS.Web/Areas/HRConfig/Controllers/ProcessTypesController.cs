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
    public class ProcessTypesController : Controller
    {
        private readonly HRDSContext _context;

        public ProcessTypesController(HRDSContext context)
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

            var list = await _context.ProcessTypes
                .Select(x => new ProcessTypeViewModel
                {
                    ProcessTypeId = x.ProcessTypeId,
                    ProcessCode = x.ProcessCode,
                    ProcessNameAr = x.ProcessNameAr,
                    ProcessNameEn = x.ProcessNameEn,
                    DisplayProcessName = isEn && !string.IsNullOrEmpty(x.ProcessNameEn) ? x.ProcessNameEn : x.ProcessNameAr,
                    IsActive = x.IsActive
                })
                .ToListAsync();

            return Json(new { data = list });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var entity = await _context.ProcessTypes.FirstOrDefaultAsync(x => x.ProcessTypeId == id);
            if (entity == null) return Json(null);

            var model = new ProcessTypeViewModel
            {
                ProcessTypeId = entity.ProcessTypeId,
                ProcessCode = entity.ProcessCode,
                ProcessNameAr = entity.ProcessNameAr,
                ProcessNameEn = entity.ProcessNameEn,
                IsActive = entity.IsActive
            };

            return Json(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save([FromBody] ProcessTypeViewModel model)
        {
            if (!ModelState.IsValid) return Json(new { success = false, message = "Invalid Data" });

            if (model.ProcessTypeId == 0)
            {
                var entity = new ProcessType
                {
                    ProcessCode = model.ProcessCode.ToUpper().Trim(),
                    ProcessNameAr = model.ProcessNameAr.Trim(),
                    ProcessNameEn = model.ProcessNameEn?.Trim(),
                    IsActive = model.IsActive
                };
                _context.ProcessTypes.Add(entity);
            }
            else
            {
                var entity = await _context.ProcessTypes.FirstOrDefaultAsync(x => x.ProcessTypeId == model.ProcessTypeId);
                if (entity == null) return Json(new { success = false, message = "Record Not Found" });

                entity.ProcessCode = model.ProcessCode.ToUpper().Trim();
                entity.ProcessNameAr = model.ProcessNameAr.Trim();
                entity.ProcessNameEn = model.ProcessNameEn?.Trim();
                entity.IsActive = model.IsActive;
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Saved Successfully" });
        }
    }
}