using DO;
namespace DalApi
{
    public interface IOrderItem:ICrud<OrderItem>
    {
        public IEnumerable<OrderItem> GetAllItemsByOrderId(int orderId);
        public OrderItem GetByOrderIdAndProductId(int orderId, int productId);

    }
}
