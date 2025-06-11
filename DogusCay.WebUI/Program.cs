using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Claims;
using System.Text.Json.Serialization;
using DogusCay.WebUI.Services.TokenServices;
using DogusCay.WebUI.Services.UserServices;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using DogusCay.WebUI.Handlers;

var builder = WebApplication.CreateBuilder(args);

// ---------------------- SESSION ve COOKIE POLICY ----------------------
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// 🔧 Cookie policy tanımlanmalı, yoksa bazı ortamlarda session cookie yazılmaz
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.MinimumSameSitePolicy = SameSiteMode.Lax;
});

// ----------------------------------------------------------------------

// Servis kayıtları
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddHttpContextAccessor();

// HttpClient için Token Header handler
builder.Services.AddTransient<AuthHeaderHandler>();
builder.Services.AddHttpClient("EduClient", cfg =>
{
    cfg.BaseAddress = new Uri("https://localhost:7076/api/");
})
.AddHttpMessageHandler<AuthHeaderHandler>();

// ------------------ COOKIE tabanlı JWT Authentication ------------------
builder.Services.AddAuthentication("DogusCookie")
    .AddCookie("DogusCookie", opt =>
    {
        opt.LoginPath = "/Login/SignIn";
        opt.LogoutPath = "/Login/Logout";
        opt.AccessDeniedPath = "/ErrorPage/AccessDenied";
        opt.Cookie.SameSite = SameSiteMode.Strict;
        opt.Cookie.HttpOnly = true;
        opt.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
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

builder.Services.AddControllersWithViews();

var app = builder.Build();

// -------------------- MIDDLEWARE PIPELINE --------------------
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// 🔥 CookiePolicy mutlaka burada olmalı!
app.UseCookiePolicy();

// 🔥 Session cookie yazılmadan önce çalışmalı
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=SignIn}/{id?}"
);

app.Run();
