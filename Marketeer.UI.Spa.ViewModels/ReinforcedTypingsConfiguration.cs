using Marketeer.Core.Domain.Dtos;
using Marketeer.Core.Domain.Enums;
using Marketeer.UI.Spa.ViewModels.GenericDtos;
using Microsoft.Extensions.Logging;
using Reinforced.Typings;
using Reinforced.Typings.Ast.TypeNames;
using Reinforced.Typings.Fluent;

namespace Marketeer.UI.Spa.ViewModels
{
    public static class ReinforcedTypingsConfiguration
    {
        private const string DTOFILE = "model.d.ts";
        private const string ENUMFILE = "model.enum.ts";

        public static void Configure(ConfigurationBuilder builder)
        {
            // OverrideName breaks generic class names
            var specialDtos = new List<Type>
            {
                typeof(Paginate),
                typeof(PaginateGeneric<>),
                typeof(PaginateGenericFilter<>)
            };
            builder.ExportAsInterfaces(specialDtos, x => x
                .FlattenHierarchy()
                .WithPublicProperties(p =>
                {
                    if (p.Member.IsReferenceForcedNullable())
                        p.ForceNullable();
                })
                .ExportTo(DTOFILE)
                .DontIncludeToNamespace());

            var iRefactorType = typeof(IRefactorType);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => iRefactorType.IsAssignableFrom(x) &&
                    x != iRefactorType &&
                    !x.IsGenericType);
            builder.ExportAsInterfaces(types, x => x
                .FlattenHierarchy()
                .WithPublicProperties(p =>
                {
                    if (p.Member.IsReferenceForcedNullable())
                        p.ForceNullable();
                })
                .ExportTo(DTOFILE)
                .OverrideName(x.Type.Name.Replace("Dto", ""))
                .DontIncludeToNamespace());

            builder.ExportAsEnums(new[]
                {
                    typeof(LogLevel),
                    typeof(HistoryDataIntervalEnum),
                    typeof(DelistEnum)
                }, x =>
                {
                    x.ExportTo(ENUMFILE)
                        .DontIncludeToNamespace();
                    if (!x.Type.Name.Contains("Enum"))
                        x.OverrideName(x.Type.Name + "Enum");
                });

            // Global type substitutions
            builder.Substitute(typeof(DateTimeOffset), new RtSimpleTypeName("string"));
            builder.Substitute(typeof(Guid), new RtSimpleTypeName("string"));
            builder.Substitute(typeof(DateTime), new RtSimpleTypeName("Date"));
            builder.Substitute(typeof(DateTime?), new RtSimpleTypeName("Date"));

            // Gobal settings
            builder.Global(x =>
            {
                x.CamelCaseForProperties()
                    .UseModules()
                    .AutoOptionalProperties();
            });
        }
    }
}
