using System;
using SMARTplanner.Entities.Domain;
using SMARTplanner.Entities.Helpers;

namespace SMARTplanner.Logic.Contracts
{
    public interface IIssueService
    {
        ServiceCollectionResult<Issue> GetIssuesPaged(string userId, string summaryPart = null, int page = 1, int pageSize = 10);
        ServiceSingleResult<Issue> GetIssue(long issueId, string userId);
        ServiceSingleResult<Issue> GetIssue(int trackingNumber, string userId);
        ServiceSingleResult<bool> AddIssue(Issue issue);
        ServiceSingleResult<bool> UpdateIssue(Issue issue, string userId);
    }
}
