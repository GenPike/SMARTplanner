using System;
using System.Collections.Generic;

namespace SMARTplanner.Entities
{
    public class Issue
    {
        public long Id { get; set; }
        public string Summary { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
