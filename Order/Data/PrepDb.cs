using OrderService.Models;
using OrderService.SyncDataServices.Grpc;

namespace OrderService.Data;

public static class PrepDb
{
    public static void PrepPopulation(IApplicationBuilder app)
    {
        using(var serviceScope = app.ApplicationServices.CreateScope())
        {
            var grpcClient = serviceScope.ServiceProvider.GetService<IApiDataClient>();
            var StocksViaGrpc = grpcClient.GetAllApis();
            
            SeedData(serviceScope.ServiceProvider.GetService<IOrderRepo>(), StocksViaGrpc);
            // SeedDataManually(serviceScope.ServiceProvider.GetService<AppDbContext>());
        }
    }   

            private static void SeedData(IOrderRepo repo, IEnumerable<Order> potentialOrders)
            {
                foreach (var potentialOrder in potentialOrders)
                {
                    if(!repo.ApiExists(potentialOrder.ApiId))
                    {
                        repo.CreateOrder(potentialOrder);
                    }

                    repo.SaveChanges();
                }
            }
            private static void SeedDataManually(IOrderRepo repo)
            {
                if (repo.GetAllOrders == null)
                {
                    repo.CreateOrder
                    (
                        new Order(){Date = "2023-01-01", High = "135.79", Low = "130.2", Open = "131.44", Close = "135.79", IsPurchase = true, TransactionAmount = "350"}
                    );

                    repo.SaveChanges();
                }
                else
                {
                    Console.WriteLine("No manual seeding necessary. Data available.");
                }
            }
}