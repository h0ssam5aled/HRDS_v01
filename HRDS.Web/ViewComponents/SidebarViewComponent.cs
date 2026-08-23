using HRDS.Web.Models;
using HRDS.Web.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Security.Claims;

namespace HRDS.Web.ViewComponents
{
    public class SidebarViewComponent : ViewComponent
    {
        private readonly HRDSContext _context;
        private readonly IConfiguration _configuration;

        public SidebarViewComponent(HRDSContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = UserClaimsPrincipal;
            if (user?.Identity?.IsAuthenticated != true)
            {
                return View(new SidebarMenuViewModel());
            }

            var isArabic = CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToLower() == "ar";
            var superAdminUsername = _configuration["SuperAdmin:Username"];
            var currentUsername = user.Identity.Name ?? user.FindFirst(ClaimTypes.Name)?.Value;

            // 1. التحقق مما إذا كان المستخدم هو SuperAdmin أو يمتلك صلاحية نظام كاملة
            bool isSuperAdmin = (!string.IsNullOrEmpty(superAdminUsername) &&
                                 string.Equals(currentUsername, superAdminUsername, StringComparison.OrdinalIgnoreCase)) ||
                                user.IsInRole("Admin") ||
                                user.HasClaim("Permission", "System.Admin");

            List<ModuleMenuItemDto> menuModules;

            if (isSuperAdmin)
            {
                // الـ SuperAdmin يعرض جميع الموديولات والشاشات المفعلة في النظام
                menuModules = await _context.Modules
                    .AsNoTracking()
                    .Where(m => m.IsActive)
                    .Select(m => new ModuleMenuItemDto
                    {
                        ModuleId = m.ModuleId,
                        ModuleCode = m.ModuleCode,
                        ModuleName = isArabic ? m.ModuleNameAr : m.ModuleNameEn,
                        IconClass = GetModuleIcon(m.ModuleCode),
                        Models = m.Models
                            .Where(md => md.IsActive)
                            .Select(md => new ModelMenuItemDto
                            {
                                ModelId = md.ModelId,
                                ModelCode = md.ModelCode,
                                ModelName = isArabic ? md.ModelNameAr : md.ModelNameEn,
                                Url = $"/{m.ModuleCode}/{md.ModelCode}/Index"
                            })
                            .ToList()
                    })
                    .ToListAsync();
            }
            else
            {
                // 2. للمستخدمين العاديين: جلب الموديولات والشاشات المصرح بها فقط من قاعدة البيانات
                var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!int.TryParse(userIdClaim, out var userId))
                {
                    return View(new SidebarMenuViewModel());
                }

                // استخراج معرفات الشاشات المصرح بها عبر أدوار المستخدم
                var allowedModelIds = await _context.UserAccesses
                    .AsNoTracking()
                    .Where(ua => ua.UserId == userId && ua.IsActive)
                    .SelectMany(ua => ua.UserRoles)
                    .Where(ur => ur.IsActive)
                    .Select(ur => ur.Role)
                    .Where(r => r.IsActive)
                    .SelectMany(r => r.RolePermissions)
                    .Where(rp => rp.IsActive)
                    .Select(rp => rp.Permission)
                    .Where(p => p.IsActive)
                    .Select(p => p.ModelId)
                    .Distinct()
                    .ToListAsync();

                menuModules = await _context.Modules
                    .AsNoTracking()
                    .Where(m => m.IsActive && m.Models.Any(md => md.IsActive && allowedModelIds.Contains(md.ModelId)))
                    .Select(m => new ModuleMenuItemDto
                    {
                        ModuleId = m.ModuleId,
                        ModuleCode = m.ModuleCode,
                        ModuleName = isArabic ? m.ModuleNameAr : m.ModuleNameEn,
                        IconClass = GetModuleIcon(m.ModuleCode),
                        Models = m.Models
                            .Where(md => md.IsActive && allowedModelIds.Contains(md.ModelId))
                            .Select(md => new ModelMenuItemDto
                            {
                                ModelId = md.ModelId,
                                ModelCode = md.ModelCode,
                                ModelName = isArabic ? md.ModelNameAr : md.ModelNameEn,
                                Url = $"/{m.ModuleCode}/{md.ModelCode}/Index"
                            })
                            .ToList()
                    })
                    .ToListAsync();
            }

            var viewModel = new SidebarMenuViewModel
            {
                Modules = menuModules
            };

            return View(viewModel);
        }

        private static string GetModuleIcon(string? moduleCode) => moduleCode?.Trim().ToUpperInvariant() switch
        {
            // Finance & Accounting
            "FIN" => "bi bi-cash-coin text-success",

            // Human Resources
            "HR" => "bi bi-people-fill text-warning",

            // Supply Chain Management
            "SCM" => "bi bi-diagram-3-fill text-primary",

            // Inventory & Warehouse
            "INV" => "bi bi-boxes text-primary",

            // Procurement & Purchasing
            "PUR" => "bi bi-cart-check-fill text-info",

            // Sales
            "SAL" => "bi bi-receipt-cutoff text-success",

            // Customer Relationship Management
            "CRM" => "bi bi-person-lines-fill text-info",

            // Marketing
            "MKT" => "bi bi-megaphone-fill text-danger",

            // Planning
            "PLAN" => "bi bi-calendar3 text-primary",

            // Manufacturing & Production
            "MFG" => "bi bi-gear-wide-connected text-secondary",

            // Engineering
            "ENG" => "bi bi-rulers text-dark",

            // Research & Development
            "RND" => "bi bi-lightbulb-fill text-warning",

            // Quality Assurance
            "QA" => "bi bi-shield-check text-success",

            // Quality Control
            "QC" => "bi bi-clipboard2-check-fill text-info",

            // Project Management
            "PM" => "bi bi-kanban-fill text-primary",

            // Maintenance & Asset Management
            "MAINT" => "bi bi-tools text-warning",

            // Logistics
            "LOG" => "bi bi-truck text-secondary",

            // Business Intelligence & Reporting
            "BI" => "bi bi-bar-chart-line-fill text-primary",

            // Workflow & Approvals
            "WF" => "bi bi-diagram-2-fill text-info",

            // Document Management
            "DOC" => "bi bi-file-earmark-text-fill text-secondary",

            // Security & Access Management
            "SECURITY" => "bi bi-shield-lock-fill text-danger",

            // System Configuration
            "CONFIG" or "CONFIGURATION" => "bi bi-gear-wide-connected text-info",

            // Audit & Activity Log
            "AUDIT" => "bi bi-journal-check text-warning",

            // Notifications & Alerts
            "NOTIFY" => "bi bi-bell-fill text-warning",

            // Integrations
            "INTEGRATION" => "bi bi-plug-fill text-success",

            // Core Data
            "COREDATA" => "bi bi-database-fill text-primary",

            // HR Configuration
            "HRCONFIG" => "bi bi-person-gear text-warning",

            "FINCONFIG" => "bi bi-gear-wide-connected text-success",

            // Default
            _ => "bi bi-folder2-open text-info"
        };
    }
}