using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SMARTplanner.Entities.Helpers;

namespace SMARTplanner.Entities.Domain
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
        [Required]
        public double EstimatedTime { get; set; }

        public long IssueId { get; set; }
        [ForeignKey("IssueId")]
        public virtual Issue Issue { get; set; }
        public string CreatorId { get; set; }
        [ForeignKey("CreatorId")]
        public virtual ApplicationUser Creator { get; set; }
        public virtual ICollection<Report> Reports { get; set; }
    }
}
