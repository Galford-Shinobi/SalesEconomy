using Microsoft.EntityFrameworkCore;
using SalesEconomy.BackEnd.Helpers;
using SalesEconomy.BackEnd.Intertfaces;
using SalesEconomy.BackEnd.Repositories;
using SalesEconomy.BackEnd.Services;
using SalesEconomy.BackEnd.UnitsOfWork;

namespace SalesEconomy.BackEnd.Data
{
    public static class Dependence
    {
        public static void InjectDependencies(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<DataContext>(options => {
                options.UseSqlServer(configuration.GetConnectionString("DockerConnection"));
            });


            services.AddScoped(typeof(IGenericUnitOfWork<>), typeof(GenericUnitOfWork<>));
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IApiService, ApiService>();
            services.AddTransient<SeedDb>();

            services.AddScoped<IFileStorage, FileStorage>();
            services.AddScoped<IMailHelper, MailHelper>();
            services.AddScoped<IOrdersHelper, OrdersHelper>();
        }
    }
}
