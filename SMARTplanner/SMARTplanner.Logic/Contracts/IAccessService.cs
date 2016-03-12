using System;
using System.Collections.Generic;
using SMARTplanner.Entities.Domain;

namespace SMARTplanner.Logic.Contracts
{
    public interface IAccessService
    {
        IEnumerable<Project> GetAccessibleProjects(string userId);
        IEnumerable<Issue> GetAccessibleIssues(string userId);
        ProjectUserAccess GetAccessByProject(long projectId, string userId);
        ProjectUserAccess GetAccessByIssue(Issue issue, string userId);
        ProjectUserAccess GetAccessByWorkItem(WorkItem item, string userId);
        ProjectUserAccess GetAccessByReport(long reportId, string userId);
        void GrantAccess(ProjectUserAccess grantedAccess, string grantorId);
    }
}
