using DogusCay.Business.Abstract;
using DogusCay.Business.Concrete;
using DogusCay.Business.Configurations;
using DogusCay.DataAccess.Abstract;
using DogusCay.DataAccess.Concrete;
using DogusCay.DataAccess.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DogusCay.API.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddServiceExtensions(this IServiceCollection services, IConfiguration configuration)
        {
            // Generic repository/service
            services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
            services.AddScoped(typeof(IGenericService<>), typeof(GenericManager<>));

            // Token ayarları ve JWT servisi
            services.Configure<JwtTokenOptions>(configuration.GetSection("TokenOptions"));
            services.AddScoped<IJwtService, JwtService>();

            // Uygulama özel servisleri
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICategoryService, CategoryManager>();

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductService, ProductManager>();

            services.AddScoped<IPointGroupTypeRepository, PointGroupTypeRepository>();
            services.AddScoped<IPointGroupTypeService, PointGroupTypeManager>();

            services.AddScoped<IPointRepository, PointRepository>();
            services.AddScoped<IPointService, PointManager>();

            services.AddScoped<IKanalRepository, KanalRepository>();
            services.AddScoped<IKanalService, KanalManager>();

            services.AddScoped<IPaymentTypeRepository, PaymentTypeRepository>();
            services.AddScoped<IPaymentTypeService, PaymentTypeManager>();

            services.AddScoped<ITalepFormRepository, TalepFormRepository>();
            services.AddScoped<ITalepFormService, TalepFormManager>();

            services.AddScoped<IDistributorRepository, DistributorRepository>();
            services.AddScoped<IDistributorService, DistributorManager>();

            services.AddScoped<IMalYuklemeTalepFormRepository, MalYuklemeTalepFormRepository>();
            services.AddScoped<IMalYuklemeTalepFormService, MalYuklemeTalepFormManager>();

        }
    }
}


