using Marketeer.Core.Domain.Entities.Market;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marketeer.Persistance.Database.EntityConfig.Market
{
    public class TickerEntityConfig : IEntityConfig, IEntityTypeConfiguration<Ticker>
    {
        public void Configure(EntityTypeBuilder<Ticker> builder)
        {
            builder.HasIndex(x => x.Symbol)
                .IsUnique();

            builder.Navigation(x => x.DelistReasons)
                .AutoInclude();
        }
    }
}
