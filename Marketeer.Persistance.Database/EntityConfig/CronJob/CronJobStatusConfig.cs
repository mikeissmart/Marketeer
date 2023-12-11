using Marketeer.Core.Domain.Entities.CronJob;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketeer.Persistance.Database.EntityConfig.CronJob
{
    public class CronJobStatusConfig : IEntityConfig, IEntityTypeConfiguration<CronJobStatus>
    {
        public void Configure(EntityTypeBuilder<CronJobStatus> builder) => builder
                .HasIndex(x => x.Name)
                .IsUnique();
    }
}
