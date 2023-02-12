﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Marketeer.Core.Domain.Entities.Market
{
    public class TickerSetting : Entity
    {
        public int TickerId { get; set; }
        public bool IsDelisted { get; set; }
        public bool UpdateDailyHistory { get; set; }

        #region Nav

        [ForeignKey(nameof(TickerId))]
        public Ticker Ticker { get; set; }

        public List<TickerSettingHistoryDisable> TempHistoryDisable { get; set; }

        #endregion
    }
}
