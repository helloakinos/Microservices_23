using System.ComponentModel.DataAnnotations;

namespace TradeAnalysisService.Models;
public class Order
{
    [Key]
    public int OrderId { get; set; }
    public string? Close { get; set; }
    public bool? IsPurchase {get; set; }
    public string? TransactionAmount {get; set;}
    public string? Date { get; set; }
    
    public ICollection<TradeAnalysis> TradeAnalyses { get; set; } = new List<TradeAnalysis>();
}