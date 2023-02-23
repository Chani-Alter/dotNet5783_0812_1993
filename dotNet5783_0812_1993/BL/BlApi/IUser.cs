﻿using BO;

namespace BlApi;

/// <summary>
/// Interface to the User entity
/// </summary>
public interface IUser
{

    /// <summary>
    /// A function Defination for adding a product
    /// </summary>
    /// <param name="product"></param>
    /// <returns></returns>
    public int AddUser(User user);

    /// <summary>
    /// A function Defination for deleteing a product
    /// </summary>
    /// <param name="id"></param>
    public void DeleteUser(int id);

    /// <summary>
    /// A function Defination for updateing a product
    /// </summary>
    /// <param name="product"></param>
    /// <returns></returns>
    public Product UpdateUser(Product product);

    /// <summary>
    /// A function Defination for return a user by email and password
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public ProductItem GetUserByEmailAndPass(string email , string password);
}
