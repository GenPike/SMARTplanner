using System;
using System.Collections.Generic;
using System.Linq;
using SMARTplanner.Data.Contracts;
using SMARTplanner.Entities.Domain;
using SMARTplanner.Logic.Business;
using SMARTplanner.Logic.Contracts;

namespace SMARTplanner.Logic.Exact
{
    public class ReportService : IReportService
    {
        private readonly ISmartPlannerContext _context;
        private readonly IAccessService _accessService;

        public ReportService(ISmartPlannerContext ctx, IAccessService access)
        {
            _context = ctx;
            _accessService = access;
        }

        public IEnumerable<Report> GetIssueReports(long issueId, string userId)
        {
            //check can user get access to project
            var issue = _context.Issues
                .SingleOrDefault(i => i.Id == issueId);

            if (issue != null)
            {
                var projUserRef = _accessService.GetAccessByIssue(issue, userId);
                if (projUserRef == null) return null;

                //get issue workitems
                return issue.Reports;
            }

            return null;
        }

        public Report GetReport(long reportId, string userId)
        {
            var report = GetReportById(reportId);
            if (report != null)
            {
                //does user have any binding with project
                var projUserRef = _accessService.GetAccessByReport(report, userId);
                if (projUserRef == null) return null;
            }

            return report;
        }

        public void AddReport(Report report)
        {
            if (report != null && report.Issue != null)
            {
                var projUserRef = _accessService.GetAccessByReport(report, report.ReporterId);
                if (projUserRef == null || !Inspector.CanUserAddReport(projUserRef)) return;

                _context.Reports.Add(report);
                _context.SaveChanges();
            }
        }

        public void UpdateReport(Report report, string userId)
        {
            if (report != null)
            {
                var reportToUpdate = GetReportById(report.Id);

                if (reportToUpdate != null)
                {
                    //check security
                    var projUserRef = _accessService.GetAccessByReport(reportToUpdate, userId);
                    if (projUserRef == null || 
                        !Inspector.CanUserUpdateReport(projUserRef, reportToUpdate, userId)) return;

                    _context.Entry(reportToUpdate).CurrentValues.SetValues(report);
                    _context.SaveChanges();
                }
            }
        }

        public void DeleteReport(long reportId, string userId)
        {
            var reportToDelete = GetReportById(reportId);
            if (reportToDelete != null)
            {
                //check security
                var projUserRef = _accessService.GetAccessByReport(reportToDelete, userId);
                if (projUserRef == null || 
                    !Inspector.CanUserDeleteReport(projUserRef, reportToDelete, userId)) return;

                _context.Reports.Remove(reportToDelete);
                _context.SaveChanges();
            }
        }

        #region Helpers

        private Report GetReportById(long reportId)
        {
            return _context.Reports
                .SingleOrDefault(r => r.Id == reportId);
        }

        #endregion
    }
}
