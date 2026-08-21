using HRDS.Web.Models;
using HRDS.Web.Models.Entities;
using HRDS.Web.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace HRDS.Web.Areas.Security.Controllers
{
    [Area("Security")]
    [HasModuleAccess("SECURITY")]
    public class UsersController : Controller
    {
        private readonly HRDSContext _context;

        public UsersController(HRDSContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetUsersJson()
        {
            bool isEnglish = CultureInfo.CurrentCulture
                .TwoLetterISOLanguageName
                .Equals("en", StringComparison.OrdinalIgnoreCase);

            var usersList = await _context.Users
                .AsNoTracking()
                .Select(u => new
                {
                    u.UserId,
                    u.Username,
                    u.Email,
                    u.IsActive,

                    Roles = u.UserAccesses
                        .SelectMany(ua => ua.UserRoles)
                        .Where(ur => ur.IsActive && ur.Role != null)
                        .Select(ur => isEnglish
                            ? ur.Role.RoleNameEn
                            : ur.Role.RoleNameAr)
                        .Distinct()
                })
                .OrderBy(u => u.UserId)
                .ToListAsync();

            var result = usersList.Select(u => new UserViewModel
            {
                UserId = u.UserId,
                Username = u.Username,
                Email = u.Email,
                IsActive = u.IsActive,
                Roles = string.Join(", ", u.Roles)
            }).ToList();

            return Json(new { data = result });
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await PopulateDropdownsAsync();
            return View(new CreateUserViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropdownsAsync(model.CompanyId);
                return View(model);
            }

            // 1. إنشاء المستخدم الأساسي
            var user = new User
            {
                Username = model.Username,
                Email = model.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
                IsActive = model.IsActive,
                FailedLoginCount = 0
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // 2. إنشاء سجل صلاحيات الوصول (UserAccess)
            var userAccess = new UserAccess
            {
                UserId = user.UserId,
                CompanyId = model.CompanyId,
                CompanyBranchId = model.CompanyBranchId,
                IsDefault = true,
                IsActive = true
            };

            _context.UserAccesses.Add(userAccess);
            await _context.SaveChangesAsync();

            // 3. ربط الأدوار المختارة (UserRoles)
            if (model.SelectedRoleIds != null && model.SelectedRoleIds.Any())
            {
                foreach (var roleId in model.SelectedRoleIds)
                {
                    _context.UserRoles.Add(new UserRole
                    {
                        UserAccessId = userAccess.UserAccessId,
                        RoleId = roleId,
                        AssignedAt = DateTime.Now,
                        IsActive = true
                    });
                }
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            bool isEnglish = CultureInfo.CurrentCulture
                .TwoLetterISOLanguageName
                .Equals("en", StringComparison.OrdinalIgnoreCase);

            // ==========================================
            // 1. جلب المستخدم
            // ==========================================
            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null)
                return NotFound();

            // ==========================================
            // 2. جلب صلاحيات الوصول الخاصة بالمستخدم
            // ==========================================
            var userAccesses = await _context.UserAccesses
                .AsNoTracking()
                .Where(ua => ua.UserId == id)
                .ToListAsync();

            // ==========================================
            // 3. جلب جميع الفروع النشطة مع الشركات
            // ==========================================
            var allBranches = await _context.CompanyBranches
                .AsNoTracking()
                .Include(b => b.Company)
                .Where(b => b.IsActive)
                .ToListAsync();

            // ==========================================
            // 4. تجهيز قائمة الشركات والفروع
            // ==========================================
            var userBranchesAccess = allBranches
                .Select(branch =>
                {
                    var userAccess = userAccesses.FirstOrDefault(ua =>
                        ua.CompanyId == branch.CompanyId &&
                        ua.CompanyBranchId == branch.CompanyBranchId &&
                        ua.IsActive);

                    string companyName = "-";

                    if (branch.Company != null)
                    {
                        companyName = isEnglish
                            ? (!string.IsNullOrWhiteSpace(branch.Company.CompanyNameEn)
                                ? branch.Company.CompanyNameEn
                                : branch.Company.CompanyNameAr)
                            : (!string.IsNullOrWhiteSpace(branch.Company.CompanyNameAr)
                                ? branch.Company.CompanyNameAr
                                : branch.Company.CompanyNameEn);
                    }

                    string branchName = isEnglish
                        ? (!string.IsNullOrWhiteSpace(branch.BranchNameEn)
                            ? branch.BranchNameEn
                            : branch.BranchNameAr)
                        : (!string.IsNullOrWhiteSpace(branch.BranchNameAr)
                            ? branch.BranchNameAr
                            : branch.BranchNameEn);

                    return new UserAccessItemViewModel
                    {
                        CompanyId = branch.CompanyId,

                        CompanyName = string.IsNullOrWhiteSpace(companyName)
                            ? "-"
                            : companyName,

                        CompanyBranchId = branch.CompanyBranchId,

                        BranchName = string.IsNullOrWhiteSpace(branchName)
                            ? "-"
                            : branchName,

                        IsSelected = userAccess != null,

                        IsDefault = userAccess?.IsDefault == true
                    };
                })
                .ToList();

            // ==========================================
            // 5. جلب الأدوار الخاصة بالمستخدم
            // ==========================================
            var accessIds = userAccesses
                .Select(ua => ua.UserAccessId)
                .ToList();

            var selectedRoles = await _context.UserRoles
                .AsNoTracking()
                .Where(ur =>
                    accessIds.Contains(ur.UserAccessId) &&
                    ur.IsActive)
                .Select(ur => ur.RoleId)
                .Distinct()
                .ToListAsync();

            // ==========================================
            // 6. تحديد الـ Default Access
            // ==========================================
            var defaultAccess =
                userAccesses.FirstOrDefault(ua => ua.IsDefault)
                ?? userAccesses.FirstOrDefault();

            // ==========================================
            // 7. بناء ViewModel
            // ==========================================
            var model = new EditUserViewModel
            {
                UserId = user.UserId,

                Username = user.Username,

                Email = user.Email,

                IsActive = user.IsActive,

                CompanyId = defaultAccess?.CompanyId,

                CompanyBranchId = defaultAccess?.CompanyBranchId,

                SelectedRoleIds = selectedRoles,

                UserBranchesAccess = userBranchesAccess
            };

            // ==========================================
            // 8. جلب الأدوار المتاحة
            // ==========================================
            ViewBag.Roles = await _context.Roles
                .AsNoTracking()
                .Where(r => r.IsActive)
                .ToListAsync();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            EditUserViewModel model,
            int? DefaultBranchId)
        {
            // ==========================================
            // 1. تنظيف Validation الخاصة بجدول Access
            // ==========================================
            var accessKeys = ModelState.Keys
                .Where(k =>
                    k.StartsWith("UserBranchesAccess") ||
                    k == "CompanyId" ||
                    k == "CompanyBranchId")
                .ToList();

            foreach (var key in accessKeys)
            {
                ModelState.Remove(key);
            }

            // ==========================================
            // 2. التحقق من Validation
            // ==========================================
            if (!ModelState.IsValid)
            {
                ViewBag.Roles = await _context.Roles
                    .Where(r => r.IsActive)
                    .ToListAsync();

                return View(model);
            }

            // ==========================================
            // 3. جلب المستخدم
            // ==========================================
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == model.UserId);

            if (user == null)
                return NotFound();

            // ==========================================
            // 4. تحديث البيانات الأساسية للمستخدم
            // ==========================================
            user.Username = model.Username;
            user.Email = model.Email;
            user.IsActive = model.IsActive;

            // تحديث كلمة المرور فقط إذا تم إدخال كلمة مرور جديدة
            if (!string.IsNullOrWhiteSpace(model.NewPassword))
            {
                user.PasswordHash =
                    BCrypt.Net.BCrypt.HashPassword(model.NewPassword);

                user.PasswordChangedAt = DateTime.Now;
            }

            // ==========================================
            // 5. تجهيز قائمة الفروع
            // ==========================================
            model.UserBranchesAccess ??=
                new List<UserAccessItemViewModel>();

            // الفروع التي قام المستخدم بتحديدها
            var selectedBranches = model.UserBranchesAccess
                .Where(x =>
                    x.IsSelected &&
                    x.CompanyBranchId.HasValue)
                .ToList();

            // ==========================================
            // 6. تحديد الفرع الافتراضي
            // ==========================================
            UserAccessItemViewModel? defaultBranch = null;

            // لو المستخدم اختار Default من الـ Radio Button
            if (DefaultBranchId.HasValue)
            {
                defaultBranch = selectedBranches
                    .FirstOrDefault(x =>
                        x.CompanyBranchId == DefaultBranchId.Value);
            }

            // لو لم يتم اختيار Default
            // نأخذ أول فرع مختار كـ Default
            if (defaultBranch == null && selectedBranches.Any())
            {
                defaultBranch = selectedBranches.First();
            }

            // ==========================================
            // 7. جلب كل UserAccess الموجودة للمستخدم
            // ==========================================
            var existingAccesses = await _context.UserAccesses
                .Where(ua => ua.UserId == model.UserId)
                .Include(ua => ua.UserRoles)
                .ToListAsync();

            // ==========================================
            // 8. تحديث / إضافة صلاحيات الفروع
            // ==========================================
            foreach (var selected in selectedBranches)
            {
                var existingAccess = existingAccesses
                    .FirstOrDefault(ua =>
                        ua.CompanyId == selected.CompanyId &&
                        ua.CompanyBranchId == selected.CompanyBranchId);

                if (existingAccess == null)
                {
                    existingAccess = new UserAccess
                    {
                        UserId = model.UserId,
                        CompanyId = selected.CompanyId,
                        CompanyBranchId = selected.CompanyBranchId,
                        IsActive = true,
                        IsDefault = false
                    };

                    _context.UserAccesses.Add(existingAccess);
                    existingAccesses.Add(existingAccess);
                }
                else
                {
                    existingAccess.IsActive = true;
                }
            }

            // ==========================================
            // 9. إلغاء الفروع التي لم تعد مختارة
            // ==========================================
            foreach (var existingAccess in existingAccesses)
            {
                var stillSelected = selectedBranches.Any(x =>
                    x.CompanyId == existingAccess.CompanyId &&
                    x.CompanyBranchId == existingAccess.CompanyBranchId);

                if (!stillSelected)
                {
                    existingAccess.IsActive = false;
                    existingAccess.IsDefault = false;
                }
            }

            // ==========================================
            // تحديث الـ Default Branch
            // ==========================================

            // أولاً: إلغاء الـ Default الحالي
            foreach (var access in existingAccesses)
            {
                access.IsDefault = false;
            }

            await _context.SaveChangesAsync();

            // ثانياً: تعيين الـ Default الجديد
            if (defaultBranch != null)
            {
                var newDefaultAccess = existingAccesses
                    .FirstOrDefault(ua =>
                        ua.CompanyId == defaultBranch.CompanyId &&
                        ua.CompanyBranchId == defaultBranch.CompanyBranchId &&
                        ua.IsActive);

                if (newDefaultAccess != null)
                {
                    newDefaultAccess.IsDefault = true;
                }
            }

            // ==========================================
            // 10. تحديث الأدوار Roles
            // ==========================================
            model.SelectedRoleIds ??=
                new List<int>();

            // الحصول على الـ Access النشطة فقط
            var activeAccesses = existingAccesses
                .Where(ua => ua.IsActive)
                .ToList();

            foreach (var access in activeAccesses)
            {
                // الأدوار الحالية لهذا الـ Access
                var currentRoles = access.UserRoles.ToList();

                // ------------------------------------------
                // حذف الأدوار التي لم تعد مختارة
                // ------------------------------------------
                foreach (var existingRole in currentRoles)
                {
                    if (!model.SelectedRoleIds.Contains(existingRole.RoleId))
                    {
                        _context.UserRoles.Remove(existingRole);
                    }
                }

                // ------------------------------------------
                // الحصول على IDs الأدوار الحالية
                // ------------------------------------------
                var currentRoleIds = currentRoles
                    .Select(r => r.RoleId)
                    .ToHashSet();

                // ------------------------------------------
                // إضافة الأدوار الجديدة
                // ------------------------------------------
                foreach (var roleId in model.SelectedRoleIds)
                {
                    if (!currentRoleIds.Contains(roleId))
                    {
                        _context.UserRoles.Add(new UserRole
                        {
                            UserAccess = access,

                            RoleId = roleId,

                            AssignedAt = DateTime.Now,

                            IsActive = true
                        });
                    }
                }
            }

            // ==========================================
            // 11. حفظ كل التعديلات
            // ==========================================
            await _context.SaveChangesAsync();

            // ==========================================
            // 12. الرجوع لقائمة المستخدمين
            // ==========================================
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return Json(new { success = false, message = "المستخدم غير موجود" });
            }

            user.IsActive = !user.IsActive;
            await _context.SaveChangesAsync();

            return Json(new { success = true, isActive = user.IsActive, message = "تم تغيير حالة الحساب بنجاح" });
        }

        // Endpoint لاستجابة الـ AJAX الخاصة بالقائمة المنسدلة للفروع
        [HttpGet]
        public async Task<IActionResult> GetBranchesByCompany(int companyId)
        {
            bool isEnglish = CultureInfo.CurrentCulture.TwoLetterISOLanguageName.Equals("en", StringComparison.OrdinalIgnoreCase);

            var branches = await _context.CompanyBranches
                .Where(b => b.CompanyId == companyId && b.IsActive)
                .Select(b => new
                {
                    id = b.CompanyBranchId,
                    name = isEnglish ? b.BranchNameEn : b.BranchNameAr
                })
                .ToListAsync();

            return Json(branches);
        }

        // دالة مسبقة لملء الـ Dropdowns الخاصة بالشركات، الفروع، والأدوار
        private async Task PopulateDropdownsAsync(int? selectedCompanyId = null)
        {
            bool isEnglish = CultureInfo.CurrentCulture.TwoLetterISOLanguageName.Equals("en", StringComparison.OrdinalIgnoreCase);

            ViewBag.Companies = new SelectList(
                await _context.Companies.Where(c => c.IsActive).ToListAsync(),
                "CompanyId",
                isEnglish ? "CompanyNameEn" : "CompanyNameAr"
            );

            if (selectedCompanyId.HasValue)
            {
                ViewBag.CompanyBranches = new SelectList(
                    await _context.CompanyBranches.Where(b => b.CompanyId == selectedCompanyId.Value && b.IsActive).ToListAsync(),
                    "CompanyBranchId",
                    isEnglish ? "BranchNameEn" : "BranchNameAr"
                );
            }
            else
            {
                ViewBag.CompanyBranches = new SelectList(Enumerable.Empty<SelectListItem>());
            }

            ViewBag.Roles = await _context.Roles.Where(r => r.IsActive).ToListAsync();
        }
    }
}