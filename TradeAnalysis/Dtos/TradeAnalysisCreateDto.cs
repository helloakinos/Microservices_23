using System.ComponentModel.DataAnnotations;

namespace TradeAnalysisService.Dtos;
public class TradeAnalysisCreateDto
{
    [Required]
    public int OrderId { get; set; }
    [Required]
    public string Comments { get; set; }
    [Required]
    public string TradeDecision { get; set; }
    public string? InternalComment { get; set; }
}