using AutoMapper.Internal;
using Marketeer.Common;
using Marketeer.Common.Mapper;
using Marketeer.Core.CronJob;
using Marketeer.Core.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Marketeer.Core
{
    public static class SetupCore
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services, ConfigurationManager configuration)
        {
            services
                .AddAutoMapper(x => x.AddProfile(new MappingProfile()))
                .AddServices()
                .AddCronJobs();

            return services;
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            SetupHelper.ReflectionServiceRegister(services,
                typeof(ICoreService),
                typeof(BaseCoreService),
                typeof(SetupCore).GetStaticMethod("AddService"));

            return services;
        }

        private static IServiceCollection AddCronJobs(this IServiceCollection services)
        {
            var cronType = typeof(BaseCronJobService);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => cronType.IsAssignableFrom(x) &&
                    !x.IsAbstract &&
                    x.IsClass &&
                    x != cronType);

            var addMethod = typeof(SetupCore).GetStaticMethod("AddCronJobService");
            foreach (var t in types)
            {
                addMethod.MakeGenericMethod(t)
                    .Invoke(null, new[] { services });
            }

            return services;
        }


        private static void AddService<TIService, TService>(IServiceCollection services)
            where TIService : class
            where TService : class, TIService => services.AddTransient<TIService, TService>();

        private static void AddCronJobService<T>(this IServiceCollection services) where T : BaseCronJobService => services.AddHostedService<T>();
    }
}
