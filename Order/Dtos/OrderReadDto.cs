namespace OrderService.Dtos;
public class OrderReadDto
{
    public int OrderId { get; set; }
    public int ApiId { get; set; }
    public string? Close { get; set; }
    public bool? IsPurchase {get; set; }
    public string? TransactionAmount {get; set;}
    public string? Date { get; set; }
}