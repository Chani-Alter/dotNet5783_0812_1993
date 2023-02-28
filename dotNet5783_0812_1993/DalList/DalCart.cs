using DalApi;
using DO;
using static Dal.DataSource;
using System.Runtime.CompilerServices;

namespace Dal;

/// <summary>
/// A department that performs operations: 
/// adding, updating, repeating and deleting on the product array
/// </summary>
internal class DalCart : ICart
{
    /// <summary>
    /// add the cart to the list
    /// </summary>
    /// <param name="cart"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public int Add(Cart cart)
    {
        cart.ID = CartId;
        CartList.Add(cart);
        return cart.ID;
    }

    /// <summary>
    /// delete cart from list
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="DoesNotExistedDalException"></exception>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Delete(int id)
    {
        var result = CartItemList.FirstOrDefault(cart => cart?.ID == id);

        if (result == null)
            throw new DoesNotExistedDalException(id, "cart", "cart is not exist");

        CartItemList.Remove(result);
    }

    /// <summary>
    /// get a cart that match the condition
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    /// <exception cref="DoesNotExistedDalException"></exception>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public Cart GetByCondition(Func<Cart?, bool> predicate)
    {
        return CartList.FirstOrDefault(predicate) ??
           throw new DoesNotExistedDalException("There is no cart that matches the condition");
    }

    /// <summary>
    /// get cart list filter by the predicat
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public IEnumerable<Cart?> GetList(Func<Cart?, bool>? predicate = null)
    {
        if (predicate == null)
            return CartList.Select(cart => cart);
        return CartList.Where(predicate); throw new NotImplementedException();
    }

    /// <summary>
    /// update cart in list
    /// </summary>
    /// <param name="cart"></param>
    /// <exception cref="DoesNotExistedDalException"></exception>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Update(Cart cart)
    {
        int index = CartList.FindIndex(c => c?.ID == cart.ID);
        if (index == -1)
            throw new DoesNotExistedDalException(cart.ID, "order", "order is not exist");

        CartList[index] = cart;
    }
}
