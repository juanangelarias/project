using System.ComponentModel.DataAnnotations;
using CM.Model.Dto.Base;

namespace CM.Model.Dto;

public class ClubTypeDto: BaseDto
{
    [Required]
    [MaxLength(20)]
    public string? Name { get; set; }
}
