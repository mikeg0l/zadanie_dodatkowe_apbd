using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZadanieDodatkowe.Models;

[Table("Atendee")]
public class Attendee
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [MaxLength(32)]
    public string FirstName { get; set; }
    
    [MaxLength(32)]
    public string LastName { get; set; }
    
    [MaxLength(48)]
    public string Email { get; set; }
    
    public virtual ICollection<Event> Events { get; set; }
}