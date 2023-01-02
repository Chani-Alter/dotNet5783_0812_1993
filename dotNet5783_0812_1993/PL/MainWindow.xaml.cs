using System.Windows;
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
    private void showProductBtn_Click(object sender, RoutedEventArgs e)
    {
        new ProductListWindow().Show();
        Close();
    }
}
