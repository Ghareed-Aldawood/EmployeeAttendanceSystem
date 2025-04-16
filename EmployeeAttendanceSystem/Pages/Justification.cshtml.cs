using EmployeeAttendanceSystem.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace EmployeeAttendanceSystem.Pages;

[Authorize(Roles = "Employee")]
public class JustificationModel : PageModel
{
    private readonly AppDbContext _context;

    public JustificationModel(AppDbContext context)
    {
        _context = context;
    }

    [BindProperty(SupportsGet = true)]
    public int Id { get; set; } // AttendanceId passed from URL

    [BindProperty]
    [Required(ErrorMessage = "Justification is required.")]
    [StringLength(500, ErrorMessage = "Justification cannot exceed 500 characters.")]
    public string Justification { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync()
    {
        var employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var attendance = await _context.Attendances.FirstOrDefaultAsync(a => a.Id == Id && a.EmployeeId == employeeId);

        if (attendance == null)
        {
            return NotFound();
        }

        Justification = attendance.Justification ?? string.Empty;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        var employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var attendance = await _context.Attendances.FirstOrDefaultAsync(a => a.Id == Id && a.EmployeeId == employeeId);

        if (attendance == null)
        {
            return NotFound();
        }

        attendance.Justification = Justification;
        _context.Attendances.Update(attendance);
        await _context.SaveChangesAsync();

        return RedirectToPage("ViewAttendance");
    }
}