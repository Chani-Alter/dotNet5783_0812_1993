using BO;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.Intrinsics.Arm;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PL.Product;


/// <summary>
/// Interaction logic for ProductListWindow.xaml
/// </summary>
public partial class ProductListWindow : Window
{

    private BlApi.IBl? bl = BlApi.Factory.Get();

    public ObservableCollection<ProductForList?> ProductsList
    {
        get { return (ObservableCollection<ProductForList?>)GetValue(ProductsProperty); }
        set { SetValue(ProductsProperty , value); }
    }

    public static readonly DependencyProperty ProductsProperty =
        DependencyProperty.Register("ProductsList", typeof(ObservableCollection<ProductForList?>), typeof(Window), new PropertyMetadata(null));

    public Category Category { get; set; } = Category.All;

    public ProductListWindow()
    {
        InitializeComponent();
        var temp = bl.Product.GetAllProductListForManager();

        ProductsList = (temp == null) ? new() : new(temp);
    }

    /// <summary>
    /// A function for the SelectionChanged event for the categorySelector whoe shoes all the product whoe mach to the category
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void CategorySelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        Category? category = categorySelector.SelectedItem as Category?;


        if (category == BO.Category.All)
        {
            var temp = bl.Product.GetAllProductListForManager();
            ProductsList = (temp == null) ? new() : new(temp);
        }
        else
        {
            var temp = bl.Product.GetProductListForManagerByCategory(category);
            ProductsList = (temp == null) ? new() : new(temp);


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
        ProductForList product = (ProductForList)productListView.SelectedItem;

        int helpInt = product.ID;
        new ProductWindow(helpInt).Show();
        Close();

    }

    private void productListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

    }
}
