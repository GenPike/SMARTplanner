using System.Collections.Generic;
using System.Linq;
using SMARTplanner.Data.Contracts;
using SMARTplanner.Entities.Domain;
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

        public IEnumerable<WorkItem> GetIssueWorkItems(long issueId, string userId)
        {
            //check can user get access to project
            var issue = _context.Issues
                .SingleOrDefault(i => i.Id == issueId);

            if (issue != null)
            {
                //does user have any binding with project
                var projUserRef = _accessService.GetAccessByIssue(issue, userId);
                if (projUserRef == null) return null;
                
                //get issue workitems
                return issue.WorkItems;
            }

            return null;
        }

        public WorkItem GetWorkItem(long itemId, string userId)
        {
            var workItem = GetWorkItemById(itemId);
            if (workItem != null)
            {
                //does user have any binding with project
                var projUserRef = _accessService.GetAccessByWorkItem(workItem, userId);
                if (projUserRef == null) return null;
            }

            return workItem;
        }

        public void AddWorkItem(WorkItem item)
        {
            if (item != null && item.Issue != null)
            {
                var projUserRef = _accessService.GetAccessByWorkItem(item, item.CreatorId);
                if (projUserRef == null || !Inspector.CanUserUpdateProject(projUserRef)) return;

                _context.WorkItems.Add(item);
                _context.SaveChanges();
            }
        }

        public void UpdateWorkItem(WorkItem item, string userId)
        {
            if (item != null)
            {
                var itemToUpdate = GetWorkItemById(item.Id);

                if (itemToUpdate != null)
                {
                    //check security
                    var projUserRef = _accessService.GetAccessByWorkItem(itemToUpdate, userId);
                    if (projUserRef == null || !Inspector.CanUserUpdateProject(projUserRef)) return;

                    if (!Inspector.IsValidWorkItemTime(item)) return;

                    _context.Entry(itemToUpdate).CurrentValues.SetValues(item);
                    _context.SaveChanges();
                }
            }
        }

        public void DeleteWorkItem(long itemId, string userId)
        {
            var item = GetWorkItemById(itemId);
            if (item != null)
            {
                //check security
                var projUserRef = _accessService.GetAccessByWorkItem(item, userId);
                if (projUserRef == null || !Inspector.CanUserUpdateProject(projUserRef)) return;

                _context.WorkItems.Remove(item);
                _context.SaveChanges();
            }
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
