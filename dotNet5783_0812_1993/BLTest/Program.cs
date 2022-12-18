// See https://aka.ms/new-console-template for more information
using BlApi;
using BlImplementation;
using BO;

namespace BLTest;

/// <summary>
/// Enum of the main Menu input options
/// </summary>
enum MainMenu { Exit, Product, Order, Cart }

/// <summary>
/// Enum of the product options
/// </summary>
enum ProductOptions { Add = 1, GetForManager, GetForCustomer, GetListForManager, GetListForCustomer, Update, Delete }

/// <summary>
/// Enum of the order options
/// </summary>
enum OrderOptions { Get = 1, GetAll, UpdateShippingOrder, UpdateDeliveryOrder, OrderTracking, UpdateAmountOfProductInOrder }

/// <summary>
/// Enum of the cart options
/// </summary>
enum CartOptions { Add = 1, Update, MakeOrder }


/// <summary>
/// The class of the main program
/// </summary>
public class Program
{
    static private DalList.DalList dalList = new DalList.DalList();
    static string readString;
    static int readInt;
    static int orderId;
    static int productId;
    static int readInt1;
    static double readDouble;
    static private IBl iBl = new Bl();
    static private Cart currentCart = new Cart();


    /// <summary>
    ///A static private function, that is called by the main program
    ///when the user requests to perform operations on  product 
    /// </summary>
    private static void ProductManagement()
    {
        Console.WriteLine("Product menu: \n 1- add \n 2- get for manager \n 3- get for customer \n 4- get list for manager \n 5- get list for customer  \n 6- update \n 7- delete");
        readString = Console.ReadLine();
        ProductOptions productOptions = (ProductOptions)int.Parse(readString);
        Product product = new Product();
        ProductItem productItem = new ProductItem();
        IEnumerable<ProductForList> productsForList = new List<ProductForList>();
        IEnumerable<ProductItem> productsItem = new List<ProductItem>();
        try
        {
            switch (productOptions)
            {
                case ProductOptions.Add:
                    Console.WriteLine("enter product details:\n product id, product name, category , price ,amount");
                    readString = Console.ReadLine();
                    int.TryParse(readString, out readInt);
                    product.ID = readInt;
                    product.Name = Console.ReadLine();
                    readString = Console.ReadLine();
                    int.TryParse(readString, out readInt);
                    product.Category = (Category)readInt;
                    readString = Console.ReadLine();
                    double.TryParse(readString, out readDouble);
                    product.Price = readDouble;
                    readString = Console.ReadLine();
                    int.TryParse(readString, out readInt);
                    product.InStock = readInt;
                    product = iBl.Product.AddProduct(product);
                    Console.WriteLine(product);
                    break;

                case ProductOptions.GetForManager:
                    Console.WriteLine("Enter id product:");
                    readString = Console.ReadLine();
                    int.TryParse(readString, out productId);
                    product = iBl.Product.GetProductByIdManager(productId);
                    Console.WriteLine(product);
                    break;

                case ProductOptions.GetForCustomer:
                    Console.WriteLine("Enter id product:");
                    readString = Console.ReadLine();
                    int.TryParse(readString, out productId);
                    productItem = iBl.Product.GetProductByIdCustomer(productId);
                    Console.WriteLine(productItem);
                    break;

                case ProductOptions.GetListForManager:
                    productsForList = iBl.Product.GetProductListManager();
                    
                    foreach (ProductForList productForList in productsForList)
                        Console.WriteLine(productForList);
                    break;

                case ProductOptions.GetListForCustomer:
                    productsItem = iBl.Product.GetProductListForCustomer();
                   
                    foreach (ProductItem item in productsItem)
                        Console.WriteLine(item);
                    break;

                case ProductOptions.Update:
                    Console.WriteLine("enter product details:\n product id, product name, category , price ,amount");
                    readString = Console.ReadLine();
                    int.TryParse(readString, out productId);
                    product.ID = productId;
                    product.Name = Console.ReadLine();
                    readString = Console.ReadLine();
                    int.TryParse(readString, out readInt);
                    product.Category = (Category)readInt;
                    readString = Console.ReadLine();
                    double.TryParse(readString, out readDouble);
                    product.Price = readDouble;
                    readString = Console.ReadLine();
                    int.TryParse(readString, out readInt);
                    product.InStock = readInt;
                    product = iBl.Product.UpdateProduct(product);
                    Console.WriteLine(product);
                    break;

                case ProductOptions.Delete:
                    Console.WriteLine("Enter id product:");
                    readString = Console.ReadLine();
                    int.TryParse(readString, out productId);
                    iBl.Product.DeleteProduct(productId);
                    break;

                default:
                    break;
            }
        }
        catch (DoesNotExistedBlException ex)
        {
            Console.WriteLine(ex);
            Console.WriteLine(ex.InnerException?.ToString());
        }
        catch (BLAlreadyExistException ex)
        {
            Console.WriteLine(ex);
            Console.WriteLine(ex.InnerException?.ToString());
        }
        catch (ImpossibleActionBlException ex)
        {
            Console.WriteLine(ex);
        }
        catch (InvalidInputBlException ex)
        {
            Console.WriteLine(ex);
        }
        catch (UpdateErrorBlException ex)
        {
            Console.WriteLine(ex);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }


    /// <summary>
    /// A static private function, that is called by the main program
    ///when the user requests to perform operations on  order 
    /// </summary>
    private static void OrdersManagement()
    {
        Console.WriteLine("Order menu: \n 1- get \n 2- get all \n 3- update shipping order \n 4- update delivery order  \n 5- order tracking \n 6- update amount of product in order");
        readString = Console.ReadLine();
        OrderOptions orderOptions = (OrderOptions)int.Parse(readString);
        Order order = new Order();
        OrderItem orderItem = new OrderItem();
        OrderTracking orderTracking = new OrderTracking();
        List<OrderForList> ordersForList = new List<OrderForList>();
        try
        {
            switch (orderOptions)
            {
                case OrderOptions.Get:
                    Console.WriteLine("Enter id oreder:");
                    readString = Console.ReadLine();
                    int.TryParse(readString, out orderId);
                    order = iBl.Order.GetOrderById(orderId);
                    Console.WriteLine(order);
                    break;

                case OrderOptions.GetAll:
                    IEnumerable<OrderForList> orders = new List<OrderForList>();
                    orders = iBl.Order.GetOrderList();

                    foreach (OrderForList o in orders)
                    {
                        Console.WriteLine(o);
                    }
                    break;
                case OrderOptions.UpdateShippingOrder:
                    Console.WriteLine("Enter id order:");
                    readString = Console.ReadLine();
                    int.TryParse(readString, out orderId);
                    order = iBl.Order.UpdateSendOrderByManager(orderId);
                    Console.WriteLine(order);
                    break;

                case OrderOptions.UpdateDeliveryOrder:
                    Console.WriteLine("Enter id order:");
                    readString = Console.ReadLine();
                    int.TryParse(readString, out orderId);
                    order = iBl.Order.UpdateSupplyOrderByManager(orderId);
                    Console.WriteLine(order);
                    break;

                case OrderOptions.OrderTracking:
                    Console.WriteLine("Enter id order:");
                    readString = Console.ReadLine();
                    int.TryParse(readString, out orderId);
                    orderTracking = iBl.Order.TrackingOrder(orderId);
                    Console.WriteLine(orderTracking);
                    break;

                case OrderOptions.UpdateAmountOfProductInOrder:
                    Console.WriteLine("Enter id order: ");
                    readString = Console.ReadLine();
                    int.TryParse(readString, out orderId);
                    Console.WriteLine("Enter id product: ");
                    readString = Console.ReadLine();
                    int.TryParse(readString, out productId);
                    Console.WriteLine("Enter amount of product: ");
                    readString = Console.ReadLine();
                    int.TryParse(readString, out readInt);
                    orderItem = iBl.Order.UpdateAmountOfOProductInOrder(orderId, productId, readInt);
                    break;

                default:
                    break;
            }
        }

        catch (DoesNotExistedBlException ex)
        {
            Console.WriteLine(ex);
            Console.WriteLine(ex.InnerException?.ToString());
        }
        catch (BLAlreadyExistException ex)
        {
            Console.WriteLine(ex);
            Console.WriteLine(ex.InnerException?.ToString());
        }
        catch (ImpossibleActionBlException ex)
        {
            Console.WriteLine(ex);
        }
        catch (InvalidInputBlException ex)
        {
            Console.WriteLine(ex);
        }
        catch (UpdateErrorBlException ex)
        {
            Console.WriteLine(ex);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }


    /// <summary>
    /// A static private function, that is called by the main program
    ///when the user requests to perform operations on cart 
    /// </summary>
    private static void CartManagement()
    {

        Console.WriteLine("cart menu: \n 1- add \n 2- update  \n 3- make order ");
        readString = Console.ReadLine();
        CartOptions cartOptions = (CartOptions)int.Parse(readString);
        currentCart.CustomerName = "david coen";
        currentCart.CustomerEmail = "davidcoen@gmail.com";
        currentCart.CustomerAdress = "micael 15 petach tikva";
        Order order = new Order();
        try
        {
            switch (cartOptions)
            {
                case CartOptions.Add:
                    Console.WriteLine("Enter product id:");
                    readString = Console.ReadLine();
                    int.TryParse(readString, out readInt);
                    currentCart = iBl.cart.AddProductToCart(currentCart, readInt);
                    Console.WriteLine(currentCart);
                    break;
                case CartOptions.Update:
                    Console.WriteLine("Enter product id: ");
                    readString = Console.ReadLine();
                    int.TryParse(readString, out readInt);
                    Console.WriteLine("Enter a new amount: ");
                    readString = Console.ReadLine();
                    int.TryParse(readString, out readInt1);
                    currentCart = iBl.cart.UpdateProductAmountInCart(currentCart, readInt, readInt1);
                    Console.WriteLine(currentCart);
                    break;
                case CartOptions.MakeOrder:
                    iBl.cart.MakeOrder(currentCart);
                    currentCart = new Cart();
                    break;
                default:
                    break;
            }
        }
        catch (DoesNotExistedBlException ex)
        {
            Console.WriteLine(ex);
            Console.WriteLine(ex.InnerException?.ToString());
        }
        catch (BLAlreadyExistException ex)
        {
            Console.WriteLine(ex);
            Console.WriteLine(ex.InnerException?.ToString());
        }
        catch (ImpossibleActionBlException ex)
        {
            Console.WriteLine(ex);
        }
        catch (InvalidInputBlException ex)
        {
            Console.WriteLine(ex);
        }
        catch (UpdateErrorBlException ex)
        {
            Console.WriteLine(ex);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }


    /// <summary>
    /// The main program
    /// </summary>
    /// <param name="args"></param>
    public static void Main(string[] args)
    {
        MainMenu MainMenu;
        Console.WriteLine("Shop menu: \n 0-Exit \n 1-product \n 2-order\n 3-cart");
        string choice = Console.ReadLine();
        MainMenu.TryParse(choice, out MainMenu);

        while (MainMenu != MainMenu.Exit)
        {
            switch (MainMenu)
            {

                case MainMenu.Product:
                    ProductManagement();
                    break;
                case MainMenu.Order:
                    OrdersManagement();
                    break;
                case MainMenu.Cart:
                    CartManagement();
                    break;
                default:
                    break;
            }
            Console.WriteLine("Shop menu: \n 0-Exit \n 1-product \n 2-order \n 3-cart ");
            choice = Console.ReadLine();
            MainMenu.TryParse(choice, out MainMenu);

        }
    }
}