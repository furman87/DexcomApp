﻿// <copyright file="DataRange.cs" company="Ken Watson">
// Copyright (c) Ken Watson. All rights reserved.
// </copyright>

namespace DexcomApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class DataRange
    {
        public Calibrations Calibrations { get; set; }

        public Egvs Egvs { get; set; }

        public object Events { get; set; }
    }
}
