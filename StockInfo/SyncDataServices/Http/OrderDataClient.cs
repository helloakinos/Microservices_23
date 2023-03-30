using System.Text;
using System.Text.Json;
using StockInfo.Models;
using AutoMapper;

namespace StockInfo.SyncDataServices.Http;
public class OrderDataClient : IOrderDataClient
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;

    public OrderDataClient(HttpClient httpClient, IConfiguration configuration, IMapper mapper)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _mapper = mapper;
    }
    public async Task SendStockDataToOrder(Stock apiData)
    {
        var mappedContent = _mapper.Map<ApiPublishDto>(apiData);
        var httpContent = new StringContent(
            JsonSerializer.Serialize(mappedContent),
            Encoding.UTF8,
            "application/json"
        );
        
        var httpResponse = await _httpClient.PostAsync($"{_configuration["OrderService"]}", httpContent);
        
    }
}