using BO;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PL.Customer;

/// <summary>
/// Interaction logic for Catalog.xaml
/// </summary>
public partial class Catalog : Window
{
    #region PUBLIC MEMBERS

    /// <summary>
    /// observer collection to the product list
    /// </summary>
    public ObservableCollection<ProductItem?> ProductsCatalog
    {
        get { return (ObservableCollection<ProductItem?>)GetValue(ProductsProperty); }
        set { SetValue(ProductsProperty, value); }
    }

    public static readonly DependencyProperty ProductsProperty =
        DependencyProperty.Register("ProductsCatalog", typeof(ObservableCollection<ProductItem?>), typeof(Window), new PropertyMetadata(null));

    /// <summary>
    /// the customer cart
    /// </summary>
    public BO.Cart? Cart;


    /// <summary>
    /// the prev window
    /// </summary>
    public Window prev_window;

    /// <summary>
    /// the category to filter the list acording to him
    /// </summary>
    public Category Category { get; set; } = Category.All;

    /// <summary>
    /// the catalog window ctor
    /// </summary>
    public Catalog(Window prev_window, User user = new User())
    {
        InitializeComponent();
        changeProductList();
        this.prev_window = prev_window;
        this.user = user;
        try
        {
            var temp = bl.Cart.GetUserCart(user.ID);
            Cart = (temp == null) ? new() : temp;
        }
        catch (DoesNotExistedBlException ex)
        {
            MessageBox.Show(ex.InnerException?.ToString(), ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    #endregion

    #region PRIVATE MEMBERS

    /// <summary>
    /// instance of the bl who contains access to all the bl implementation
    /// </summary>
    BlApi.IBl? bl = BlApi.Factory.Get();


    User user;


    /// <summary>
    /// hopen a window of a specific product
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void productListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        ProductItem product = (ProductItem)productListView.SelectedItem;
        new Product(product, this).Show();

    }

    /// <summary>
    /// change the category according to the user choise and caloing the changeProductList function
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void category_click(object sender, RoutedEventArgs e)
    {
        Button? button = sender as Button;
        string st = (string)button.Content;
        switch (st)
        {
            case "All":
                Category = Category.All;
                break;
            case "Screens":
                Category = Category.Screens;
                break;
            case "Laptop":
                Category = Category.Laptop;
                break;
            case "Desktop Computer":
                Category = Category.Desktop_Computer;
                break;
            case "Peripheral Equipment":
                Category = Category.Peripheral_Equipment;
                break;
            default:
                break;
        }
        changeProductList();
    }

    /// <summary>
    /// cales the GetcheapestProductListForCustomer from the bl and set the result to the productList
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void sales_click(object sender, RoutedEventArgs e)
    {
        var temp = bl.Product.GetcheapestProductListForCustomer();
        ProductsCatalog = (temp == null) ? new() : new(temp);
    }

    /// <summary>
    /// cales the GetPopularProductListForCustomer from the bl and set the result to the productList
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void Popular_click(object sender, RoutedEventArgs e)
    {
        var temp = bl.Product.GetPopularProductListForCustomer();
        ProductsCatalog = (temp == null) ? new() : new(temp);
    }

    /// <summary>
    /// open the cart window
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void cart_Click(object sender, RoutedEventArgs e)
    {
        new Cart(this).Show();
    }

    /// <summary>
    /// a function that change the product list according to the category
    /// </summary>
    void changeProductList()
    {
        var temp = bl.Product.GetProductListForCustomer(Category);
        ProductsCatalog = (temp == null) ? new() : new(temp);
    }

    /// <summary>
    /// make the prev window active and close this window
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void back_to_parent_Click(object sender, RoutedEventArgs e)
    {
        prev_window.Activate();
        Close();
    }

    #endregion
}