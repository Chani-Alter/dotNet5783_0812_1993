using BO;
using IUser = BlApi.IUser;


namespace BlImplementation;

/// <summary>
/// A class that implements the iproduct interface
/// </summary>
internal class User : IUser
{
    public int AddUser(BO.User user)
    {
        throw new NotImplementedException();
    }

    public void DeleteUser(int id)
    {
        throw new NotImplementedException();
    }

    public ProductItem GetUserByEmailAndPass(string email, string password)
    {
        throw new NotImplementedException();
    }

    public BO.Product UpdateUser(BO.Product product)
    {
        throw new NotImplementedException();
    }
}