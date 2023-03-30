using TradeAnalysisService.Models;
using TradeAnalysisService.SyncDataServices.Grpc;

namespace TradeAnalysisService.Data;
public static class PrepDb
{
    public static void PrepPopulation(IApplicationBuilder applicationBuilder)
    {
        using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
        {
            var grpcClient = serviceScope.ServiceProvider.GetService<IOrderDataClient>();
            var orders = grpcClient.ReturnAllOrders();

            SeedData(serviceScope.ServiceProvider.GetService<ITradeAnalysisRepo>(), orders);
        }
    }
    private static void SeedData(ITradeAnalysisRepo repo, IEnumerable<Order> orders)
    {
        foreach(var order in orders)
        {
            if(!repo.OrderExists(order.OrderId))
            {
                repo.CreateOrder(order);
            }
            repo.SaveChanges();
        }
    }
}