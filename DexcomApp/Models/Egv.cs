// <copyright file="Egv.cs" company="Ken Watson">
// Copyright (c) Ken Watson. All rights reserved.
// </copyright>

namespace DexcomApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class Egv
    {
        public DateTime SystemTime { get; set; }

        public DateTime DisplayTime { get; set; }

        public int Value { get; set; }

        public int RealtimeValue { get; set; }

        public int? SmoothedValue { get; set; }

        public object Status { get; set; }

        public string Trend { get; set; }

        public float? TrendRate { get; set; }
    }
}
