using System;
using System.Collections.Generic;
using SMARTplanner.Entities.Domain;

namespace SMARTplanner.Logic.Contracts
{
    public interface IWorkItemService
    {
        IEnumerable<WorkItem> GetIssueWorkItems(long issueId, string userId);
        WorkItem GetWorkItem(long itemId, string userId);
        void AddWorkItem(WorkItem item, string userId);
        void UpdateWorkItem(WorkItem item, string userId);
        void DeleteWorkItem(long itemId, string userId);
    }
}
