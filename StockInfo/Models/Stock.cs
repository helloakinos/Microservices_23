using System.ComponentModel.DataAnnotations;

namespace StockInfo.Models;
public class Stock
{
    [Key]
    public int ApiId { get; set; }
    public string? AdjustedClose { get; set; }
    public string? Date { get; set; }
    public string? SplitCoefficient { get; set; }
    public string? Open { get; set; }
    public string? High { get; set; }
    public string? Low { get; set; }
    public string? Close { get; set; }
    public string? DividendAmount { get; set; }
    public string? Volume { get; set; }
    public bool? IsPurchase {get; set; }
    public string? TransactionAmount {get; set;}
}