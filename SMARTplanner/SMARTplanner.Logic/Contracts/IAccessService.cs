using System;
using System.Collections.Generic;
using SMARTplanner.Entities.Domain;

namespace SMARTplanner.Logic.Contracts
{
    public interface IAccessService
    {
        IEnumerable<Project> GetAccessibleProjects(string userId);
        IEnumerable<Issue> GetAccessibleIssues(string userId);
        ProjectUserAccess GetProjectAccessByProject(long projectId, string userId);
        ProjectUserAccess GetProjectAccessByIssue(long projectId, string userId);
        ProjectUserAccess GetProjectAccessByIssue(Issue issue);
        ProjectUserAccess GetProjectAccessByWorkItem(long projectId, string userId);
        ProjectUserAccess GetProjectAccessByReport(long projectId, string userId);
        void GrantAccess(ProjectUserAccess grantedAccess, string grantorId);
    }
}
