using System;
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

        public IssueService(ISmartPlannerContext ctx)
        {
            _context = ctx;
        }

        public IEnumerable<Issue> GetIssuesPaged(string userId, string summaryPart = null, int page = 1, int pageSize = 10)
        {
            if (!Inspector.IsValidPageSize(pageSize)) return null;

            //get accessible issues
            var accessibleIssues = GetAccessibleIssues(userId);

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

        #region AccessHelpers

        private IEnumerable<Issue> GetAccessibleIssues(string userId)
        {
            return _context.ProjectUserAccesses
                .Where(pu => pu.UserId.Equals(userId))
                .SelectMany(pu => pu.Project.Issues);
        } 

        #endregion

        public Issue GetIssue(long issueId, string userId)
        {
            return GetAccessibleIssues(userId)
                .SingleOrDefault(i => i.Id == issueId);
        }

        public Issue GetIssue(int trackingNumber, string userId)
        {
            return GetAccessibleIssues(userId)
                .FirstOrDefault(i => i.IssueTrackingNumber == trackingNumber);
        }

        public void AddIssue(Issue issue)
        {
            if (issue != null)
            {
                //check user access to the project
                var projUserRef = _context.ProjectUserAccesses
                    .SingleOrDefault(pu => pu.ProjectId == issue.ProjectId &&
                        pu.UserId.Equals(issue.CreatorId));

                if (!Inspector.CanUserUpdateProject(projUserRef)) return;

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
                    var projUserRef = _context.ProjectUserAccesses
                        .SingleOrDefault(pu => pu.ProjectId == issueToUpdate.ProjectId &&
                                               pu.UserId.Equals(userId));

                    if (!Inspector.CanUserUpdateProject(projUserRef)) return;

                    _context.Entry(issueToUpdate).CurrentValues.SetValues(issue);
                    _context.SaveChanges();
                }
            }
        }
    }
}
