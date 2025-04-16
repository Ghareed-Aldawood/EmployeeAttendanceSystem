using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EmployeeAttendanceSystem.Models;

public class Attendance
{
    public int Id { get; set; }

    [Required]
    public string EmployeeId { get; set; } = null!;
    [ForeignKey("EmployeeId")]
    public Employee? Employee { get; set; }

    [DataType(DataType.Date)]
    public DateTime Date { get; set; } = DateTime.UtcNow.Date;

    [DataType(DataType.Time)]
    public DateTime? CheckIn { get; set; }

    [DataType(DataType.Time)]
    public DateTime? CheckOut { get; set; }

    public bool IsLateCheckIn { get; set; }
    public bool IsEarlyCheckOut { get; set; }

    public string? Justification { get; set; }
}
