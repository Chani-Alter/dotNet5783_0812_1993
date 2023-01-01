using BlApi;
using BlImplementation;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PL.Product;

/// <summary>
/// Interaction logic for ProductWindow.xaml
/// </summary>
public partial class ProductWindow : Window
{
    private IBl bl = new Bl();

    public ProductWindow()
    {
        InitializeComponent();
        var items = Enum.GetNames(typeof(BO.Category)).Where(item => item != "None");
        categoryComboBox.ItemsSource = items;
        confirmBtn.Content = "Add";
    }

    public ProductWindow(int id)
    {
        try
        {
            InitializeComponent();
            var items = Enum.GetNames(typeof(BO.Category)).Where(item => item != "None");
            categoryComboBox.ItemsSource = items;
            delete.Visibility = Visibility.Visible;
            confirmBtn.Content = "Update";
            BO.Product product = new BO.Product();
            try
            {
                product = bl.Product.GetProductByIdManager(id);
               
            }
            catch (BO.DoesNotExistedBlException ex)
            {
                MessageBox.Show(ex.InnerException?.ToString(), ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            idTextBox.Text = product.ID.ToString();
            nameTextBox.Text = product.Name;
            priceTextBox.Text = product.Price.ToString();
            categoryComboBox.SelectedValue = product.Category.ToString();
            inStockTextBox.Text = product.InStock.ToString();

            idTextBox.IsEnabled = false;
            nameTextBox.IsEnabled = false;
            categoryComboBox.IsEnabled = false;
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void confirmBtn_Click(object sender, RoutedEventArgs e)
    {
        BO.Product product = new BO.Product();

        int helpInt;
        double helpDouble;

        int.TryParse(idTextBox.Text, out helpInt);
        product.ID = helpInt;
        product.Name = nameTextBox.Text;
        double.TryParse(priceTextBox.Text, out helpDouble);
        product.Price = helpDouble;
        product.Category = ((BO.Category)categoryComboBox.SelectedItem);
        int.TryParse(inStockTextBox.Text, out helpInt);
        product.InStock = helpInt;

        if (confirmBtn.Content.ToString() == "Add")
        {
            try
            {
                bl.Product.AddProduct(product);
                new ProductListWindow().Show();
                Close();
            }
            catch (BO.BLAlreadyExistException ex)
            {
                MessageBox.Show(ex.InnerException?.ToString(), ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                new ProductListWindow().Show();
                Close();

            }
            catch (BO.InvalidInputBlException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                new ProductListWindow().Show();
            }
        }
        else if (confirmBtn.Content.ToString() == "Update")
        {
            try
            {
                bl.Product.UpdateProduct(product);
                new ProductListWindow().Show();
                Close();
            }
            catch (BO.BLAlreadyExistException ex)
            {
                MessageBox.Show(ex.InnerException?.ToString(), ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (BO.InvalidInputBlException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }


    private void delete_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            int id;
            int.TryParse(idTextBox.Text, out id);
            bl.Product.DeleteProduct(id);
            new ProductListWindow().Show();
            Close();

        }
        catch (BO.ImpossibleActionBlException ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            ProductListWindow productListWindow = new ProductListWindow();
            productListWindow.Show();
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

    }

    private void idTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (idTextBox.Text.Length > 0)
            confirmIsEnabledCheck();
        else
            confirmBtn.IsEnabled = false;
    }

    private void nameTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (nameTextBox.Text.Length > 0)
            confirmIsEnabledCheck();
        else
            confirmBtn.IsEnabled = false;
    }

    private void priceTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (priceTextBox.Text.Length > 0)
            confirmIsEnabledCheck();
        else
            confirmBtn.IsEnabled = false;
    }

    private void categoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (categoryComboBox.SelectedItem != null)
            confirmIsEnabledCheck();
        else
            confirmBtn.IsEnabled = false;
    }

    private void inStockTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (inStockTextBox.Text.Length > 0)
            confirmIsEnabledCheck();
        else
            confirmBtn.IsEnabled = false;
    }

    private void idTextBox_PreviewKeyDown(object sender, KeyEventArgs e) => onlyNumber(sender , e);
    
    private void inStockTextBox_PreviewKeyDown(object sender, KeyEventArgs e) => onlyNumber(sender, e);

    private void priceTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
    {

        onlyNumber(sender, e);
    }

    private void confirmIsEnabledCheck()
    {
        if (idTextBox.Text.Length == 6 && nameTextBox.Text.Length > 0 && categoryComboBox.SelectedItem != null
            && priceTextBox.Text.Length > 0 && inStockTextBox.Text.Length > 0)
            confirmBtn.IsEnabled = true;
        else
            confirmBtn.IsEnabled = false;
    }

    private void onlyNumber(object sender, KeyEventArgs e)
    {
        TextBox? text = sender as TextBox;
        if (text == null) return;
        if (e == null) return;

        //allow get out of the text box
        if (e.Key == Key.Enter || e.Key == Key.Return || e.Key == Key.Tab)
            return;

        //allow list of system keys (add other key here if you want to allow)
        if (e.Key == Key.Escape || e.Key == Key.Back || e.Key == Key.Delete ||
        e.Key == Key.CapsLock || e.Key == Key.LeftShift || e.Key == Key.Home
        || e.Key == Key.End || e.Key == Key.Insert || e.Key == Key.Down || e.Key == Key.Right
        || e.Key == Key.NumPad0 || e.Key == Key.NumPad1 || e.Key == Key.NumPad2 || e.Key == Key.NumPad3 || e.Key == Key.NumPad4 
        || e.Key == Key.NumPad5 || e.Key == Key.NumPad6 || e.Key == Key.NumPad7 || e.Key == Key.NumPad8 || e.Key == Key.NumPad9)
            return;

        char c = (char)KeyInterop.VirtualKeyFromKey(e.Key);

        //allow control system keys
        if (Char.IsControl(c)) return;

        //allow digits (without Shift or Alt)
        if (Char.IsDigit(c))
            if (!(Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightAlt)))
                return; //let this key be written inside the textbox
                        //forbid letters and signs (#,$, %, ...)
        e.Handled = true; //ignore this key. mark event as handled, will not be routed to other 
        return;
    }
}
