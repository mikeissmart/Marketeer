using Marketeer.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketeer.Core.Domain.Entities.Market
{
    public class TickerDelistReason : EntityAuditCreate
    {
        public int TickerId { get; set; }
        public DelistEnum Delist { get; set; }

        #region Nav

        [ForeignKey(nameof(TickerId))]
        public Ticker Ticker { get; set; }

        #endregion
    }
}
