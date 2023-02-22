using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BO;
using PL.Manager.Order;

namespace PL.Customer;

/// <summary>
/// Interaction logic for OrderTrackingWindow.xaml
/// </summary>
public partial class OrderTrackingWindow : Window
{
    #region PUBLIC MEMBERS

    /// <summary>
    /// dependency property that get the order id from the textt box
    /// </summary>
    public int IdTextBox
    {
        get { return (int)GetValue(IdTextBoxProperty); }
        set { SetValue(IdTextBoxProperty, value); }
    }

    public static readonly DependencyProperty IdTextBoxProperty =
        DependencyProperty.Register("IdTextBox", typeof(int), typeof(Window), new PropertyMetadata(0));

    /// <summary>
    /// dependency property for the order tracking data
    /// </summary>
    public BO.OrderTracking? OrderTrackingData
    {
        get { return (BO.OrderTracking?)GetValue(OrderTrackingDataProperty); }
        set { SetValue(OrderTrackingDataProperty, value); }
    }

    public static readonly DependencyProperty OrderTrackingDataProperty =
        DependencyProperty.Register("OrderTrackingData", typeof(BO.OrderTracking), typeof(Window), new PropertyMetadata(null));

    /// <summary>
    /// the order tracking window ctor
    /// </summary>
    /// <param name="prev_window"></param>
    public OrderTrackingWindow(Window prev_window)
    {
        InitializeComponent();
        OrderTrackingData = new BO.OrderTracking();
        this.prev_window = prev_window;
    }

    #endregion

    #region PRIVATE MEMBERS

    /// <summary>
    /// instance of the bl who contains access to all the bl implementation
    /// </summary>
    BlApi.IBl? bl = BlApi.Factory.Get();

    /// <summary>
    /// the prev window
    /// </summary>
    Window prev_window;

    /// <summary>
    /// the function get the id and bringes the order tracking of this order
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <exception cref="Exception"></exception>
    void confirmBtn_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var temp = bl.Order.TrackingOrder(IdTextBox);
            OrderTrackingData = (temp != null) ? temp : throw new Exception("system error, Try again");
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
    /// open the order window
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void order_details_Click(object sender, RoutedEventArgs e) => new OrderWindow(this, IdTextBox).Show();

    /// <summary>
    /// reset the window
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void clean_choice_Click(object sender, RoutedEventArgs e) => OrderTrackingData = new();

    /// <summary>
    /// makes the prev window active and close this window
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void back_to_menu_Click(object sender, RoutedEventArgs e)
    {
        prev_window.Activate();
        Close();
    }

    /// <summary>
    /// chakes that only numbers are writing in the input
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void IdTextBox_PreviewKeyDown(object sender, KeyEventArgs e) => onlyNumber(sender, e);

    /// <summary>
    /// a function that check that only numbers are writing into the input
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void onlyNumber(object sender, KeyEventArgs e)
    {
        TextBox? text = sender as TextBox;
        if (text == null) return;
        if (e == null) return;

        //allow get out of the text box
        if (e.Key == Key.Enter || e.Key == Key.Return || e.Key == Key.Tab)
            return;

        //allow list of system keys (add other key here if you want to allow)
        if (e.Key == Key.Escape || e.Key == Key.Back || e.Key == Key.Delete ||
        e.Key == Key.CapsLock || e.Key == Key.LeftShift || e.Key == Key.Home
        || e.Key == Key.End || e.Key == Key.Insert || e.Key == Key.Down || e.Key == Key.Right
        || e.Key == Key.NumPad0 || e.Key == Key.NumPad1 || e.Key == Key.NumPad2 || e.Key == Key.NumPad3 || e.Key == Key.NumPad4
        || e.Key == Key.NumPad5 || e.Key == Key.NumPad6 || e.Key == Key.NumPad7 || e.Key == Key.NumPad8 || e.Key == Key.NumPad9)
            return;

        char c = (char)KeyInterop.VirtualKeyFromKey(e.Key);

        //allow control system keys
        if (Char.IsControl(c)) return;

        //allow digits (without Shift or Alt)
        if (Char.IsDigit(c))
            if (!(Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightAlt)))
                return; //let this key be written inside the textbox
                        //forbid letters and signs (#,$, %, ...)
        e.Handled = true; //ignore this key. mark event as handled, will not be routed to other 
        return;
    }

    #endregion
}
