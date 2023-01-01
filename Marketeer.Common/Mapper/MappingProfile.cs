﻿using AutoMapper;
using Marketeer.Core.Domain;
using Marketeer.Core.Domain.Dtos;
using System.Reflection;

namespace Marketeer.Common.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
            ApplyMappingsManually();
        }

        private void ApplyMappingsManually() => CreateMap(typeof(Paginate<>), typeof(PaginateDto<>));

        private void ApplyMappingsFromAssembly(Assembly assembly)
        {
            var mapFromTypes = assembly.GetTypes()
                .Where(x => x.GetInterfaces().Any(y =>
                    y.IsGenericType && y.GetGenericTypeDefinition() == typeof(IMapFrom<>)))
                .Select(x => new KeyValuePair<Type, IEnumerable<Type>>(
                    x,
                    x.GetInterfaces().Where(i => i.IsGenericType &&
                        i.GetGenericTypeDefinition() == typeof(IMapFrom<>))))
                .ToList();

            foreach (var typePair in mapFromTypes)
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
