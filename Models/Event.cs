#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
//next line is used for [NotMapped]
using System.ComponentModel.DataAnnotations.Schema;
namespace activites.Models;
public class Event

{
    [Key]
    public int EventId { get; set; }

    [Required(ErrorMessage="Title is Required!")]
    [MinLength(2,ErrorMessage ="Title must be at least 2 characters")]
    public string Title { get; set; } 
 
    [Required]
    [FutureDate]
    [DataType(DataType.Date)]
    public DateTime Date { get; set; }
 
    [Required]
    [DataType(DataType.Time)]
    public DateTime Time { get; set; } 

    [Required]
    public int Duration { get; set; } 
 
    [Required]
    public string DurationSet { get; set; } 
 
    [Required(ErrorMessage="Description is Required!")]
    [MinLength(10,ErrorMessage ="Description must be at least 10 characters")]
    public string Description { get; set; } 
 
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    //use for many side on one to many
    public int UserId { get;set; }
    public User? Creator {get;set;}

    public List<Participant> EventParticipants { get;set;} = new List<Participant>();
}

public class FutureDateAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {

        if ((DateTime?)value < DateTime.Now)
        {

            return new ValidationResult("Event must be in the future");
        }
        else
        {

            return ValidationResult.Success;
        }
    }
}