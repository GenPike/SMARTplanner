using System;
using System.Collections.Generic;
using System.Linq;
using SMARTplanner.Data.Contracts;
using SMARTplanner.Entities.Domain;
using SMARTplanner.Entities.Helpers;
using SMARTplanner.Logic.Contracts;

namespace SMARTplanner.Logic.Exact
{
    public class ProjectService : IProjectService
    {
        private readonly ISmartPlannerContext _context;
        private const int PageSizeMax = 50;

        public ProjectService(ISmartPlannerContext ctx)
        {
            _context = ctx;
        }

        public IEnumerable<Project> GetProjectsPaged(string userId, bool? ownership = null, int page = 1, int pageSize = 10)
        {
            if (pageSize > PageSizeMax) return null;
            IEnumerable<Project> targetProjects;

            if (ownership.HasValue)
            {
                targetProjects = (ownership.Value) 
                    ? GetProjectsUserCreated(userId)
                    : GetProjectsUserInvolvedOnly(userId);
            }
            else targetProjects = GetProjectsUserAccessible(userId);

            return targetProjects
                .Skip((page - 1) * pageSize)
                .Take(pageSize);
        }

        #region GetProjectsFilters

        private IEnumerable<Project> GetProjectsUserAccessible(string userId)
        {
            return _context.ProjectUserAccesses
                .Where(pu => pu.UserId.Equals(userId))
                .Select(pu => pu.Project);
        }

        private IEnumerable<Project> GetProjectsUserInvolvedOnly(string userId)
        {
            var userAccessibleProjects = _context.ProjectUserAccesses
                .Where(pu => pu.UserId.Equals(userId))
                .Select(pu => pu.Project);

            var userCreatedProjects = _context.Projects
                .Where(p => p.CreatorId.Equals(userId));

            return userAccessibleProjects.Except(userCreatedProjects);
        }

        private IEnumerable<Project> GetProjectsUserCreated(string userId)
        {
            return _context.Projects
                .Where(p => p.CreatorId.Equals(userId));
        }

        #endregion

        public Project GetProject(long projectId)
        {
            return _context.Projects
                .SingleOrDefault(p => p.Id == projectId);
        }

        public Project GetProject(string projectName)
        {
            return _context.Projects
                .SingleOrDefault(p => p.Name == projectName);
        }

        public void AddProject(Project project)
        {
            if (project != null)
            {
                //create user-project reference
                project.ProjectUsers.Add(
                    new ProjectUserAccess
                    {
                        UserId = project.CreatorId,
                        ProjectAccess = ProjectAccess.ReadReportEdit,
                        CanGrantAccess = false
                    }
                );
                //add project
                _context.Projects.Add(project);
                _context.SaveChanges();
            }
        }

        public void UpdateProject(Project project)
        {
            if (project != null)
            {
                var projectToUpdate = _context.Projects
                    .SingleOrDefault(p => p.Id == project.Id);

                if (projectToUpdate != null)
                {
                    _context.Entry(projectToUpdate).CurrentValues.SetValues(project);
                    _context.SaveChanges();
                }
            }
        }
    }
}
