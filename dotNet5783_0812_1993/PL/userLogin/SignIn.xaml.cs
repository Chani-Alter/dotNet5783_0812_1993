using PL.Customer;
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

namespace PL.userLogin
{
    /// <summary>
    /// Interaction logic for SignIn.xaml
    /// </summary>
    public partial class SignIn : Window
    {
        public Window prev_window;

        /// <summary>
        /// instance of the bl who contains access to all the bl implementation
        /// </summary>
        BlApi.IBl? bl = BlApi.Factory.Get();



        public BO.User MyUser
        {
            get { return (BO.User)GetValue(MyUserProperty); }
            set { SetValue(MyUserProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyUser.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyUserProperty =
            DependencyProperty.Register("MyUser", typeof(BO.User), typeof(Window), new PropertyMetadata(null));


        public SignIn(Window prev_window)
        {
            InitializeComponent();
            this.prev_window = prev_window;
        }

        private void confirm_Click(object sender, RoutedEventArgs e)
        {
            BO.User user = MyUser;        
            int id = bl.User.AddUser(user);
            user.ID = id;
            new Catalog(this, user).Show();
        }
    }
}
