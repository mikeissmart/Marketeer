﻿using AutoMapper.Internal;
using Marketeer.Common;
using Marketeer.Infrastructure.External;
using Marketeer.Infrastructure.External.Market;
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

        public static void SetupInfrastructureServices(this IServiceScope scope)
        {
            var setupService = scope.ServiceProvider
                .GetRequiredService<IPythonSetupService>();

            setupService.CreatePythonEnvironmentAsync().Wait();
            setupService.InstallPackagesAsync().Wait();
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            SetupHelper.ReflectionServiceRegister(services,
                typeof(IHttpClient),
                typeof(BaseHttpClient),
                typeof(InfrastructureSetup).GetStaticMethod("AddHttpClientService"));

            SetupHelper.ReflectionServiceRegister(services,
                typeof(IPythonService),
                typeof(BasePythonService),
                typeof(InfrastructureSetup).GetStaticMethod("AddPythonService"));

            AddHttpClientService<INasdaqApiClient, NasdaqApiClient>(services);

            return services;
        }

        private static void AddHttpClientService<TIService, TService>(IServiceCollection services)
            where TIService : class
            where TService : class, TIService => services.AddTransient<TIService, TService>();

        private static void AddPythonService<TIService, TService>(IServiceCollection services)
            where TIService : class
            where TService : class, TIService => services.AddTransient<TIService, TService>();
    }
}