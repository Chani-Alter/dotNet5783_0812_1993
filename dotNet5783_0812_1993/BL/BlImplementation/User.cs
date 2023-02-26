using BO;
using IUser = BlApi.IUser;
using System.Runtime.CompilerServices;

namespace BlImplementation;

/// <summary>
/// A class that implements the iproduct interface
/// </summary>
internal class User : IUser
{
    [MethodImpl(MethodImplOptions.Synchronized)]
    public int AddUser(BO.User user)
    {
        try
        {
           int id = dal.User.Add(Utils.cast<DO.User,BO.User>(user)); 
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

    [MethodImpl(MethodImplOptions.Synchronized)]
    public BO.User GetUserByEmailAndPass(string email, string password)
    {
        try
        {
            return Utils.cast<BO.User, DO.User>(dal.User.GetByCondition(u => u?.CustomerEmail == email && u?.Password == password));
        }
        catch(DO.DoesNotExistedDalException ex) 
        { 
            throw new DoesNotExistedBlException("User doesnt exist",ex);
        }
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void UpdateUser(BO.User user)
    {
        try
        {
            dal.User.Update(Utils.cast<DO.User,BO.User>(user));
        }
        catch (DO.DoesNotExistedDalException ex)
        {
            throw new DoesNotExistedBlException("User doesnt exist", ex);
        }
    }


    /// <summary>
    /// An attribute that contains access to all the dallist data
    /// </summary>
    DalApi.IDal? dal = DalApi.Factory.Get();

}