using System;
using System.Collections.Generic;
using System.Linq;
using SMARTplanner.Data.Contracts;
using SMARTplanner.Entities.Domain;
using SMARTplanner.Logic.Contracts;

namespace SMARTplanner.Logic.Exact
{
    public class ProjectLogService : ILogService<ProjectLog>
    {
        private readonly ISmartPlannerContext _context;
        private readonly IAccessService _accessService;

        public ProjectLogService(ISmartPlannerContext ctx, IAccessService access)
        {
            _context = ctx;
            _accessService = access;
        }

        public IEnumerable<ProjectLog> GetItemHistory(long projectId, string userId)
        {
            var project = _context.Projects.SingleOrDefault(p => p.Id == projectId);

            if (project != null)
            {
                //find issue history
                var projectLogs = _context.ProjectsHistory
                    .Where(ph => ph.ProjectId == projectId);

                if (_accessService.GetAccessByProject(projectId, userId) != null) return projectLogs;
            }

            return null;
        }

        public void LogAction(ProjectLog projectLog)
        {
            if (projectLog != null)
            {
                _context.ProjectsHistory.Add(projectLog);
                _context.SaveChanges();
            }
        }
    }
}
