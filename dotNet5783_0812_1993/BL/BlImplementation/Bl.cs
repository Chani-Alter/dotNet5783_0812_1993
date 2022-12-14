using BlApi;
namespace BlImplementation;

/// <summary>
///A class that implements the interface - Ibl and contains all the data entities
/// </summary>

sealed public class Bl : IBl
{
    /// <summary>
    ///Returns the product entity
    /// </summary>
    public IProduct Product => new Product();

    /// <summary>
    /// Returns the order entity
    /// </summary>
    public IOrder Order => new Order();

    /// <summary>
    ///Returns the cart entity
    /// </summary>
    public ICart cart => new Cart();
}
