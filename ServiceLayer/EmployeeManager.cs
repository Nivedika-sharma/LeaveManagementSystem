using Core;
using Core.BusinessObject;
using Core.Services;
using Data;
using NLog;

namespace ServiceLayer
{
    /// <summary>
    /// Handles employee business logic.
    /// </summary>
    public class EmployeeManager : IEmployeeService
    {
        private static readonly Logger Log = LogManager.GetLogger("Service Layer");
        private static readonly Logger AuditLog = LogManager.GetLogger("Audit Log");

        /// <summary>
        /// Adds employee after validation.
        /// </summary>
        public OperationResult Add(Employee employee)
        {
            Log.Debug("Employee add process started.");

            if (string.IsNullOrWhiteSpace(employee.Name))
            {
                return new OperationResult((int)OperationStatus.Failure, SLConstants.Messages.EmployeeNameRequired);
            }

            if (string.IsNullOrWhiteSpace(employee.Email))
            {
                return new OperationResult((int)OperationStatus.Failure, SLConstants.Messages.EmployeeEmailRequired);
            }

            if (string.IsNullOrWhiteSpace(employee.Password))
            {
                return new OperationResult((int)OperationStatus.Failure, SLConstants.Messages.EmployeePasswordRequired);
            }

            if (!string.IsNullOrWhiteSpace(employee.Phone) && employee.Phone.Length > 12)
            {
                return new OperationResult((int)OperationStatus.Failure, SLConstants.Messages.EmployeePhoneLength);
            }

            Employee? existingName = GetByName(employee.Name);
            if (existingName != null)
            {
                return new OperationResult((int)OperationStatus.Failure, SLConstants.Messages.EmployeeNameUnique);
            }

            Employee? existingEmail = GetByEmail(employee.Email);
            if (existingEmail != null)
            {
                return new OperationResult((int)OperationStatus.Failure, SLConstants.Messages.EmployeeEmailUnique);
            }

            EmployeeDB.Add(employee);

            Log.Info($"Employee added. Id: {employee.Id}, Name: {employee.Name}");
            AuditLog.Info($"AUDIT: Employee added. Id: {employee.Id}, Email: {employee.Email}, By: {employee.CreatedBy}");

            return new OperationResult(
                (int)OperationStatus.Success,
                SLConstants.Messages.EmployeeAddedSuccess,
                employee
            );
        }

        /// <summary>
        /// Gets all employees.
        /// </summary>
        public List<Employee> GetAll()
        {
            Log.Debug("Fetching all employees.");
            return EmployeeDB.GetAll();
        }

        /// <summary>
        /// Gets employee by email.
        /// </summary>
        public Employee? GetByEmail(string email)
        {
            return EmployeeDB.GetByEmail(email);
        }

        /// <summary>
        /// Gets employee by name.
        /// </summary>
        public Employee? GetByName(string name)
        {
            return EmployeeDB.GetByName(name);
        }
    }
}