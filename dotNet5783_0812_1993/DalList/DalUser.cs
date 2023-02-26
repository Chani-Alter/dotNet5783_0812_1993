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
    [MethodImpl(MethodImplOptions.Synchronized)]
    public int Add(User user)
    {
        user.ID = UserId;
        UserList.Add(user);
        return user.ID;
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Delete(int id)
    {
        var result = UserList.FirstOrDefault(user => user?.ID == id);

        if (result == null)
            throw new DoesNotExistedDalException(id, "user", "user is not exist");

        UserList.Remove(result);
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public User GetByCondition(Func<User?, bool> predicate)
    {
        return UserList.FirstOrDefault(predicate) ??
                   throw new DoesNotExistedDalException("There is no user that matches the condition");
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public IEnumerable<User?> GetList(Func<User?, bool>? predicate = null)
    {
        if (predicate == null)
            return UserList.Select(user => user);
        return UserList.Where(predicate);
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Update(User user)
    {
        int index = UserList.FindIndex(us => us?.ID == user.ID);
        if (index == -1)
            throw new DoesNotExistedDalException(user.ID, "order", "order is not exist");

        UserList[index] = user;
    }
}

