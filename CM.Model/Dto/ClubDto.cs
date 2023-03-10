using System.ComponentModel.DataAnnotations;
using CM.Model.Dto.Base;

namespace CM.Model.Dto;

public class ClubDto: BaseDto
{
    [MaxLength(20)]
    public string? Code { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string? Name { get; set; }
    
    [Required] 
    public long ClubTypeId { get; set; }
    
    [Required] 
    public ClubTypeDto? ClubType { get; set; }
    
    [MaxLength(4)] 
    public string? District { get; set; }
    
    [Required] 
    public long CountryId { get; set; }

    [Required]
    public CountryDto? Country { get; set; }
}