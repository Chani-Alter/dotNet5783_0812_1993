using DalApi;
using DO;
using static Dal.DataSource;
using System.Runtime.CompilerServices;

namespace Dal;

/// <summary>
/// A department that performs operations: 
/// adding, updating, repeating and deleting on the product array
/// </summary>
internal class DalCartItem : ICartItem
{
    /// <summary>
    /// add cart item to list
    /// </summary>
    /// <param name="cartItem"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public int Add(CartItem cartItem)
    {
        cartItem.ID = CartItemId;
        CartItemList.Add(cartItem);
        return cartItem.ID;
    }

    /// <summary>
    /// delete cart item from list
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="DoesNotExistedDalException"></exception>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Delete(int id)
    {
        var result = CartItemList.FirstOrDefault(cart => cart?.ID == id);

        if (result == null)
            throw new DoesNotExistedDalException(id, "cart item", "cartitem is not exist");

        CartItemList.Remove(result);
    }

    /// <summary>
    /// get cart item that metch the condition
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    /// <exception cref="DoesNotExistedDalException"></exception>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public CartItem GetByCondition(Func<CartItem?, bool> predicate)
    {
        return CartItemList.FirstOrDefault(predicate) ??
                   throw new DoesNotExistedDalException("There is no cart item that matches the condition");
    }

    /// <summary>
    /// get list of cart items filter by condition
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public IEnumerable<CartItem?> GetList(Func<CartItem?, bool>? predicate = null)
    {
        if (predicate == null)
            return CartItemList.Select(cartItem => cartItem);
        return CartItemList.Where(predicate);
    }

    /// <summary>
    /// update cart item in list
    /// </summary>
    /// <param name="cartItem"></param>
    /// <exception cref="DoesNotExistedDalException"></exception>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Update(CartItem cartItem)
    {
        int index = CartItemList.FindIndex(item => item?.ID == cartItem.ID);
        if (index == -1)
            throw new DoesNotExistedDalException(cartItem.ID, "order", "order is not exist");

        CartItemList[index] = cartItem;
    }
}

