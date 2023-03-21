#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
//next line is used for [NotMapped]
using System.ComponentModel.DataAnnotations.Schema;
namespace activites.Models;
public class Participant

{
    [Key]
    public int ParticipantId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public int UserId { get; set;}
    public User? User { get; set;}

    public int EventId { get; set;}
    public Event? Event { get; set;}
}