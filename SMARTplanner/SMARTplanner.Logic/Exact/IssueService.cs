using System.Collections.Generic;
using System.Linq;
using SMARTplanner.Data.Contracts;
using SMARTplanner.Entities.Domain;
using SMARTplanner.Logic.Business;
using SMARTplanner.Logic.Contracts;

namespace SMARTplanner.Logic.Exact
{
    public class IssueService : IIssueService
    {
        private readonly ISmartPlannerContext _context;
        private readonly IAccessService _accessService;

        public IssueService(ISmartPlannerContext ctx, IAccessService access)
        {
            _context = ctx;
            _accessService = access;
        }

        public IEnumerable<Issue> GetIssuesPaged(string userId, string summaryPart = null, int page = 1, int pageSize = 10)
        {
            if (!Inspector.IsValidPageSize(pageSize)) return null;

            //get accessible issues
            var accessibleIssues = _accessService.GetAccessibleIssues(userId);

            //filter issues
            if (summaryPart != null)
            {
                accessibleIssues = accessibleIssues
                    .Where(i => i.Summary.Contains(summaryPart.ToLower()));
            }

            return accessibleIssues
                .Skip((page - 1) * pageSize)
                .Take(pageSize);
        }

        public Issue GetIssue(long issueId, string userId)
        {
            return _accessService.GetAccessibleIssues(userId)
                .SingleOrDefault(i => i.Id == issueId);
        }

        public Issue GetIssue(int trackingNumber, string userId)
        {
            return _accessService.GetAccessibleIssues(userId)
                .FirstOrDefault(i => i.IssueTrackingNumber == trackingNumber);
        }

        public void AddIssue(Issue issue)
        {
            if (issue != null && issue.Project != null)
            {
                //check user access to the project
                var projUserRef = _accessService.GetAccessByIssue(issue, issue.CreatorId);
                if (projUserRef == null || !Inspector.CanUserUpdateProject(projUserRef)) return;

                _context.Issues.Add(issue);
                _context.SaveChanges();
            }
        }

        public void UpdateIssue(Issue issue, string userId)
        {
            if (issue != null)
            {
                var issueToUpdate = _context.Issues
                    .SingleOrDefault(i => i.Id == issue.Id);

                if (issueToUpdate != null)
                {
                    //check user access to the project
                    var projUserRef = _accessService.GetAccessByIssue(issueToUpdate, userId);
                    if (projUserRef == null || !Inspector.CanUserUpdateProject(projUserRef)) return;

                    _context.Entry(issueToUpdate).CurrentValues.SetValues(issue);
                    _context.SaveChanges();
                }
            }
        }
    }
}
