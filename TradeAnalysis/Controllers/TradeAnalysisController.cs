using AutoMapper;
using TradeAnalysisService.Data;
using TradeAnalysisService.Dtos;
using TradeAnalysisService.Models;
using Microsoft.AspNetCore.Mvc;

namespace TradeAnalysisService.Controllers;
[Route("api/[controller]/{orderId}/")]
[ApiController]
public class TradeAnalysisController : ControllerBase
{
    private readonly ITradeAnalysisRepo _repository;
    private readonly IMapper _mapper;

    public TradeAnalysisController(ITradeAnalysisRepo repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpGet]
    public ActionResult<TradeAnalysisReadDto> GetAllTradeAnalysesForOrder(int orderId)
    {
        if (!_repository.OrderExists(orderId))
        {
            return NotFound();
        }
        else
        {
            var tradeAnalyses = _repository.GetTradeAnalysesBasedOnOrder(orderId);
            return Ok(_mapper.Map<TradeAnalysisReadDto>(tradeAnalyses));
        }
    }

    [HttpGet("{analysisId}", Name = "GetTradeAnalysisForOrder")]
    public ActionResult<TradeAnalysisReadDto> GetTradeAnalysisForOrder(int orderId, int analysisId)
    {
        if (!_repository.OrderExists(orderId))
        {
            return NotFound();
        }
        else
        {
            var tradeAnalysis = _repository.GetTradeAnalysis(orderId, analysisId);
            if(tradeAnalysis == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(_mapper.Map<TradeAnalysisReadDto>(tradeAnalysis));
            }
        }

    }
    [HttpPost]
    public ActionResult<TradeAnalysisReadDto> CreateTradeAnalysisForOrder(TradeAnalysisCreateDto tradeAnalysisDto)
    {
        if(tradeAnalysisDto == null)
        {
            return NotFound();
        }
        else
        {
            if (!_repository.OrderExists(tradeAnalysisDto.OrderId))
            {
                return NotFound();
            }
            else
            {
                _repository.CreateTradeAnalysis(tradeAnalysisDto.OrderId, tradeAnalysisDto);
                _repository.SaveChanges();
                var tradeAnalysisReadDto = _mapper.Map<TradeAnalysisReadDto>(tradeAnalysisDto);
                return CreatedAtRoute(nameof(GetTradeAnalysisForOrder),
                    new{OrderId = tradeAnalysisDto.OrderId, AnalysisId = tradeAnalysisReadDto.AnalysisId}, tradeAnalysisReadDto);
            }
        }
    }
}