using DO;
using DalApi;

namespace Dal;

/// <summary>
/// A department that performs operations: 
/// adding, updating, repeating and deleting on the order file
/// </summary>
internal class DalOrder : IOrder
{
    #region PUBLIC MEMBERS

    /// <summary>
    /// add a order to the order file
    /// </summary>
    /// <param name="order">the new order</param>
    /// <returns>the insert new order id</returns>
    public int Add(Order order)
    {
        List<Order?> orderList = XmlTools.LoadListFromXmlSerializer<Order>(entityName);
        order.ID = XmlTools.NewID(entityName);
        orderList.Add(order);

        XmlTools.SaveListForXmlSerializer(orderList, entityName);

        return order.ID;
    }

    /// <summary>
    /// get list of orders by condition
    /// </summary>
    /// <returns>colection of orders</returns>
    public IEnumerable<Order?> GetList(Func<Order?, bool>? predicate)
    {
        List<Order?> orderList = XmlTools.LoadListFromXmlSerializer<Order>(entityName);
        if (predicate == null)
            return orderList.Select(order => order);
        return orderList.Where(predicate);
    }

    /// <summary>
    /// get a spesific order by condition
    /// </summary>
    /// <param name="predicate">a condition</param>
    /// <returns>the order</returns>
    /// <exception cref="DoesNotExistedDalException">if the order does not exist</exception>
    public Order GetByCondition(Func<Order?, bool> predicate)
    {
        List<Order?> orderList = XmlTools.LoadListFromXmlSerializer<Order>(entityName);

        return orderList.FirstOrDefault(predicate) ??
            throw new DoesNotExistedDalException("There is no order that matches the condition");
    }

    /// <summary>
    /// delete an order
    /// </summary>
    /// <param name="id">the id of the order</param>
    /// <exception cref="Exception">if the order didoes notdnt exist</exception>
    public void Delete(int id)
    {
        List<Order?> orderList = XmlTools.LoadListFromXmlSerializer<Order>(entityName);

        var result = orderList.FirstOrDefault(ord => ord?.ID == id);

        if (result == null)
            throw new DoesNotExistedDalException(id, "order", "order is not exist");

        orderList.Remove(result);

        XmlTools.SaveListForXmlSerializer(orderList, entityName);

    }

    /// <summary>
    /// update an order
    /// </summary>
    /// <param name="order">the updated order details</param>
    /// <exception cref="Exception">if the order doesnt exist</exception>
    public void Update(Order order)
    {
        List<Order?> orderList = XmlTools.LoadListFromXmlSerializer<Order>(entityName);

        int index = orderList.FindIndex(ord => ord?.ID == order.ID);
        if (index == -1)
            throw new DoesNotExistedDalException(order.ID, "order", "order is not exist");

        orderList[index] = order;
        XmlTools.SaveListForXmlSerializer(orderList, entityName);

    }

    #endregion

    #region PRIVATE MEMBER

    /// <summary>
    /// the entity name 
    /// </summary>
    const string entityName = @"Order";

    #endregion

}