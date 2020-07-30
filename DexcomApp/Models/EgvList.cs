// <copyright file="EgvList.cs" company="Ken Watson">
// Copyright (c) Ken Watson. All rights reserved.
// </copyright>

namespace DexcomApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class EgvList
    {
        public string Unit { get; set; }

        public string RateUnit { get; set; }

        public List<Egv> Egvs { get; set; }
    }
}
