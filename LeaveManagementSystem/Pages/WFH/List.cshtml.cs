using Core.BusinessObject;
using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceLayer;

namespace LeaveManagementSystem.Pages.WFH
{
    /// <summary>
    /// WFH list page model.
    /// </summary>
[Authorize]
public class ListModel : PageModel
    {
        public List<Core.BusinessObject.WFH> WFHList { get; set; } = new();

        public void OnGet()
        {
            IWFHService wfhManager = new WFHManager();

            var email = User.Identity?.Name;
            if (!string.IsNullOrEmpty(email))
            {
                var emp = Data.EmployeeDB.GetByEmail(email);
                if (emp != null && (emp.Status ?? string.Empty).Equals("Admin", StringComparison.OrdinalIgnoreCase))
                {
                    WFHList = wfhManager.GetAll();
                    return;
                }

                if (emp != null)
                {
                    WFHList = wfhManager.GetAll().Where(x => x.EmployeeId == emp.Id).ToList();
                    return;
                }
            }

            WFHList = new List<Core.BusinessObject.WFH>();
        }
    }
}