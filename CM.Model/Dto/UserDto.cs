using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using CM.Model.Dto.Base;

namespace CM.Model.Dto;

public class UserDto: BaseDto
{
    [Required]
    [MaxLength(256)]
    public string? UserName { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string? FirstName { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string? LastName { get; set; }

    public string? FullName { get; set; }
      
    [Required]
    [MaxLength(256)] 
    public string? Email { get; set; }
    public bool EmailConfirmed { get; set; }
    
    [MaxLength(20)]
    public string? PhoneNumber { get; set; }
    public string? Token { get; set; }

    [Required]
    public bool IsEnabled { get; set; }

    public List<UserRoleDto>? Roles { get; set; }
}