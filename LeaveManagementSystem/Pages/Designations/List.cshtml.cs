using Core.BusinessObject;
using Core.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceLayer;

namespace LeaveManagementSystem.Pages.Designations
{
    /// <summary>
    /// Designation list page model.
    /// </summary>
    public class ListModel : PageModel
    {
        /// <summary>
        /// Gets designation list.
        /// </summary>
        public List<Designation> DesignationList
        {
            get;
            set;
        } = new();

        /// <summary>
        /// Loads designation list.
        /// </summary>
        public void OnGet()
        {
            IDesignationService designationManager =
                new DesignationManager();

            DesignationList =
                designationManager.GetAll();
        }
    }
}