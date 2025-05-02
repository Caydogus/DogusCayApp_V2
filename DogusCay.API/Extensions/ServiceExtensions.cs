using DogusCay.Business.Abstract;
using DogusCay.Business.Concrete;
using DogusCay.Business.Configurations;
using DogusCay.DataAccess.Abstract;
using DogusCay.DataAccess.Repositories;
using Microsoft.AspNetCore.Cors.Infrastructure;

namespace DogusCay.API.Extensions
{
    public static class ServiceExtensions
    {

        public static void AddServiceExtensions(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
            services.AddScoped(typeof(IGenericService<>), typeof(GenericManager<>));
          

            services.Configure<JwtTokenOptions>(configuration.GetSection("TokenOptions"));

            services.AddScoped<IJwtService, JwtService>();
        }

    }

}
