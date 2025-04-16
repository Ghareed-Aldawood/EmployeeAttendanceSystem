using EmployeeAttendanceSystem.Data;
using EmployeeAttendanceSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EmployeeAttendanceSystem.Pages;

[Authorize(Roles = "Employee")]
public class CheckOutModel : PageModel
{
    private readonly AppDbContext _context;

    public CheckOutModel(AppDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Attendance? Input { get; set; }
    [BindProperty]
    public string Justification { get; set; }

    public string? Message { get; set; }
    public DateTime? CheckOutTime { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var today = DateTime.Today;

        // Check if the employee has already checked out today
        var existingAttendance = await _context.Attendances
            .FirstOrDefaultAsync(a => a.EmployeeId == employeeId && a.Date.Date == today && a.CheckOut != null);

        if (existingAttendance != null)
        {
            // If already checked out, show the time of check-out
            CheckOutTime = existingAttendance.CheckOut;
            Message = "You have already checked out for today.";
            return Page();
        }

        // No checkout done yet
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var today = DateTime.Today;

        var existingAttendance = await _context.Attendances
            .FirstOrDefaultAsync(a => a.EmployeeId == employeeId && a.Date.Date == today && a.CheckOut == null);

        if (existingAttendance == null)
        {
            Message = "You haven't checked in today or you have already checked out.";
            return Page();
        }

        // Check-out logic
        var checkOutTime = DateTime.Now;
        existingAttendance.CheckOut = checkOutTime;

        // Flag early check-out if before 3:00 PM
        if (checkOutTime.Hour < 15)
        {
            existingAttendance.IsEarlyCheckOut = true;
            existingAttendance.Justification = Justification;
        }

        await _context.SaveChangesAsync();

        Message = "You have successfully checked out.";
        return RedirectToPage("ViewAttendance");
    }
}

