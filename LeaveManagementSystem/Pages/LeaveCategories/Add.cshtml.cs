using Core;
using Core.BusinessObject;
using Core.Services;
using LeaveManagementSystem.AppCode;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceLayer;
using System.ComponentModel.DataAnnotations;

namespace LeaveManagementSystem.Pages.LeaveCategories
{
    /// <summary>
    /// Add leave category page model.
    /// </summary>
    public class AddModel : PageModel
    {
        [BindProperty]
        [Required(ErrorMessage = "Please enter title")]
        public string Title { get; set; } = string.Empty;

        [BindProperty]
        public string? Description { get; set; }

        [BindProperty]
        public bool IsDocumentRequired { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            LeaveCategory leaveCategory = new()
            {
                Title = Title,
                Description = Description,
                IsDocumentRequired = IsDocumentRequired,
                CreatedBy = CommonFunction.GetCurrentUserName(HttpContext),
                UpdatedBy = CommonFunction.GetCurrentUserName(HttpContext),
                CreatedOnUTC = DateTime.UtcNow,
                UpdatedOnUTC = DateTime.UtcNow,
                IPAddress = CommonFunction.GetIpAddress(HttpContext)
            };

            ILeaveCategoryService leaveCategoryManager = new LeaveCategoryManager();
            OperationResult result = leaveCategoryManager.Add(leaveCategory);

            if (result.Status == (int)OperationStatus.Success)
            {
                return RedirectToPage("List");
            }

            ModelState.AddModelError(string.Empty, result.Message);
            return Page();
        }
    }
}