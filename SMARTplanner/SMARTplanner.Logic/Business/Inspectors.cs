using System;
using System.Collections.Generic;
using SMARTplanner.Entities.Domain;
using SMARTplanner.Entities.Helpers;

namespace SMARTplanner.Logic.Business
{
    class Inspector
    {
        private const int ProjectsMaxUserCanCreate = 5;
        private const int PageSizeMax = 50;

        public static bool IsValidPageSize(int pageSize)
        {
            if (pageSize > PageSizeMax) return false;
            return true;
        }

        public static bool CanUserCreateProject(int nCreatedProjects)
        {
            if (nCreatedProjects >= ProjectsMaxUserCanCreate) return false;
            return true;
        }

        public static bool CanUserUpdateProject(ProjectUserAccess pu)
        {
            if (pu == null || pu.ProjectAccess != ProjectAccess.ReadReportEdit) return false;
            return true;
        }
    }
}
