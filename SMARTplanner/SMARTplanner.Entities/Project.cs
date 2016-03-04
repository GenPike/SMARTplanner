using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMARTplanner.Entities
{
    public class Project
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string CodeName { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime DateCreated { get; set; }

        public string CreatorId { get; set; }
        [ForeignKey("CreatorId")]
        public virtual ApplicationUser Creator { get; set; }
        public virtual ICollection<Issue> Issues { get; set; }
        public virtual ICollection<ProjectUserAccess> ProjectUsers { get; set; }
    }
}
