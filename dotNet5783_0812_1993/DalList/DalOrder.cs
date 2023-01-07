using DO;
using static Dal.DataSource;
using DalApi;

namespace Dal;

/// <summary>
/// A department that performs operations: 
/// adding, updating, repeating and deleting on the order array
/// </summary>
internal class DalOrder : IOrder
{
    /// <summary>
    /// add a order to the order array
    /// </summary>
    /// <param name="order">the new order</param>
    /// <returns>the insert new order id</returns>
    public int Add(Order order)
    {
        order.ID = OrderId;
        OrderList.Add(order);
        return order.ID;
    }


    /// <summary>
    /// get list of orders by condition
    /// </summary>
    /// <returns>an array of orders</returns>
    public IEnumerable<Order?> GetList(Func<Order?, bool>? predicate)
    {
        if (predicate == null)
            return OrderList.Select(order => order);
        return OrderList.Where(predicate);
    }

    /// <summary>
    /// get a spesific order by condition
    /// </summary>
    /// <param name="predicate">a condition</param>
    /// <returns>the order</returns>
    /// <exception cref="DoesNotExistedDalException">if the order does not exist</exception>
    public Order GetByCondition(Func<Order?, bool> predicate)
    {
        return OrderList.FirstOrDefault(predicate) ??
            throw new DoesNotExistedDalException("There is no order that matches the condition");
    }

    /// <summary>
    /// delete an order
    /// </summary>
    /// <param name="id">the id of the order</param>
    /// <exception cref="Exception">if the order didoes notdnt exist</exception>
    public void Delete(int id)
    {
        var result = OrderList.FirstOrDefault(ord => ord?.ID == id);

        if (result == null)
            throw new DoesNotExistedDalException(id , "order" , "order is not exist");

        OrderList.Remove(result);
    }


    /// <summary>
    /// update an order
    /// </summary>
    /// <param name="order">the updated order details</param>
    /// <exception cref="Exception">if the order doesnt exist</exception>
    public void Update(Order order)
    {
        int index = OrderList.FindIndex(ord => ord?.ID == order.ID);
        if (index == -1)
            throw new DoesNotExistedDalException(order.ID , "order" , "order is not exist");

        OrderList[index] = order;

    }

}