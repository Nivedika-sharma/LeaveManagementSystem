using Core.BusinessObject;

namespace Core.Services
{
    /// <summary>
    /// Employee service contract.
    /// </summary>
    public interface IEmployeeService
    {
        /// <summary>
        /// Adds employee.
        /// </summary>
        OperationResult Add(Employee employee);

        /// <summary>
        /// Gets all employees.
        /// </summary>
        List<Employee> GetAll();

        /// <summary>
        /// Gets employee by email.
        /// </summary>
        Employee? GetByEmail(string email);

        /// <summary>
        /// Gets employee by name.
        /// </summary>
        Employee? GetByName(string name);
    }
}