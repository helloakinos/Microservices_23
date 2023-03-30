using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OrderService.AsyncDataServices;
using OrderService.Data;
using OrderService.Dtos;
using OrderService.SyncDataServices.Http;

namespace OrderService.Controllers;

[Route("api/o/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly IOrderRepo _repository;
    private readonly IMapper _mapper;
    private readonly ITradeAnalysisDataClient _tradeAnalysisDataClient;
    private readonly IMessageBusClient _messageBusClient;

    
    public OrdersController(IOrderRepo repository, IMapper mapper, ITradeAnalysisDataClient tradeAnalysisDataClient, IMessageBusClient messageBusClient)
    {
        _repository = repository;
        _mapper = mapper;
        _tradeAnalysisDataClient = tradeAnalysisDataClient;
        _messageBusClient = messageBusClient;
    }
    
    [HttpGet]
    public ActionResult<IEnumerable<OrderReadDto>> GetOrders()
    {
        var result = _repository.GetAllOrders();
        return Ok(_mapper.Map<IEnumerable<OrderReadDto>>(result));
    }

    
    [HttpGet("{id}", Name = "GetOrderById")]
    public ActionResult<OrderReadDto> GetOrderById(int id)
    {
        var result = _repository.GetOrderById(id);
        if (result == null)
        {
            return NotFound();
        }
        else
        {
            return Ok(_mapper.Map<OrderReadDto>(result));
        }
    }

    [HttpPost]
    [Route("{id}")]
    public async Task<ActionResult<OrderReadDto>> CreateOrder (int id)
    {
        var orderToBeSent = _repository.GetOrderById(id);
        if(orderToBeSent != null)
        {
            var result = _mapper.Map<OrderReadDto>(orderToBeSent);
            
            //synchronous send
            try
            {
                await _tradeAnalysisDataClient.SendOrderToTradeAnalysis(result);
            }
            catch(Exception ex)
            {
                System.Console.WriteLine($"--> Error, could not send synchronously via http: {ex}");
            }

            //asynchronous send
            try
            {
                var orderPublishDto = _mapper.Map<OrderPublishDto>(result);
                orderPublishDto.Event = "Order Published";
                _messageBusClient.PublishNewOrder(orderPublishDto);
            }
            catch(Exception ex)
            {
                System.Console.WriteLine($"--> Error, could not send asynchronously via RabbitMQ: {ex}");
            }

            return CreatedAtRoute(nameof(GetOrderById), new {Id = result.OrderId}, result);
        }
        else
        {
            return NotFound();
        }
    }
}