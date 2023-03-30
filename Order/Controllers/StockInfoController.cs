using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OrderService.Data;
using OrderService.Dtos;
using OrderService.Models;

namespace OrderService.Controllers;
[Route("api/a/[controller]")]
[ApiController]
public class StockInfoController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public StockInfoController(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpPost]
    public ActionResult InboundFromApiCall(OrderCreateDto orderCreateDto)
    {
        var mappedDto = _mapper.Map<Order>(orderCreateDto);
        _context.Orders.Add(mappedDto);
        _context.SaveChanges();
        return Ok(mappedDto);
    }
}