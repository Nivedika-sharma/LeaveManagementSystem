using Core;
using Core.BusinessObject;
using Core.Services;
using Data;
using NLog;

namespace ServiceLayer
{
    /// <summary>
    /// Handles OnDuty business logic.
    /// </summary>
    public class OnDutyManager : IOnDutyService
    {
        private static readonly Logger Log = LogManager.GetLogger("Service Layer");
        private static readonly Logger AuditLog = LogManager.GetLogger("Audit Log");

        /// <summary>
        /// Adds OnDuty request after validation.
        /// </summary>
        public OperationResult Add(OnDuty onDuty)
        {
            Log.Debug("OnDuty add process started.");

            if (onDuty.EmployeeId <= 0)
            {
                return new OperationResult((int)OperationStatus.Failure, SLConstants.Messages.EmployeeRequired);
            }

            if (onDuty.EndDate < onDuty.StartDate)
            {
                return new OperationResult((int)OperationStatus.Failure, SLConstants.Messages.OnDutyInvalidDateRange);
            }

            if (string.IsNullOrWhiteSpace(onDuty.Reason))
            {
                return new OperationResult((int)OperationStatus.Failure, SLConstants.Messages.OnDutyReasonRequired);
            }

            onDuty.Duration = CalculateDuration(onDuty.StartDate, onDuty.EndDate);

            OnDutyDB.Add(onDuty);

            Log.Info($"OnDuty request added. Id: {onDuty.Id}, EmployeeId: {onDuty.EmployeeId}");
            AuditLog.Info($"AUDIT: OnDuty request added. Id: {onDuty.Id}, EmployeeId: {onDuty.EmployeeId}, By: {onDuty.CreatedBy}");

            return new OperationResult(
                (int)OperationStatus.Success,
                SLConstants.Messages.OnDutyAddedSuccess,
                onDuty
            );
        }

        /// <summary>
        /// Gets all OnDuty requests.
        /// </summary>
        public List<OnDuty> GetAll()
        {
            return OnDutyDB.GetAll();
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