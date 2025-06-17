namespace ZadanieDodatkowe.DTOs;

public class GetAttendeeReportDto
{
    public string Title { get; set; } 
    public string Description { get; set; } 
    public DateTime StartDate { get; set; } 
    public List<string> SpeakerLastNames { get; set; }
}
