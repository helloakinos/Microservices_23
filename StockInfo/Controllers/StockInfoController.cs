using StockInfo.AsyncDataServices;
using StockInfo.Data;
using StockInfo.Dtos;
using StockInfo.SyncDataServices.Http;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace StockInfo.Controllers;

[ApiController]
[Route("api/[controller]")]

public class StockInfoController : ControllerBase
{
    private readonly IOrderDataClient _httpDataClient;
    private readonly IStockInfoRepository _repository;
    private readonly IMapper _mapper;
    private readonly IMessageBus _messageBus;

    public StockInfoController(
        IStockInfoRepository repository,
        IOrderDataClient httpDataClient,
        IMapper mapper,
        IMessageBus messageBus
        )
    {
        _httpDataClient = httpDataClient;
        _repository = repository;
        _mapper = mapper;
        _messageBus = messageBus;
    }


    [HttpGet(Name = "GetStockById")]
    [Route("si/si/getstockdata/{id}")]
    public ActionResult GetStockById(int id)
    {
        var result = _repository.GetStockById(id);
        return Ok(result);
    }


    [HttpPost]
    [Route("si/si/getstockdata")]
    public async Task<ActionResult> GetAllStockData(ApiCreateDto apiCreateDto)
        {            
            bool isDateOnly = DateOnly.TryParse(apiCreateDto.Date, out DateOnly todayAsDateOnly);
            string date = todayAsDateOnly.ToString("yyyy-MM-dd");

            var result = await _repository.FetchDataFromApi(date, apiCreateDto.isPurchase, apiCreateDto.TransactionAmount);
            
            return Ok(result);
        }
        

    [HttpPost]
    [Route("si/si/sendstockdata/{id}")]
    public async Task<ActionResult> PublishApiData(int id)
    {
        var publishApi = _repository.GetStockById(id);
        //synchronous http 
        try
        {
            await _httpDataClient.SendStockDataToOrder(publishApi);    
        }
        catch (Exception ex)
        {
            System.Console.WriteLine($"--> Failed to send data to Order service via http: {ex}");
        }

        //asynchronous messagebus
        try
        {
            var apiPublishDto = _mapper.Map<ApiRabbitMQPublishDto>(publishApi);
            apiPublishDto.Event = "Api Published";
            _messageBus.PublishNewStock(apiPublishDto);
        }
        catch(Exception ex)
        {
            System.Console.WriteLine($"--> Failed to send data to Order service via MessageBus: {ex}");
        }
        
        return CreatedAtRoute(nameof(GetStockById), new {Id = publishApi.ApiId}, publishApi);

    }
}
