using DogusCay.API.Background;
using DogusCay.API.Helpers;
using DogusCay.Business.Abstract;
using DogusCay.Business.Concrete;
using DogusCay.Business.Configurations;
using DogusCay.Business.Importer;
using DogusCay.DataAccess.Abstract;
using DogusCay.DataAccess.Concrete;
using DogusCay.DataAccess.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace DogusCay.API.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddServiceExtensions(this IServiceCollection services, IConfiguration configuration)
        {
            //  Genel servisler
            services.AddHttpContextAccessor();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddSingleton<MailHelper>();

            // Generic servisler
            services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
            services.AddScoped(typeof(IGenericService<>), typeof(GenericManager<>));

            //JWT
            services.Configure<JwtTokenOptions>(configuration.GetSection("TokenOptions"));
            services.AddScoped<IJwtService, JwtService>();

            // User
            services.AddScoped<IUserService, UserService>();

            //Category
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICategoryService, CategoryManager>();

            //Product
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductService, ProductManager>();

            //PointGroupType
            services.AddScoped<IPointGroupTypeRepository, PointGroupTypeRepository>();
            services.AddScoped<IPointGroupTypeService, PointGroupTypeManager>();

            // Point
            services.AddScoped<IPointRepository, PointRepository>();
            services.AddScoped<IPointService, PointManager>();

            // Kanal
            services.AddScoped<IKanalRepository, KanalRepository>();
            services.AddScoped<IKanalService, KanalManager>();

            //  PaymentType
            services.AddScoped<IPaymentTypeRepository, PaymentTypeRepository>();
            services.AddScoped<IPaymentTypeService, PaymentTypeManager>();

            //  TalepForm
            services.AddScoped<ITalepFormRepository, TalepFormRepository>();
            services.AddScoped<ITalepFormService, TalepFormManager>();

            //  Distributor
            services.AddScoped<IDistributorRepository, DistributorRepository>();
            services.AddScoped<IDistributorService, DistributorManager>();

            // MalYuklemeTalepForm
            services.AddScoped<IMalYuklemeTalepFormRepository, MalYuklemeTalepFormRepository>();
            services.AddScoped<IMalYuklemeTalepFormService, MalYuklemeTalepFormManager>();

            // Importer Servisleri
            services.AddScoped<IDistributorExcelImporter, DistributorExcelImporter>();
            services.AddHostedService<DistributorImportBackgroundService>();

            services.AddScoped<IPointExcelImporter, PointExcelImporter>();
            services.AddHostedService<PointImportBackgroundService>();

            services.AddScoped<IPivotRepository, PivotRepository>();
            services.AddScoped<IPivotService, PivotManager>();

            services.AddScoped<IIhaleAnlasmaSozlesmeRepository, IhaleAnlasmaSozlesmeRepository>();
            services.AddScoped<IIhaleAnlasmaSozlesmeService, IhaleAnlasmaSozlesmeManager>();
        }
    }
}
