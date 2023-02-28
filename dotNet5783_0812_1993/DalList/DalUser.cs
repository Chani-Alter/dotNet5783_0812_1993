using DalApi;
using DO;
using System.Runtime.CompilerServices;
using static Dal.DataSource;

namespace Dal;

/// <summary>
/// A department that performs operations: 
/// adding, updating, repeating and deleting on the product array
/// </summary>
internal class DalUser : IUser
{
    /// <summary>
    /// add user to list
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public int Add(User user)
    {
        user.ID = UserId;
        UserList.Add(user);
        return user.ID;
    }

    /// <summary>
    /// delete user from list
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="DoesNotExistedDalException"></exception>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Delete(int id)
    {
        var result = UserList.FirstOrDefault(user => user?.ID == id);

        if (result == null)
            throw new DoesNotExistedDalException(id, "user", "user is not exist");

        UserList.Remove(result);
    }

    /// <summary>
    /// get user that match the condition
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    /// <exception cref="DoesNotExistedDalException"></exception>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public User GetByCondition(Func<User?, bool> predicate)
    {
        return UserList.FirstOrDefault(predicate) ??
                   throw new DoesNotExistedDalException("There is no user that matches the condition");
    }

    /// <summary>
    /// get list of users filter by condition
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public IEnumerable<User?> GetList(Func<User?, bool>? predicate = null)
    {
        if (predicate == null)
            return UserList.Select(user => user);
        return UserList.Where(predicate);
    }

    /// <summary>
    /// oupdate user in list
    /// </summary>
    /// <param name="user"></param>
    /// <exception cref="DoesNotExistedDalException"></exception>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Update(User user)
    {
        int index = UserList.FindIndex(us => us?.ID == user.ID);
        if (index == -1)
            throw new DoesNotExistedDalException(user.ID, "order", "order is not exist");

        UserList[index] = user;
    }
}

