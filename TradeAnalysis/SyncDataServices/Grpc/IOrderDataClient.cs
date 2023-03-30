using TradeAnalysisService.Models;

namespace TradeAnalysisService.SyncDataServices.Grpc;
public interface IOrderDataClient
{
    IEnumerable<Order> ReturnAllOrders();
}