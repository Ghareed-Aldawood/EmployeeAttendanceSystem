using EmployeeAttendanceSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeAttendanceSystem.Pages.Admin.Employees
{
    public class CreateModel : PageModel
    {
        private readonly UserManager<Employee> _userManager;

        public CreateModel(UserManager<Employee> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public Employee Input { get; set; } = new();

        [BindProperty]
        public string Password { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            Input.UserName = Input.Email;

            var result = await _userManager.CreateAsync(Input, Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(Input, "Employee");
                return RedirectToPage("Index");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return Page();
        }
    }
}
