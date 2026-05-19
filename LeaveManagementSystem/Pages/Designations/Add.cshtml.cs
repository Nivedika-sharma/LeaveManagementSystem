using Core;
using Core.BusinessObject;
using Core.Services;
using LeaveManagementSystem.AppCode;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceLayer;
using System.ComponentModel.DataAnnotations;

namespace LeaveManagementSystem.Pages.Designations
{
    /// <summary>
    /// Add designation page model.
    /// </summary>
    public class AddModel : PageModel
    {
        [BindProperty]
        [Required(ErrorMessage = "Please enter designation name")]
        public string Name { get; set; } = string.Empty;

        [BindProperty]
        public string? Description { get; set; }

        /// <summary>
        /// Loads page.
        /// </summary>
        public void OnGet()
        {
        }

        /// <summary>
        /// Saves designation.
        /// </summary>
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Designation designation = new()
            {
                Name = Name,
                Description = Description,

                CreatedBy =
                    CommonFunction.GetCurrentUserName(HttpContext),

                UpdatedBy =
                    CommonFunction.GetCurrentUserName(HttpContext),

                CreatedOnUTC = DateTime.UtcNow,
                UpdatedOnUTC = DateTime.UtcNow,

                IPAddress =
                    CommonFunction.GetIpAddress(HttpContext)
            };

            IDesignationService designationManager =
                new DesignationManager();

            OperationResult result =
                designationManager.Add(designation);

            if (result.Status ==
                (int)OperationStatus.Success)
            {
                return RedirectToPage("List");
            }

            ModelState.AddModelError(
                string.Empty,
                result.Message
            );

            return Page();
        }
    }
}