using AutoMapper;
using TradeAnalysisService.Dtos;
using TradeAnalysisService.Models;

namespace TradeAnalysisService.Data;
public class TradeAnalysisRepo : ITradeAnalysisRepo
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public TradeAnalysisRepo(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public void CreateTradeAnalysis(int orderID, TradeAnalysisCreateDto analysis)
    {
        if (analysis == null)
        {
            throw new ArgumentNullException(nameof(analysis));
        }
        else
        {
            analysis.OrderId = orderID;
            _context.TradeAnalyses.Add(_mapper.Map<TradeAnalysis>(analysis));
        }
    }

    public void CreateOrder(Order order)
    {
        if (order == null)
        {
            throw new ArgumentNullException(nameof(order));
        }
        else
        {
            _context.Orders.Add(order);
        }
    }


    public IEnumerable<Order> GetAllOrders()
    {
        return _context.Orders.ToList();
    }

    public TradeAnalysis GetTradeAnalysis(int orderID, int analysisId)
    {
        return _context.TradeAnalyses.Where(c=>c.OrderId == orderID && c.AnalysisId == analysisId).FirstOrDefault();
    }

    public IEnumerable<TradeAnalysis> GetTradeAnalysesBasedOnOrder(int orderId)
    {
        return _context.TradeAnalyses
        .Where
        (c=>c.OrderId == orderId)
        .OrderBy(c=>c.Order.OrderId);
    }

    public bool OrderExists(int orderId)
    {
        return _context.Orders.Any(o=>o.OrderId == orderId);
    }

    public bool SaveChanges()
    {
        return (_context.SaveChanges()>=0);
    }
}