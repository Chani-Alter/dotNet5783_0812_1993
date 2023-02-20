using BO;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PL.Product;

/// <summary>
/// Interaction logic for ProductListWindow.xaml
/// </summary>
public partial class ProductListWindow : Window
{
    #region PUBLIC MEMBER 

    /// <summary>
    /// A dependency property who contains the product list
    /// </summary>
    public ObservableCollection<ProductForList?> ProductsList
    {
        get { return (ObservableCollection<ProductForList?>)GetValue(ProductsProperty); }
        set { SetValue(ProductsProperty, value); }
    }

    public static readonly DependencyProperty ProductsProperty =
        DependencyProperty.Register("ProductsList", typeof(ObservableCollection<ProductForList?>), typeof(Window), new PropertyMetadata(null));

    /// <summary>
    /// A property who contains the selected category
    /// </summary>
    public Category Category { get; set; } = Category.All;

    /// <summary>
    /// ctor who open the window
    /// </summary>
    public ProductListWindow()
    {
        InitializeComponent();
        changeProductList();
    }
    #endregion

    #region PRIVATE MEMBER

    /// <summary>
    /// instance of the bl who contains access to all the bl implementation
    /// </summary>
    BlApi.IBl? bl = BlApi.Factory.Get();


    /// <summary>
    /// A function for the SelectionChanged event for the categorySelector whoe shoes all the product whoe mach to the category
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void CategorySelector_SelectionChanged(object sender, SelectionChangedEventArgs e) => changeProductList();

    /// <summary>
    /// A function for the click event for the Add product btn whoe open the product window for adding
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void AddProductBtn_Click(object sender, RoutedEventArgs e)
    {
        new ProductWindow().Show();
    }

    /// <summary>
    /// A function for the MouseDoubleClick event for the productListView whoe open the product window for update
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void productListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        ProductForList product = (ProductForList)productListView.SelectedItem;
        new ProductWindow(product.ID).Show();

    }

    /// <summary>
    /// A function for the activated event for the window whoe calles the changeProductList function
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Window_Activated(object sender, EventArgs e) => changeProductList();

    /// <summary>
    /// A private function whoe change the product list incording to the category selector
    /// </summary>
    private void changeProductList()
    {
        var temp = Category == Category.All ?
           bl.Product.GetAllProductListForManager()
          : bl.Product.GetProductListForManagerByCategory(Category);
        ProductsList = (temp == null) ? new() : new(temp);
    }
    #endregion
}
