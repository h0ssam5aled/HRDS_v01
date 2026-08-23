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
    public class DocumentTypesController : Controller
    {
        private readonly HRDSContext _context;

        public DocumentTypesController(HRDSContext context)
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
            var list = await _context.DocumentTypes
                .Select(x => new DocumentTypeViewModel
                {
                    DocumentTypeId = x.DocumentTypeId,
                    TypeCode = x.TypeCode,
                    TypeNameAr = x.TypeNameAr,
                    TypeNameEn = x.TypeNameEn,
                    IsExpiryRequired = x.IsExpiryRequired,
                    ExpiryAlertDays = x.ExpiryAlertDays,
                    IsMandatory = x.IsMandatory
                })
                .ToListAsync();

            return Json(new { data = list });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var entity = await _context.DocumentTypes.FindAsync(id);
            if (entity == null)
                return Json(null);

            var model = new DocumentTypeViewModel
            {
                DocumentTypeId = entity.DocumentTypeId,
                TypeCode = entity.TypeCode,
                TypeNameAr = entity.TypeNameAr,
                TypeNameEn = entity.TypeNameEn,
                IsExpiryRequired = entity.IsExpiryRequired,
                ExpiryAlertDays = entity.ExpiryAlertDays,
                IsMandatory = entity.IsMandatory
            };

            return Json(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save([FromBody] DocumentTypeViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, message = "بيانات غير صالحة" });

            if (model.DocumentTypeId == 0)
            {
                var entity = new DocumentType
                {
                    TypeCode = model.TypeCode.ToUpper().Trim(),
                    TypeNameAr = model.TypeNameAr.Trim(),
                    TypeNameEn = model.TypeNameEn?.Trim(),
                    IsExpiryRequired = model.IsExpiryRequired,
                    ExpiryAlertDays = model.IsExpiryRequired ? model.ExpiryAlertDays : null,
                    IsMandatory = model.IsMandatory
                };
                _context.DocumentTypes.Add(entity);
            }
            else
            {
                var entity = await _context.DocumentTypes.FindAsync(model.DocumentTypeId);
                if (entity == null)
                    return Json(new { success = false, message = "السجل غير موجود" });

                entity.TypeCode = model.TypeCode.ToUpper().Trim();
                entity.TypeNameAr = model.TypeNameAr.Trim();
                entity.TypeNameEn = model.TypeNameEn?.Trim();
                entity.IsExpiryRequired = model.IsExpiryRequired;
                entity.ExpiryAlertDays = model.IsExpiryRequired ? model.ExpiryAlertDays : null;
                entity.IsMandatory = model.IsMandatory;
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "تم الحفظ بنجاح" });
        }
    }
}