using BO;

namespace BlApi
{
    /// <summary>
    /// Interface for a order logical entity
    /// </summary>
    public interface IOrder
    {   /// <summary>
        /// Definition of a function that returns all orders
        /// </summary>
        /// <returns></returns>
        public IEnumerable<OrderForList> GetOrderList();
        /// <summary>
        /// Definition of a function that returns order by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Order GetOrderById(int id);
        /// <summary>
        /// Definition of a function that update the send order
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Order UpdateSendOrderByManager(int id);
        /// <summary>
        /// Definition of a function that update the supply order
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Order UpdateSupplyOrderByManager(int id);
        /// <summary>
        /// Definition of a function that tracks the order
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public OrderTracking TrackingOrder(int id);
        /// <summary>
        /// bonus:Definition of a function that updates the quantity of a product in the order
        /// </summary>
        /// <param name="idOrder"></param>
        /// <param name="idProduct"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public OrderItem UpdateAmountOfOProductInOrder(int idOrder, int idProduct, int amount);
    }
}
