using BO;
using IUser = BlApi.IUser;
using System.Runtime.CompilerServices;

namespace BlImplementation;

/// <summary>
/// A class that implements the iproduct interface
/// </summary>
internal class User : IUser
{
    #region PUBLIC MEMBERS

    /// <summary>
    /// add user to the dal
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    /// <exception cref="BLAlreadyExistException"></exception>
    /// <exception cref="DoesNotExistedBlException"></exception>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public int AddUser(BO.User user)
    {
        try
        {
           int id = dal.User.Add(user.Cast<DO.User,BO.User>()); 
           return id;
        }
        catch(DO.DuplicateDalException ex)
        {
          throw new BLAlreadyExistException("Email already exist" , ex);
        }
        catch(DO.XMLFileNullExeption ex)
        {
            throw new DoesNotExistedBlException("user doesnwt exist" ,ex);  
        }
    }

    /// <summary>
    /// delete user from dal
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="DoesNotExistedBlException"></exception>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void DeleteUser(int id)
    {
        try
        {
            dal.User.Delete(id); 
        }
        catch (DO.DoesNotExistedDalException ex) 
        {
            throw new DoesNotExistedBlException("user doesnwt exist", ex);
        }
    }

    /// <summary>
    /// return the user by id and email
    /// </summary>
    /// <param name="email"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    /// <exception cref="DoesNotExistedBlException"></exception>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public BO.User GetUserByEmailAndPass(string email, string password)
    {
        try
        {
            return dal.User.GetByCondition(u => u?.CustomerEmail == email && u?.Password == password).Cast<BO.User, DO.User>();
        }
        catch(DO.DoesNotExistedDalException ex) 
        { 
            throw new DoesNotExistedBlException("User doesnt exist",ex);
        }
    }

    /// <summary>
    /// updet the user details
    /// </summary>
    /// <param name="user"></param>
    /// <exception cref="DoesNotExistedBlException"></exception>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void UpdateUser(BO.User user)
    {
        try
        {
            dal.User.Update(user.Cast<DO.User,BO.User>());
        }
        catch (DO.DoesNotExistedDalException ex)
        {
            throw new DoesNotExistedBlException("User doesnt exist", ex);
        }
    }

    #endregion

    #region PRIVATE MEMBER

    /// <summary>
    /// An attribute that contains access to all the dallist data
    /// </summary>
    DalApi.IDal? dal = DalApi.Factory.Get();

    #endregion
}