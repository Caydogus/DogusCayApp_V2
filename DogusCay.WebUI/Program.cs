using DogusCay.WebUI.Handlers;
using DogusCay.WebUI.Services.TokenServices;
using DogusCay.WebUI.Services.UserServices;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using System.Reflection;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Session (HTTP yayında Secure=None olmalı)
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.None; // ÖNEMLİ: HTTP için
    options.Cookie.SameSite = SameSiteMode.Lax;
});

// CookiePolicy
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.MinimumSameSitePolicy = SameSiteMode.Lax;
});

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<AuthHeaderHandler>();

// Canlı API adresi (HTTP)
var apiBase = builder.Configuration["ApiSettings:BaseUrl"];
Console.WriteLine($"🔍 ApiSettings:BaseUrl = {apiBase}");
if (string.IsNullOrWhiteSpace(apiBase))
    throw new InvalidOperationException("❌ ApiSettings:BaseUrl değeri appsettings dosyalarında tanımlı değil!");

// HttpClient
builder.Services.AddHttpClient("EduClient", cfg =>
{
    cfg.BaseAddress = new Uri(apiBase);
})
.AddHttpMessageHandler<AuthHeaderHandler>();

// Cookie Auth (HTTP: Secure=None)
builder.Services.AddAuthentication("DogusCookie")
    .AddCookie("DogusCookie", opt =>
    {
        opt.LoginPath = "/Login/SignIn";
        opt.LogoutPath = "/Login/Logout";
        opt.AccessDeniedPath = "/ErrorPage/AccessDenied";
        opt.Cookie.SameSite = SameSiteMode.Lax;
        opt.Cookie.HttpOnly = true;
        opt.Cookie.SecurePolicy = CookieSecurePolicy.None; // ÖNEMLİ: HTTP için
        opt.Cookie.Name = "DogusCayJwt";
        opt.SlidingExpiration = true;

        opt.Events.OnSigningIn = async context =>
        {
            var identity = (ClaimsIdentity)context.Principal.Identity;
            if (!identity.HasClaim(c => c.Type == ClaimTypes.NameIdentifier))
            {
                var userId = context.Principal.Claims
                    .FirstOrDefault(c => c.Type == "UserId" || c.Type.EndsWith("nameidentifier"))?.Value;

                if (userId != null)
                {
                    identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, userId));
                }
            }
            await Task.CompletedTask;
        };
    });

// FluentValidation
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())
    .AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters();

// MVC
builder.Services.AddControllersWithViews();

// --- Kültür Ayarları: Para birimi ₺, tarih formatı Türkçe ---
var cultureInfo = new CultureInfo("tr-TR");
cultureInfo.NumberFormat.CurrencySymbol = "₺";
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

var app = builder.Build();

// --- Localization Middleware ---
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("tr-TR"),
    SupportedCultures = new[] { cultureInfo },
    SupportedUICultures = new[] { cultureInfo }
});

// Production hataları
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // app.UseHsts(); // HTTP yayında KAPALI
}

// *** ÖNEMLİ: HTTPS yönlendirmeyi kapat ***
/*app.UseHttpsRedirection(); */ // HTTP’de sorun çıkarır

app.UseStaticFiles();
app.UseRouting();
app.UseCookiePolicy();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=SignIn}/{id?}"
);

app.Run();
