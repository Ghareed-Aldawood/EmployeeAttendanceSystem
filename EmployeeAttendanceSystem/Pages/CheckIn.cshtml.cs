using EmployeeAttendanceSystem.Data;
using EmployeeAttendanceSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EmployeeAttendanceSystem.Pages;

[Authorize(Roles = "Employee")]
public class CheckInModel : PageModel
{
    private readonly AppDbContext _context;

    public CheckInModel(AppDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Attendance? Input { get; set; }
    [BindProperty]
    public string? Justification { get; set; }

    public string Message { get; set; }

    public async Task OnGetAsync()
    {
        var employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var today = DateTime.Today;

        var existingAttendance = await _context.Attendances
            .FirstOrDefaultAsync(a => a.EmployeeId == employeeId && a.Date.Date == today);

        if (existingAttendance != null)
        {
            Input = existingAttendance;
            if (existingAttendance.CheckIn != null)
            {
                Message = "You have already checked in today.";
            }
        }
        else
        {
            Input = new Attendance
            {
                CheckIn = DateTime.Now
            };
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var today = DateTime.Today;

        var checkInTime = DateTime.Now;

        var minCheckInTime = today.AddHours(7);
        var maxAllowedCheckIn = today.AddHours(8);
        var lateCheckInThreshold = today.AddHours(8).AddMinutes(30);

        if (checkInTime < minCheckInTime)
        {
            Message = "You cannot check in before 7:00 AM.";
            ModelState.AddModelError(string.Empty, Message);
            Input = new Attendance { CheckIn = checkInTime };
            return Page();
        }

        var existingAttendance = await _context.Attendances
            .FirstOrDefaultAsync(a => a.EmployeeId == employeeId && a.Date.Date == today);

        if (existingAttendance != null && existingAttendance.CheckIn != null)
        {
            Message = "You have already checked in today.";
            return RedirectToPage("ViewAttendance");
        }

        var attendance = existingAttendance ?? new Attendance
        {
            EmployeeId = employeeId,
            Date = today
        };

        attendance.CheckIn = checkInTime;

        // Flag late check-in if after 8:30 AM
        if (checkInTime > lateCheckInThreshold)
        {
            attendance.IsLateCheckIn = true;

            if (string.IsNullOrWhiteSpace(Justification))
            {
                ModelState.AddModelError("Justification", "Justification is required for late check-in after 8:30 AM.");
                Input = attendance;
                return Page();
            }

            attendance.Justification = Justification;
        }

        if (existingAttendance == null)
            _context.Attendances.Add(attendance);

        await _context.SaveChangesAsync();

        Message = "You have successfully checked in.";
        return RedirectToPage("ViewAttendance");
    }
}
