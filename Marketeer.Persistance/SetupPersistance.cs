using AutoMapper.Internal;
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
                    configuration.GetConnectionString("DefaultConnection")));
            services.AddDbContext<LogDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection")));

            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            var iRepositoryType = typeof(IRepository);
            var repositoryType = typeof(Repository);
            var iRTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => iRepositoryType.IsAssignableFrom(x) &&
                    x != iRepositoryType);
            var rTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => repositoryType.IsAssignableFrom(x) &&
                    x != repositoryType);

            var addRepoMethod = typeof(SetupPersistance).GetStaticMethod("AddRepository");
            foreach (var repo in rTypes)
            {
                var iRepo = iRTypes.FirstOrDefault(x => repo.IsAssignableTo(x) &&
                    x.IsInterface &&
                    x != repo);
                if (iRepo == null)
                    throw new Exception($"Unknown IRepository type {repo}");
                else
                {
                    addRepoMethod.MakeGenericMethod(iRepo, repo)
                        .Invoke(null, new[] { services });
                }
            }

            return services;
        }

        private static void AddRepository<TIRepository, TRepository>(IServiceCollection services)
            where TIRepository : class
            where TRepository : class, TIRepository => services.AddScoped<TRepository>()
                .AddTransient<TIRepository, TRepository>();
    }
}
