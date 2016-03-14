using System;

namespace SMARTplanner.Entities.Helpers
{
    public enum TaskType : uint
    {
        Analysis,
        Implementation
    }

    public enum ProjectAccess : uint
    {
        Read,
        ReadReport,
        ReadReportEdit,
        ProjectCreator
    }

    public enum ProjectActionType : uint
    {
        IssueCreated,
        IssueDeleted,
        MemberAdded,
        MemberAccessChanged
    }

    public enum IssueActionType : uint
    {
        IssueIdeaChanged,
        IssueTrackNumberChanged,
        WorkItemAdded,
        WorkItemUpdated,
        WorkItemDeleted,
        ReportAdded,
        ReportUpdated,
        ReportDeleted
    }
}
