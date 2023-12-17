using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketeer.Core.Domain.Dtos.News
{
    public class NewsFilterDto : IRefactorType
    {
        public DateTime? MinDate { get; set; }
        public DateTime? MaxDate { get; set; }
    }
}
