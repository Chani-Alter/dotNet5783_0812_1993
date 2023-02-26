using PL.Manager.Order;
using PL.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;

namespace PL.Manager;

/// <summary>
/// Interaction logic for ManagerMenue.xaml
/// </summary>
public partial class ManagerMenue : Window
{
    public ManagerMenue()
    {
        InitializeComponent();
    }

    private void showOrdersBtn_Click(object sender, RoutedEventArgs e) => new OrderList(this).Show();

    private void showProductBtn_Click(object sender, RoutedEventArgs e) => new ProductListWindow(this).Show();

    private void startSimulatorBtn_Click(object sender, RoutedEventArgs e) => new SimulatorWindow().Show();
}
