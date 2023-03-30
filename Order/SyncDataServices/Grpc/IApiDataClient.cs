using OrderService.Models;

namespace OrderService.SyncDataServices.Grpc;
public interface IApiDataClient
{
    IEnumerable<Order> GetAllApis();
}