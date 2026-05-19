using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceLayer;

namespace LeaveManagementSystem.Pages.OnDuty
{
    /// <summary>
    /// OnDuty list page model.
    /// </summary>
[Authorize]
public class ListModel : PageModel
    {
        public List<Core.BusinessObject.OnDuty> OnDutyList { get; set; } = new();

        public void OnGet()
        {
            IOnDutyService onDutyManager = new OnDutyManager();

            var email = User.Identity?.Name;
            if (!string.IsNullOrEmpty(email))
            {
                var emp = Data.EmployeeDB.GetByEmail(email);
                if (emp != null && (emp.Status ?? string.Empty).Equals("Admin", StringComparison.OrdinalIgnoreCase))
                {
                    OnDutyList = onDutyManager.GetAll();
                    return;
                }

                if (emp != null)
                {
                    OnDutyList = onDutyManager.GetAll().Where(x => x.EmployeeId == emp.Id).ToList();
                    return;
                }
            }

            OnDutyList = new List<Core.BusinessObject.OnDuty>();
        }
    }
}