using AutoMapper.Internal;
using Marketeer.Common;
using Marketeer.Infrastructure.Python;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Marketeer.Infrastructure
{
    public static class InfrastructureSetup
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, ConfigurationManager configuration)
        {
            services
                .AddServices();

            return services;
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            SetupHelper.ReflectionServiceRegister(services,
                typeof(IPythonService),
                typeof(BasePythonService),
                typeof(InfrastructureSetup).GetStaticMethod("AddPythonService"));

            /*var iType = typeof(IPythonService);
            var baseType = typeof(BasePythonService);
            var iSTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => iType.IsAssignableFrom(x) &&
                    x != iType);
            var sTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => baseType.IsAssignableFrom(x) &&
                    x != baseType);

            var addRepoMethod = typeof(InfrastructureSetup).GetStaticMethod("AddPythonService");
            foreach (var service in sTypes)
            {
                var iService = iSTypes.FirstOrDefault(x => service.IsAssignableTo(x) &&
                    x.IsInterface &&
                    x != service);
                if (iService == null)
                    throw new Exception($"Unknown IPythonService type {service}");
                else
                {
                    addRepoMethod.MakeGenericMethod(iService, service)
                        .Invoke(null, new[] { services });
                }
            }*/

            return services;
        }

        private static void AddPythonService<TIService, TService>(IServiceCollection services)
            where TIService : class
            where TService : class, TIService => services.AddTransient<TIService, TService>();
    }
}