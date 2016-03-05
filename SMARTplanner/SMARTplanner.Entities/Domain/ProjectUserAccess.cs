using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SMARTplanner.Entities.Helpers;

namespace SMARTplanner.Entities.Domain
{
    public class ProjectUserAccess
    {
        [Key, Column(Order = 0)]
        public long ProjectId { get; set; }
        [Key, Column(Order = 1)]
        public string UserId { get; set; }

        public virtual Project Project { get; set; }
        public virtual ApplicationUser User { get; set; }

        public ProjectAccess ProjectAccess { get; set; }
        public bool CanGrantAccess { get; set; }
    }
}
