using Marketeer.Core.Domain.Entities.Watch;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketeer.Persistance.Database.EntityConfig.Watch
{
    public class WatchTickerConfig : IEntityConfig, IEntityTypeConfiguration<WatchTicker>
    {
        public void Configure(EntityTypeBuilder<WatchTicker> builder) => builder
                .HasIndex(x => new { x.TickerId, x.AppUserId })
                .IsUnique();
    }
}
