using System.ComponentModel.DataAnnotations;
using CM.Model.Dto.Base;

namespace CM.Model.Dto;

public class RoleDto: BaseDto
{
    [Required]
    [MaxLength(10)]
    public string? Code { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string? Name { get; set; }
    
    [Required]
    [MaxLength(250)]
    public string? Description { get; set; }
}