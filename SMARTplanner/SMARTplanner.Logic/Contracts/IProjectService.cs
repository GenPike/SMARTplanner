using System;
using SMARTplanner.Entities.Domain;
using SMARTplanner.Entities.Helpers;

namespace SMARTplanner.Logic.Contracts
{
    public interface IProjectService
    {
        ServiceCollectionResult<Project> GetProjectsPaged(string userId, bool? ownership = null, int page = 1, int pageSize = 10);
        ServiceSingleResult<Project> GetProject(long projectId, string userId);
        ServiceSingleResult<Project> GetProject(string projectName, string userId);
        ServiceSingleResult<bool> AddProject(Project project);
        ServiceSingleResult<bool> UpdateProject(Project project, string userId);
    }
}
