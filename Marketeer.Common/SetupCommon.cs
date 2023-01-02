using Marketeer.Common.Configs;
using Marketeer.Common.Mapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Marketeer.Common
{
    public static class SetupCommon
    {
        public static IServiceCollection AddCommonServices(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddAutoMapper(x => x.AddProfile(new MappingProfile()));

            var iConfigType = typeof(IConfig);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => iConfigType.IsAssignableFrom(x) &&
                    x != iConfigType);

            foreach (var type in types)
            {
                var config = configuration.GetSection(type.Name).Get(type);
                if (config == null)
                    throw new Exception($"Missing config in appsettings: {type}");

                services.AddSingleton(type, config);
            }

            return services;
        }
    }
}
