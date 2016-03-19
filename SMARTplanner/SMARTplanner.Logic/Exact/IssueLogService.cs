using System.Collections.Generic;
using System.Linq;
using SMARTplanner.Data.Contracts;
using SMARTplanner.Entities.Domain;
using SMARTplanner.Entities.Helpers;
using SMARTplanner.Logic.Contracts;

namespace SMARTplanner.Logic.Exact
{
    public class IssueLogService : ILogService<IssueLog>
    {
        private readonly ISmartPlannerContext _context;
        private readonly IAccessService _accessService;

        public IssueLogService(ISmartPlannerContext ctx, IAccessService access)
        {
            _context = ctx;
            _accessService = access;
        }

        public ServiceCollectionResult<IssueLog> GetItemHistory(long issueId, string userId)
        {
            var result = new ServiceCollectionResult<IssueLog>();
            var issue = _context.Issues.SingleOrDefault(i => i.Id == issueId);

            if (issue != null)
            {
                //find issue history
                var issueLogs = _context.IssuesHistory
                    .Where(ih => ih.IssueId == issueId);

                if (_accessService.GetAccessByIssue(issue, userId) != null)
                {
                    result.TargetCollection = issueLogs;
                    return result;
                }

                result.HandleError(ErrorMessagesDict.AccessDenied);
                return result;
            }

            result.HandleError(ErrorMessagesDict.NotFoundResource);
            return result;
        }

        public void LogAction(IssueLog issueLog)
        {
            if (issueLog != null)
            {
                _context.IssuesHistory.Add(issueLog);
                _context.SaveChanges();
            }
        }
    }
}
