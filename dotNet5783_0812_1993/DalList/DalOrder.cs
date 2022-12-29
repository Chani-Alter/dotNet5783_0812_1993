using DO;
using static Dal.DataSource;
using DalApi;

namespace Dal;

/// <summary>
/// A department that performs operations: 
/// adding, updating, repeating and deleting on the order array
/// </summary>
internal class DalOrder:IOrder
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
    /// get all the orders
    /// </summary>
    /// <returns>an array of orders</returns>
    public IEnumerable<Order?> GetList(Func<Order?, bool>? predicate)
    {
        if (predicate == null)
            return OrderList.Select(order => order);
        return  OrderList.Where(predicate);
    }

    public Order GetByCondition(Func<Order?, bool> predicate)
    {
        return  OrderList.FirstOrDefault(predicate) ?? 
            throw new DoesNotExistedDalException("There is no order that matches the condition");
    }

    /// <summary>
    /// delete an order
    /// </summary>
    /// <param name="id">the id of the order</param>
    /// <exception cref="Exception">if the order didnt exist</exception>
    public void Delete(int id)
    {
        int index = search(id);
        if (index != -1)
        {
            OrderList.RemoveAt(index);
        }
        else
            throw new DoesNotExistedDalException("order is not exist");
    }

    /// <summary>
    /// update an order
    /// </summary>
    /// <param name="order">the updated order details</param>
    /// <exception cref="Exception">if the order doesnt exist</exception>
    public void Update(Order order)
    {
        int index = search(order.ID);
        if (index != -1)
            OrderList[index] = order;
        else
            throw new DoesNotExistedDalException("order is not exist");
    }

    /// <summary>
    ///search function
    /// </summary>
    /// <returns>returns the index of the member found</returns>
    private int search(int id)
    {
        for (int i = 0; i < OrderList.Count; i++)
        {
            if (OrderList[i]?.ID == id)
                return i;
        }
        return -1;
    }

}
