using System;
using System.Collections.Generic;
using System.Linq;
using SMARTplanner.Data.Contracts;
using SMARTplanner.Entities.Domain;
using SMARTplanner.Entities.Helpers;
using SMARTplanner.Logic.Contracts;
using SMARTplanner.Logic.Business;

namespace SMARTplanner.Logic.Exact
{
    public class ProjectService : IProjectService
    {
        private readonly ISmartPlannerContext _context;

        public ProjectService(ISmartPlannerContext ctx)
        {
            _context = ctx;
        }

        public IEnumerable<Project> GetProjectsPaged(string userId, bool? ownership = null, int page = 1, int pageSize = 10)
        {
            if (!Inspector.IsValidPageSize(pageSize)) return null;
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

        public Project GetProject(long projectId, string userId)
        {
            var project = _context.Projects
                .SingleOrDefault(p => p.Id == projectId);

            //check user access
            if (project != null)
            {
                if (project.ProjectUsers
                    .Count(pu => pu.UserId.Equals(userId)) != 0) return project;
            }

            return null;
        }

        public Project GetProject(string projectName, string userId)
        {
            var project = _context.Projects
                .SingleOrDefault(p => p.Name == projectName);

            //check user access
            if (project != null)
            {
                if (project.ProjectUsers
                    .Count(pu => pu.UserId.Equals(userId)) != 0) return project;
            }

            return null;
        }

        public void AddProject(Project project)
        {
            if (project != null)
            {
                //check permission to create
                int nCreatedProjects = GetProjectsUserCreated(project.CreatorId).Count();
                if (!Inspector.CanUserCreateProject(nCreatedProjects)) return;

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

        public void UpdateProject(Project project, string userId)
        {
            if (project != null)
            {
                var projectToUpdate = _context.Projects
                    .SingleOrDefault(p => p.Id == project.Id);

                if (projectToUpdate != null)
                {
                    //check permission for editiing project info
                    var projUserRef = projectToUpdate.ProjectUsers
                        .SingleOrDefault(pu => pu.UserId.Equals(userId));
                    if (!Inspector.CanUserUpdateProject(projUserRef)) return;

                    _context.Entry(projectToUpdate).CurrentValues.SetValues(project);
                    _context.SaveChanges();
                }
            }
        }
    }
}
