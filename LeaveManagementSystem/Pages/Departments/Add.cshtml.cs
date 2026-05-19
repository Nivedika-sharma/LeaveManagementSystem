using Core;
using Core.Services;
using LeaveManagementSystem.AppCode;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceLayer;
using System.ComponentModel.DataAnnotations;

namespace LeaveManagementSystem.Pages.Departments
{
    public class AddModel : PageModel
    {
        [BindProperty]
        [Required(ErrorMessage = "Please enter department name")]
        public string Name { get; set; } = string.Empty;

        [BindProperty]
        public string? Description { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Core.BusinessObject.Department department = new()
            {
                Name = Name,
                Description = Description,
                CreatedBy = CommonFunction.GetCurrentUserName(HttpContext),
                UpdatedBy = CommonFunction.GetCurrentUserName(HttpContext),
                CreatedOnUTC = DateTime.UtcNow,
                UpdatedOnUTC = DateTime.UtcNow,
                IPAddress = CommonFunction.GetIpAddress(HttpContext)
            };

            IDepartmentService departmentManager = new DepartmentManager();
            OperationResult result = departmentManager.Add(department);

            if (result.Status == (int)OperationStatus.Success)
            {
                return RedirectToPage("List");
            }

            ModelState.AddModelError(string.Empty, result.Message);
            return Page();
        }
    }
}