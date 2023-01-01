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

    private void AddProductBtn_Click(object sender, RoutedEventArgs e)
    {
        new ProductWindow().Show();
        Close();
    }
     
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
