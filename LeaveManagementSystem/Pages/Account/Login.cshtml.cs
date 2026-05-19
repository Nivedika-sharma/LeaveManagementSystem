using Core.BusinessObject;
using Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace LeaveManagementSystem.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        [BindProperty]
        public string Email { get; set; } = string.Empty;

        [BindProperty]
        public string Password { get; set; } = string.Empty;

        [BindProperty]
        public bool RememberMe { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                ModelState.AddModelError(string.Empty, "Invalid credentials");
                return Page();
            }

            // authenticate user from employee table
            var emailTrim = Email?.Trim() ?? string.Empty;
            var employee = EmployeeDB.GetByEmail(emailTrim);

            // fallback: case-insensitive search if direct lookup fails
            if (employee == null)
            {
                try
                {
                    var all = EmployeeDB.GetAll();
                    employee = all.FirstOrDefault(e => string.Equals(e.Email?.Trim(), emailTrim, StringComparison.OrdinalIgnoreCase));
                }
                catch
                {
                    // ignore and continue
                }
            }

            if (employee == null || (employee.Password ?? string.Empty) != (Password ?? string.Empty))
            {
                // log for debugging (remove or replace with proper logger in production)
                Console.WriteLine($"Failed login attempt for '{emailTrim}'");
                ModelState.AddModelError(string.Empty, "Invalid email or password");
                return Page();
            }


            // role is inferred from stored status

            // create claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, employee.Email),
                new Claim(ClaimTypes.GivenName, employee.Name ?? string.Empty),
                new Claim(ClaimTypes.Role, employee.Status ?? "Employee")
            };

            var identity = new ClaimsIdentity(claims, "LmsCookie");
            var principal = new ClaimsPrincipal(identity);

            var props = new AuthenticationProperties
            {
                IsPersistent = RememberMe
            };
            if (RememberMe)
            {
                props.ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7);
            }

            await HttpContext.SignInAsync("LmsCookie", principal, props);

            // redirect based on role
            if ((employee.Status ?? string.Empty).Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToPage("/Index");
            }

            return RedirectToPage("/Index");
        }
    }
}
