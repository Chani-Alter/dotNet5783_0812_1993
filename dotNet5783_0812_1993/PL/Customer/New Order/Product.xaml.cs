using System.Linq;
using System.Windows;

namespace PL.Customer;

/// <summary>
/// Interaction logic for Product.xaml
/// </summary>
public partial class Product : Window
{
    #region PUBLIC MEMBERS

    /// <summary>
    /// A dependency property of the product data
    /// </summary>
    public BO.ProductItem ProductData
    {
        get { return (BO.ProductItem)GetValue(ProductDataProperty); }
        set { SetValue(ProductDataProperty, value); }
    }

    public static readonly DependencyProperty ProductDataProperty =
        DependencyProperty.Register("ProductData", typeof(BO.ProductItem), typeof(Window), new PropertyMetadata(null));

    /// <summary>
    /// A depency property for the amount of the product in the cart
    /// </summary>
    public int AmountCart
    {
        get { return (int)GetValue(AmountCartProperty); }
        set { SetValue(AmountCartProperty, value); }
    }

    public static readonly DependencyProperty AmountCartProperty =
        DependencyProperty.Register("AmountCart", typeof(int), typeof(Window), new PropertyMetadata(0));

    /// <summary>
    /// a ctor for the product window
    /// </summary>
    /// <param name="product"></param>
    /// <param name="catalog"></param>
    public Product(BO.ProductItem product, Catalog catalog)
    {
        ProductData = (product == null) ? new() : product;
        cart = catalog.Cart;
        myCatalog = catalog;
        amountInCart();
        InitializeComponent();
    }

    #endregion

    #region PRIVATE MEMBERS

    /// <summary>
    /// instance of the bl who contains access to all the bl implementation
    /// </summary>
    BlApi.IBl? bl = BlApi.Factory.Get();

    /// <summary>
    ///The catalog window instance
    /// </summary>
    Catalog myCatalog;

    /// <summary>
    /// The cart property
    /// </summary>
    BO.Cart? cart;

    /// <summary>
    /// take from the cart the amount of the product
    /// </summary>
    private void amountInCart()
    {
        var result = cart.Items?.FirstOrDefault(item => item.ProductID == ProductData.ID);
        AmountCart = (result == null) ? 0 : result.Amount;

    }

    /// <summary>
    /// adds the product to cart and close the window
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void addCart_Click(object sender, RoutedEventArgs e)
    {
        bl.cart.AddProductToCart(cart!, ProductData.ID, AmountCart);
        myCatalog.Activate();
        Close();
    }

    /// <summary>
    /// Increases the quantity by 1
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void pluse_Click(object sender, RoutedEventArgs e)
    {
        AmountCart = AmountCart + 1;
    }

    /// <summary>
    /// Decreases the quantity by 1
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void minuse_Click(object sender, RoutedEventArgs e)
    {
        AmountCart = AmountCart - 1;
    }

    /// <summary>
    /// delete the product from the cart
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void deleteCart_Click(object sender, RoutedEventArgs e)
    {
        AmountCart = 0;
        updateAmount();
        myCatalog.Activate();
        Close();
    }

    /// <summary>
    /// update the amount of the product in cart by calling the update method
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void updateCart_Click(object sender, RoutedEventArgs e) => updateAmount();

    /// <summary>
    /// update the amount of product in cart
    /// </summary>
    private void updateAmount()
    {
        bl.cart.UpdateProductAmountInCart(cart!, ProductData.ID, AmountCart);
        myCatalog.Activate();
        Close();
    }

    /// <summary>
    /// close the window and return to the catalog window
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void back_to_catalog_Click(object sender, RoutedEventArgs e)
    {
        myCatalog.Activate();
        Close();
    }
    #endregion
}