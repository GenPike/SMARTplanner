using System;
using SMARTplanner.Entities.Domain;

namespace SMARTplanner.Entities.Helpers
{
    public interface ILogEntity
    {
        long Id { get; set; }
        DateTime DateModified { get; set; }
        string ActionDescription { get; set; }

        string UserId { get; set; }
        ApplicationUser User { get; set; }
    }
}
