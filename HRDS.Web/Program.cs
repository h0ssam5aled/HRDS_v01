using System.Globalization;
using HRDS.Web.Models.Entities;
using HRDS.Web.Security; // إضافة النيم سبيس الخاص بالصلاحيات
using HRDS.Web.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// 1. إضافة خدمات الترجمة وتحديد مجلد Resources
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.AddControllersWithViews()
    .AddViewLocalization()
    .AddDataAnnotationsLocalization(options => {
        options.DataAnnotationLocalizerProvider = (type, factory) =>
            factory.Create(typeof(HRDS.Web.Resources.Resource));
    });

// 2. تسجيل IHttpContextAccessor والـ Interceptor للتتبع التلقائي
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<AuditSaveChangesInterceptor>();

// 3. ربط قاعدة البيانات HRDSContext وتفعيل الـ Interceptor مرة واحدة فقط
builder.Services.AddDbContext<HRDSContext>((sp, options) =>
{
    var interceptor = sp.GetRequiredService<AuditSaveChangesInterceptor>();
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
           .AddInterceptors(interceptor);
});

// 4. إضافة نظام الـ Cookie Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Security/Account/Login";
        options.AccessDeniedPath = "/Security/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
    });

// 5. ضبط اللغات المدعومة
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[]
    {
        new CultureInfo("ar"),
        new CultureInfo("ar-EG"),
        new CultureInfo("en"),
        new CultureInfo("en-US")
    };

    options.DefaultRequestCulture = new RequestCulture("ar", "ar");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;

    options.RequestCultureProviders.Clear();
    options.RequestCultureProviders.Add(new CookieRequestCultureProvider
    {
        CookieName = CookieRequestCultureProvider.DefaultCookieName
    });
});

// 6. تسجيل خدمات نظام الصلاحيات المخصص (Custom Module Authorization)
builder.Services.AddSingleton<IAuthorizationPolicyProvider, ModulePolicyProvider>();
builder.Services.AddScoped<IAuthorizationHandler, ModuleAccessHandler>();

// 1. إضافة خدمة Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(8); // مدة الجلسة
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddHttpClient();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// الترتيب الصحيح للميدل وير:
app.UseRouting();

// تفعيل Localization بعد الـ Routing وقبل الـ Auth
var localizationOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(localizationOptions.Value);

// 2. تفعيل Middleware الـ Session (يجب وضعه قبل UseAuthorization)
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();