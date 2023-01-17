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

namespace PL.Manager.Order;

/// <summary>
/// Interaction logic for OrderWindow.xaml
/// </summary>
public partial class OrderWindow : Window
{
    private BlApi.IBl? bl = BlApi.Factory.Get();

    public BO.Order OrderData
    {
        get { return (BO.Order)GetValue(OrderDataProperty); }
        set { SetValue(OrderDataProperty, value); }
    }

    // Using a DependencyProperty as the backing store for ProductData.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty OrderDataProperty =
        DependencyProperty.Register("OrderData", typeof(BO.Order), typeof(Window), new PropertyMetadata(null));

    public OrderWindow(int id = 0)
    {
        InitializeComponent();
        setOrderData(id);
    }

    private void shippingUpdate_Click(object sender, RoutedEventArgs e)
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

        catch (BO.ImpossibleActionBlException ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void deliveryUpdate_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            bl.Order.UpdateSupplyOrderByManager(OrderData.ID);
            setOrderData(OrderData.ID);
        }
        catch (BO.DoesNotExistedBlException ex)
        {
            MessageBox.Show(ex.InnerException?.ToString(), ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
            Close();
        }

        catch (BO.ImpossibleActionBlException ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

    }

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
}
