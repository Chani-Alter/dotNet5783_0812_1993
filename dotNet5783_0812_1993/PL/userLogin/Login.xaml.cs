using BO;
using PL.Customer;
using PL.Manager;
using System.Windows;

namespace PL.userLogin;

/// <summary>
/// Interaction logic for Login.xaml
/// </summary>
public partial class Login : Window
{
    #region PUBLIC MEMBERS

    /// <summary>
    /// dependency property for the customer email
    /// </summary>
    public string CustomerEmail
    {
        get { return (string)GetValue(CustomerEmailProperty); }
        set { SetValue(CustomerEmailProperty, value); }
    }

    public static readonly DependencyProperty CustomerEmailProperty =
        DependencyProperty.Register("CustomerEmail", typeof(string), typeof(Window), new PropertyMetadata(""));

    /// <summary>
    /// dependency property for the password 
    /// </summary>
    public string Password
    {
        get { return (string)GetValue(PasswordProperty); }
        set { SetValue(PasswordProperty, value); }
    }

    public static readonly DependencyProperty PasswordProperty =
        DependencyProperty.Register("Password", typeof(string), typeof(Window), new PropertyMetadata(""));

    /// <summary>
    /// the login window ctor
    /// </summary>
    /// <param name="prev_window"></param>
    public Login(Window prev_window)
    {
        InitializeComponent();
        this.prev_window = prev_window;
    }

    /// <summary>
    /// the prev window
    /// </summary>
    public Window prev_window;

    #endregion

    #region PRIVATE MEMBERS

    /// <summary>
    /// instance of the bl who contains access to all the bl implementation
    /// </summary>
    BlApi.IBl? bl = BlApi.Factory.Get();

    /// <summary>
    /// confirm the log in
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void logIn_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            BO.User user = bl.User.GetUserByEmailAndPass(CustomerEmail, Password);
            if (user.IsAdmin)
            {
                new ManagerMenue().Show();
                Close();
            }
            else
                new Catalog(this, user).Show();
        }
        catch (DoesNotExistedBlException)
        {
            MessageBox.Show("email or password doesnt exist", "try again", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    /// <summary>
    /// past the user to the sign up window
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void signUp_Click(object sender, RoutedEventArgs e)
    {
        new SignIn(prev_window).Show();
        Close();
    }

    #endregion
}
