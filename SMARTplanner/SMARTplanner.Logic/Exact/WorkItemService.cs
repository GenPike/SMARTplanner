using System;
using System.Linq;
using SMARTplanner.Data.Contracts;
using SMARTplanner.Entities.Domain;
using SMARTplanner.Entities.Helpers;
using SMARTplanner.Logic.Business;
using SMARTplanner.Logic.Contracts;

namespace SMARTplanner.Logic.Exact
{
    public class WorkItemService : IWorkItemService
    {
        private readonly ISmartPlannerContext _context;
        private readonly IAccessService _accessService;

        public WorkItemService(ISmartPlannerContext ctx, IAccessService access)
        {
            _context = ctx;
            _accessService = access;
        }

        public ServiceCollectionResult<WorkItem> GetIssueWorkItems(long issueId, string userId)
        {
            var result = new ServiceCollectionResult<WorkItem>();
            //check can user get access to project
            var issue = _context.Issues
                .SingleOrDefault(i => i.Id == issueId);

            if (issue != null)
            {
                //does user have any binding with project
                var projUserRef = _accessService.GetAccessByIssue(issue, userId);

                if (projUserRef == null) result.HandleError(ErrorMessagesDict.AccessDenied);
                else result.TargetCollection = issue.WorkItems; //get issue workitems
                
                return result;
            }

            result.HandleError(ErrorMessagesDict.NotFoundResource);
            return result;
        }

        public ServiceSingleResult<WorkItem> GetWorkItem(long itemId, string userId)
        {
            var result = new ServiceSingleResult<WorkItem>();
            var workItem = GetWorkItemById(itemId);
            if (workItem != null)
            {
                //does user have any binding with project
                var projUserRef = _accessService.GetAccessByWorkItem(workItem, userId);
                
                if (projUserRef == null) result.HandleError(ErrorMessagesDict.AccessDenied);
                else result.TargetObject = workItem;

                return result;
            }

            result.HandleError(ErrorMessagesDict.NotFoundResource);
            return result;
        }

        public ServiceSingleResult<bool> AddWorkItem(WorkItem item)
        {
            var result = new ServiceSingleResult<bool>();
            if (item != null && item.Issue != null)
            {
                var projUserRef = _accessService.GetAccessByWorkItem(item, item.CreatorId);
                if (projUserRef == null || !Inspector.CanUserUpdateProject(projUserRef))
                {
                    result.HandleError(ErrorMessagesDict.AccessDenied);
                    return result;
                }

                if (!Inspector.IsValidWorkItemTime(item))
                {
                    result.HandleError(ErrorMessagesDict.WrongEstimatedTimeFormat);
                    return result;
                }

                _context.WorkItems.Add(item);
                try
                {
                    _context.SaveChanges();
                    result.TargetObject = true;
                }
                catch (Exception exc)
                {
                    result.HandleError(exc.Message);
                }
                return result;
            }

            result.HandleError(ErrorMessagesDict.NullInstance);
            return result;
        }

        public ServiceSingleResult<bool> UpdateWorkItem(WorkItem item, string userId)
        {
            var result = new ServiceSingleResult<bool>();
            if (item != null)
            {
                var itemToUpdate = GetWorkItemById(item.Id);

                if (itemToUpdate != null)
                {
                    //check security
                    var projUserRef = _accessService.GetAccessByWorkItem(itemToUpdate, userId);
                    if (projUserRef == null || !Inspector.CanUserUpdateProject(projUserRef))
                    {
                        result.HandleError(ErrorMessagesDict.AccessDenied);
                        return result;
                    }

                    if (!Inspector.IsValidWorkItemTime(item))
                    {
                        result.HandleError(ErrorMessagesDict.WrongEstimatedTimeFormat);
                        return result;
                    }

                    _context.Entry(itemToUpdate).CurrentValues.SetValues(item);
                    try
                    {
                        _context.SaveChanges();
                        result.TargetObject = true;
                    }
                    catch (Exception exc)
                    {
                        result.HandleError(exc.Message);
                    }
                    return result;
                }

                result.HandleError(ErrorMessagesDict.NotFoundResource);
                return result;
            }

            result.HandleError(ErrorMessagesDict.NullInstance);
            return result;
        }

        public ServiceSingleResult<bool> DeleteWorkItem(long itemId, string userId)
        {
            var result = new ServiceSingleResult<bool>();
            var item = GetWorkItemById(itemId);
            if (item != null)
            {
                //check security
                var projUserRef = _accessService.GetAccessByWorkItem(item, userId);
                if (projUserRef == null || !Inspector.CanUserUpdateProject(projUserRef))
                {
                    result.HandleError(ErrorMessagesDict.AccessDenied);
                    return result;
                }

                _context.WorkItems.Remove(item);
                try
                {
                    _context.SaveChanges();
                    result.TargetObject = true;
                }
                catch (Exception exc)
                {
                    result.HandleError(exc.Message);
                }
                return result;
            }

            result.HandleError(ErrorMessagesDict.NotFoundResource);
            return result;
        }

        #region Helpers

        private WorkItem GetWorkItemById(long itemId)
        {
            return _context.WorkItems
                .SingleOrDefault(w => w.Id == itemId);
        }

        #endregion
    }
}
