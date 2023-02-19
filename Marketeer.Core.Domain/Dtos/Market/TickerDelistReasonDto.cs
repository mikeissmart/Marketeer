using Marketeer.Core.Domain.Entities.Market;
using Marketeer.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketeer.Core.Domain.Dtos.Market
{
    public class TickerDelistReasonDto : EntityAuditCreateDto, IRefactorType,
        IMapFrom<TickerDelistReason>,
        IMapTo<TickerDelistReason>
    {
        public int TickerId { get; set; }
        public DelistEnum Delist { get; set; }
    }
}
