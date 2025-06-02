using System.Net.Http.Headers;
using System.Reflection;
using System.Text.Json.Serialization;
using DogusCay.WebUI.Services.TokenServices;
using DogusCay.WebUI.Services.UserServices;
using FluentValidation;
using FluentValidation.AspNetCore;  // FluentValidation namespace ekleyin
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddHttpContextAccessor();

//builder.Services.AddHttpClient("EduClient", cfg =>
//{
//    var tokenService = builder.Services.BuildServiceProvider().GetRequiredService<ITokenService>();
//    var token = tokenService.GetUserToken;
//    cfg.BaseAddress = new Uri("https://localhost:7076/api/");
//    if (token != null)
//    {
//        cfg.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenService.GetUserToken);
//    }
//});
builder.Services.AddHttpClient("EduClient", cfg =>
{
    cfg.BaseAddress = new Uri("https://localhost:7076/api/");
});


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddCookie(JwtBearerDefaults.AuthenticationScheme, opt =>
{
    opt.LoginPath = "/Login/SignIn";
    opt.LogoutPath = "/Login/Logout";
    opt.AccessDeniedPath = "/ErrorPage/AccessDenied";
    opt.Cookie.SameSite = SameSiteMode.Strict;
    opt.Cookie.HttpOnly = true;
    opt.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    opt.Cookie.Name = "DogusCayJwt";
    opt.SlidingExpiration = true;
});

builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly()).AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
builder.Services.AddControllersWithViews();

//consume iþlemi yapmak için ekle.
builder.Services.AddHttpClient();

var app = builder.Build();

// HTTP request pipeline'ý yapýlandýrýn
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Normal (area olmayan) controller'lar

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);
app.UseEndpoints(endpoints =>
{ 
    endpoints.MapControllerRoute(
        name: "areas",
        pattern: "{area=exists}/{controller=Home}/{action=Index}/{id?}"
    );
});



app.Run();
