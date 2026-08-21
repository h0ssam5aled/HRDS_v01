using HRDS.Web.Models;
using HRDS.Web.Models.Entities;
using HRDS.Web.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HRDS.Web.Areas.CoreData.Controllers
{
    [Area("CoreData")]
    [HasModuleAccess("Core")]
    public class CompanyBranchesController : Controller
    {
        private readonly HRDSContext _context;

        public CompanyBranchesController(HRDSContext context)
        {
            _context = context;
        }

        // GET: CompanyBranches
        // GET: CompanyBranches
        public async Task<IActionResult> Index()
        {
            var branches = await _context.CompanyBranches
                .Include(b => b.Company)
                .Include(b => b.City)
                .Include(b => b.Governorate)
                    .ThenInclude(g => g.Country) // جلب الدولة عن طريق المحافظة
                .ToListAsync();

            return View(branches);
        }

        // GET: CompanyBranches/Create
        public IActionResult Create()
        {
            PopulateDropdowns();
            return View();
        }

        // POST: CompanyBranches/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CompanyBranchViewModel model)
        {
            if (ModelState.IsValid)
            {
                // تحويل الـ ViewModel إلى Entity
                var entity = new CompanyBranch
                {
                    CompanyId = model.CompanyId,
                    BranchCode = model.BranchCode,
                    BranchNameAr = model.BranchNameAr,
                    BranchNameEn = model.BranchNameEn,
                    CountryId = model.CountryId,
                    GovernorateId = model.GovernorateId,
                    CityId = model.CityId,
                    Address = model.Address,
                    Phone = model.Phone,
                    Email = model.Email,
                    IsMainBranch = model.IsMainBranch,
                    IsActive = model.IsActive
                };

                _context.CompanyBranches.Add(entity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            PopulateDropdowns(model.CompanyId, model.CountryId, model.GovernorateId, model.CityId);
            return View(model);
        }

        // GET: CoreData/CompanyBranches/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var branch = await _context.CompanyBranches
                .Include(b => b.Governorate)
                .FirstOrDefaultAsync(b => b.CompanyBranchId == id);

            if (branch == null) return NotFound();

            // جلب CountryId من جدول Governorate لأن Entity الفروع لا تحوي Country navigation property مباشر
            int? countryId = branch.Governorate?.CountryId;

            PopulateDropdowns(branch.CompanyId, countryId, branch.GovernorateId, branch.CityId);
            return View(branch);
        }

        // POST: CoreData/CompanyBranches/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CompanyBranch model)
        {
            if (id != model.CompanyBranchId) return NotFound();

            // إزالة التحقق من علاقات الكيانات لتجنب خطأ ModelState.IsValid
            ModelState.Remove("Company");
            ModelState.Remove("Governorate");
            ModelState.Remove("City");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(model);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.CompanyBranches.Any(e => e.CompanyBranchId == model.CompanyBranchId))
                        return NotFound();
                    else
                        throw;
                }
            }

            // في حالة وجود خطأ، نعيد جلب CountryId لإعادة تعبئة القوائم
            var selectedGov = await _context.Governorates.FindAsync(model.GovernorateId);
            int? countryId = selectedGov?.CountryId;

            PopulateDropdowns(model.CompanyId, countryId, model.GovernorateId, model.CityId);
            return View(model);
        }
        // Helper Method لتعبئة القوائم
        private void PopulateDropdowns(int? companyId = null, int? countryId = null, int? govId = null, int? cityId = null)
        {
            var isArabic = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "ar";

            ViewBag.Companies = new SelectList(_context.Companies, "CompanyId", isArabic ? "CompanyNameAr" : "CompanyNameEn", companyId);
            ViewBag.Countries = new SelectList(_context.Countries, "CountryId", isArabic ? "CountryNameAr" : "CountryNameEn", countryId);

            ViewBag.Governorates = countryId.HasValue
                ? new SelectList(_context.Governorates.Where(g => g.CountryId == countryId), "GovernorateId", isArabic ? "GovernorateNameAr" : "GovernorateNameEn", govId)
                : new SelectList(Enumerable.Empty<SelectListItem>());

            ViewBag.Cities = govId.HasValue
                ? new SelectList(_context.Cities.Where(c => c.GovernorateId == govId), "CityId", isArabic ? "CityNameAr" : "CityNameEn", cityId)
                : new SelectList(Enumerable.Empty<SelectListItem>());
        }
    }
}