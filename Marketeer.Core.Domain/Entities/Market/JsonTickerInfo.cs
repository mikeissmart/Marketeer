using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketeer.Core.Domain.Entities.Market
{
    public class JsonTickerInfo : Entity
    {
        [StringLength(10)]
        public string Symbol { get; set; }
        public string InfoJson { get; set; }
    }
}
