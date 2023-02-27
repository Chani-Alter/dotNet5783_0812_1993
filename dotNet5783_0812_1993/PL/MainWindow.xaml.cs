using System.Windows;
using PL.Customer;
using PL.userLogin;

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

    private void orderTracking_Click(object sender, RoutedEventArgs e)=> new OrderTrackingWindow(this).Show();  
    

    private void newOrder_Click(object sender, RoutedEventArgs e) => new Catalog(this).Show();
  

    private void signin_Click(object sender, RoutedEventArgs e)=> new SignIn(this).Show();

    private void login_Click(object sender, RoutedEventArgs e) => new Login(this).Show();

}
