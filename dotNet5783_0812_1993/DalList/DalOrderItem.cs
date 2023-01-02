using DalApi;
using DO;
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
    /// <exception cref="Exception">if the order  or the product  doesnt exist</exception>
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
    /// get list of order items
    /// </summary>
    /// <returns>a list of order items</returns>
    public IEnumerable<OrderItem?> GetList(Func<OrderItem?, bool>? predicate)
    {
        if (predicate == null)
            return OrderItemList.Select(product => product);
        return OrderItemList.Where(predicate);
    }

    /// <summary>
    /// get a spesific order item by condition
    /// </summary>
    /// <param name="predicate">the condition</param>
    /// <returns>the order item</returns>
    /// <exception cref="DoesNotExistedDalException"></exception>
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
        var result = OrderItemList.FirstOrDefault(ord => ord?.ID == id);

        if (result == null)
            throw new DoesNotExistedDalException("order item is not exist");

        OrderItemList.Remove(result);

    }


    /// <summary>
    /// update an order item
    /// </summary>
    /// <param name="orderItem">the new details of the order item</param>
    /// <exception cref="Exception">if the order cdoesnt exist</exception>
    public void Update(OrderItem orderItem)
    {
        int index = OrderItemList.FindIndex(item => item?.ID == orderItem.ID);

        if (index == -1)
            throw new DoesNotExistedDalException(" OrderItem is not exist");

        OrderItemList[index] = orderItem;

    }

}