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
        private readonly IAccessService _accessService;

        public ProjectService(ISmartPlannerContext ctx, IAccessService access)
        {
            _context = ctx;
            _accessService = access;
        }

        public ServiceCollectionResult<Project> GetProjectsPaged(string userId, bool? ownership = null, int page = 1, int pageSize = 10)
        {
            var result = new ServiceCollectionResult<Project>();
            if (!Inspector.IsValidPageSize(pageSize))
            {
                result.HandleError(ErrorMessagesDict.InvalidPageSize);
                return result;
            }
            IEnumerable<Project> targetProjects;

            if (ownership.HasValue)
            {
                targetProjects = (ownership.Value) 
                    ? GetProjectsUserCreated(userId)
                    : GetProjectsUserInvolvedOnly(userId);
            }
            else targetProjects = _accessService.GetAccessibleProjects(userId);

            result.TargetCollection = targetProjects
                                    .Skip((page - 1) * pageSize)
                                    .Take(pageSize);
            return result;
        }

        #region GetProjectsFilters

        private IEnumerable<Project> GetProjectsUserInvolvedOnly(string userId)
        {
            var userAccessibleProjects = _accessService.GetAccessibleProjects(userId);

            var userCreatedProjects = GetProjectsUserCreated(userId);

            return userAccessibleProjects.Except(userCreatedProjects);
        }

        private IEnumerable<Project> GetProjectsUserCreated(string userId)
        {
            return _context.Projects
                .Where(p => p.CreatorId.Equals(userId));
        }

        #endregion

        public ServiceSingleResult<Project> GetProject(long projectId, string userId)
        {
            var result = new ServiceSingleResult<Project>();
            var project = _context.Projects
                .SingleOrDefault(p => p.Id == projectId);

            if (project == null)
            {
                result.HandleError(ErrorMessagesDict.NotFoundResource);
                return result;
            }

            //check user access
            if (_accessService.GetAccessByProject(project.Id, userId) != null)
            {
                result.TargetObject = project;
                return result;
            }

            result.HandleError(ErrorMessagesDict.AccessDenied);
            return result;
        }

        public ServiceSingleResult<Project> GetProject(string projectName, string userId)
        {
            var result = new ServiceSingleResult<Project>();
            var project = _context.Projects
                .SingleOrDefault(p => p.Name == projectName);

            if (project == null)
            {
                result.HandleError(ErrorMessagesDict.NotFoundResource);
                return result;
            }

            //check user access
            if (_accessService.GetAccessByProject(project.Id, userId) != null)
            {
                result.TargetObject = project;
                return result;
            }

            result.HandleError(ErrorMessagesDict.AccessDenied);
            return result;
        }

        public ServiceSingleResult<bool> AddProject(Project project)
        {
            var result = new ServiceSingleResult<bool>();
            if (project != null)
            {
                //check permission to create
                int nCreatedProjects = GetProjectsUserCreated(project.CreatorId).Count();
                if (!Inspector.CanUserCreateProject(nCreatedProjects))
                {
                    result.HandleError(ErrorMessagesDict.TooMuchProjectsCreated);
                    return result;
                }

                //create user-project reference
                project.ProjectUsers.Add(
                    new ProjectUserAccess
                    {
                        UserId = project.CreatorId,
                        ProjectAccess = ProjectAccess.ProjectCreator,
                        CanGrantAccess = true
                    }
                );

                //add project
                _context.Projects.Add(project);
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

        public ServiceSingleResult<bool> UpdateProject(Project project, string userId)
        {
            var result = new ServiceSingleResult<bool>();
            if (project != null)
            {
                var projectToUpdate = _context.Projects
                    .SingleOrDefault(p => p.Id == project.Id);

                if (projectToUpdate != null)
                {
                    //check permission for editiing project info
                    var projUserRef = _accessService.GetAccessByProject(projectToUpdate.Id, userId);
                    if (projUserRef == null || !Inspector.CanUserUpdateProject(projUserRef))
                    {
                        result.HandleError(ErrorMessagesDict.AccessDenied);
                        return result;
                    }

                    _context.Entry(projectToUpdate).CurrentValues.SetValues(project);
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
    }
}
