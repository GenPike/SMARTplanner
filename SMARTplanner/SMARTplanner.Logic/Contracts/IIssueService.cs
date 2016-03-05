using System;
using System.Collections.Generic;
using SMARTplanner.Entities.Domain;

namespace SMARTplanner.Logic.Contracts
{
    public interface IIssueService
    {
        IEnumerable<Issue> GetIssuesPaged(string userId, string summaryPart = null, int page = 1, int pageSize = 10);
        Issue GetIssue(long issueId, string userId);
        Issue GetIssue(int trackingNumber, string userId);
        void AddIssue(Issue issue);
        void UpdateIssue(Issue issue, string userId);
    }
}
