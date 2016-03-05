﻿using System;
using System.Collections.Generic;
using SMARTplanner.Entities.Domain;

namespace SMARTplanner.Logic.Contracts
{
    public interface IProjectService
    {
        IEnumerable<Project> GetProjectsPaged(string userId, bool? ownership = null, int page = 1, int pageSize = 10);
        Project GetProject(long projectId);
        Project GetProject(string projectName);
        void AddProject(Project project);
        void UpdateProject(Project project);
    }
}
