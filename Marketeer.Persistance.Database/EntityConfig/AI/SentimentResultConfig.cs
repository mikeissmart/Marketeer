using Marketeer.Core.Domain.Entities.AI;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketeer.Persistance.Database.EntityConfig.AI
{
    public class SentimentResultConfig : IEntityConfig, IEntityTypeConfiguration<SentimentResult>
    {
        public void Configure(EntityTypeBuilder<SentimentResult> builder)
        {
            builder
                .HasIndex(x => new { x.HuggingFaceModelId, x.NewsArticleId })
                .IsUnique();
        }
    }
}
