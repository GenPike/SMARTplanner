using System;
using System.Collections.Generic;
using SMARTplanner.Entities.Domain;

namespace SMARTplanner.Logic.Contracts
{
    public interface IWorkItemService
    {
        IEnumerable<WorkItem> GetIssueWorkItems(long issueId, string userId);
        WorkItem GetWorkItem(long itemId);
        void UpdateWorkItem(WorkItem item);
        void DeleteWorkItem(long itemId);
    }
}
