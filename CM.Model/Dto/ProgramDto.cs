using System.ComponentModel.DataAnnotations;
using CM.Model.Dto.Base;

namespace CM.Model.Dto;

public class ProgramDto: BaseDto
{
    [Required]
    public long? ConferenceId { get; set; }
    
    [Required]
    public DateTime? DateTime { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string? Name { get; set; }
    
    [Required]
    [MaxLength(500)]
    public string? Description { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string? Speaker { get; set; }
    
    public string Date => DateTime == null ? "" : Convert.ToDateTime(DateTime).ToString("dd MMMM yyyy");
    public string Time => DateTime == null ? "" : Convert.ToDateTime(DateTime).ToString("hh:mm tt");

    public ConferenceDto? Conference { get; set; }
}