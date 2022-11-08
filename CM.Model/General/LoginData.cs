using System.ComponentModel.DataAnnotations;

namespace CM.Model.General;

public class LoginData
{
    [Required(ErrorMessage = "Email address is required.")]
    [StringLength(200)]
    [DataType(DataType.EmailAddress)]
    [EmailAddress(ErrorMessage = "Email address is not valid.")]
    public string? UserName { get; set; }
    
    [Required(ErrorMessage = "Password is required.")]
    [DataType(DataType.Password)]
    [StringLength(200)]
    public string? Password { get; set; }
}