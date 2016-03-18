using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SMARTplanner.Entities.Helpers;

namespace SMARTplanner.Entities.Domain
{
    public class IssueLog : ILogEntity
    {
        [Key]
        public long Id { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime DateModified { get; set; }
        [Required]
        public string ActionDescription { get; set; }
        public IssueActionType ActionType { get; set; }
        
        public long IssueId { get; set; }
        [ForeignKey("IssueId")]
        public virtual Issue Project { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
    }
}
