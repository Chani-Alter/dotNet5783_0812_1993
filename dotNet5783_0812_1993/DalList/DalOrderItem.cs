using DO;
using System;
using static Dal.DataSource;

namespace Dal;


/// <summary>
/// A department that performs operations: 
/// adding, updating, repeating and deleting on the orderItem array
/// </summary>
public class DalOrderItem
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
        int sizeOrders = Config.IndexOrderArray;
        int sizeProducts = Config.IndexOrderArray;
        for (i = 0; i < sizeOrders && OrderArray[i].ID != orderItem.OrderID; i++) ;
        if (i == sizeOrders)
            throw new Exception("order id is not exist");
        for (i = 0; i < sizeProducts && ProductArray[i].ID != orderItem.ProductID; i++) ;
        if (i == sizeProducts)
            throw new Exception("product id is not exist");
        int id = Config.OrderItemId;
        orderItem.ID = id;
        OrderItemArray[Config.IndexOrderItemArray++] = orderItem;
        return id;
    }

    /// <summary>
    /// get order item by id
    /// </summary>
    /// <param name="id">the id of the order item</param>
    /// <returns>the requested order item</returns>
    /// <exception cref="Exception">if the order item doesnt exist</exception>
    public OrderItem GetById(int id)
    {
        for (int i = 0; i < Config.IndexOrderItemArray; i++)
        {
            if (OrderItemArray[i].ID == id)
                return OrderItemArray[i];
        }
        throw new Exception("Order item is not exist");
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
        for (int i = 0; i < Config.IndexOrderItemArray; i++)
        {
            if (OrderItemArray[i].ProductID == productId && OrderItemArray[i].OrderID==orderId)
                return OrderItemArray[i];
        }
        throw new Exception("Order item is not exist");
    }

    /// <summary>
    /// get all order item of a specific order
    /// </summary>
    /// <param name="orderId">the order id</param>
    /// <returns>an array of order items</returns>
    /// <exception cref="Exception">if the order is not exist</exception>
    public OrderItem[] GetAllItemsByOrderId(int orderId)
    {
        OrderItem[] orderItems = new OrderItem[Config.IndexOrderItemArray];
        int index = 0;
        for (int i = 0; i < Config.IndexOrderItemArray; i++)
        {
            if (OrderItemArray[i].OrderID == orderId)
                orderItems[index++] = OrderItemArray[i];
        }
        if(index==0)
            throw new Exception("Order item is not exist");
        return orderItems;
    }

    /// <summary>
    /// get all the order items
    /// </summary>
    /// <returns>an array of all the order items</returns>
    public OrderItem[] GetAll()
    {
        OrderItem[] orderItems = new OrderItem[Config.IndexOrderItemArray];
        for (int i = 0; i < Config.IndexOrderItemArray; i++)
        {
            orderItems[i] = OrderItemArray[i];
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
        int i;
        for (i = 0; i < Config.IndexOrderItemArray && OrderItemArray[i].ID != id; i++) ;
        if (i == Config.IndexOrderItemArray)
            throw new Exception("order item is not exist");
        i++;
        for (; i < Config.IndexOrderItemArray; i++)
            OrderItemArray[i - 1] = OrderItemArray[i];
        Config.IndexOrderItemArray--;
    }

    /// <summary>
    /// update an order item
    /// </summary>
    /// <param name="orderItem">the new details of the order item</param>
    /// <exception cref="Exception">if the order cdoesnt exist</exception>
    public void Update(OrderItem orderItem)
    {
        int i;
        for (i = 0; i < Config.IndexOrderArray && OrderArray[i].ID != orderItem.OrderID; i++) ;
        if (i == Config.IndexOrderArray)
            throw new Exception("order id is not exist");
        for (i = 0; i < Config.IndexProductArray && ProductArray[i].ID != orderItem.ProductID; i++) ;
        if (i == Config.IndexProductArray)
            throw new Exception("product id is not exist");
        for (i = 0; i < Config.IndexOrderItemArray && OrderItemArray[i].ID != orderItem.ID; i++) ;
        if (i == Config.IndexOrderItemArray)
            throw new Exception("order item is not exist");
        OrderItemArray[i] = orderItem;
    }


}
