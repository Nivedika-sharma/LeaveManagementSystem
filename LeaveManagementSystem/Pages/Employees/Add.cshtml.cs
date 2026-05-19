using Core;
using Core.BusinessObject;
using Core.Services;
using LeaveManagementSystem.AppCode;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceLayer;
using System.ComponentModel.DataAnnotations;

namespace LeaveManagementSystem.Pages.Employees
{
    /// <summary>
    /// Add employee page model.
    /// </summary>
    public class AddModel : PageModel
    {
        [BindProperty]
        [Required(ErrorMessage = "Please enter name")]
        public string Name { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "Please enter email")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "Please enter password")]
        public string Password { get; set; } = string.Empty;

        [BindProperty]
        public string? Phone { get; set; }

        [BindProperty]
        public DateTime? DOB { get; set; }

        [BindProperty]
        public string? Status { get; set; }

        [BindProperty]
        public int? DepartmentId { get; set; }

        [BindProperty]
        public int? DesignationId { get; set; }

        [BindProperty]
        public IFormFile? ProfilePic { get; set; }

        public List<Department> DepartmentList { get; set; } = new();

        public List<Designation> DesignationList { get; set; } = new();

        public List<string> StatusList { get; set; } = new();

        /// <summary>
        /// Loads dropdowns.
        /// </summary>
        private void LoadDropdowns()
        {
            IDepartmentService departmentManager = new DepartmentManager();
            DepartmentList = departmentManager.GetAll();

            IDesignationService designationManager = new DesignationManager();
            DesignationList = designationManager.GetAll();

            StatusList = new List<string>
            {
                Constants.Active,
                Constants.InActive
            };
        }

        /// <summary>
        /// Loads page.
        /// </summary>
        public void OnGet()
        {
            LoadDropdowns();
        }

        /// <summary>
        /// Saves employee.
        /// </summary>
        public IActionResult OnPost()
        {
            LoadDropdowns();

            if (!ModelState.IsValid)
            {
                return Page();
            }

            Employee employee = new()
            {
                Name = Name,
                Email = Email,
                Password = Password,
                Phone = Phone,
                DOB = DOB,
                Status = Status,
                DepartmentId = DepartmentId,
                DesignationId = DesignationId,

                CreatedBy = CommonFunction.GetCurrentUserName(HttpContext),
                UpdatedBy = CommonFunction.GetCurrentUserName(HttpContext),
                CreatedOnUTC = DateTime.UtcNow,
                UpdatedOnUTC = DateTime.UtcNow,
                IPAddress = CommonFunction.GetIpAddress(HttpContext)
            };

            if (ProfilePic != null)
            {
                string uploadFolder = CommonFunction.GetUploadFolderPath();

                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }

                string fileName = $"{Guid.NewGuid()}_{ProfilePic.FileName}";
                string filePath = Path.Combine(uploadFolder, fileName);

                using FileStream stream = new(filePath, FileMode.Create);
                ProfilePic.CopyTo(stream);

                employee.ProfilePic = fileName;
            }

            IEmployeeService employeeManager = new EmployeeManager();
            OperationResult result = employeeManager.Add(employee);

            if (result.Status == (int)OperationStatus.Success)
            {
                return RedirectToPage("List");
            }

            ModelState.AddModelError(string.Empty, result.Message);

            return Page();
        }
    }
}