using DO;
using System;
using static Dal.DataSource;

namespace Dal;

public class DalOrderItem
{
    public int Add(OrderItem orderItem)
    {
        int i;
        for (i = 0; i < OrderArray.Length || OrderArray[i].ID != orderItem.OrderID; i++) ;
        if (i == OrderArray.Length)
            throw new Exception("order id is not exist");
        for (i = 0; i < ProductArray.Length || ProductArray[i].ID != orderItem.OrderID; i++) ;
        if (i == ProductArray.Length)
            throw new Exception("product id is not exist");
        int id = Config.OrderItemId;
        orderItem.ID = id;
        OrderItemArray[Config.IndexOrderItemArray++] = orderItem;
        return id;
    }
    public OrderItem GetById(int id)
    {
        for (int i = 0; i < OrderItemArray.Length; i++)
        {
            if (OrderItemArray[i].ID == id)
                return OrderItemArray[i];
        }
        throw new Exception("Order item is not exist");
    }

    public OrderItem GetByOrderIdAndProductId(int orderId , int productId)
    {
        for (int i = 0; i < OrderItemArray.Length; i++)
        {
            if (OrderItemArray[i].ProductID == productId && OrderItemArray[i].OrderID==orderId)
                return OrderItemArray[i];
        }
        throw new Exception("Order item is not exist");
    }

    public OrderItem[] GetAllItemsByOrderId(int orderId)
    {
        OrderItem[] orderItems = new OrderItem[OrderItemArray.Length];
        int index = 0;
        for (int i = 0; i < OrderItemArray.Length; i++)
        {
            if (OrderItemArray[i].OrderID == orderId)
                orderItems[index++] = OrderItemArray[i];
        }
        if(index==0)
            throw new Exception("Order item is not exist");
        return orderItems;
    }

    public OrderItem[] GetAll()
    {
        OrderItem[] orderItems = new OrderItem[OrderItemArray.Length];
        for (int i = 0; i < OrderItemArray.Length; i++)
        {
            orderItems[i] = OrderItemArray[i];
        }
        return orderItems;
    }

    public void Delete(int id)
    {
        int i;
        for (i = 0; i < OrderItemArray.Length || OrderItemArray[i].ID != id; i++) ;
        if (i == OrderItemArray.Length)
            throw new Exception("order item is not exist");
        i++;
        for (; i < OrderItemArray.Length; i++)
            OrderItemArray[i - 1] = OrderItemArray[i];
        Config.IndexOrderItemArray--;
    }

    public void Update(OrderItem orderItem)
    {
        int i;
        for (i = 0; i < OrderArray.Length || OrderArray[i].ID != orderItem.OrderID; i++) ;
        if (i == OrderArray.Length)
            throw new Exception("order id is not exist");
        for (i = 0; i < ProductArray.Length || ProductArray[i].ID != orderItem.OrderID; i++) ;
        if (i == ProductArray.Length)
            throw new Exception("product id is not exist");
        for (i = 0; i < OrderItemArray.Length || OrderItemArray[i].ID != orderItem.ID; i++) ;
        if (i == OrderArray.Length)
            throw new Exception("order item is not exist");
        OrderItemArray[i] = orderItem;
    }


}
