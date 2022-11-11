using System.ComponentModel.DataAnnotations;

namespace CM.App.Helper.Model;

public class PasswordModel
{
    public string FullName { get; set; }
    
    public string Username { get; set; }
    
    [Required(ErrorMessage = "Password is required.")]
    [DataType(DataType.Password)]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", 
        ErrorMessage = "Password should have at least 8 character, one lowercase letter, one uppercase letter, one number and one special character")]
    [StringLength(200)]
    public string NewPassword { get; set; }
    
    [Required(ErrorMessage = "Password is required.")]
    [DataType(DataType.Password)]
    [Compare(nameof(NewPassword), ErrorMessage = "Passwords do not match")]
    [StringLength(200)]public string Confirm { get; set; }
}