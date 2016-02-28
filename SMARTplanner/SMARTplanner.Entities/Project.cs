using System;
using System.Collections.Generic;

namespace SMARTplanner.Entities
{
    public class Project
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string CodeName { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
