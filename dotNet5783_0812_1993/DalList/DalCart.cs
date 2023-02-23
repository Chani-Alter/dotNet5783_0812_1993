﻿using DalApi;
using DO;
using System;
using System.Linq;
using static Dal.DataSource;

namespace Dal;

/// <summary>
/// A department that performs operations: 
/// adding, updating, repeating and deleting on the product array
/// </summary>
internal class DalCart : ICart
{
    public int Add(Cart cart)
    {
        cart.ID = CartId;
        CartList.Add(cart);
        return cart.ID;
    }

    public void Delete(int id)
    {
        var result = OrderList.FirstOrDefault(ord => ord?.ID == id);

        if (result == null)
            throw new DoesNotExistedDalException(id, "order", "order is not exist");

        OrderList.Remove(result);
    }

    public Cart GetByCondition(Func<Cart?, bool> predicate)
    {
        return CartList.FirstOrDefault(predicate) ??
           throw new DoesNotExistedDalException("There is no cart that matches the condition");
    }

    public IEnumerable<Cart?> GetList(Func<Cart?, bool>? predicate = null)
    {
        if (predicate == null)
            return CartList.Select(cart => cart);
        return CartList.Where(predicate); throw new NotImplementedException();
    }

    public void Update(Cart cart)
    {
        int index = CartList.FindIndex(c => c?.ID == cart.ID);
        if (index == -1)
            throw new DoesNotExistedDalException(cart.ID, "order", "order is not exist");

        CartList[index] = cart;
    }
}