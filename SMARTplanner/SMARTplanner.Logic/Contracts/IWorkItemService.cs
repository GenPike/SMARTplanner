using SMARTplanner.Entities.Domain;
using SMARTplanner.Entities.Helpers;

namespace SMARTplanner.Logic.Contracts
{
    public interface IWorkItemService
    {
        ServiceCollectionResult<WorkItem> GetIssueWorkItems(long issueId, string userId);
        ServiceSingleResult<WorkItem> GetWorkItem(long itemId, string userId);
        ServiceSingleResult<bool> AddWorkItem(WorkItem item);
        ServiceSingleResult<bool> UpdateWorkItem(WorkItem item, string userId);
        ServiceSingleResult<bool> DeleteWorkItem(long itemId, string userId);
    }
}
