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
                var projUserRef = _context.ProjectUserAccesses
                    .SingleOrDefault(pu => pu.UserId.Equals(userId) &&
                                           pu.Project.Issues.Contains(issue));

                if (!Inspector.CanUserUpdateProject(projUserRef)) return null;
                //get issue workitems
                return issue.WorkItems;
            }

            return null;
        }

        public WorkItem GetWorkItem(long itemId)
        {
            return GetWorkItemById(itemId);
        }

        public void UpdateWorkItem(WorkItem item)
        {
            if (item != null)
            {
                var itemToUpdate = GetWorkItemById(item.Id);

                if (itemToUpdate != null)
                {
                    if (!Inspector.IsValidWorkItemTime(item)) return;

                    _context.Entry(itemToUpdate).CurrentValues.SetValues(item);
                    _context.SaveChanges();
                }
            }
        }

        public void DeleteWorkItem(long itemId)
        {
            var item = GetWorkItemById(itemId);

            if (item != null)
            {
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
