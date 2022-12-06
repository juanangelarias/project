using System.ComponentModel.DataAnnotations;
using CM.Model.Dto.Base;

namespace CM.Model.Dto;

public class PaymentMethodDto: BaseDto
{
    [Required]
    [MaxLength(25)]
    public string? Name { get; set; }
    
    [Required]
    public bool? RequiredBankName { get; set; }
    
    [Required]
    public bool? RequireDocumentNumber { get; set; }
}