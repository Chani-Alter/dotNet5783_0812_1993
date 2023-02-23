using DO;
using DalApi;

namespace Dal;

/// <summary>
/// A department that performs operations: 
/// adding, updating, repeating and deleting on the order file
/// </summary>
internal class DalUser : IUser
{
    #region PUBLIC MEMBERS

    /// <summary>
    /// add a order to the order file
    /// </summary>
    /// <param name="order">the new order</param>
    /// <returns>the insert new order id</returns>
    public int Add(User user)
    {
        List<User?> userList = XmlTools.LoadListFromXmlSerializer<User>(entityName);
        var result = userList.Where(u => u?.CustomerEmail == user.CustomerEmail);
        if (result.Count() > 0) throw new DuplicateDalException("Email already exist in the system");
        user.ID = XmlTools.NewID(entityName);
        userList.Add(user);

        XmlTools.SaveListForXmlSerializer(userList, entityName);

        return user.ID;
    }

    /// <summary>
    /// get list of orders by condition
    /// </summary>
    /// <returns>colection of orders</returns>
    public IEnumerable<User?> GetList(Func<User?, bool>? predicate)
    {
        List<User?> userList = XmlTools.LoadListFromXmlSerializer<User>(entityName);
        if (predicate == null)
            return userList.Select(user => user);
        return userList.Where(predicate);
    }

    /// <summary>
    /// get a spesific order by condition
    /// </summary>
    /// <param name="predicate">a condition</param>
    /// <returns>the order</returns>
    /// <exception cref="DoesNotExistedDalException">if the order does not exist</exception>
    public User GetByCondition(Func<User?, bool> predicate)
    {
        List<User?> userList = XmlTools.LoadListFromXmlSerializer<User>(entityName);

        return userList.FirstOrDefault(predicate) ??
            throw new DoesNotExistedDalException("There is no user that matches the condition");
    }

    /// <summary>
    /// delete an order
    /// </summary>
    /// <param name="id">the id of the order</param>
    /// <exception cref="Exception">if the order didoes notdnt exist</exception>
    public void Delete(int id)
    {
        List<User?> userList = XmlTools.LoadListFromXmlSerializer<User>(entityName);

        var result = userList.FirstOrDefault(user => user?.ID == id);

        if (result == null)
            throw new DoesNotExistedDalException(id, "user", "order is not exist");

        userList.Remove(result);

        XmlTools.SaveListForXmlSerializer(userList, entityName);

    }

    /// <summary>
    /// update an order
    /// </summary>
    /// <param name="order">the updated order details</param>
    /// <exception cref="Exception">if the order doesnt exist</exception>
    public void Update(User user)
    {
        List<User?> userList = XmlTools.LoadListFromXmlSerializer<User>(entityName);

        int index = userList.FindIndex(u => u?.ID == user.ID);
        if (index == -1)
            throw new DoesNotExistedDalException(user.ID, "user", "user is not exist");

        userList[index] = user;
        XmlTools.SaveListForXmlSerializer(userList, entityName);

    }

    #endregion

    #region PRIVATE MEMBER

    /// <summary>
    /// the entity name 
    /// </summary>
    const string entityName = @"User";

    #endregion

}