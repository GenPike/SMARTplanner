using System;
using SMARTplanner.Entities.Domain;
using SMARTplanner.Entities.Helpers;

namespace SMARTplanner.Logic.Business
{
    class Inspector
    {
        private const int ProjectsMaxUserCanCreate = 5;
        private const int PageSizeMax = 25;
        public const double UnitOfTime = 0.25;

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

        public static bool IsValidWorkItemTime(WorkItem item)
        {
            if ((item.EstimatedTime % UnitOfTime).Equals(0)) return true;
            return false;
        }
    }
}
