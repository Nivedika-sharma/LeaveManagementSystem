using Core.BusinessObject;
using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceLayer;

namespace LeaveManagementSystem.Pages.Employees
{
    /// <summary>
    /// Employee list page model.
    /// </summary>
    [Authorize]
    public class ListModel : PageModel
    {
        /// <summary>
        /// Gets employee list.
        /// </summary>
        public List<Employee> EmployeeList { get; set; } = new();

        /// <summary>
        /// Loads employee list.
        /// </summary>
        public void OnGet()
        {
            IEmployeeService employeeManager = new EmployeeManager();
            EmployeeList = employeeManager.GetAll();
        }
    }
}