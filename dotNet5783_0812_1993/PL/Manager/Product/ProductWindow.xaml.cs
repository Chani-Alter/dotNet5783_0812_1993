using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PL.Product;

/// <summary>
/// Interaction logic for ProductWindow.xaml
/// </summary>
public partial class ProductWindow : Window
{
    #region PUBLIC MEMBER

    /// <summary>
    /// A dependency property who contains all the product data
    /// </summary>
    public BO.Product ProductData
    {
        get { return (BO.Product)GetValue(ProductDataProperty); }
        set { SetValue(ProductDataProperty, value); }
    }

    public static readonly DependencyProperty ProductDataProperty =
        DependencyProperty.Register("ProductData", typeof(BO.Product), typeof(Window), new PropertyMetadata(null));

    /// <summary>
    /// an one parameter ctor whoe open the window
    /// </summary>
    /// <param name="id"></param>
    public ProductWindow(int id = 0)
    {
        InitializeComponent();
        try
        {
            var temp = id == 0 ? new BO.Product() : bl.Product.GetProductByIdManager(id);
            ProductData = (temp == null) ? new() : temp;
        }
        catch (BO.DoesNotExistedBlException ex)
        {
            MessageBox.Show(ex.InnerException?.ToString(), ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    #endregion

    #region PRIVATE MEMBER

    /// <summary>
    /// instance of the bl who contains access to all the bl implementation
    /// </summary>
    private BlApi.IBl? bl = BlApi.Factory.Get();

    /// <summary>
    /// A function that confirm the updatimg of the product in the dl
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void updateBtn_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            bl.Product.UpdateProduct(ProductData);
            Close();
        }
        catch (BO.DoesNotExistedBlException ex)
        {
            MessageBox.Show(ex.InnerException?.ToString(), ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
            Close();
        }

        catch (BO.ImpossibleActionBlException ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    /// <summary>
    /// A function that add the new product in the dl
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void addBtn_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            bl.Product.AddProduct(ProductData);
            Close();
        }
        catch (BO.BLAlreadyExistException ex)
        {
            MessageBox.Show(ex.InnerException?.ToString(), ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
            Close();

        }
        catch (BO.InvalidInputBlException ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            Close();
        }
    }

    /// <summary>
    /// A function for the click event for the delete btn whoe delete the product from the dl 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void delete_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            bl.Product.DeleteProduct(ProductData.ID);
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
    /// A function for the PreviewKeyDown event for the idTextBox whoe cales the onlyNumbers Check
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void idTextBox_PreviewKeyDown(object sender, KeyEventArgs e) => onlyNumber(sender, e);

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
    /// a function that check that only numbers are writing into the input
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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
    #endregion
}
