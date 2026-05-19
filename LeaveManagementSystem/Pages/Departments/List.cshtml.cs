using Core.BusinessObject;
using Core.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceLayer;

namespace LeaveManagementSystem.Pages.Departments
{
    /// <summary>
    /// Department list page model.
    /// </summary>
    public class ListModel : PageModel
    {
        /// <summary>
        /// Gets or sets department list.
        /// </summary>
        public List<Department> DepartmentList { get; set; } = new();

        /// <summary>
        /// Loads departments.
        /// </summary>
        public void OnGet()
        {
            IDepartmentService departmentManager = new DepartmentManager();
            DepartmentList = departmentManager.GetAll();
        }
    }
}