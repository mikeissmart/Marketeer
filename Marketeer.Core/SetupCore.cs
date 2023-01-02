using AutoMapper.Internal;
using Marketeer.Common;
using Marketeer.Common.Configs;
using Marketeer.Common.Mapper;
using Marketeer.Core.CronJob;
using Marketeer.Core.CronJob.Market;
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

            var cronConfig = configuration.GetSection("CronConfig").Get<CronConfig>()!;
            services.AddCronJobService<ClearDisabledFetchHistoryDataCronJob>(x =>
                {
                    x.CronExpression = cronConfig.ClearDisabledFetchHistoryData;
                    x.TimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("US Eastern Standard Time");
                });

            #endregion

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
