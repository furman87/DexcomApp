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
        private DateTime systemTime;
        private DateTime displayTime;

        public DateTime SystemTime
        {
            get => this.systemTime; // TimeZoneInfo.ConvertTimeToUtc(this.systemTime);
            set => this.systemTime = value;
        }

        public DateTime DisplayTime
        {
            get => this.displayTime; // TimeZoneInfo.ConvertTimeToUtc(this.displayTime);
            set => this.displayTime = value;
        }

        public int Value { get; set; }

        public int RealtimeValue { get; set; }

        public int? SmoothedValue { get; set; }

        public object Status { get; set; }

        public string Trend { get; set; }

        public float? TrendRate { get; set; }
    }
}
