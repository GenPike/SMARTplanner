using System.ComponentModel.DataAnnotations.Schema;
using SMARTplanner.Entities.Helpers;

namespace SMARTplanner.Entities.Domain
{
    public class ProjectLog : LogEntity
    {
        public ProjectActionType ActionType { get; set; }

        public long ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        public virtual Project Project { get; set; }
    }
}
