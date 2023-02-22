using BO;
using System;
using System.Windows;
using System.Windows.Controls;

namespace PL.Manager.Order;

/// <summary>
/// Interaction logic for OrderWindow.xaml
/// </summary>
public partial class OrderWindow : Window
{
    #region PUBLIC MEMBERS

    /// <summary>
    /// the status of the window: user=0 ,manager =1, update=2
    /// </summary>
    public int StatusPage
    {
        get { return (int)GetValue(StatusPageProperty); }
        set { SetValue(StatusPageProperty, value); }
    }

    public static readonly DependencyProperty StatusPageProperty =
        DependencyProperty.Register("StatusPage", typeof(int), typeof(Window), new PropertyMetadata(0));

    /// <summary>
    /// all the order data
    /// </summary>
    public BO.Order OrderData
    {
        get { return (BO.Order)GetValue(OrderDataProperty); }
        set { SetValue(OrderDataProperty, value); }
    }

    public static readonly DependencyProperty OrderDataProperty =
        DependencyProperty.Register("OrderData", typeof(BO.Order), typeof(Window), new PropertyMetadata(null));

    /// <summary>
    /// the order window ctor
    /// </summary>
    /// <param name="prev_window"></param>
    /// <param name="id"></param>
    public OrderWindow(Window prev_window, int id = 0)
    {
        InitializeComponent();
        setOrderData(id);
        OrderList? wind = prev_window as OrderList;
        if (wind != null)
            StatusPage = 1;
        this.prev_window = prev_window;
    }

    #endregion

    #region PRIVATE MEMBERS

    /// <summary>
    /// instance of the bl who contains access to all the bl implementation
    /// </summary>
    BlApi.IBl? bl = BlApi.Factory.Get();

    /// <summary>
    /// the parent window
    /// </summary>
    Window prev_window;

    /// <summary>
    /// update the shipping date to be now
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void shippingUpdate_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            bl.Order.UpdateSendOrderByManager(OrderData.ID);
            setOrderData(OrderData.ID);
        }
        catch (BO.DoesNotExistedBlException ex)
        {
            MessageBox.Show(ex.InnerException?.ToString(), ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
            Close();
        }

        catch (BO.UpdateErrorBlException ex)
        {
            MessageBox.Show(ex.InnerException?.ToString(), ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    /// <summary>
    /// update the delivery date  to be now
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void deliveryUpdate_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            bl.Order.UpdateSupplyOrderByManager(OrderData.ID);
            setOrderData(OrderData.ID);
        }
        catch (DoesNotExistedBlException ex)
        {
            MessageBox.Show(ex.InnerException?.ToString(), ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
            Close();
        }
        catch (UpdateErrorBlException ex)
        {
            MessageBox.Show(ex.InnerException?.ToString(), ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
            Close();
        }
        catch (ImpossibleActionBlException ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

    }

    /// <summary>
    /// set the order data according to the id
    /// </summary>
    /// <param name="id"></param>
    private void setOrderData(int id)
    {
        try
        {
            var temp = bl.Order.GetOrderById(id);
            OrderData = (temp != null) ? temp : throw new Exception("דystem error, Try again");
        }
        catch (BO.DoesNotExistedBlException ex)
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
            var temp = bl.Order.UpdateAmountOfOProductInOrder(OrderData.ID, item.ProductID, amount);
            OrderData = (temp != null) ? temp : throw new Exception("system error, Try again");
        }
        catch (ImpossibleActionBlException ex)
        {
            MessageBox.Show("Quantity not in stock", ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
        }
        catch (DoesNotExistedBlException ex)
        {
            MessageBox.Show(ex.InnerException?.ToString(), ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
        }
        catch (UpdateErrorBlException ex)
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
        try
        {
            OrderItem item = (OrderItem)((Button)sender).DataContext;
            int amount = item.Amount;
            amount -= 1;
            var temp = bl.Order.UpdateAmountOfOProductInOrder(OrderData.ID, item.ProductID, amount);
            OrderData = (temp != null) ? temp : throw new Exception("system error, Try again");
        }
        catch (ImpossibleActionBlException ex)
        {
            MessageBox.Show("imposible Quantity", ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
        }
        catch (DoesNotExistedBlException ex)
        {
            MessageBox.Show(ex.InnerException?.ToString(), ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
        }
        catch (UpdateErrorBlException ex)
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
    /// update the page status to be update
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void update_Status_Click(object sender, RoutedEventArgs e) => StatusPage = 2;

    /// <summary>
    /// make the prev window active and close this window
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void back_to_parent_Click(object sender, RoutedEventArgs e)
    {
        prev_window.Activate();
        Close();
    }

    #endregion

}