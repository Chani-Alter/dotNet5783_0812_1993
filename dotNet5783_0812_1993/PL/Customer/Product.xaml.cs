using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PL.Customer;

/// <summary>
/// Interaction logic for Product.xaml
/// </summary>
public partial class Product : Window
{
    
    public BO.Cart Cart;
    public BO.ProductItem ProductData
    {
        get { return (BO.ProductItem)GetValue(ProductDataProperty); }
        set { SetValue(ProductDataProperty, value); }
    }

    public static readonly DependencyProperty ProductDataProperty =
        DependencyProperty.Register("ProductData", typeof(BO.ProductItem), typeof(Window), new PropertyMetadata(null));

    public int AmountCart 
    {
        get { return (int)GetValue(AmountCartProperty); }
        set { SetValue(AmountCartProperty, value); }
    }

    public static readonly DependencyProperty AmountCartProperty =
        DependencyProperty.Register("AmountCart", typeof(int), typeof(Window), new PropertyMetadata(0));

    public Product(BO.ProductItem product , BO.Cart cart)
    {
        ProductData = (product == null) ? new() : product;
        Cart = (cart == null) ? new() : cart;
        amountInCart();
        InitializeComponent();
    }
    private BlApi.IBl? bl = BlApi.Factory.Get();


    private void amountInCart()
    {
        var result = Cart.Items?.FirstOrDefault(item=>item.ProductID == ProductData.ID);
        AmountCart = (result == null) ? 0 : result.Amount;            

    }

    private void addCart_Click(object sender, RoutedEventArgs e)
    {
        bl.cart.AddProductToCart(Cart, ProductData.ID);
    }
}
