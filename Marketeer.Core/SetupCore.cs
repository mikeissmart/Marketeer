using AutoMapper.Internal;
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
                .AddServices();

            #region CronJobServices

            /*var cronConfig = configuration.GetSection("CronConfig").Get<CronConfig>();
            services.AddCronJobService<CleanAppLogCronJobService>(x =>
                {
                    x.CronExpression = cronConfig.CleanAppLog;
                    x.TimeZoneInfo = TimeZoneInfo.Local;
                })
                .AddCronJobService<CleanCronLogCronJobService>(x =>
                {
                    x.CronExpression = cronConfig.CleanCronLog;
                    x.TimeZoneInfo = TimeZoneInfo.Local;
                });*/

            #endregion

            return services;
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            var iServiceType = typeof(IService);
            var serviceType = typeof(BaseService);
            var iSTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => iServiceType.IsAssignableFrom(x) &&
                    x != iServiceType);
            var sTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => serviceType.IsAssignableFrom(x) &&
                    x != serviceType);

            var addRepoMethod = typeof(SetupCore).GetStaticMethod("AddService");
            foreach (var service in sTypes)
            {
                var iService = iSTypes.FirstOrDefault(x => service.IsAssignableTo(x) &&
                    x.IsInterface &&
                    x != service);
                if (iService == null)
                    throw new Exception($"Unknown IService type {service}");
                else
                {
                    addRepoMethod.MakeGenericMethod(iService, service)
                        .Invoke(null, new[] { services });
                }
            }

            return services;
        }

        private static void AddService<TIService, TService>(IServiceCollection services)
            where TIService : class
            where TService : class, TIService => services.AddTransient<TIService, TService>();

        private static IServiceCollection AddCronJobService<T>(this IServiceCollection services, Action<ICronJobConfig<T>> options) where T : BaseCronJobService
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options), $"No options provided for {nameof(T)}");
            var config = new CronJobConfig<T>();
            options(config);
            if (config.CronExpression == "")
                throw new ArgumentNullException(nameof(CronJobConfig<T>.CronExpression), $"Empty cron expression for {nameof(T)}");
            services.AddSingleton<ICronJobConfig<T>>(config);
            services.AddHostedService<T>();

            return services;
        }
    }
}
