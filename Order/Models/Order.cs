using System.ComponentModel.DataAnnotations;

namespace OrderService.Models;
public class Order
{
    [Key]
    public int OrderId { get; set; }
    public int ApiId { get; set; }
    public string? Date { get; set; }
    public string? High { get; set; }
    public string? Low { get; set; }
    public string? Open { get; set; }
    public string? Close { get; set; }
    public bool? IsPurchase {get; set; }
    public string? TransactionAmount {get; set;}
}
