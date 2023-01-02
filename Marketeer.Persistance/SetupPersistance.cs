using AutoMapper.Internal;
using Marketeer.Common;
using Marketeer.Persistance.Database.DbContexts;
using Marketeer.Persistance.Database.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Marketeer.Persistance
{
    public static class SetupPersistance
    {
        public static IServiceCollection AddPersistanceServices(this IServiceCollection services, ConfigurationManager configuration)
        {
            services
                .AddDbContexts(configuration)
                .AddRepositories();

            return services;
        }

        private static IServiceCollection AddDbContexts(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection")!));
            services.AddDbContext<LogDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection")!));

            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            SetupHelper.ReflectionServiceRegister(services,
                typeof(IRepositorySetup),
                typeof(IBaseRepositorySetup),
                typeof(SetupPersistance).GetStaticMethod("AddRepository"));

            return services;
        }

        private static void AddRepository<TIRepository, TRepository>(IServiceCollection services)
            where TIRepository : class
            where TRepository : class, TIRepository => services.AddTransient<TIRepository, TRepository>();
    }
}
