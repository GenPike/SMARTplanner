using System;
using System.Collections.Generic;
using System.Linq;
using SMARTplanner.Data.Contracts;
using SMARTplanner.Entities.Domain;
using SMARTplanner.Entities.Helpers;
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

        public ServiceCollectionResult<Issue> GetIssuesPaged(string userId, string summaryPart = null, int page = 1, int pageSize = 10)
        {
            var result = new ServiceCollectionResult<Issue>();
            if (!Inspector.IsValidPageSize(pageSize))
            {
                result.HandleError(ErrorMessagesDict.InvalidPageSize);
                return result;
            }

            //get accessible issues
            var accessibleIssues = _accessService.GetAccessibleIssues(userId);

            //filter issues
            if (summaryPart != null)
            {
                accessibleIssues = accessibleIssues
                    .Where(i => i.Summary.Contains(summaryPart.ToLower()));
            }

            result.TargetCollection = accessibleIssues
                .Skip((page - 1) * pageSize)
                .Take(pageSize);
            return result;
        }

        public ServiceSingleResult<Issue> GetIssue(long issueId, string userId)
        {
            var result = new ServiceSingleResult<Issue>();
            var issue = _accessService.GetAccessibleIssues(userId)
                .SingleOrDefault(i => i.Id == issueId);

            if (issue != null)
                result.TargetObject = issue;
            else
                result.HandleError(ErrorMessagesDict.AccessDenied);

            return result;
        }

        public ServiceSingleResult<Issue> GetIssue(int trackingNumber, string userId)
        {
            var result = new ServiceSingleResult<Issue>();
            var issue = _accessService.GetAccessibleIssues(userId)
                .FirstOrDefault(i => i.IssueTrackingNumber == trackingNumber);

            if (issue != null)
                result.TargetObject = issue;
            else
                result.HandleError(ErrorMessagesDict.AccessDenied);

            return result;
        }

        public ServiceSingleResult<bool> AddIssue(Issue issue)
        {
            var result = new ServiceSingleResult<bool>();
            if (issue != null && issue.Project != null)
            {
                //check user access to the project
                var projUserRef = _accessService.GetAccessByIssue(issue, issue.CreatorId);
                if (projUserRef == null || !Inspector.CanUserUpdateProject(projUserRef))
                {
                    result.HandleError(ErrorMessagesDict.AccessDenied);
                    return result;
                }

                _context.Issues.Add(issue);
                try
                {
                    _context.SaveChanges();
                    result.TargetObject = true;
                    return result;
                }
                catch (Exception exc)
                {
                    result.HandleError(exc.Message);
                    return result;
                }
            }

            result.HandleError(ErrorMessagesDict.NullInstance);
            return result;
        }

        public ServiceSingleResult<bool> UpdateIssue(Issue issue, string userId)
        {
            var result = new ServiceSingleResult<bool>();
            if (issue != null)
            {
                var issueToUpdate = _context.Issues
                    .SingleOrDefault(i => i.Id == issue.Id);

                if (issueToUpdate != null)
                {
                    //check user access to the project
                    var projUserRef = _accessService.GetAccessByIssue(issueToUpdate, userId);
                    if (projUserRef == null || !Inspector.CanUserUpdateProject(projUserRef))
                    {
                        result.HandleError(ErrorMessagesDict.AccessDenied);
                        return result;
                    }

                    _context.Entry(issueToUpdate).CurrentValues.SetValues(issue);
                    try
                    {
                        _context.SaveChanges();
                        result.TargetObject = true;
                        return result;
                    }
                    catch (Exception exc)
                    {
                        result.HandleError(exc.Message);
                        return result;
                    }
                }

                result.HandleError(ErrorMessagesDict.NotFoundResource);
                return result;
            }

            result.HandleError(ErrorMessagesDict.NullInstance);
            return result;
        }
    }
}
