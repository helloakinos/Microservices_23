using StockInfo.Dtos;
using StockInfo.Models;
using AutoMapper;

namespace StockInfo.Profiles;
public class StockInfo : Profile
{
    public StockInfo()
    {
        //source to target
        CreateMap<Stock, ApiPublishDto>();
        CreateMap<Stock, ApiRabbitMQPublishDto>();
    }
}