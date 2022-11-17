using System.ComponentModel.DataAnnotations;
using CM.Model.Dto.Base;

namespace CM.Model.Dto;

public class CountryDto : BaseDto
{
    [Required] [MaxLength(3)] public string? Code { get; set; }

    [Required] [MaxLength(50)] public string? Name { get; set; }
}