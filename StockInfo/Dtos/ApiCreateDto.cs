using System.ComponentModel.DataAnnotations;

namespace StockInfo.Dtos;
public class ApiCreateDto
{
    [Required]
    public string Date { get; set; }
    [Required]
    public bool isPurchase { get; set; }
    [Required]
    public string TransactionAmount { get; set; }
}