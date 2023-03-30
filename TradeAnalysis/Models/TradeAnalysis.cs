using System.ComponentModel.DataAnnotations;

namespace TradeAnalysisService.Models;
public class TradeAnalysis
{
    [Key]
    public int AnalysisId { get; set; }
    [Required]
    public int OrderId { get; set; }
    [Required]
    public string? Comments { get; set; }
    public string? InternalComment { get; set; }
    [Required]
    public string? TradeDecision { get; set; }
    
    public Order? Order { get; set; }
}