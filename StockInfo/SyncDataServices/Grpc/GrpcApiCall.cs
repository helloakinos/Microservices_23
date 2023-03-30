using StockInfo.Data;
using AutoMapper;
using Grpc.Core;

namespace StockInfo.SyncDataServices.Grpc;
public class GrpcApiService : GrpcApi.GrpcApiBase
{
    private readonly IStockInfoRepository _repository;
    private readonly IMapper _mapper;

    public GrpcApiService(IStockInfoRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public override Task<ApiResponse> GetAllStocks(GetAllApis request, ServerCallContext context)
    {
        var response = new ApiResponse();
        var orders = _repository.GetAllStocks();

        foreach(var order in orders)
        {
            response.Stockinfo.Add(_mapper.Map<ApiPublishDto>(order));
        }
        return Task.FromResult(response);
    }
}