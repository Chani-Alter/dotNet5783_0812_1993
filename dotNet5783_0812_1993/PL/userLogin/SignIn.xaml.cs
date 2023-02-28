using BO;
using PL.Customer;
using System.Windows;

namespace PL.userLogin;

/// <summary>
/// Interaction logic for SignIn.xaml
/// </summary>
public partial class SignIn : Window
{
    #region PUBLIC MEMBERS

    /// <summary>
    /// a dependency property for the user
    /// </summary>
    public BO.User MyUser
    {
        get { return (BO.User)GetValue(MyUserProperty); }
        set { SetValue(MyUserProperty, value); }
    }

    // Using a DependencyProperty as the backing store for MyUser.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty MyUserProperty =
        DependencyProperty.Register("MyUser", typeof(BO.User), typeof(Window), new PropertyMetadata(null));

    /// <summary>
    /// the window ctor
    /// </summary>
    /// <param name="prev_window"></param>
    public SignIn(Window prev_window)
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
    /// THE FUNCTION FOR CONFIRM THE SIGN IN
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void confirm_Click(object sender, RoutedEventArgs e)
    {
        try {
            BO.User user = MyUser;
            int id = bl.User.AddUser(user);
            user.ID = id;
            new Catalog(this, user).Show();
        }catch(DoesNotExistedBlException ex)
        {
            MessageBox.Show(ex.InnerException?.ToString(), ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
        }
        catch (BLAlreadyExistException ex) 
        {
            MessageBox.Show(ex.InnerException?.ToString(), ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    #endregion
}

