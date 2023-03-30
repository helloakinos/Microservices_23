using OrderService.Models;

namespace OrderService.Data;
public interface IOrderRepo
{
    bool SaveChanges();
    Order GetOrderById( int id);
    bool ApiExists(int id);
    IEnumerable<Order> GetAllOrders();
    void CreateOrder(Order order);
}