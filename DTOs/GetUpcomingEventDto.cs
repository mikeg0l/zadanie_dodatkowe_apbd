using System.ComponentModel.DataAnnotations;

namespace ZadanieDodatkowe.DTOs;

public class GetUpcomingEventDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public int RegisteredAttendees { get; set; }
    public int FreeSpots { get; set; }
    public List<GetUpcomingEventSpeakerDto> Speakers { get; set; }
}

public class GetUpcomingEventSpeakerDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}