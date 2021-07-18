using System;
using System.Collections.Generic;
using System.Text;

namespace congestion.calculator
{
    public class HourAmountDistribution
    {
        public int StartHour { get; set; }
        public int EndHour { get; set; }
        public int StartMin { get; set; }
        public int EndMin { get; set; }
        public int Amount { get; set; }
    }
}
