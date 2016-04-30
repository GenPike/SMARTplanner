using System;
using System.Collections.Generic;
using SMARTplanner.Entities.Domain;
using SMARTplanner.Entities.Helpers;

namespace SMARTplanner.Logic.Contracts
{
    public interface IReportService
    {
        ServiceCollectionResult<Report> GetIssueReports(long issueId, string userId);
        ServiceSingleResult<Report> GetReport(long reportId, string userId);
        ServiceSingleResult<bool> AddReport(Report report);
        ServiceSingleResult<bool> UpdateReport(Report report, string userId);
        ServiceSingleResult<bool> DeleteReport(long reportId, string userId);
    }
}
