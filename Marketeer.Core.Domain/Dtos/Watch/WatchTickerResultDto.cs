using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketeer.Core.Domain.Dtos.Watch
{
    public class WatchTickerResultDto : IRefactorType
    {
        public int UpdatedCount { get; set; }
        public int RemovedCount { get; set; }
        public int AddedCount { get; set; }
        public int CurrentCount { get; set; }
        public int MaxCount { get; set; }
    }
}
