using System.ComponentModel.DataAnnotations.Schema;
using SMARTplanner.Entities.Helpers;

namespace SMARTplanner.Entities.Domain
{
    public class IssueLog : LogEntity
    {
        public IssueActionType ActionType { get; set; }
        
        public long IssueId { get; set; }
        [ForeignKey("IssueId")]
        public virtual Issue Project { get; set; }
    }
}
