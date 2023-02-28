using DO;
using DalApi;
using System.Runtime.CompilerServices;

namespace Dal;

/// <summary>
/// A department that performs operations: 
/// adding, updating, repeating and deleting on the CartItem file
/// </summary>
internal class DalCartItem : ICartItem
{
    #region PUBLIC MEMBERS

    [MethodImpl(MethodImplOptions.Synchronized)]
    /// <summary>
    /// add a CartItem to the CartItem file
    /// </summary>
    /// <param name="CartItem">the new CartItem</param>
    /// <returns>the insert new CartItem id</returns>
    public int Add(CartItem item)
    {
        List<CartItem?> cartItemList = XmlTools.LoadListFromXmlSerializer<CartItem>(entityName);
        item.ID = XmlTools.NewID(entityName);
        cartItemList.Add(item);
        XmlTools.SaveListForXmlSerializer(cartItemList, entityName);

        return item.ID;
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    /// <summary>
    /// get list of CartItems by condition
    /// </summary>
    /// <returns>colection of CartItems</returns>
    public IEnumerable<CartItem?> GetList(Func<CartItem?, bool>? predicate)
    {
        List<CartItem?> cartItemList = XmlTools.LoadListFromXmlSerializer<CartItem>(entityName);
        if (predicate == null)
            return cartItemList.Select(CartItem => CartItem);
        var result = cartItemList.Where(predicate);
        return cartItemList.Where(predicate);
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    /// <summary>
    /// get a spesific CartItem by condition
    /// </summary>
    /// <param name="predicate">a condition</param>
    /// <returns>the CartItem</returns>
    /// <exception cref="DoesNotExistedDalException">if the CartItem does not exist</exception>
    public CartItem GetByCondition(Func<CartItem?, bool> predicate)
    {
        List<CartItem?> cartItemList = XmlTools.LoadListFromXmlSerializer<CartItem>(entityName);

        return cartItemList.FirstOrDefault(predicate) ??
            throw new DoesNotExistedDalException("There is no CartItem that matches the condition");
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    /// <summary>
    /// delete an CartItem
    /// </summary>
    /// <param name="id">the id of the CartItem</param>
    /// <exception cref="Exception">if the CartItem didoes notdnt exist</exception>
    public void Delete(int id)
    {
        List<CartItem?> cartItemList = XmlTools.LoadListFromXmlSerializer<CartItem>(entityName);

        var result = cartItemList.FirstOrDefault(ord => ord?.ID == id);

        if (result == null)
            throw new DoesNotExistedDalException(id, "CartItem", "CartItem is not exist");

        cartItemList.Remove(result);

        XmlTools.SaveListForXmlSerializer(cartItemList, entityName);

    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    /// <summary>
    /// update an CartItem
    /// </summary>
    /// <param name="CartItem">the updated CartItem details</param>
    /// <exception cref="Exception">if the CartItem doesnt exist</exception>
    public void Update(CartItem item)
    {
        List<CartItem?> cartItemList = XmlTools.LoadListFromXmlSerializer<CartItem>(entityName);

        int index = cartItemList.FindIndex(ci => ci?.ID == item.ID);
        if (index == -1)
            throw new DoesNotExistedDalException(item.ID, "CartItem", "CartItem is not exist");

        cartItemList[index] = item;
        XmlTools.SaveListForXmlSerializer(cartItemList, entityName);

    }

    #endregion

    #region PRIVATE MEMBER

    /// <summary>
    /// the entity name 
    /// </summary>
    const string entityName = @"CartItem";

    #endregion

}