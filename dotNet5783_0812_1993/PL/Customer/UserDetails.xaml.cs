using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
namespace PL.Customer;

/// <summary>
/// Interaction logic for UserDetails.xaml
/// </summary>
public partial class UserDetails : UserControl
{


    public string CustomerName
    {
        get { return (string)GetValue(CustomerNameProperty); }
        set { SetValue(CustomerNameProperty, value); }
    }

    // Using a DependencyProperty as the backing store for CustomerName.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty CustomerNameProperty =
        DependencyProperty.Register("CustomerName", typeof(string), typeof(Window), new PropertyMetadata(""));



    public string Email
    {
        get { return (string)GetValue(EmailProperty); }
        set { SetValue(EmailProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Email.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty EmailProperty =
        DependencyProperty.Register("Email", typeof(string), typeof(Window), new PropertyMetadata(""));



    public string Adress
    {
        get { return (string)GetValue(AdressProperty); }
        set { SetValue(AdressProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Adress.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty AdressProperty =
        DependencyProperty.Register("Adress", typeof(string), typeof(Window), new PropertyMetadata(""));



    public UserDetails()
    {
        InitializeComponent();
    }
    private void saveBtn_Click(object sender, RoutedEventArgs e)
    {
        Cart parentWindow = (Cart)Window.GetWindow(this);
        parentWindow.confirm_order(CustomerName, Email, Adress);
    }
}
