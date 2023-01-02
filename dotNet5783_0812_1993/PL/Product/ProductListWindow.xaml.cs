using BlApi;
using BlImplementation;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PL.Product;


/// <summary>
/// Interaction logic for ProductListWindow.xaml
/// </summary>
public partial class ProductListWindow : Window
{

    private IBl bl = new Bl();

    public ProductListWindow()
    {
        InitializeComponent();

        productListView.ItemsSource = bl.Product.GetAllProductListForManager();

        categorySelector.ItemsSource = Enum.GetValues(typeof(BO.Category));

        categorySelector.SelectedValue = BO.Category.All;
    }

    /// <summary>
    /// A function for the SelectionChanged event for the categorySelector whoe shoes all the product whoe mach to the category
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void categorySelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        BO.Category? category = categorySelector.SelectedItem as BO.Category?;

        productListView.ItemsSource = null;

        if (category == BO.Category.All)
        {
            productListView.ItemsSource = bl.Product.GetAllProductListForManager();

        }
        else
        {
            productListView.ItemsSource = bl.Product.GetProductListForManagerByCategory(category);

        }
    }

    /// <summary>
    /// A function for the click event for the Add product btn whoe open the product window for adding
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void AddProductBtn_Click(object sender, RoutedEventArgs e)
    {
        new ProductWindow().Show();
        Close();
    }

    /// <summary>
    /// A function for the MouseDoubleClick event for the productListView whoe open the product window for update
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void productListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        BO.ProductForList product = (BO.ProductForList)productListView.SelectedItem;

        int helpInt = product.ID;
        new ProductWindow(helpInt).Show();
        Close();

    }

    private void productListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

    }
}
