using System;
using System.Collections.Generic;
using SMARTplanner.Data.Contracts;
using SMARTplanner.Entities.Domain;
using SMARTplanner.Logic.Contracts;

namespace SMARTplanner.Logic.Exact
{
    public class IssueService : IIssueService
    {
        private ISmartPlannerContext _context;

        public IssueService(ISmartPlannerContext ctx)
        {
            _context = ctx;
        }

        public IEnumerable<Issue> GetIssuesPaged(string userId, int page = 1, int pageSize = 10)
        {
            throw new NotImplementedException();
        }

        public Issue GetIssue(long issueId, string userId)
        {
            throw new NotImplementedException();
        }

        public Issue GetIssue(string summaryPart, string userId)
        {
            throw new NotImplementedException();
        }

        public Issue GetIssue(int trackingNumber, string userId)
        {
            throw new NotImplementedException();
        }

        public void AddIssue(Issue issue)
        {
            throw new NotImplementedException();
        }

        public void UpdateIssue(Issue issue, string userId)
        {
            throw new NotImplementedException();
        }
    }
}
