using DalApi;
using DO;
using System.Linq;
using static Dal.DataSource;

namespace Dal;

/// <summary>
/// A department that performs operations: 
/// adding, updating, repeating and deleting on the orderItem array
/// </summary>
internal class DalOrderItem : IOrderItem
{
    /// <summary>
    /// add a orderitem to the array
    /// </summary>
    /// <param name="orderItem">the new order item </param>
    /// <returns>the id of the new order item</returns>
    /// <exception cref="Exception">if the order id or the product id doesnt exist</exception>
    public int Add(OrderItem orderItem)
    {
        var resultOrder = OrderList.FirstOrDefault(ord => ord?.ID == orderItem.OrderID);
        if (resultOrder == null)
            throw new DoesNotExistedDalException("order id is not exist");
        var resultProduct = ProductList.FirstOrDefault(prod => prod?.ID == orderItem.ProductID);
        if (resultProduct == null)
            throw new DoesNotExistedDalException("product id is not exist");

        orderItem.ID = OrderItemId;
        OrderItemList.Add(orderItem);
        return orderItem.ID;
    }

    /// <summary>
    /// get all the order items
    /// </summary>
    /// <returns>an array of all the order items</returns>
    public IEnumerable<OrderItem?> GetList(Func<OrderItem?, bool>? predicate)
    {
        if (predicate == null)
            return OrderItemList.Select(product => product);
        return OrderItemList.Where(predicate);
    }

    public OrderItem GetByCondition(Func<OrderItem?, bool> predicate)
    {
        return OrderItemList.FirstOrDefault(predicate) ??
            throw new DoesNotExistedDalException("There is no order item that matches the condition");
    }


    /// <summary>
    /// delete a order item
    /// </summary>
    /// <param name="id">the order item id</param>
    /// <exception cref="Exception">if the order item didnt exist</exception>
    public void Delete(int id)
    {
        int index = search(id);
        if (index != -1)
        {
            OrderItemList.RemoveAt(index);
        }
        else
            throw new DoesNotExistedDalException(" OrderItem is not exist");
    }

    /// <summary>
    /// update an order item
    /// </summary>
    /// <param name="orderItem">the new details of the order item</param>
    /// <exception cref="Exception">if the order cdoesnt exist</exception>
    public void Update(OrderItem orderItem)
    {
        int index = search(orderItem.ID);
        if (index != -1)
            OrderItemList[index] = orderItem;
        else
            throw new DoesNotExistedDalException(" OrderItem is not exist");

    }


    /// <summary>
    ///search function
    /// </summary>
    /// <returns>returns the index of the member found</returns>

    private int search(int id)
    {
        for (int i = 0; i < OrderItemList.Count; i++)
        {
            if (OrderItemList[i]?.ID == id)
                return i;
        }
        return -1;
    }

}
