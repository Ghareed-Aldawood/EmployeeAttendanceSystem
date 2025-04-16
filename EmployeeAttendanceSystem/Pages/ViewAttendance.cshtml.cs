using EmployeeAttendanceSystem.Data;
using EmployeeAttendanceSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EmployeeAttendanceSystem.Pages;

[Authorize(Roles = "Employee")]
public class ViewAttendanceModel(AppDbContext context) : PageModel
{
    private readonly AppDbContext _context = context;

    public IList<Attendance> Attendances { get; set; }

    public async Task OnGetAsync()
    {
        var employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        Attendances = await _context.Attendances
            .Where(a => a.EmployeeId == employeeId)
            .ToListAsync();
    }
}
