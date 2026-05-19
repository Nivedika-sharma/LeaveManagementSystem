using Core;
using Core.BusinessObject;
using Core.Services;
using LeaveManagementSystem.AppCode;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceLayer;
using System.ComponentModel.DataAnnotations;

namespace LeaveManagementSystem.Pages.EmployeeLeaves
{
    [Authorize]
    public class ApplyModel : PageModel
    {
        public bool IsAdmin { get; set; }
        [BindProperty]
        [Required(ErrorMessage = "Please select employee")]
        public int EmployeeId { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Please select leave category")]
        public int LeaveCategoryId { get; set; }

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

        public List<LeaveCategory> LeaveCategoryList { get; set; } = new();

        /// <summary>
        /// Loads dropdowns.
        /// </summary>
        private void LoadDropdowns()
        {
            IEmployeeService employeeManager = new EmployeeManager();
            EmployeeList = employeeManager.GetAll();

            ILeaveCategoryService leaveCategoryManager = new LeaveCategoryManager();
            LeaveCategoryList = leaveCategoryManager.GetAll();
        }

        /// <summary>
        /// Loads apply leave page.
        /// </summary>
        public void OnGet()
        {
            // determine if current user is admin and preselect employee for non-admins
            IsAdmin = User.IsInRole("Admin");

            var email = User.Identity?.Name;
            if (!string.IsNullOrEmpty(email))
            {
                var emp = Data.EmployeeDB.GetByEmail(email);
                if (emp != null)
                {
                    IsAdmin = IsAdmin || (emp.Status ?? string.Empty).Equals("Admin", StringComparison.OrdinalIgnoreCase);
                    if (!IsAdmin)
                    {
                        EmployeeId = emp.Id;
                    }
                }
            }

            LoadDropdowns();
        }

        /// <summary>
        /// Applies leave.
        /// </summary>
        public IActionResult OnPost()
        {
            // determine admin and enforce employee for non-admins
            IsAdmin = User.IsInRole("Admin");
            var email = User.Identity?.Name;
            if (!string.IsNullOrEmpty(email))
            {
                var emp = Data.EmployeeDB.GetByEmail(email);
                if (emp != null)
                {
                    IsAdmin = IsAdmin || (emp.Status ?? string.Empty).Equals("Admin", StringComparison.OrdinalIgnoreCase);
                    if (!IsAdmin)
                    {
                        EmployeeId = emp.Id;
                    }
                }
            }

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

                using FileStream stream = new FileStream(filePath, FileMode.Create);
                SupportingDocument.CopyTo(stream);
            }

            EmployeeLeave employeeLeave = new EmployeeLeave
            {
                EmployeeId = EmployeeId,
                LeaveCategoryId = LeaveCategoryId,
                StartDate = StartDate,
                EndDate = EndDate,
                Reason = Reason,
                Status = SLConstants.Messages.Pending,
                SupportingDocument = documentName,
                CreatedBy = CommonFunction.GetCurrentUserName(HttpContext),
                UpdatedBy = CommonFunction.GetCurrentUserName(HttpContext),
                CreatedOnUTC = DateTime.UtcNow,
                UpdatedOnUTC = DateTime.UtcNow,
                IPAddress = CommonFunction.GetIpAddress(HttpContext)
            };

            IEmployeeLeaveService employeeLeaveManager = new EmployeeLeaveManager();
            OperationResult result = employeeLeaveManager.Add(employeeLeave);

            if (result.Status == (int)OperationStatus.Success)
            {
                TempData["SuccessMessage"] = result.Message ?? "Leave applied successfully.";
                return RedirectToPage("List");
            }

            ModelState.AddModelError(string.Empty, result.Message);
            return Page();
        }
    }
}