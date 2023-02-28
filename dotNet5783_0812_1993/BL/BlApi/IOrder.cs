using BO;

namespace BlApi;

/// <summary>
/// Interface to the order entity
/// </summary>
public interface IOrder
{
    /// <summary>
    ///A function Defination that returns the list of the orders
    /// </summary>
    /// <returns></returns>
    public IEnumerable<OrderForList> GetOrderList();

    /// <summary>
    ///A function Defination that returns order by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Order GetOrderById(int id);

    /// <summary>
    ///A function Defination updateing the send order
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Order UpdateSendOrderByManager(int id);

    /// <summary>
    ///A function Defination that update the supply order
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Order UpdateSupplyOrderByManager(int id);

    /// <summary>
    ///A function Defination that tracks the order
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public OrderTracking TrackingOrder(int id);

    /// <summary>
    /// A function Defination that updates the quantity of a product in the order :Bonus!!!!!
    /// </summary>
    /// <param name="orderId"></param>
    /// <param name="productId"></param>
    /// <param name="amount"></param>
    /// <returns>the updated order item</returns>
    public Order UpdateAmountOfOProductInOrder(int orderId, int productId, int amount);

    /// <summary>
    /// select the oldest non completedd order
    /// </summary>
    /// <returns></returns>
    public int? SelectOrder();
}
