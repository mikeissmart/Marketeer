using Marketeer.Core.Domain.Dtos;
using Marketeer.Core.Domain.Enums;
using Microsoft.Extensions.Logging;
using Reinforced.Typings.Ast.TypeNames;
using Reinforced.Typings.Fluent;

namespace Marketeer.UI.Spa.ViewModels
{
    public static class ReinforcedTypingsConfiguration
    {
        public static void Configure(ConfigurationBuilder builder)
        {
            var iRefactorType = typeof(IRefactorType);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => iRefactorType.IsAssignableFrom(x) &&
                    x != iRefactorType);

            builder.ExportAsInterfaces(types,
                c => c.WithPublicProperties(p => p
                    .ForceNullable())
                    .ExportTo("model.d.ts")
                    .DontIncludeToNamespace());

            builder.ExportAsEnums(new[]
                {
                    typeof(LogLevel),
                    typeof(HistoryDataIntervalEnum)
                }, c => c.ExportTo("model.enums.ts")
                    .DontIncludeToNamespace());

            // Global type substitutions
            builder.Substitute(typeof(DateTimeOffset), new RtSimpleTypeName("string"));
            builder.Substitute(typeof(Guid), new RtSimpleTypeName("string"));
            builder.Substitute(typeof(DateTime), new RtSimpleTypeName("Date"));
            builder.Substitute(typeof(DateTime?), new RtSimpleTypeName("Date"));

            // global type substitutions
            builder.Substitute(typeof(DateTimeOffset), new RtSimpleTypeName("string"));
            builder.Substitute(typeof(Guid), new RtSimpleTypeName("string"));
            builder.Substitute(typeof(DateTime), new RtSimpleTypeName("Date"));
            builder.Substitute(typeof(DateTime?), new RtSimpleTypeName("Date"));

            // Gobal settings
            builder.Global(x =>
            {
                x.CamelCaseForProperties();
                x.UseModules();
            });
        }
    }
}
