using System.ComponentModel.DataAnnotations;

namespace OrderService.Dtos;
public class OrderCreateDto
{
    [Required]
    public int OrderId { get; set; }
    [Required]
    public int ApiId { get; set; }
    [Required]
    public string Close { get; set; }
    [Required]
    public bool IsPurchase {get; set; }
    [Required]
    public string TransactionAmount {get; set;}
    [Required]
    public string Date { get; set; }
    [Required]
    public string Open { get; set; }
    [Required]
    public string Low { get; set; }
    [Required]
    public string High { get; set; }
}