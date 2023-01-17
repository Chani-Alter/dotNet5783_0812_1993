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
    public ObservableCollection<OrderForList?> OrdersList
    {
        get { return (ObservableCollection<OrderForList?>)GetValue(ProductsProperty); }
        set { SetValue(ProductsProperty, value); }
    }

    public static readonly DependencyProperty ProductsProperty =
        DependencyProperty.Register("OrdersList", typeof(ObservableCollection<OrderForList?>), typeof(Window), new PropertyMetadata(null));

    public OrderList()
    {
        InitializeComponent();
        var temp = bl.Order.GetOrderList();
        OrdersList = (temp == null) ? new() : new(temp);

    }

    #endregion

    #region PRIVATE MEMBERS

    private BlApi.IBl? bl = BlApi.Factory.Get();

    private void orderListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        OrderForList order = ((OrderForList)orderListView.SelectedItem);
        int id = order.ID;
        new OrderWindow(id).Show();
    }
    #endregion

}
