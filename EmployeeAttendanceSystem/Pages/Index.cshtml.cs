using EmployeeAttendanceSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeAttendanceSystem.Pages;

public class IndexModel(SignInManager<Employee> signInManager) : PageModel
{
    public void OnGet()
    {

    }

    public async Task<IActionResult> OnPostLogout()
    {
        await signInManager.SignOutAsync();
        return RedirectToPage("/Index");
    }
}
