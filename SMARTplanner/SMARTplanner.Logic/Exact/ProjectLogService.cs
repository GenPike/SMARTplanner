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

        public ProjectLogService(ISmartPlannerContext ctx)
        {
            _context = ctx;
        }

        public IEnumerable<ProjectLog> GetItemHistory(long projectId)
        {
            throw new NotImplementedException();
        }

        public void LogAction(ProjectLog log)
        {
            throw new NotImplementedException();
        }
    }
}
