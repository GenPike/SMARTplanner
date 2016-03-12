using System;
using System.Collections.Generic;
using System.Linq;
using SMARTplanner.Data.Contracts;
using SMARTplanner.Entities.Domain;
using SMARTplanner.Logic.Contracts;

namespace SMARTplanner.Logic.Exact
{
    public class ReportService : IReportService
    {
        private ISmartPlannerContext _context;

        public ReportService(ISmartPlannerContext ctx)
        {
            _context = ctx;
        }

        public IEnumerable<Report> GetIssueReports(long issueId, string userId)
        {
            throw new NotImplementedException();
        }

        public Report GetReport(long reportId, string userId)
        {
            throw new NotImplementedException();
        }

        public void AddReport(Report report, string userId)
        {
            throw new NotImplementedException();
        }

        public void UpdateReport(Report report, string userId)
        {
            throw new NotImplementedException();
        }

        public void DeleteReport(Report report, string userId)
        {
            throw new NotImplementedException();
        }

        #region Helpers

        private ProjectUserAccess GetUserAccess(string userId)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
