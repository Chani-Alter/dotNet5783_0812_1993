using BO;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace PL.Manager.Order;

/// <summary>
/// Interaction logic for OrderList.xaml
/// </summary>
public partial class OrderList : Window
{
    #region PUBLIC MEMBERS

    /// <summary>
    /// dependency property for the order list
    /// </summary>
    public ObservableCollection<OrderForList?> OrdersList
    {
        get { return (ObservableCollection<OrderForList?>)GetValue(ProductsProperty); }
        set { SetValue(ProductsProperty, value); }
    }

    public static readonly DependencyProperty ProductsProperty =
        DependencyProperty.Register("OrdersList", typeof(ObservableCollection<OrderForList?>), typeof(Window), new PropertyMetadata(null));

    /// <summary>
    /// the order list window ctor
    /// </summary>
    /// <param name="prev_window"></param>
    public OrderList(Window prev_window)
    {
        InitializeComponent();
        var temp = bl.Order.GetOrderList();
        OrdersList = (temp == null) ? new() : new(temp);
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
    /// open the order window
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void orderListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        OrderForList order = ((OrderForList)orderListView.SelectedItem);
        int id = order.ID;
        new OrderWindow( this , id).Show();
    }

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
