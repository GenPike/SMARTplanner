using System;
using System.Collections.Generic;
using SMARTplanner.Entities.Domain;

namespace SMARTplanner.Logic.Contracts
{
    public interface IProjectService
    {
        IEnumerable<Project> GetProjectsPaged(string userId, bool? ownership = null, int page = 1, int pageSize = 10);
        Project GetProject(long projectId, string userId);
        Project GetProject(string projectName, string userId);
        void AddProject(Project project);
        void UpdateProject(Project project, string userId);
    }
}
