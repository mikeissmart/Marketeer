﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketeer.Common.Configs
{
    public abstract class CronQueueConfig
    {
        public bool AutoRun { get; set; }
        public int MinutesBetweenRuns { get; set; }
    }
}
