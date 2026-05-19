using Core;
using Core.BusinessObject;
using Core.Services;
using Data;
using NLog;

namespace ServiceLayer
{
    /// <summary>
    /// Handles WFH business logic.
    /// </summary>
    public class WFHManager : IWFHService
    {
        private static readonly Logger Log = LogManager.GetLogger("Service Layer");
        private static readonly Logger AuditLog = LogManager.GetLogger("Audit Log");

        /// <summary>
        /// Adds WFH request after validation.
        /// </summary>
        public OperationResult Add(WFH wfh)
        {
            Log.Debug("WFH add process started.");

            if (wfh.EmployeeId <= 0)
            {
                return new OperationResult((int)OperationStatus.Failure, SLConstants.Messages.EmployeeRequired);
            }

            if (wfh.EndDate < wfh.StartDate)
            {
                return new OperationResult((int)OperationStatus.Failure, SLConstants.Messages.WFHInvalidDateRange);
            }

            if (string.IsNullOrWhiteSpace(wfh.Reason))
            {
                return new OperationResult((int)OperationStatus.Failure, SLConstants.Messages.WFHReasonRequired);
            }

            wfh.Duration = CalculateDuration(wfh.StartDate, wfh.EndDate);

            WFHDB.Add(wfh);

            Log.Info($"WFH request added. Id: {wfh.Id}, EmployeeId: {wfh.EmployeeId}");
            AuditLog.Info($"AUDIT: WFH request added. Id: {wfh.Id}, EmployeeId: {wfh.EmployeeId}, By: {wfh.CreatedBy}");

            return new OperationResult(
                (int)OperationStatus.Success,
                SLConstants.Messages.WFHAddedSuccess,
                wfh
            );
        }

        /// <summary>
        /// Gets all WFH requests.
        /// </summary>
        public List<WFH> GetAll()
        {
            return WFHDB.GetAll();
        }

        /// <summary>
        /// Calculates duration.
        /// </summary>
        private static decimal CalculateDuration(DateTime startDate, DateTime endDate)
        {
            return Convert.ToDecimal((endDate.Date - startDate.Date).TotalDays + 1);
        }
    }
}