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

        public WorkItemService(ISmartPlannerContext ctx)
        {
            _context = ctx;
        }

        public IEnumerable<WorkItem> GetIssueWorkItems(long issueId, string userId)
        {
            //check can user get access to project
            var issue = _context.Issues
                .SingleOrDefault(i => i.Id == issueId);

            if (issue != null)
            {
                var projUserRef = GetUserAccess(issue, userId);
                if (!Inspector.CanUserUpdateProject(projUserRef)) return null;
                
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
                var projUserRef = GetUserAccess(workItem, userId);
                if (!Inspector.CanUserUpdateProject(projUserRef)) return null;
            }

            return workItem;
        }

        public void AddWorkItem(WorkItem item, string userId)
        {
            if (item != null)
            {
                var issue = _context.Issues
                    .SingleOrDefault(i => i.Id == item.IssueId);
                if (issue != null)
                {
                    var projUserRef = GetUserAccess(issue, userId);
                    if (!Inspector.CanUserUpdateProject(projUserRef)) return;

                    _context.WorkItems.Add(item);
                    _context.SaveChanges();
                }
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
                    var projUserRef = GetUserAccess(itemToUpdate, userId);
                    if (!Inspector.CanUserUpdateProject(projUserRef)) return;

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
                var projUserRef = GetUserAccess(item, userId);
                if (!Inspector.CanUserUpdateProject(projUserRef)) return;

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

        private ProjectUserAccess GetUserAccess(WorkItem item, string userId)
        {
            return item.Issue.Project.ProjectUsers
                    .SingleOrDefault(pu => pu.UserId.Equals(userId));
        }

        private ProjectUserAccess GetUserAccess(Issue issue, string userId)
        {
            return issue.Project.ProjectUsers
                    .SingleOrDefault(pu => pu.UserId.Equals(userId));
        }

        #endregion
    }
}
