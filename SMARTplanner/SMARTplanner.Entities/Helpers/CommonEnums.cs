
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
        ProjectCreator
    }

    public enum ProjectActionType
    {
        IssueCreated,
        IssueDeleted,
        MemberAdded,
        MemberAccessChanged
    }

    public enum IssueActionType
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
