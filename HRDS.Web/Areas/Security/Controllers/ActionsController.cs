using HRDS.Web.Models;
using HRDS.Web.Models.Entities;
using HRDS.Web.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Action = HRDS.Web.Models.Entities.Action; // تجنب التضارب مع System.Action

namespace HRDS.Web.Areas.Security.Controllers
{
    [Area("Security")]
    [Authorize]
    [HasModuleAccess("SECURITY")]
    public class ActionsController : Controller
    {
        private readonly HRDSContext _context; // استبدل HRDSContext باسم الـ DbContext عندك

        public ActionsController(HRDSContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetActionsJson()
        {
            var actionsList = await _context.Actions
                .AsNoTracking()
                .Select(a => new ActionViewModel
                {
                    ActionId = a.ActionId,
                    ActionCode = a.ActionCode,
                    ActionNameAr = a.ActionNameAr,
                    ActionNameEn = a.ActionNameEn,
                    Description = a.Description,
                    IsActive = a.IsActive
                })
                .OrderBy(a => a.ActionId)
                .ToListAsync();

            return Json(new { data = actionsList });
        }
    }
}