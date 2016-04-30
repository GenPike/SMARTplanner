using System;
using System.Linq;
using SMARTplanner.Data.Contracts;
using SMARTplanner.Entities.Domain;
using SMARTplanner.Entities.Helpers;
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

        public ServiceCollectionResult<Report> GetIssueReports(long issueId, string userId)
        {
            var result = new ServiceCollectionResult<Report>();
            //check can user get access to project
            var issue = _context.Issues
                .SingleOrDefault(i => i.Id == issueId);

            if (issue != null)
            {
                var projUserRef = _accessService.GetAccessByIssue(issue, userId);
                
                if (projUserRef == null) result.HandleError(ErrorMessagesDict.AccessDenied);
                else result.TargetCollection = issue.Reports;

                return result;
            }

            result.HandleError(ErrorMessagesDict.NotFoundResource);
            return result;
        }

        public ServiceSingleResult<Report> GetReport(long reportId, string userId)
        {
            var result = new ServiceSingleResult<Report>();
            var report = GetReportById(reportId);
            if (report != null)
            {
                //does user have any binding with project
                var projUserRef = _accessService.GetAccessByReport(report, userId);
                
                if (projUserRef == null) result.HandleError(ErrorMessagesDict.AccessDenied);
                else result.TargetObject = report;

                return result;
            }

            result.HandleError(ErrorMessagesDict.NotFoundResource);
            return result;
        }

        public ServiceSingleResult<bool> AddReport(Report report)
        {
            var result = new ServiceSingleResult<bool>();
            if (report != null && report.Issue != null)
            {
                var projUserRef = _accessService.GetAccessByReport(report, report.ReporterId);
                if (projUserRef == null || !Inspector.CanUserAddReport(projUserRef))
                {
                    result.HandleError(ErrorMessagesDict.AccessDenied);
                    return result;
                }

                _context.Reports.Add(report);
                try
                {
                    _context.SaveChanges();
                    result.TargetObject = true;
                }
                catch (Exception exc)
                {
                    result.HandleError(exc.Message);
                }

                return result;
            }

            result.HandleError(ErrorMessagesDict.NullInstance);
            return result;
        }

        public ServiceSingleResult<bool> UpdateReport(Report report, string userId)
        {
            var result = new ServiceSingleResult<bool>();
            if (report != null)
            {
                var reportToUpdate = GetReportById(report.Id);

                if (reportToUpdate != null)
                {
                    //check security
                    var projUserRef = _accessService.GetAccessByReport(reportToUpdate, userId);
                    if (projUserRef == null ||
                        !Inspector.CanUserUpdateReport(projUserRef, reportToUpdate, userId))
                    {
                        result.HandleError(ErrorMessagesDict.AccessDenied);
                        return result;
                    }

                    _context.Entry(reportToUpdate).CurrentValues.SetValues(report);
                    try
                    {
                        _context.SaveChanges();
                        result.TargetObject = true;
                    }
                    catch (Exception exc)
                    {
                        result.HandleError(exc.Message);
                    }
                    return result;
                }

                result.HandleError(ErrorMessagesDict.NotFoundResource);
                return result;
            }

            result.HandleError(ErrorMessagesDict.NullInstance);
            return result;
        }

        public ServiceSingleResult<bool> DeleteReport(long reportId, string userId)
        {
            var result = new ServiceSingleResult<bool>();
            var reportToDelete = GetReportById(reportId);
            if (reportToDelete != null)
            {
                //check security
                var projUserRef = _accessService.GetAccessByReport(reportToDelete, userId);
                if (projUserRef == null ||
                    !Inspector.CanUserDeleteReport(projUserRef, reportToDelete, userId))
                {
                    result.HandleError(ErrorMessagesDict.AccessDenied);
                    return result;
                }

                _context.Reports.Remove(reportToDelete);
                try
                {
                    _context.SaveChanges();
                    result.TargetObject = true;
                }
                catch (Exception exc)
                {
                    result.HandleError(exc.Message);
                }
                return result;
            }

            result.HandleError(ErrorMessagesDict.NotFoundResource);
            return result;
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
