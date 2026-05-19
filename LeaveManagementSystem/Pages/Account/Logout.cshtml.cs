using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace LeaveManagementSystem.Pages.Account
{
    [Microsoft.AspNetCore.Authorization.AllowAnonymous]
    public class LogoutModel : PageModel
    {
        public string? UserName { get; set; }

        public void OnGet()
        {
            UserName = User?.Identity?.Name;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await HttpContext.SignOutAsync("LmsCookie");
            return RedirectToPage("/Account/Login");
        }
    }
}
