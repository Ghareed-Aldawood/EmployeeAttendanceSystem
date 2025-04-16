using EmployeeAttendanceSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeAttendanceSystem.Pages.Admin.Employees
{
    public class IndexModel(UserManager<Employee> userManager) : PageModel
    {
        private readonly UserManager<Employee> _userManager = userManager;

        public IList<Employee> Employees { get; set; }

        public void OnGet()
        {
            var currentUserId = _userManager.GetUserId(User);
            Employees = _userManager.Users
                .Where(u => u.Id != currentUserId)
                .ToList();
        }

        public async Task<IActionResult> OnPostDeleteAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var employee = await _userManager.FindByIdAsync(id);
            if (employee == null)
                return NotFound();

            var result = await _userManager.DeleteAsync(employee);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return RedirectToPage();
        }
    }
}
