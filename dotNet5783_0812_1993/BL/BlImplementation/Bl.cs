using BlApi;
namespace BlImplementation;

/// <summary>
///A class that implements the interface - Ibl and contains all the data entities
/// </summary>

sealed internal class Bl : IBl
{
    /// <summary>
    ///Returns the product entity
    /// </summary>
    public IProduct Product { get; } = new Product();

    /// <summary>
    /// Returns the order entity
    /// </summary>
    public IOrder Order { get; } = new Order();

    /// <summary>
    ///Returns the cart entity
    /// </summary>
    public ICart Cart { get; } = new Cart();

    /// <summary>
    /// Returns the user entity
    /// </summary>
    public IUser User { get; } = new User();


}
