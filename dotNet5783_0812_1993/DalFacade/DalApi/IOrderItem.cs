using DO;
namespace DalApi
{
    public interface IOrderItem:ICrud<IOrderItem>
    {
        public IEnumerable<OrderItem> GetAllItemsByOrderId(int orderId);
        public OrderItem GetByOrderIdAndProductId(int orderId, int productId);

    }
}
