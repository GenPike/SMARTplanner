using System;
using System.Collections.Generic;

namespace SMARTplanner.Entities
{
    public class Task
    {
        public long Id { get; set; }
        public string Idea { get; set; }
        public double EstimatedTime { get; set; }
        public TaskType TaskType { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
