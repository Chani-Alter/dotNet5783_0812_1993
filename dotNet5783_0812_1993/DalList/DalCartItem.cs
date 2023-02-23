using DalApi;
using DO;
using System;
using System.Linq;
using static Dal.DataSource;

namespace Dal;

/// <summary>
/// A department that performs operations: 
/// adding, updating, repeating and deleting on the product array
/// </summary>
internal class DalCartItem : ICartItem
{
    public int Add(CartItem cartItem)
    {
        cartItem.ID = CartItemId;
        CartItemList.Add(cartItem);
        return cartItem.ID;
    }

    public void Delete(int id)
    {
        var result = CartItemList.FirstOrDefault(ord => ord?.ID == id);

        if (result == null)
            throw new DoesNotExistedDalException(id, "cart item", "cartitem is not exist");

        CartItemList.Remove(result);
    }

    public CartItem GetByCondition(Func<CartItem?, bool> predicate)
    {
        return CartItemList.FirstOrDefault(predicate) ??
                   throw new DoesNotExistedDalException("There is no cart item that matches the condition");
    }

    public IEnumerable<CartItem?> GetList(Func<CartItem?, bool>? predicate = null)
    {
        if (predicate == null)
            return CartItemList.Select(cartItem => cartItem);
        return CartItemList.Where(predicate);
    }

    public void Update(CartItem cartItem)
    {
        int index = CartItemList.FindIndex(item => item?.ID == cartItem.ID);
        if (index == -1)
            throw new DoesNotExistedDalException(cartItem.ID, "order", "order is not exist");

        CartItemList[index] = cartItem;
    }
}

