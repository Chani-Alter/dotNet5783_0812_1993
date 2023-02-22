using System.Windows;
using System.Windows.Controls;
namespace PL.Customer;

/// <summary>
/// Interaction logic for UserDetails.xaml
/// </summary>
public partial class UserDetails : UserControl
{
    #region PUBLIC MEMBERS

    /// <summary>
    /// cstomer name dependency property
    /// </summary>
    public string CustomerName
    {
        get { return (string)GetValue(CustomerNameProperty); }
        set { SetValue(CustomerNameProperty, value); }
    }

    // Using a DependencyProperty as the backing store for CustomerName.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty CustomerNameProperty =
        DependencyProperty.Register("CustomerName", typeof(string), typeof(Window), new PropertyMetadata(""));

    /// <summary>
    /// cstomer email dependency property
    /// </summary>
    public string Email
    {
        get { return (string)GetValue(EmailProperty); }
        set { SetValue(EmailProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Email.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty EmailProperty =
        DependencyProperty.Register("Email", typeof(string), typeof(Window), new PropertyMetadata(""));

    /// <summary>
    /// cstomer Adress dependency property
    /// </summary>
    public string Adress
    {
        get { return (string)GetValue(AdressProperty); }
        set { SetValue(AdressProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Adress.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty AdressProperty =
        DependencyProperty.Register("Adress", typeof(string), typeof(Window), new PropertyMetadata(""));

    /// <summary>
    /// the user detailes control window
    /// </summary>
    public UserDetails()
    {
        InitializeComponent();
    }

    #endregion

    #region PRIVATE MEMBERS

    /// <summary>
    /// save the user detailes and caled tothe function from the cart to make order
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void saveBtn_Click(object sender, RoutedEventArgs e)
    {
        Cart parentWindow = (Cart)Window.GetWindow(this);
        parentWindow.confirm_order(CustomerName, Email, Adress);
    }

    #endregion
}
