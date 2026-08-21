using HRDS.Web.Models;
using HRDS.Web.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRDS.Web.ViewComponents
{
    public class CompanySwitchViewComponent : ViewComponent
    {
        private readonly HRDSContext _context;

        public CompanySwitchViewComponent(HRDSContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var isArabic = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "ar";

            // قراءة الفرع المحدد من السيشن إن وجد
            int? selectedBranchId = HttpContext.Session.GetInt32("SelectedBranchId");

            var companies = await _context.Companies
                .Where(c => c.IsActive)
                .ToListAsync();

            var branches = await _context.CompanyBranches
                .Where(b => b.IsActive)
                .ToListAsync();

            ViewBag.IsArabic = isArabic;
            ViewBag.Branches = branches;
            ViewBag.SelectedBranchId = selectedBranchId; // تمرير المعرف المختار

            return View(companies);
        }
    }
}