using System.ComponentModel.DataAnnotations;

namespace CM.App.Helper.Model;

public class PasswordModel
{
    public string FullName { get; set; }
    
    public string Username { get; set; }
    
    public string Password { get; set; }
    
    [Required(ErrorMessage = "Password is required.")]
    [DataType(DataType.Password)]
    [StringLength(200)]
    public string NewPassword { get; set; }
    
    [Required(ErrorMessage = "Password is required.")]
    [DataType(DataType.Password)]
    [StringLength(200)]public string Confirm { get; set; }
}