using DalApi;
using DO;
using static Dal.DataSource;

namespace Dal;

/// <summary>
/// A department that performs operations: 
/// adding, updating, repeating and deleting on the orderItem array
/// </summary>
internal class DalOrderItem:IOrderItem
{
    /// <summary>
    /// add a orderitem to the array
    /// </summary>
    /// <param name="orderItem">the new order item </param>
    /// <returns>the id of the new order item</returns>
    /// <exception cref="Exception">if the order id or the product id doesnt exist</exception>
    public int Add(OrderItem orderItem)
    {
        int i;
        for (i = 0; i < OrderList.Count && OrderList[i].ID != orderItem.OrderID; i++) ;
        if (i == OrderList.Count)
            throw new DoesNotExistedDalException("order id is not exist");
        for (i = 0; i < ProductList.Count && ProductList[i].ID != orderItem.ProductID; i++) ;
        if (i == ProductList.Count)
            throw new DoesNotExistedDalException("product id is not exist");
        orderItem.ID =OrderItemId;
        OrderItemList.Add(orderItem);
        return orderItem.ID;
    }

    /// <summary>
    /// get order item by id
    /// </summary>
    /// <param name="id">the id of the order item</param>
    /// <returns>the requested order item</returns>
    /// <exception cref="Exception">if the order item doesnt exist</exception>
    public OrderItem GetById(int id)
    {
        int index = search(id);
        if (index != -1)
            return OrderItemList[index];
        else
            throw new DoesNotExistedDalException(" OrderItem is not exist");
    }

    /// <summary>
    /// get order item by order id and product id
    /// </summary>
    /// <param name="orderId">the order item orderId</param>
    /// <param name="productId">the order item productId</param>
    /// <returns>the order item</returns>
    /// <exception cref="Exception">if the order item doesnt exist</exception>
    public OrderItem GetByOrderIdAndProductId(int orderId , int productId)
    {
        for (int i = 0; i < OrderItemList.Count; i++)
        {
            if (OrderItemList[i].ProductID == productId && OrderItemList[i].OrderID == orderId)
                return OrderItemList[i];
        }
        throw new DoesNotExistedDalException("Order item is not exist");
    }

    /// <summary>
    /// get all order item of a specific order
    /// </summary>
    /// <param name="orderId">the order id</param>
    /// <returns>an array of order items</returns>
    /// <exception cref="Exception">if the order is not exist</exception>
    public IEnumerable<OrderItem> GetAllItemsByOrderId(int orderId)
    {
        List<OrderItem> orderItems = new List<OrderItem>();
        bool flag = false;
        for (int i = OrderItemList.Count - 1; i >= 0; i--)
        {
            if (OrderItemList[i].OrderID == orderId)
            {
                flag = true;
                orderItems.Add(OrderItemList[i]);
            }
        }
        if (flag == false)
            throw new DoesNotExistedDalException("Order item is not exist");
        return orderItems;
    }

    /// <summary>
    /// get all the order items
    /// </summary>
    /// <returns>an array of all the order items</returns>
    public IEnumerable<OrderItem> GetAll()
    {
        List<OrderItem> orderItems = new List<OrderItem>();
        for (int i = OrderItemList.Count - 1; i >= 0; i--)
        {
            orderItems.Add(OrderItemList[i]);
        }
        return orderItems;
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
            if (OrderItemList[i].ID == id)
                return i;
        }
        return -1;
    }

}
