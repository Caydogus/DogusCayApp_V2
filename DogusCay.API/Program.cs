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
using DogusCay.API.Services;
using Microsoft.OpenApi.Models;
using DogusCay.API.Helpers;

var builder = WebApplication.CreateBuilder(args);
// Service Config
builder.Services.AddServiceExtensions(builder.Configuration);
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
    options.RequireHttpsMetadata = false; // Required for HTTP usage
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidIssuer = tokenOptions.Issuer,
        ValidAudience = tokenOptions.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions.Key)),
        ClockSkew = TimeSpan.Zero,
        NameClaimType = ClaimTypes.NameIdentifier,
        RoleClaimType = ClaimTypes.Role
    };
});

// CORS
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowCustomOrigins", policy =>
    {
        policy
            .WithOrigins(allowedOrigins) // Allowed origins defined in appsettings.json
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

// Kestrel limiti
builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 200 * 1024 * 1024; // 200MB
});

// Form limiti
builder.Services.Configure<Microsoft.AspNetCore.Http.Features.FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 200 * 1024 * 1024; // 200MB
    options.ValueLengthLimit = int.MaxValue;
    options.MultipartHeadersLengthLimit = int.MaxValue;
});

var compatibilityLevel = builder.Configuration.GetSection("Database:CompatibilityLevel").Get<int>();

builder.Services.AddDbContext<DogusCayContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"), sqlOptions =>
    {
        sqlOptions.UseCompatibilityLevel(compatibilityLevel);
    }));


// JSON & Controllers
builder.Services.AddControllers()
    .AddJsonOptions(x =>
        x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "DogusCay API",
        Version = "v1"
    });

    // JWT Security Schema
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,   // ?? BURASI DEÐÝÞTÝ
        Scheme = "bearer",                // ?? lowercase
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter your JWT token."
        //In = ParameterLocation.Header,
        //Description = "Enter JWT token as 'Bearer <token>'.",
        //Name = "Authorization",
        //Type = SecuritySchemeType.ApiKey,
        //Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

// Swagger UI enabled in all environments
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "DogusCay API v1");
    c.RoutePrefix = "swagger";
});

// HTTPS redirection disabled for HTTP deployment
//app.UseHttpsRedirection();

app.UseRouting();
app.UseStaticFiles();
app.UseCors("AllowCustomOrigins");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
