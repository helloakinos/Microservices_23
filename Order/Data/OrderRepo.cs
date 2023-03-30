using OrderService.Models;

namespace OrderService.Data;
public class OrderRepo : IOrderRepo
{
    private readonly AppDbContext _context;

    public OrderRepo(AppDbContext context)
    {
        _context = context;
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

    public bool ApiExists(int id)
    {
        //best to use Any, instead of first or default, so the return type is a bool
        return _context.Orders.Any(a=>a.ApiId == id);
    }
    public IEnumerable<Order> GetAllOrders()
    {
        return _context.Orders.ToList();
    }

    public Order GetOrderById(int id)
    {
        return _context.Orders.FirstOrDefault(o=>o.OrderId == id);
    }

    public bool SaveChanges()
    {
        return (_context.SaveChanges() >= 0);
    }
}