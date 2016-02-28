using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMARTplanner.Entities
{
    public class Issue
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public string Summary { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime DateCreated { get; set; }

        public string CreatorId { get; set; }
        [ForeignKey("CreatorId")]
        public virtual ApplicationUser Creator { get; set; }
        public long ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        public virtual Project Project { get; set; }
        public virtual ICollection<WorkItem> WorkItems { get; set; }
        public virtual ICollection<Report> Reports { get; set; } 

    }
}
