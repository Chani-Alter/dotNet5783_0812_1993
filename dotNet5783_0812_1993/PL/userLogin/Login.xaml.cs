using BO;
using PL.Customer;
using PL.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PL.userLogin;

/// <summary>
/// Interaction logic for Login.xaml
/// </summary>
public partial class Login : Window
{


    public string CustomerEmail
    {
        get { return (string)GetValue(CustomerEmailProperty); }
        set { SetValue(CustomerEmailProperty, value); }
    }

    // Using a DependencyProperty as the backing store for CustomerEmail.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty CustomerEmailProperty =
        DependencyProperty.Register("CustomerEmail", typeof(string), typeof(Window), new PropertyMetadata(""));



    public string Password
    {
        get { return (string)GetValue(PasswordProperty); }
        set { SetValue(PasswordProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Password.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty PasswordProperty =
        DependencyProperty.Register("Password", typeof(string), typeof(Window), new PropertyMetadata(""));



    public Window prev_window;

    /// <summary>
    /// instance of the bl who contains access to all the bl implementation
    /// </summary>
    BlApi.IBl? bl = BlApi.Factory.Get();

    public Login(Window prev_window)
    {
        InitializeComponent();
        this.prev_window = prev_window;
    }

    private void logIn_Click(object sender, RoutedEventArgs e)
    {
        try {
            BO.User user = bl.User.GetUserByEmailAndPass(CustomerEmail, Password);
            if (user.IsAdmin)
            {
                new ManagerMenue().Show();
                Close();
            }
            else
                new Catalog(this, user).Show();
        }
        catch(DoesNotExistedBlException)
        {
            MessageBox.Show("email or password doesnt exist", "try again", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void signUp_Click(object sender, RoutedEventArgs e)
    {
        new SignIn(prev_window).Show();
        Close();
    }

}
