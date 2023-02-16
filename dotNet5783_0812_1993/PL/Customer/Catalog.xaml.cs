using BlApi;
using BO;
using PL.Product;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace PL.Customer
{
    /// <summary>
    /// Interaction logic for Catalog.xaml
    /// </summary>
    public partial class Catalog : Window
    {
        public ObservableCollection<ProductItem?> ProductsList
        {
            get { return (ObservableCollection<ProductItem?>)GetValue(ProductsProperty); }
            set { SetValue(ProductsProperty, value); }
        }

        public static readonly DependencyProperty ProductsProperty =
            DependencyProperty.Register("ProductsList", typeof(ObservableCollection<ProductItem?>), typeof(Window), new PropertyMetadata(null));

        public BO.Cart? Cart = new BO.Cart() { Items = new() };
        //{
        //    get { return (BO.Cart)GetValue(CartProperty); }
        //    set { SetValue(CartProperty, value); }
        //}

        //public static readonly DependencyProperty CartProperty =
        //    DependencyProperty.Register("Cart", typeof(BO.Cart), typeof(Window), new PropertyMetadata(null));

        /// <summary>
        /// A property who contains the selected category
        /// </summary>
        public Category Category { get; set; } = Category.All;


        public Catalog()
        {
            InitializeComponent();
            changeProductList();

        }

        private void productListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ProductItem product = (ProductItem)productListView.SelectedItem;
            new Product(product, Cart).Show();

        }

        private void category_click(object sender, RoutedEventArgs e)
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

        private void cart_Click(object sender, RoutedEventArgs e)
        {
            new Cart(Cart).Show();
        }


        private BlApi.IBl? bl = BlApi.Factory.Get();


        private void changeProductList()
        {
            var temp = bl.Product.GetProductListForCustomer(Category);
            ProductsList = (temp == null) ? new() : new(temp);
        }

    }
}
