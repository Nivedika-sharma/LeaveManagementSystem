using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Data;
using System.Linq;
using Core.BusinessObject;
using ServiceLayer;

namespace LeaveManagementSystem.Pages
{
    public class IndexModel : PageModel
    {
        public int EmployeesCount { get; set; }

        public int LeaveRequestsCount { get; set; }

        public int WFHCount { get; set; }

        public int OnDutyCount { get; set; }

        public List<EmployeeLeave> RecentLeaves { get; set; } = new();

        public List<Core.BusinessObject.WFH> RecentWfhs { get; set; } = new();

        public List<Core.BusinessObject.OnDuty> RecentOnDuties { get; set; } = new();

        public void OnGet()
        {
            // Show dashboard data scoped to the logged-in employee.
            // If the user is Admin, show global counts; otherwise show counts only for the current employee.
            try
            {
                var email = User.Identity?.Name;
                if (!string.IsNullOrEmpty(email))
                {
                    var emp = EmployeeDB.GetByEmail(email);
                    if (emp != null && (emp.Status ?? string.Empty).Equals("Admin", StringComparison.OrdinalIgnoreCase))
                    {
                        // Admin: global counts
                        EmployeesCount = EmployeeDB.GetAll()?.Count ?? 0;
                        LeaveRequestsCount = EmployeeLeaveDB.GetAll()?.Count ?? 0;
                        WFHCount = WFHDB.GetAll()?.Count ?? 0;
                        OnDutyCount = OnDutyDB.GetAll()?.Count ?? 0;
                        return;
                    }

                    if (emp != null)
                    {
                        // Employee: only their own counts
                        EmployeesCount = 1;
                        var leaves = EmployeeLeaveDB.GetAll() ?? new System.Collections.Generic.List<Core.BusinessObject.EmployeeLeave>();
                        LeaveRequestsCount = leaves.Count(x => x.EmployeeId == emp.Id);

                        var wfh = WFHDB.GetAll() ?? new System.Collections.Generic.List<Core.BusinessObject.WFH>();
                        WFHCount = wfh.Count(x => x.EmployeeId == emp.Id);

                        var onduty = OnDutyDB.GetAll() ?? new System.Collections.Generic.List<Core.BusinessObject.OnDuty>();
                        OnDutyCount = onduty.Count(x => x.EmployeeId == emp.Id);

                        // load recent items for dashboard
                        var leaveManager = new EmployeeLeaveManager();
                        RecentLeaves = leaveManager.GetAll()?.Where(x => x.EmployeeId == emp.Id).OrderByDescending(x => x.CreatedOnUTC).Take(5).ToList() ?? new List<EmployeeLeave>();

                        var wfhManager = new WFHManager();
                        RecentWfhs = wfhManager.GetAll()?.Where(x => x.EmployeeId == emp.Id).OrderByDescending(x => x.CreatedOnUTC).Take(5).ToList() ?? new List<Core.BusinessObject.WFH>();

                        var onDutyManager = new OnDutyManager();
                        RecentOnDuties = onDutyManager.GetAll()?.Where(x => x.EmployeeId == emp.Id).OrderByDescending(x => x.CreatedOnUTC).Take(5).ToList() ?? new List<Core.BusinessObject.OnDuty>();
                        return;
                    }
                }

                // fallback: zeroes
                EmployeesCount = 0;
                LeaveRequestsCount = 0;
                WFHCount = 0;
                OnDutyCount = 0;
            }
            catch
            {
                EmployeesCount = 0;
                LeaveRequestsCount = 0;
                WFHCount = 0;
                OnDutyCount = 0;
            }
        }
    }
}
