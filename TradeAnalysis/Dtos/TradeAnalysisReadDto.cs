namespace TradeAnalysisService.Dtos;
public class TradeAnalysisReadDto
{
    public int AnalysisId { get; set; }
    public int OrderId { get; set; }
    public string Comments { get; set; }
    public string TradeDecision { get; set; }
}