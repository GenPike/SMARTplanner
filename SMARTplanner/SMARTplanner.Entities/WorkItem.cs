using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMARTplanner.Entities
{
    public class WorkItem
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public string Idea { get; set; }
        public TaskType TaskType { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime DateCreated { get; set; }

        private double _estimatedTime;
        public double EstimatedTime
        {
            get { return _estimatedTime; }
            set
            {
                if ((value % UnitOfTime).Equals(0))
                {
                    _estimatedTime = value;
                }
            }
        }

        public long IssueId { get; set; }
        [ForeignKey("IssueId")]
        public virtual Issue Issue { get; set; }
        public virtual ICollection<Report> Reports { get; set; }

        public const double UnitOfTime = 0.25; 
    }
}
