using BO;

namespace BlApi;

/// <summary>
/// Interface to the cart entity
/// </summary>
public interface ICart
{
    public Cart GetUserCart(int id);

    /// <summary>
    /// A function Defination for Adding a item to the cart
    /// </summary>
    /// <param name="cart"></param>
    /// <param name="productId"></param>
    /// <returns>the cart</returns>
    public Cart AddProductToCart(Cart cart, int productId , int amount);

    /// <summary>
    ///  A function Defination for update the amount of single product in the cart
    /// </summary>
    /// <param name="cart"></param>
    /// <param name="productId"></param>
    /// <param name="amount"></param>
    /// <returns>cart</returns>
    public Cart UpdateProductAmountInCart(Cart cart, int productId, int amount);

    /// <summary>
    ///  A function Defination for  confirm an order
    /// </summary>
    /// <param name="cart"></param>
    public void MakeOrder(Cart cart);


}
