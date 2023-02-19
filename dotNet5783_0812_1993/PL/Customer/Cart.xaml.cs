using BO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PL.Customer;

/// <summary>
/// Interaction logic for Cart.xaml
/// </summary>
public partial class Cart : Window
{
    BO.Cart myCart;
    private BlApi.IBl? bl = BlApi.Factory.Get();
    public int TotalPrice
    {
        get { return (int)GetValue(TotalPriceProperty); }
        set { SetValue(TotalPriceProperty, value); }
    }

    // Using a DependencyProperty as the backing store for ProductData.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty TotalPriceProperty =
        DependencyProperty.Register("TotalPrice", typeof(int, typeof(Window), new PropertyMetadata(0));



    public ObservableCollection<OrderItem?> CartItems
    {
        get { return (ObservableCollection<OrderItem?>)GetValue(CartItemsProperty); }
        set { SetValue(CartItemsProperty, value); }
    }

    public static readonly DependencyProperty CartItemsProperty =
        DependencyProperty.Register("CartItems", typeof(ObservableCollection<OrderItem?>), typeof(Window), new PropertyMetadata(null));

    public Cart(BO.Cart cart)
    {
        InitializeComponent();
        myCart = cart;
        IEnumerable<OrderItem?>? temp = myCart.Items;
        CartItems = (temp == null) ? new() : new(temp!);

    }

    private void pluse_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            OrderItem item = (OrderItem)((Button)sender).DataContext;
            BO.Product bbl = bl.Product.GetProductByIdManager(item.ProductID);
            int amountInstock = bbl.InStock;
            if (amountInstock > item.Amount)
            {
                var temp = CartItems;
                var index = temp.IndexOf(item);
                item.Amount += 1;
                if (index != -1)
                    temp[index] = item;
                CartItems = (temp == null) ? new() : new(temp!);
            }
            else
            {
                MessageBox.Show("Quantity not in stock", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        catch (DoesNotExistedBlException ex)
        {
            MessageBox.Show(ex.InnerException?.ToString(), ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            Close();
        }

    }

    private void minuse_Click(object sender, RoutedEventArgs e)
    {
        var temp = CartItems;
        OrderItem item = (OrderItem)((Button)sender).DataContext;
        var index = temp.IndexOf(item);
        if (item.Amount > 1)
            item.Amount -= 1;
        if (index != -1)
            temp[index] = item;
        CartItems = (temp == null) ? new() : new(temp!);
    }

    private void removeFromCart_Click(object sender, RoutedEventArgs e)
    {
       
            int id = ((OrderItem)((Button)sender).DataContext).ProductID;
            var temp = CartItems.Where(item => item.ProductID != id);
            CartItems = (temp == null) ? new() : new(temp!);

    }

}