using Apicall;
using AutoMapper;
using Grpc.Net.Client;
using OrderService.Models;

namespace OrderService.SyncDataServices.Grpc;
public class ApiDataClient : IApiDataClient
{
    private readonly IMapper _mapper;
    private readonly IConfiguration _config;

    public ApiDataClient(IConfiguration config, IMapper mapper)
    {
        _mapper = mapper;
        _config = config;
    }
    public IEnumerable<Order> GetAllApis()
    {
        var channel = GrpcChannel.ForAddress(_config["GrpcApi"]);
        var client = new GrpcApi.GrpcApiClient(channel);
        var request = new GetAllApis();

        try
        {
            var reply = client.GetAllStocks(request);
            return _mapper.Map<IEnumerable<Order>>(reply.ApiCall);
        }
        catch(Exception ex)
        {
            System.Console.WriteLine($"--> Error, failed to get stock information via gRPC: {ex}");
            return null;
        }
    }
}