using System.ComponentModel.DataAnnotations;

namespace MillaWeb.Models;

public class RegisterVM
{
    [Required, EmailAddress]
    public string Email { get; set; } = "";

    [Required, MinLength(4)]
    public string Password { get; set; } = "";

    [Required]
    public string FirstName { get; set; } = "";

    [Required]
    public string LastName { get; set; } = "";
}
