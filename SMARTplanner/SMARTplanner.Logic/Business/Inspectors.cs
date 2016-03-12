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

        public static bool IsValidWorkItemTime(WorkItem item)
        {
            if ((item.EstimatedTime % UnitOfTime).Equals(0)) return true;
            return false;
        }

        public static bool CanUserCreateProject(int nCreatedProjects)
        {
            if (nCreatedProjects >= ProjectsMaxUserCanCreate) return false;
            return true;
        }

        public static bool CanUserUpdateProject(ProjectUserAccess pu)
        {
            if (pu.ProjectAccess == ProjectAccess.ReadReportEdit ||
                pu.ProjectAccess == ProjectAccess.ProjectCreator) return false;
            return true;
        }

        public static bool CanUserAddReport(ProjectUserAccess pu)
        {
            if (pu.ProjectAccess == ProjectAccess.ReadReport ||
                pu.ProjectAccess == ProjectAccess.ReadReportEdit ||
                pu.ProjectAccess == ProjectAccess.ProjectCreator) return true;
            return false;
        }

        public static bool CanUserUpdateReport(ProjectUserAccess pu, Report report, string userId)
        {
            if (pu.ProjectAccess == ProjectAccess.ReadReportEdit ||
                (pu.ProjectAccess == ProjectAccess.ReadReport && report.ReporterId.Equals(userId)) ||
                pu.ProjectAccess == ProjectAccess.ProjectCreator) return true;
            return false;
        }

        public static bool CanUserDeleteReport(ProjectUserAccess pu, Report report, string userId)
        {
            if (report.ReporterId.Equals(userId) || 
                pu.ProjectAccess == ProjectAccess.ProjectCreator) return true;
            return false;
        }
    }
}
