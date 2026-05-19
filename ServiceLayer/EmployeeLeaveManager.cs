using Core;
using Core.BusinessObject;
using Core.Services;
using Data;
using NLog;

namespace ServiceLayer
{
    /// <summary>
    /// Handles employee leave business logic.
    /// </summary>
    public class EmployeeLeaveManager : IEmployeeLeaveService
    {
        private static readonly Logger Log = LogManager.GetLogger("Service Layer");
        private static readonly Logger AuditLog = LogManager.GetLogger("Audit Log");

        /// <summary>
        /// Adds employee leave after validation.
        /// </summary>
        public OperationResult Add(EmployeeLeave employeeLeave)
        {
            Log.Debug("Employee leave add process started.");

            if (employeeLeave.EmployeeId <= 0)
            {
                return new OperationResult((int)OperationStatus.Failure, SLConstants.Messages.EmployeeRequired);
            }

            if (employeeLeave.LeaveCategoryId <= 0)
            {
                return new OperationResult((int)OperationStatus.Failure, SLConstants.Messages.LeaveCategoryRequired);
            }

            if (employeeLeave.StartDate == DateTime.MinValue)
            {
                return new OperationResult((int)OperationStatus.Failure, SLConstants.Messages.LeaveStartDateRequired);
            }

            if (employeeLeave.EndDate == DateTime.MinValue)
            {
                return new OperationResult((int)OperationStatus.Failure, SLConstants.Messages.LeaveEndDateRequired);
            }

            if (employeeLeave.EndDate < employeeLeave.StartDate)
            {
                return new OperationResult((int)OperationStatus.Failure, SLConstants.Messages.LeaveInvalidDateRange);
            }

            if (string.IsNullOrWhiteSpace(employeeLeave.Reason))
            {
                return new OperationResult((int)OperationStatus.Failure, SLConstants.Messages.LeaveReasonRequired);
            }

            employeeLeave.Duration = CalculateDuration(employeeLeave.StartDate, employeeLeave.EndDate);

            EmployeeLeaveDB.Add(employeeLeave);

            Log.Info($"Leave applied. Id: {employeeLeave.Id}, EmployeeId: {employeeLeave.EmployeeId}");
            AuditLog.Info($"AUDIT: Leave applied. Id: {employeeLeave.Id}, EmployeeId: {employeeLeave.EmployeeId}, By: {employeeLeave.CreatedBy}");

            return new OperationResult(
                (int)OperationStatus.Success,
                SLConstants.Messages.EmployeeLeaveAddedSuccess,
                employeeLeave
            );
        }

        public EmployeeLeave? GetById(int id)
        {
            return EmployeeLeaveDB.GetById(id);
        }

        public OperationResult UpdateStatus(int id, string status)
        {
            EmployeeLeave? leave = GetById(id);

            if (leave == null)
            {
                return new OperationResult(
                    (int)OperationStatus.Failure,
                    "Leave request not found."
                );
            }

            EmployeeLeaveDB.UpdateStatus(id, status);

            return new OperationResult(
                (int)OperationStatus.Success,
                "Leave status updated successfully."
            );
        }

        /// <summary>
        /// Gets all employee leaves.
        /// </summary>
        public List<EmployeeLeave> GetAll()
        {
            return EmployeeLeaveDB.GetAll();
        }

        /// <summary>
        /// Calculates leave duration including both start and end date.
        /// </summary>
        private static decimal CalculateDuration(DateTime startDate, DateTime endDate)
        {
            return Convert.ToDecimal((endDate.Date - startDate.Date).TotalDays + 1);
        }
    }
}