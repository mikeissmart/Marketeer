﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketeer.Core.Domain.InfrastructureDtos.Python.Market
{
    public class PythonHistoryDataDto
    {
        public decimal? Open { get; set; }
        public decimal? Close { get; set; }
        public decimal? High { get; set; }
        public decimal? Low { get; set; }
        public long? Volume { get; set; }
        public DateTime Date { get; set; }
    }
}
