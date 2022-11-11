using System.ComponentModel.DataAnnotations;

namespace CM.App.Helper.Model;

public class ChangePasswordModel: PasswordModel
{
    [Required(ErrorMessage = "Password is required.")]
    [DataType(DataType.Password)]
    [StringLength(200)]
    public string OldPassword { get; set; }
}