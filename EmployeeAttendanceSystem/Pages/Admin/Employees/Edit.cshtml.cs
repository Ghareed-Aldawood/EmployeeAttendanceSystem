using EmployeeAttendanceSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeAttendanceSystem.Pages.Admin.Employees
{
    public class EditModel : PageModel
    {
        private readonly UserManager<Employee> _userManager;

        public EditModel(UserManager<Employee> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public Employee Input { get; set; } = new();
        [BindProperty]
        public string? Password { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var employee = await _userManager.FindByIdAsync(id);
            if (employee == null)
                return NotFound();

            Input = employee;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var employee = await _userManager.FindByIdAsync(Input.Id);
            if (employee == null)
                return NotFound();

            employee.FullName = Input.FullName;
            employee.Email = Input.Email;
            employee.UserName = Input.Email;
            employee.PhoneNumber = Input.PhoneNumber;

            var updateResult = await _userManager.UpdateAsync(employee);
            if (!updateResult.Succeeded)
            {
                foreach (var error in updateResult.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
                return Page();
            }

            if (!string.IsNullOrWhiteSpace(Password))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(employee);
                var passwordResult = await _userManager.ResetPasswordAsync(employee, token, Password);

                if (!passwordResult.Succeeded)
                {
                    foreach (var error in passwordResult.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
                    return Page();
                }
            }

            return RedirectToPage("Index");
        }
    }
}
