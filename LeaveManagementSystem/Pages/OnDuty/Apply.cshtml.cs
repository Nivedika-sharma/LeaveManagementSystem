using Core;
using Core.BusinessObject;
using Core.Services;
using LeaveManagementSystem.AppCode;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceLayer;
using System.ComponentModel.DataAnnotations;

namespace LeaveManagementSystem.Pages.OnDuty
{
    /// <summary>
    /// Apply OnDuty page model.
    /// </summary>
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class ApplyModel : PageModel
    {
        public bool IsAdmin { get; set; }
        [BindProperty]
        [Required(ErrorMessage = "Please select employee")]
        public int EmployeeId { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Please select start date")]
        public DateTime StartDate { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Please select end date")]
        public DateTime EndDate { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Please enter reason")]
        public string Reason { get; set; } = string.Empty;

        [BindProperty]
        public IFormFile? SupportingDocument { get; set; }

        public List<Employee> EmployeeList { get; set; } = new();

        private void LoadDropdowns(int? currentEmployeeId = null)
        {
            IEmployeeService employeeManager = new EmployeeManager();
            EmployeeList = employeeManager.GetAll();

            if (currentEmployeeId.HasValue)
            {
                EmployeeList = EmployeeList.OrderByDescending(e => e.Id == currentEmployeeId.Value).ToList();
            }
        }

        public void OnGet()
        {
            var email = User.Identity?.Name;
            IsAdmin = false;
            int? currentEmployeeId = null;
            if (!string.IsNullOrEmpty(email))
            {
                var emp = Data.EmployeeDB.GetByEmail(email);
                if (emp != null)
                {
                    if ((emp.Status ?? string.Empty).Equals("Admin", StringComparison.OrdinalIgnoreCase))
                    {
                        IsAdmin = true;
                    }
                    else
                    {
                        currentEmployeeId = emp.Id;
                        EmployeeId = emp.Id;
                    }
                }
            }

            LoadDropdowns(currentEmployeeId);
        }

        public IActionResult OnPost()
        {
            LoadDropdowns();

            if (!ModelState.IsValid)
            {
                return Page();
            }

            string? documentName = null;

            if (SupportingDocument != null)
            {
                string uploadFolder = CommonFunction.GetUploadFolderPath();

                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }

                documentName = $"{Guid.NewGuid()}_{SupportingDocument.FileName}";
                string filePath = Path.Combine(uploadFolder, documentName);

                using FileStream stream = new(filePath, FileMode.Create);
                SupportingDocument.CopyTo(stream);
            }

            var email = User.Identity?.Name;
            if (!string.IsNullOrEmpty(email))
            {
                var emp = Data.EmployeeDB.GetByEmail(email);
                if (emp != null && !(emp.Status ?? string.Empty).Equals("Admin", StringComparison.OrdinalIgnoreCase))
                {
                    EmployeeId = emp.Id;
                }
            }

            Core.BusinessObject.OnDuty onDuty = new()
            {
                EmployeeId = EmployeeId,
                StartDate = StartDate,
                EndDate = EndDate,
                Reason = Reason,
                SupportingDocument = documentName,
                CreatedBy = CommonFunction.GetCurrentUserName(HttpContext),
                UpdatedBy = CommonFunction.GetCurrentUserName(HttpContext),
                CreatedOnUTC = DateTime.UtcNow,
                UpdatedOnUTC = DateTime.UtcNow,
                IPAddress = CommonFunction.GetIpAddress(HttpContext)
            };

            IOnDutyService onDutyManager = new OnDutyManager();
            OperationResult result = onDutyManager.Add(onDuty);

            if (result.Status == (int)OperationStatus.Success)
            {
                TempData["SuccessMessage"] = result.Message ?? "OnDuty request applied successfully.";
                return RedirectToPage("List");
            }

            ModelState.AddModelError(string.Empty, result.Message);
            return Page();
        }
    }
}