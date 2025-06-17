using System.ComponentModel.DataAnnotations;

namespace ZadanieDodatkowe.DTOs;

public class CreateEventDto
{
    [Required]
    [MaxLength(100)]
    public string Title { get; set; }

    [Required]
    [MaxLength(500)]
    public string Description { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public int MaxAttendees { get; set; }
}