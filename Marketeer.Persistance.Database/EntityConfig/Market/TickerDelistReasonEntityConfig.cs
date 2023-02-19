using Marketeer.Core.Domain.Entities.Market;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketeer.Persistance.Database.EntityConfig.Market
{
    public class TickerDelistReasonEntityConfig : IEntityConfig, IEntityTypeConfiguration<TickerDelistReason>
    {
        public void Configure(EntityTypeBuilder<TickerDelistReason> builder)
        {
            builder
                .HasIndex(x => new { x.TickerId, x.Delist })
                .IsUnique();
        }
    }
}
