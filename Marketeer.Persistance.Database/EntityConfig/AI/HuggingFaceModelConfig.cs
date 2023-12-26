using Marketeer.Core.Domain.Entities.Market;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Marketeer.Core.Domain.Entities.AI;

namespace Marketeer.Persistance.Database.EntityConfig.AI
{
    public class HuggingFaceModelConfig : IEntityConfig, IEntityTypeConfiguration<HuggingFaceModel>
    {
        public void Configure(EntityTypeBuilder<HuggingFaceModel> builder)
        {
            builder
                .HasIndex(x => x.Name)
                .IsUnique();

            builder.HasData(new HuggingFaceModel
            {
                Id = 1,
                Name = "mrm8488/distilroberta-finetuned-financial-news-sentiment-analysis",
                IsDefault = true
            });
        }
    }
}
