using AutoMapper;
using Marketeer.Core.Domain.Dtos;
using System.Reflection;

namespace Marketeer.Core.Domain.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile() => ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());

        private void ApplyMappingsFromAssembly(Assembly assembly)
        {
            var types = assembly.GetExportedTypes()
                .Where(x => x.GetInterfaces().Any(y =>
                    y.IsGenericType && y.GetGenericTypeDefinition() == typeof(IMapFrom<>)))
                .Select(x => new KeyValuePair<Type, IEnumerable<Type>>(
                    x,
                    x.GetInterfaces().Where(i => i.IsGenericType &&
                        i.GetGenericTypeDefinition() == typeof(IMapFrom<>))))
                .ToList();

            foreach (var typePair in types)
            {
                var rootType = typePair.Key;
                var interfaceTypes = typePair.Value;

                var instance = Activator.CreateInstance(rootType);
                foreach (var interfaceType in interfaceTypes)
                {
                    var methodInfo = interfaceType.GetMethod("MapFrom")!;
                    methodInfo.Invoke(instance, new object[] { this });
                }

            }

            var mapToTypes = assembly.GetExportedTypes()
                .Where(x => x.GetInterfaces().Any(y =>
                    y.IsGenericType && y.GetGenericTypeDefinition() == typeof(IMapTo<>)))
                .Select(x => new KeyValuePair<Type, IEnumerable<Type>>(
                    x,
                    x.GetInterfaces().Where(i => i.IsGenericType &&
                        i.GetGenericTypeDefinition() == typeof(IMapTo<>))))
                .ToList();

            foreach (var typePair in mapToTypes)
            {
                var rootType = typePair.Key;
                var interfaceTypes = typePair.Value;

                var instance = Activator.CreateInstance(rootType);
                foreach (var interfaceType in interfaceTypes)
                {
                    var methodInfo = interfaceType.GetMethod("MapTo")!;
                    methodInfo.Invoke(instance, new object[] { this });
                }
            }
        }
    }
}
