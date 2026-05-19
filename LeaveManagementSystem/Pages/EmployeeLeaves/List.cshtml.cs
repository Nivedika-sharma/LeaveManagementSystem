using Core.BusinessObject;
using Core.Services;
using LeaveManagementSystem.AppCode;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceLayer;
using System.Linq;
using System.Security.Claims;

namespace LeaveManagementSystem.Pages.EmployeeLeaves
{
    [Authorize]
    public class ListModel : PageModel
    {
        public List<EmployeeLeave> EmployeeLeaveList { get; set; } = new();

        // debug helpers
        public string? DebugEmail { get; set; }
        public int? DebugEmployeeId { get; set; }
        public int DebugAllLeavesCount { get; set; }
        // debug helpers (optional) - removed from UI

        public void OnGet()
        {
            LoadLeaves();
        }

        // Temporary diagnostic endpoint: returns JSON with count and sample rows
        public JsonResult OnGetDump()
        {
            try
            {
                IEmployeeLeaveService leaveManager = new EmployeeLeaveManager();
                var all = leaveManager.GetAll() ?? new List<EmployeeLeave>();
                var sample = all.Take(10).Select(x => new { x.Id, x.EmployeeId, x.EmployeeName, x.StartDate, x.EndDate, x.Status }).ToList();
                return new JsonResult(new { count = all.Count, sample });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { error = ex.Message });
            }
        }

        public IActionResult OnGetApprove(int id)
        {
            // only admin should be able to call this; UI also hides buttons for non-admin
            if (!User.IsInRole("Admin") && !string.Equals(User.FindFirst(ClaimTypes.Role)?.Value, "Admin", StringComparison.OrdinalIgnoreCase))
            {
                return Forbid();
            }

            IEmployeeLeaveService leaveManager = new EmployeeLeaveManager();
            leaveManager.UpdateStatus(id, SLConstants.Messages.Approved);

            return RedirectToPage();
        }

        public IActionResult OnGetReject(int id)
        {
            if (!User.IsInRole("Admin") && !string.Equals(User.FindFirst(ClaimTypes.Role)?.Value, "Admin", StringComparison.OrdinalIgnoreCase))
            {
                return Forbid();
            }

            IEmployeeLeaveService leaveManager = new EmployeeLeaveManager();
            leaveManager.UpdateStatus(id, SLConstants.Messages.Rejected);

            return RedirectToPage();
        }

        private void LoadLeaves()
        {
            IEmployeeLeaveService leaveManager = new EmployeeLeaveManager();
            try
            {
                var email = User.Identity?.Name;
                if (!string.IsNullOrEmpty(email))
                {
                    var emp = Data.EmployeeDB.GetByEmail(email);
                    if (emp != null && (emp.Status ?? string.Empty).Equals("Admin", StringComparison.OrdinalIgnoreCase))
                    {
                        EmployeeLeaveList = leaveManager.GetAll() ?? new List<EmployeeLeave>();
                        return;
                    }

                    if (emp != null)
                    {
                        var all = leaveManager.GetAll() ?? new List<EmployeeLeave>();
                        EmployeeLeaveList = all.Where(x => x.EmployeeId == emp.Id).ToList();
                        return;
                    }
                }

                EmployeeLeaveList = new List<EmployeeLeave>();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                TempData["ErrorMessage"] = "Unable to load leave requests. Please check logs.";
                EmployeeLeaveList = new List<EmployeeLeave>();
            }
        }
    }
}