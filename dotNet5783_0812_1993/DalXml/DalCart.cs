using DO;
using DalApi;
using System.Runtime.CompilerServices;
namespace Dal;

/// <summary>
/// A department that performs operations: 
/// adding, updating, repeating and deleting on the cart file
/// </summary>
internal class DalCart : ICart
{
    #region PUBLIC MEMBERS

    [MethodImpl(MethodImplOptions.Synchronized)]
    /// <summary>
    /// add a cart to the cart file
    /// </summary>
    /// <param name="cart">the new cart</param>
    /// <returns>the insert new cart id</returns>
    public int Add(Cart cart)
    {
        List<Cart?> cartList = XmlTools.LoadListFromXmlSerializer<Cart>(entityName);
        cart.ID = XmlTools.NewID(entityName);
        cartList.Add(cart);
        XmlTools.SaveListForXmlSerializer(cartList, entityName);

        return cart.ID;
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    /// <summary>
    /// get list of carts by condition
    /// </summary>
    /// <returns>colection of carts</returns>
    public IEnumerable<Cart?> GetList(Func<Cart?, bool>? predicate)
    {
        List<Cart?> cartList = XmlTools.LoadListFromXmlSerializer<Cart>(entityName);
        if (predicate == null)
            return cartList.Select(cart => cart);
        return cartList.Where(predicate);
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    /// <summary>
    /// get a spesific cart by condition
    /// </summary>
    /// <param name="predicate">a condition</param>
    /// <returns>the cart</returns>
    /// <exception cref="DoesNotExistedDalException">if the cart does not exist</exception>
    public Cart GetByCondition(Func<Cart?, bool> predicate)
    {
        List<Cart?> cartList = XmlTools.LoadListFromXmlSerializer<Cart>(entityName);

        return cartList.FirstOrDefault(predicate) ??
            throw new DoesNotExistedDalException("There is no cart that matches the condition");
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    /// <summary>
    /// delete an cart
    /// </summary>
    /// <param name="id">the id of the cart</param>
    /// <exception cref="Exception">if the cart didoes notdnt exist</exception>
    public void Delete(int id)
    {
        List<Cart?> cartList = XmlTools.LoadListFromXmlSerializer<Cart>(entityName);

        var result = cartList.FirstOrDefault(ord => ord?.ID == id);

        if (result == null)
            throw new DoesNotExistedDalException(id, "cart", "cart is not exist");

        cartList.Remove(result);

        XmlTools.SaveListForXmlSerializer(cartList, entityName);
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    /// <summary>
    /// update an cart
    /// </summary>
    /// <param name="cart">the updated cart details</param>
    /// <exception cref="Exception">if the cart doesnt exist</exception>
    public void Update(Cart cart)
    {
        List<Cart?> cartList = XmlTools.LoadListFromXmlSerializer<Cart>(entityName);

        int index = cartList.FindIndex(c => c?.ID == cart.ID);
        if (index == -1)
            throw new DoesNotExistedDalException(cart.ID, "cart", "cart is not exist");

        cartList[index] = cart;
        XmlTools.SaveListForXmlSerializer(cartList, entityName);

    }

    #endregion

    #region PRIVATE MEMBER

    /// <summary>
    /// the entity name 
    /// </summary>
    const string entityName = @"Cart";

    #endregion

}