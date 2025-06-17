using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZadanieDodatkowe.Models;

[Table("Event")]
public class Event
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    public string Title { get; set; }
    
    public string Description { get; set; }
    
    public DateTime StartDate { get; set; }
    
    public int MaxAttendees { get; set; }

    public virtual ICollection<Attendee> Attendees { get; set; }
    public virtual ICollection<Speaker> Speakers { get; set; }
}