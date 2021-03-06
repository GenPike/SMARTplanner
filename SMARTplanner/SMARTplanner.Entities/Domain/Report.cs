﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMARTplanner.Entities.Domain
{
    public class Report
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public double WorkTime { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime ReportDate { get; set; }
        public string WorkDescription { get; set; }

        //to identify batch of reports from one user in exact time
        public long GroupId { get; set; }
        public long IssueId { get; set; }
        [ForeignKey("IssueId")]
        public virtual Issue Issue { get; set; }
        public long WorkItemId { get; set; }
        [ForeignKey("WorkItemId")]
        public virtual WorkItem WorkItem { get; set; }
        public string ReporterId { get; set; }
        [ForeignKey("ReporterId")]
        public virtual ApplicationUser Reporter { get; set; }

    }
}
