using System;
using System.Collections.Generic;
using SMARTplanner.Entities.Domain;

namespace SMARTplanner.Logic.Contracts
{
    public interface IReportService
    {
        IEnumerable<Report> GetIssueReports(long issueId, string userId);
        Report GetReport(long reportId, string userId);
        void AddReport(Report report);
        void UpdateReport(Report report, string userId);
        void DeleteReport(long reportId, string userId);
    }
}
