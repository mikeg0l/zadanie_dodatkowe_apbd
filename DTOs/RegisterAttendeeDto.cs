using System.ComponentModel.DataAnnotations;

namespace ZadanieDodatkowe.DTOs;

public class RegisterAttendeeDto
{
    [Required]
    public int EventId { get; set; }
    [Required]
    public int AttendeeId { get; set; }
}