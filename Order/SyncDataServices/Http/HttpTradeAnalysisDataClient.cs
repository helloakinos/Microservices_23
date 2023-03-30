using System.Text;
using System.Text.Json;
using OrderService.Dtos;

namespace OrderService.SyncDataServices.Http;
public class HttpTradeAnalysisDataClient : ITradeAnalysisDataClient
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public HttpTradeAnalysisDataClient(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }
    public async Task SendOrderToTradeAnalysis(OrderReadDto order)
    {
        var httpContent = new StringContent(
            JsonSerializer.Serialize(order),
            Encoding.UTF8,
            "application/json"
        );

        var response = await _httpClient.PostAsync($"{_configuration["TradeAnalysisService"]}", httpContent);

        if(response.IsSuccessStatusCode)
        {
            System.Console.WriteLine("Sync post to Trade Analysis service successful.");
        }
        else
        {
            System.Console.WriteLine("sync post to Trade Analysis service failed.");
        }
    }
}
