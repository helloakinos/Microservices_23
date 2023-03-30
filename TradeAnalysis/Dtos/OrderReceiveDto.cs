namespace TradeAnalysisService.Dtos;
public class OrderReceiveDto
{
    public int Id { get; set; }
    public string? Close { get; set; }
    public bool IsPurchase {get; set; }
    public string? TransactionAmount {get; set;}
    public string? Date { get; set; }
    public string? Event { get; set; }
}