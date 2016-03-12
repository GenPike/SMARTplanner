using System;
using System.Collections.Generic;
using System.Linq;
using SMARTplanner.Data.Contracts;
using SMARTplanner.Entities.Domain;
using SMARTplanner.Logic.Contracts;

namespace SMARTplanner.Logic.Exact
{
    public class AccessService : IAccessService
    {
        private readonly ISmartPlannerContext _context;

        public AccessService(ISmartPlannerContext ctx)
        {
            _context = ctx;
        }

        public IEnumerable<Project> GetAccessibleProjects(string userId)
        {
            return _context.ProjectUserAccesses
                .Where(pu => pu.UserId.Equals(userId))
                .Select(pu => pu.Project);   
        }

        public IEnumerable<Issue> GetAccessibleIssues(string userId)
        {
            return _context.ProjectUserAccesses
                .Where(pu => pu.UserId.Equals(userId))
                .SelectMany(pu => pu.Project.Issues);
        }

        public ProjectUserAccess GetProjectAccessByProject(long projectId, string userId)
        {
            return _context.ProjectUserAccesses
                .SingleOrDefault(pu => pu.UserId.Equals(userId) &&
                             pu.ProjectId == projectId);
        }

        public ProjectUserAccess GetProjectAccessByIssue(long projectId, string userId)
        {
            throw new NotImplementedException();
        }

        public ProjectUserAccess GetProjectAccessByIssue(Issue issue)
        {
            throw new NotImplementedException();
        }

        public ProjectUserAccess GetProjectAccessByWorkItem(long projectId, string userId)
        {
            throw new NotImplementedException();
        }

        public ProjectUserAccess GetProjectAccessByReport(long projectId, string userId)
        {
            throw new NotImplementedException();
        }
        public void GrantAccess(ProjectUserAccess grantedAccess, string grantorId)
        {
            throw new NotImplementedException();
        }
    }
}
