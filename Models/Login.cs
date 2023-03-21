#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace activites.Models;

[NotMapped]
public class Login
{
    [Required(ErrorMessage="Email is Required!")]
    [EmailAddress]
    [Display(Name ="Email")]
    public string EmailLogin { get; set; } 

    [Required(ErrorMessage="Password is Required!")]
    [DataType(DataType.Password)]
    [Display(Name ="Password")]
    public string PasswordLogin { get; set; } 
}