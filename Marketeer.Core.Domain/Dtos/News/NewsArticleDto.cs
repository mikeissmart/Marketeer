using Marketeer.Core.Domain.Dtos.AI;
using Marketeer.Core.Domain.Entities.AI;
using Marketeer.Core.Domain.Entities.News;
using Marketeer.Core.Domain.InfrastructureDtos.Python.News;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketeer.Core.Domain.Dtos.News
{
    public class NewsArticleDto : EntityDto, IRefactorType,
        IMapFrom<NewsArticle>,
        IMapTo<NewsArticle>
    {
        public string Title { get; set; }
        public string Link { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public List<SentimentResultDto> SentimentResults { get; set; }
    }
}
