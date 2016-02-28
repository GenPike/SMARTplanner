using System;
using System.Collections.Generic;

namespace SMARTplanner.Entities
{
    public class Report
    {
        public long Id { get; set; }
        public DateTime ReportDate { get; set; }
        public string Comment { get; set; }
    }
}
