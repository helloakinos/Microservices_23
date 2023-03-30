using AutoMapper;
using TradeAnalysisService.Data;
using TradeAnalysisService.Dtos;
using Microsoft.AspNetCore.Mvc;
using TradeAnalysisService.Models;

namespace TradeAnalysisService.Controllers;
[Route("api/o/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly ITradeAnalysisRepo _repository;
    private readonly IMapper _mapper;

    public OrdersController(ITradeAnalysisRepo repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpGet]
    public ActionResult<IEnumerable<OrderReadDto>> GetOrders()
    {
        var allorders = _repository.GetAllOrders();
        return Ok(_mapper.Map<IEnumerable<OrderReadDto>>(allorders));
    }

    [HttpPost]
    public ActionResult InboundFromOrderService(OrderReceiveDto orderReceiveDto)
    {
        var mappedDto = _mapper.Map<Order>(orderReceiveDto);
        _repository.CreateOrder(mappedDto);
        _repository.SaveChanges();
        return Ok(mappedDto);
    }
}