using Marketeer.Core.Domain.Entities.Market;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marketeer.Persistance.Database.EntityConfig.Market
{
    public class HistoryDataEntityConfig : IEntityConfig, IEntityTypeConfiguration<HistoryData>
    {
        public void Configure(EntityTypeBuilder<HistoryData> builder) => builder
                .HasIndex(x => new { x.TickerId, x.Interval, x.Date })
                .IsUnique();
    }
}
