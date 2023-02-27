using BO;
using PL.userLogin;
using System;
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
    #region PUBLIC MEMBERS

    /// <summary>
    /// A variable that contains the logical cart
    /// </summary>
    public BO.Cart? MyCart;

    /// <summary>
    /// observer collection for the cart items
    /// </summary>
    public ObservableCollection<OrderItem?> CartItems
    {
        get { return (ObservableCollection<OrderItem?>)GetValue(CartItemsProperty); }
        set { SetValue(CartItemsProperty, value); }
    }

    public static readonly DependencyProperty CartItemsProperty =
        DependencyProperty.Register("CartItems", typeof(ObservableCollection<OrderItem?>), typeof(Window), new PropertyMetadata(null));

    /// <summary>
    /// a dependency property to the total price
    /// </summary>
    public double MyTotalPrice
    {
        get { return (double)GetValue(MyTotalPriceProperty); }
        set { SetValue(MyTotalPriceProperty, value); }
    }

    // Using a DependencyProperty as the backing store for MyTotalPrice.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty MyTotalPriceProperty =
        DependencyProperty.Register("MyTotalPrice", typeof(double), typeof(Window), new PropertyMetadata((double)0));

    /// <summary>
    /// the Cart window ctor
    /// </summary>
    /// <param name="catalog"></param>
    public Cart(Catalog catalog)
    {
        InitializeComponent();
        myCatalog = catalog;
        MyCart = catalog.Cart;
        MyTotalPrice=catalog.Cart.TotalPrice;
        CartItems = (MyCart.Items == null) ? new() : new(MyCart.Items!);
    }

    /// <summary>
    /// The function is called from the customer details control and creates the order
    /// </summary>
    /// <param name="name">customer name</param>
    /// <param name="email">customer email</param>
    /// <param name="adress">customer adress</param>
    internal void confirm_order(string name, string email, string adress)
    {
        try
        {
            MyCart.Items = CartItems.ToList();
            MyCart.CustomerName = name;
            MyCart.CustomerAdress = adress;
            MyCart.CustomerEmail = email;
            bl.cart.MakeOrder(MyCart);
            myCatalog.Close();
            Close();
        }
        catch (InvalidInputBlException ex)
        {
            MessageBox.Show("in valid input", ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
        }
        catch (ImpossibleActionBlException ex)
        {
            MessageBox.Show("The system cannot perform this operation", ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
        }
        catch (UpdateErrorBlException ex)
        {
            MessageBox.Show(ex.InnerException?.ToString(), ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
        }
        catch (BLAlreadyExistException ex)
        {
            MessageBox.Show(ex.InnerException?.ToString(), ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            Close();
        }


    }

    #endregion

    #region PRIVATE MEMBERS

    /// <summary>
    /// Holds the catalog window
    /// </summary>
    Catalog myCatalog;

    /// <summary>
    /// instance of the bl who contains access to all the bl implementation
    /// </summary>
    BlApi.IBl? bl = BlApi.Factory.Get();

    /// <summary>
    /// Increases the amount of product in the cart
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void pluse_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            OrderItem item = (OrderItem)((Button)sender).DataContext;
            int amount = item.Amount;
            amount += 1;
            bl.cart.UpdateProductAmountInCart(MyCart!, item.ProductID, amount);
            CartItems = (MyCart.Items == null) ? new() : new(MyCart.Items!);
            MyTotalPrice = MyCart.TotalPrice;
        }
        catch (ImpossibleActionBlException ex)
        {
            MessageBox.Show("Quantity not in stock", ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
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

    /// <summary>
    /// Reduces the amount of product in the cart
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void minuse_Click(object sender, RoutedEventArgs e)
    {
        try {
            OrderItem item = (OrderItem)((Button)sender).DataContext;
            int amount = item.Amount;
            amount -= 1;
            bl.cart.UpdateProductAmountInCart(MyCart!, item.ProductID, amount);
            CartItems = (MyCart.Items == null) ? new() : new(MyCart.Items!);
            MyTotalPrice = MyCart.TotalPrice;
        }
        catch (ImpossibleActionBlException ex)
        {
            MessageBox.Show("imposible Quantity", ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
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

    /// <summary>
    /// removes a product from the user cart
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void removeFromCart_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            int id = ((OrderItem)((Button)sender).DataContext).ProductID;
            bl.cart.UpdateProductAmountInCart(MyCart!, id, 0);
            CartItems = (MyCart.Items == null) ? new() : new(MyCart.Items!);
        }
        catch (ImpossibleActionBlException ex)
        {
            MessageBox.Show(ex.InnerException?.ToString(), ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
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

    /// <summary>
    /// A user details window opens to confirm the order
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void confirm_Click(object sender, RoutedEventArgs e) {  
        if(MyCart.ID==0)
           userDetails.Visibility = Visibility.Visible;
        else
            try
            {
                bl.cart.MakeOrder(MyCart!);
                if(myCatalog.prev_window is SignIn)
                    ((SignIn)myCatalog.prev_window).prev_window.Activate();
                else
                    ((Login)myCatalog.prev_window).prev_window.Activate();
                myCatalog.prev_window.Close();
                myCatalog.Close();
                Close();
            }
            catch (InvalidInputBlException ex)
            {
                MessageBox.Show("in valid input", ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (ImpossibleActionBlException ex)
            {
                MessageBox.Show("The system cannot perform this operation", ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (UpdateErrorBlException ex)
            {
                MessageBox.Show(ex.InnerException?.ToString(), ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (BLAlreadyExistException ex)
            {
                MessageBox.Show(ex.InnerException?.ToString(), ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }


    }

    /// <summary>
    /// Closes the order window and makes the catalog window active
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