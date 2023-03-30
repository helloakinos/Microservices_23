namespace OrderService.Dtos;
public class OrderPublishDto
{
    public int OrderId { get; set; }
    public string Close { get; set; }
    public bool IsPurchase {get; set; }
    public string TransactionAmount {get; set;}
    public string Date { get; set; }
    public string  Event { get; set; }
}