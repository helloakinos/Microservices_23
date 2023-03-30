using Apicall;
using AutoMapper;
using OrderService.Dtos;
using OrderService.Models;

namespace OrderService.Profiles;
public class OrdersProfile : Profile
{
    public OrdersProfile()
    {
        CreateMap<Order, OrderReadDto>(); 
        CreateMap<OrderCreateDto, Order>(); 
        CreateMap<OrderReadDto, OrderPublishDto>();
        CreateMap<StockRabbitMQPublishDto, Order>();
        CreateMap<ApiPublishDto, Order>();
        CreateMap<Order, GrpcOrderModel>();
    }
}