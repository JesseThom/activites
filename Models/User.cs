#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace activites.Models;
public class User

{
    [Key]
    public int UserId { get; set; }

    [LettersOnly]
    [Required(ErrorMessage="First Name is Required!")]
    [MinLength(2,ErrorMessage="First Name must be at least 2 characters")]
    public string FirstName { get; set; } 

    [LettersOnly]
    [Required(ErrorMessage="Last Name is Required!")]
    [MinLength(2,ErrorMessage="Last Name must be at least 2 characters")]
    public string LastName { get; set; } 

    [Required(ErrorMessage="Email is Required!")]
    [EmailAddress]
    public string Email { get; set; } 

    [Required(ErrorMessage="Password is Required!")]
    [DataType(DataType.Password)]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
    [SpecialChar]
    public string Password { get; set; } 

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    [NotMapped]
    [Compare("Password")]
    public string Confirm { get; set; }

    //use for one to many
    public List<Event> UserEvents {get; set;} = new List<Event>();
    //use for many to many
    public List<Participant> GuestList {get;set;} = new List<Participant>();
}

public class UniqueEmailAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if(value == null)
        {
            return new ValidationResult("Email is required!");
        }
        
        MyContext _context = (MyContext)validationContext.GetService(typeof(MyContext));
        if(_context == null)
            {
                return new ValidationResult("something!");
            }
            if(_context.Users.Any(e => e.Email == value.ToString()))
            {
                return new ValidationResult("Email must be unique!");
            } else {
                return ValidationResult.Success;
        }
        
    }
}

public class LettersOnlyAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if(value == null)
        {
            return new ValidationResult("Name is Required");
        }
        if ( Regex.IsMatch((String)value, @"[0-9]") )
        {

            return new ValidationResult("Must not contain numbers");
        }
        else
        {

            return ValidationResult.Success;
        }
    }
}
public class SpecialCharAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if(value == null)
        {
            return new ValidationResult("Password is Required");
        }
        var hasNumber = new Regex(@"[0-9]+");
        var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");
        if ( !hasNumber.IsMatch((String)value) ||!hasSymbols.IsMatch((String)value))
        {
            return new ValidationResult("Password must contain 1 number and special character");
        }
        // else if (!hasSymbols.IsMatch((String)value))
        // {
        //     return new ValidationResult("Password must contain 1 special character");
        // }
        else
        {

            return ValidationResult.Success;
        }
    }
}

