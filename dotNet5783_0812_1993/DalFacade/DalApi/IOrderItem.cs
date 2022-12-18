﻿using DO;
namespace DalApi;

/// <summary>
/// An interface that implements the icrud interface and add 2 functions
/// </summary>
public interface IOrderItem:ICrud<OrderItem>
{
    public IEnumerable<OrderItem> GetAllItemsByOrderId(int orderId);
    public OrderItem GetByOrderIdAndProductId(int orderId, int productId);

}
