using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using DogusCay.API.Extensions;
using DogusCay.Business.Configurations;
using DogusCay.Business.Validators;
using DogusCay.DataAccess.Context;
using DogusCay.Entity.Entities;

var builder = WebApplication.CreateBuilder(args);

// Service Config
builder.Services.AddServiceExtensions(builder.Configuration);
builder.Services.AddHttpContextAccessor();
builder.Services.AddAutoMapper(typeof(Program));

// Identity & Authentication
builder.Services.AddIdentity<AppUser, AppRole>()
    .AddEntityFrameworkStores<DogusCayContext>()
    .AddErrorDescriber<CustomErrorDescriber>();

// JWT Token Options
var tokenOptions = builder.Configuration.GetSection("TokenOptions").Get<JwtTokenOptions>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidIssuer = tokenOptions.Issuer,
        ValidAudience = tokenOptions.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions.Key)),
        ClockSkew = TimeSpan.Zero,
        NameClaimType = ClaimTypes.Name
    };
});

// ? CORS: WebUI (localhost:5055) eriþimine izin ver
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", policy =>
    {
        policy.WithOrigins(
                           "http://localhost:5055",    // HTTP
                           "https://localhost:5055")   // HTTPS
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// DbContext
builder.Services.AddDbContext<DogusCayContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection")));

// JSON ayarlarý ve Controller servisi
builder.Services.AddControllers()
    .AddJsonOptions(x =>
        x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Swagger yalnýzca development'ta açýk
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware sýralamasý ?
app.UseHttpsRedirection();

app.UseRouting();                 // Routing önce

app.UseCors("AllowLocalhost");   // ?? CORS tam burada

app.UseAuthentication();         // Kimlik doðrulama
app.UseAuthorization();          // Yetkilendirme

app.MapControllers();            // Endpoint'leri baðla

app.Run();
