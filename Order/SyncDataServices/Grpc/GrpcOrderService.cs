using AutoMapper;
using Grpc.Core;
using OrderService.Data;

namespace OrderService.SyncDataServices.Grpc;
public class GrpcOrderService : GrpcOrder.GrpcOrderBase
{
    private readonly IOrderRepo _repository;
    private readonly IMapper _mapper;

    public GrpcOrderService(IOrderRepo repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public override Task<OrderResponse> GetAllOrders(GetAllRequest request, ServerCallContext context)
    {
        var response = new OrderResponse();
        var orders = _repository.GetAllOrders();

        foreach(var order in orders)
        {
            response.Order.Add(_mapper.Map<GrpcOrderModel>(order));
        }
        return Task.FromResult(response);
    }
}