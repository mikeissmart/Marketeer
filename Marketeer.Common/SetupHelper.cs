using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Marketeer.Common
{
    public static class SetupHelper
    {
        public static void ReflectionServiceRegister(IServiceCollection services, Type iType, Type type, MethodInfo addMethod)
        {
            var iTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => iType.IsAssignableFrom(x) &&
                    x.IsInterface &&
                    x != iType);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => type.IsAssignableFrom(x) &&
                    !x.IsAbstract &&
                    x.IsClass &&
                    x != type);

            foreach (var t in types)
            {
                var i = iTypes.FirstOrDefault(x => t.IsAssignableTo(x) &&
                    x.IsInterface &&
                    x != t);
                if (i == null)
                    throw new Exception($"Unknown {iType}: {t}");

                addMethod.MakeGenericMethod(i, t)
                    .Invoke(null, new[] { services });
            }
        }
    }
}
