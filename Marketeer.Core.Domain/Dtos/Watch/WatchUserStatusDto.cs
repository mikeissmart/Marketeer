﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketeer.Core.Domain.Dtos.Watch
{
    public class WatchUserStatusDto : IRefactorType
    {
        public int WatchTicerCount { get; set; }
        public int WatchTicerCountMax { get; set; }
    }
}
