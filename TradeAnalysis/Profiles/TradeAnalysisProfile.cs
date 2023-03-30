using AutoMapper;
using TradeAnalysisService.Dtos;
using TradeAnalysisService.Models;
using OrderService;

namespace TradeAnalysisService.Profiles;
public class TradeAnalysesProfile : Profile
{
    public TradeAnalysesProfile()
    {
        //source -> target
        CreateMap<Order, OrderReadDto>();
        CreateMap<TradeAnalysisCreateDto, TradeAnalysis>();
        CreateMap<TradeAnalysisCreateDto, TradeAnalysisReadDto>();
        CreateMap<TradeAnalysis, TradeAnalysisReadDto>();
        CreateMap<OrderReceiveDto, Order>();
        CreateMap<GrpcOrderModel, Order>();
        CreateMap<OrderPublishDto, Order>();
    }
}