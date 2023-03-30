using AutoMapper;
using TradeAnalysisService.Models;
using Grpc.Net.Client;
using OrderService;

namespace TradeAnalysisService.SyncDataServices.Grpc;
public class OrderDataClient : IOrderDataClient
{
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;

    public OrderDataClient(IConfiguration configuration, IMapper mapper)
    {
        _configuration = configuration;
        _mapper = mapper;
    }
    public IEnumerable<Order> ReturnAllOrders()
    {
        var channel = GrpcChannel.ForAddress(_configuration["GrpcOrder"]);
        var client = new GrpcOrder.GrpcOrderClient(channel);
        var request = new GetAllRequest();

        try
        {
            var reply = client.GetAllOrders(request);
            return _mapper.Map<IEnumerable<Order>>(reply.Order);
        }
        catch(Exception ex)
        {
            System.Console.WriteLine($"-->Error. Could not return orders via gRPC: {ex}");
            return null;
        }
    }
}
