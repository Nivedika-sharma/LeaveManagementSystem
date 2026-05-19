using Core;
using Core.BusinessObject;
using Core.Services;
using Data;
using NLog;

namespace ServiceLayer
{
    /// <summary>
    /// Handles department business logic.
    /// </summary>
    public class DepartmentManager : IDepartmentService
    {
        private static readonly Logger Log = LogManager.GetLogger("Service Layer");
        private static readonly Logger AuditLog = LogManager.GetLogger("Audit Log");

        /// <summary>
        /// Adds department after validation.
        /// </summary>
        public OperationResult Add(Department department)
        {
            Log.Debug("Department add process started.");

            if (string.IsNullOrWhiteSpace(department.Name))
            {
                Log.Warn("Department add failed because name is empty.");

                return new OperationResult(
                    (int)OperationStatus.Failure,
                    SLConstants.Messages.DepartmentNameRequired
                );
            }

            Department? existingDepartment = GetByName(department.Name);
            if (existingDepartment != null)
            {
                Log.Warn($"Department add failed. Duplicate name: {department.Name}");

                return new OperationResult(
                    (int)OperationStatus.Failure,
                    SLConstants.Messages.DepartmentNameUnique
                );
            }

            DepartmentDB.Add(department);

            Log.Info($"Department added successfully. Name: {department.Name}");
            AuditLog.Info($"AUDIT: Department added. Id: {department.Id}, Name: {department.Name}, By: {department.CreatedBy}");

            return new OperationResult(
                (int)OperationStatus.Success,
                SLConstants.Messages.DepartmentAddedSuccess,
                department
            );
        }

        /// <summary>
        /// Gets all departments.
        /// </summary>
        public List<Department> GetAll()
        {
            Log.Debug("Fetching all departments.");
            return DepartmentDB.GetAll();
        }

        /// <summary>
        /// Gets department by name.
        /// </summary>
        public Department? GetByName(string name)
        {
            Log.Debug($"Fetching department by name: {name}");
            return DepartmentDB.GetByName(name);
        }
    }
}