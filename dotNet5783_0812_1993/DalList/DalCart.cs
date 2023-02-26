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
    [MethodImpl(MethodImplOptions.Synchronized)]
    public int Add(Cart cart)
    {
        cart.ID = CartId;
        CartList.Add(cart);
        return cart.ID;
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Delete(int id)
    {
        var result = OrderList.FirstOrDefault(ord => ord?.ID == id);

        if (result == null)
            throw new DoesNotExistedDalException(id, "order", "order is not exist");

        OrderList.Remove(result);
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public Cart GetByCondition(Func<Cart?, bool> predicate)
    {
        return CartList.FirstOrDefault(predicate) ??
           throw new DoesNotExistedDalException("There is no cart that matches the condition");
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public IEnumerable<Cart?> GetList(Func<Cart?, bool>? predicate = null)
    {
        if (predicate == null)
            return CartList.Select(cart => cart);
        return CartList.Where(predicate); throw new NotImplementedException();
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Update(Cart cart)
    {
        int index = CartList.FindIndex(c => c?.ID == cart.ID);
        if (index == -1)
            throw new DoesNotExistedDalException(cart.ID, "order", "order is not exist");

        CartList[index] = cart;
    }
}
