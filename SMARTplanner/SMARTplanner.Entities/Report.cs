using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMARTplanner.Entities
{
    public class Report
    {
        [Key]
        public long Id { get; set; }
        
        private double _workTime;
        public double WorkTime
        {
            get { return _workTime; }
            set
            {
                if ((value % WorkItem.UnitOfTime).Equals(1))     // fix this rule - INCORRECT NOW!!!
                {
                    _workTime = value;
                }
            }
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime ReportDate { get; set; }
        [NotMapped]
        public DateTime LastModified { get; set; }
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
