using System.Windows;
using PL.Customer;
using PL.Manager;
using PL.Product;

namespace PL;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    /// <summary>
    /// A function for the click event for the show product button whoe open the product list window
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>

    private void orderTracking_Click(object sender, RoutedEventArgs e)
    {

    }

    private void newOrder_Click(object sender, RoutedEventArgs e) => new Catalog().Show();
  

    private void manager_Click(object sender, RoutedEventArgs e)=> new ManagerMenue().Show();
}
