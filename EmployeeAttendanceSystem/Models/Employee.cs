using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace EmployeeAttendanceSystem.Models;

public class Employee : IdentityUser
{
    [Required]
    [Display(Name = "Full Name")]
    public string FullName { get; set; } = null!;

    public ICollection<Attendance>? Attendances { get; set; }
}
