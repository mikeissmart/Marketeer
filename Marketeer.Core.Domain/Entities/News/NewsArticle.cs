﻿using Marketeer.Core.Domain.Entities.AI;
using Marketeer.Core.Domain.Entities.Market;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketeer.Core.Domain.Entities.News
{
    public class NewsArticle : Entity
    {
        public string Title { get; set; }
        public string Link { get; set; }
        public string Text { get; set; }
        public DateTime ArticleDate { get; set; }

        #region Nav

        public List<Ticker> Tickers { get; set; }
        public List<SentimentResult> SentimentResults { get; set; }

        #endregion
    }
}
