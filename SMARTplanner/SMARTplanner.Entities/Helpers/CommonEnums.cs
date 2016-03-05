using System;

namespace SMARTplanner.Entities.Helpers
{
    public enum TaskType
    {
        Analysis,
        Implementation
    }

    public enum ProjectAccess
    {
        Read,
        ReadReport,
        ReadReportEdit,
        AdminAccess
    }
}
