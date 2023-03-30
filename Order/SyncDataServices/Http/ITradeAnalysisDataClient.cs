using OrderService.Dtos;

namespace OrderService.SyncDataServices.Http;
public interface ITradeAnalysisDataClient
{
    Task SendOrderToTradeAnalysis(OrderReadDto order);
}