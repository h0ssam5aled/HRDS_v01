using HRDS.Web.Models;
using HRDS.Web.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;

namespace HRDS.Web.Controllers
{
    [Authorize] // تمنع الوصول لأي شاشة داخل هذا الـ Controller إلا بعد تسجيل الدخول
    public class HomeController : Controller
    {
        private readonly HRDSContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(HRDSContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // في حالة كان المسجل فعلياً (لم نعد بحاجة للقيمة الافتراضية 1)
            if (!int.TryParse(userIdClaim, out int userId))
            {
                return Challenge(); // تحويله لصفحة اللوجن إذا لم يوجد ID
            }

            var allowedModules = await GetUserAllowedModulesAsync(userId);

            var model = new DashboardViewModel
            {
                AllowedModules = allowedModules
            };

            return View(model);
        }

        private async Task<List<ModuleTileDto>> GetUserAllowedModulesAsync(int userId)
        {
            try
            {
                // استعلام EF Core يعكس هيكلية جداول الـ Security لديك بدقة
                var modules = await _context.UserAccesses
                    .Where(ua => ua.UserId == userId && ua.IsActive)
                    .SelectMany(ua => ua.UserRoles)
                    .Where(ur => ur.IsActive)
                    .Select(ur => ur.Role)
                    .Where(r => r.IsActive)
                    .SelectMany(r => r.RolePermissions)
                    .Where(rp => rp.IsActive)
                    .Select(rp => rp.Permission)
                    .Where(p => p.IsActive)
                    .Select(p => p.Model)
                    .Where(m => m.IsActive)
                    .Select(m => m.Module)
                    .Where(m => m.IsActive)
                    .Distinct()
                    .Select(m => new ModuleTileDto
                    {
                        Title = m.ModuleNameAr ?? m.ModuleNameEn,
                        Description = m.Description ?? "وحدة متكاملة ضمن نظام HRDS",
                        Url = $"/{m.ModuleCode}/Index",
                        IconClass = GetModuleIcon(m.ModuleCode),
                        Color = GetModuleColor(m.ModuleCode),
                        ActionText = "فتح الوحدة"
                    })
                    .ToListAsync();

                return modules;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "حدث خطأ أثناء جلب صلاحيات الموديولات للمستخدم {UserId}", userId);
                return new List<ModuleTileDto>();
            }
        }

        // تحديد ألوان الكروت بحسب ModuleCode
        private static string GetModuleColor(string? moduleCode) => moduleCode?.ToLower() switch
        {
            "hr" or "employees" => "#3b82f6",
            "finance" or "accounts" => "#10b981",
            "inventory" or "stock" => "#f59e0b",
            "purchasing" => "#ef4444",
            _ => "#6366f1"
        };

        // تحديد أيقونات Bootstrap بحسب ModuleCode
        private static string GetModuleIcon(string? moduleCode) => moduleCode?.ToLower() switch
        {
            "hr" or "employees" => "bi bi-people-fill",
            "finance" or "accounts" => "bi bi-cash-stack",
            "inventory" or "stock" => "bi bi-boxes",
            "purchasing" => "bi bi-cart-check",
            _ => "bi bi-grid-fill"
        };

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult SetActiveBranch(int branchId)
        {
            // حفظ رقم الفرع المختار في السيشن
            HttpContext.Session.SetInt32("SelectedBranchId", branchId);

            return Json(new { success = true });
        }
    }
}