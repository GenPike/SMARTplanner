using System;
using System.Collections.Generic;
using System.Linq;
using SMARTplanner.Data.Contracts;
using SMARTplanner.Entities.Domain;
using SMARTplanner.Logic.Contracts;

namespace SMARTplanner.Logic.Exact
{
    public class IssueLogService : ILogService<IssueLog>
    {
        private readonly ISmartPlannerContext _context;

        public IssueLogService(ISmartPlannerContext ctx)
        {
            _context = ctx;
        }

        public IEnumerable<IssueLog> GetItemHistory(long itemId)
        {
            throw new NotImplementedException();
        }

        public void LogAction(IssueLog log)
        {
            throw new NotImplementedException();
        }
    }
}
