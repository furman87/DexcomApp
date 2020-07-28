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

    public class Calibrations
    {
        public StartDate Start { get; set; }

        public EndDate End { get; set; }
    }

    public class StartDate
    {
        public DateTime SystemTime { get; set; }

        public DateTime DisplayTime { get; set; }
    }

    public class EndDate
    {
        public DateTime SystemTime { get; set; }

        public DateTime DisplayTime { get; set; }
    }

    public class Egvs
    {
        public StartDate Start { get; set; }

        public EndDate End { get; set; }
    }
}
