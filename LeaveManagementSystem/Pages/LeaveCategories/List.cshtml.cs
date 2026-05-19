using Core.BusinessObject;
using Core.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceLayer;

namespace LeaveManagementSystem.Pages.LeaveCategories
{
    /// <summary>
    /// Leave category list page model.
    /// </summary>
    public class ListModel : PageModel
    {
        public List<LeaveCategory> LeaveCategoryList { get; set; } = new();

        public void OnGet()
        {
            ILeaveCategoryService leaveCategoryManager = new LeaveCategoryManager();
            LeaveCategoryList = leaveCategoryManager.GetAll();
        }
    }
}