using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PL.Product;

/// <summary>
/// Interaction logic for ProductWindow.xaml
/// </summary>
public partial class ProductWindow : Window
{

    /// <summary>
    /// instance of the bl who contains access to all the bl implementation
    /// </summary>
    private BlApi.IBl? bl = BlApi.Factory.Get();

    /// <summary>
    /// the empty ctor who opens the window for adding aproduct
    /// </summary>
    public ProductWindow()
    {
        InitializeComponent();
        var items = Enum.GetNames(typeof(BO.Category)).Where(item => item != "None");
        categoryComboBox.ItemsSource = items;
        confirmBtn.Content = "Add";
    }

    /// <summary>
    /// an one parameter ctor whoe open the window for updateing the product 
    /// </summary>
    /// <param name="id"></param>
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

    /// <summary>
    /// A function for the click event for the confirmBtn whoe add or update the product
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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
        Enum.TryParse((string)categoryComboBox.SelectedItem, out BO.Category category);
        product.Category = category;
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
            catch (BO.UpdateErrorBlException ex)
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

    /// <summary>
    /// A function for the click event for the delete whoe delete the product 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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
        catch (BO.DoesNotExistedBlException ex)
        {
            MessageBox.Show(ex.InnerException?.ToString(), ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
            new ProductListWindow().Show();
            Close();
        }

        catch (BO.ImpossibleActionBlException ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            new ProductListWindow().Show();
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

    }

    /// <summary>
    /// A function for the TextChanged event for the idTextBox whoe check if the id is fill
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void idTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (idTextBox.Text.Length > 0)
            confirmIsEnabledCheck();
        else
            confirmBtn.IsEnabled = false;
    }

    /// <summary>
    /// A function for the TextChanged event for the nameTextBox whoe check if the name is fill
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void nameTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (nameTextBox.Text.Length > 0)
            confirmIsEnabledCheck();
        else
            confirmBtn.IsEnabled = false;
    }

    /// <summary>
    /// A function for the TextChanged event for the priceTextBox whoe check if the price is fill
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void priceTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (priceTextBox.Text.Length > 0)
            confirmIsEnabledCheck();
        else
            confirmBtn.IsEnabled = false;
    }

    /// <summary>
    /// A function for the SelectionChanged event for the categoryComboBox whoe check if the category is select
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void categoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (categoryComboBox.SelectedItem != null)
            confirmIsEnabledCheck();
        else
            confirmBtn.IsEnabled = false;
    }

    /// <summary>
    /// A function for the TextChanged event for the inStockTextBox whoe check if the inStock is fill
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void inStockTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (inStockTextBox.Text.Length > 0)
            confirmIsEnabledCheck();
        else
            confirmBtn.IsEnabled = false;
    }

    /// <summary>
    /// A function for the PreviewKeyDown event for the idTextBox whoe cales the onlyNumbers Check
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void idTextBox_PreviewKeyDown(object sender, KeyEventArgs e) => onlyNumber(sender , e);

    /// <summary>
    /// A function for the PreviewKeyDown event for the inStockTextBox whoe cales the onlyNumbers Check
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void inStockTextBox_PreviewKeyDown(object sender, KeyEventArgs e) => onlyNumber(sender, e);

    /// <summary>
    /// A function for the PreviewKeyDown event for the priceTextBox whoe cales the onlyNumbers Check and allowed the dot be also
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void priceTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        char c = (char)KeyInterop.VirtualKeyFromKey(e.Key);
        if (c == 190)
            return;
        onlyNumber(sender, e);
    }

    /// <summary>
    /// A function thet check if all the text box are filling and controles the Enabled off the confirm button
    /// </summary>
    private void confirmIsEnabledCheck()
    {
        if (idTextBox.Text.Length == 6 && nameTextBox.Text.Length > 0 && categoryComboBox.SelectedItem != null
            && priceTextBox.Text.Length > 0 && inStockTextBox.Text.Length > 0)
            confirmBtn.IsEnabled = true;
        else
            confirmBtn.IsEnabled = false;
    }

    /// <summary>
    /// a function that check that only numbers are writing into the input
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void onlyNumber(object sender, KeyEventArgs e)
    {

        var textBox = sender as TextBox;
        e.Handled = Regex.IsMatch(textBox.Text, "(?<=^| )\\d+(\\.\\d+)?(?=$| )");
    }
}
