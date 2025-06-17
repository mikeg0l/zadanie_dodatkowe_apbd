using System.ComponentModel.DataAnnotations;

namespace ZadanieDodatkowe.DTOs;

public class AssignSpeakerDto
{
    [Required]
    public int EventId { get; set; }
    [Required]
    public int SpeakerId { get; set; }
}