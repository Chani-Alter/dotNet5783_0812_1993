// See https://aka.ms/new-console-template for more information
using BlApi;
using BlImplementation;
using BO;

namespace BLTest
{
    /// <summary>
    /// Enum of the main management input options
    /// </summary>
    enum ManagementProgram { EXIT, PRODUCT, ORDER, CART }
    /// <summary>
    /// Enum of the product options
    /// </summary>
    enum ProductOptions { ADD = 1, GET_FOR_MANAGER, GET_FOR_CUSTOMER, GET_LIST_FOR_MANAGER, GET_LIST_FOR_CUSTOMER, UPDATE, DELETE }
    /// <summary>
    /// Enum of the order options
    /// </summary>
    enum OrderOptions { GET = 1, GET_ALL, UPDATE_SHIPPING_ORDER, UPDATE_DELIVERY_ORDER, ORDER_TRACKING, UPDATE_AMOUNT_OF_PRODUCT_IN_ORDER }
    /// <summary>
    /// Enum of the cart options
    /// </summary>
    enum CartOptions { ADD = 1, UPDATE, MAKE_ORDER }


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
            BO.Product product = new BO.Product();
            BO.ProductItem productItem = new BO.ProductItem();
            IEnumerable<BO.ProductForList> productsForList = new List<BO.ProductForList>();
            IEnumerable<BO.ProductItem> productsItem = new List<BO.ProductItem>();
            try
            {
                switch (productOptions)
                {
                    case ProductOptions.ADD:
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
                    case ProductOptions.GET_FOR_MANAGER:
                        Console.WriteLine("Enter id product:");
                        readString = Console.ReadLine();
                        int.TryParse(readString, out productId);
                        product = iBl.Product.GetProductByIdForManager(productId);
                        Console.WriteLine(product);
                        break;
                    case ProductOptions.GET_FOR_CUSTOMER:
                        Console.WriteLine("Enter id product:");
                        readString = Console.ReadLine();
                        int.TryParse(readString, out productId);
                        productItem = iBl.Product.GetProductByIdForCustomer(productId);
                        Console.WriteLine(productItem);
                        break;
                    case ProductOptions.GET_LIST_FOR_MANAGER:
                        productsForList = iBl.Product.GetProductListForManager();
                        foreach (ProductForList productForList in productsForList)
                            Console.WriteLine(productForList);
                        break;
                    case ProductOptions.GET_LIST_FOR_CUSTOMER:
                        productsItem = iBl.Product.GetProductListForCustomer();
                        foreach (ProductItem item in productsItem)
                            Console.WriteLine(item);
                        break;
                    case ProductOptions.UPDATE:
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
                    case ProductOptions.DELETE:
                        Console.WriteLine("Enter id product:");
                        readString = Console.ReadLine();
                        int.TryParse(readString, out productId);
                        iBl.Product.DeleteProduct(productId);
                        break;
                    default:
                        break;
                }
            }
            catch (BO.BLDoesNotExistException ex)
            {
                Console.WriteLine(ex);
                Console.WriteLine(ex.InnerException?.ToString());
            }
            catch (BO.BLAlreadyExistException ex)
            {
                Console.WriteLine(ex);
                Console.WriteLine(ex.InnerException?.ToString());
            }
            catch (BO.BLImpossibleActionException ex)
            {
                Console.WriteLine(ex);
            }
            catch (BO.BLInvalidInputException ex)
            {
                Console.WriteLine(ex);
            }
            catch (BO.BLMistakeUpdateException ex)
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
            List<BO.OrderForList> ordersForList = new List<BO.OrderForList>();
            try
            {
                switch (orderOptions)
                {
                    case OrderOptions.GET:
                        Console.WriteLine("Enter id oreder:");
                        readString = Console.ReadLine();
                        int.TryParse(readString, out orderId);
                        order = iBl.Order.GetOrderById(orderId);
                        Console.WriteLine(order);
                        break;
                    case OrderOptions.GET_ALL:
                        IEnumerable<BO.OrderForList> orders = new List<BO.OrderForList>();
                        orders = iBl.Order.GetOrderList();
                        foreach (BO.OrderForList o in orders)
                        {
                            Console.WriteLine(o);
                        }
                        break;
                    case OrderOptions.UPDATE_SHIPPING_ORDER:
                        Console.WriteLine("Enter id order:");
                        readString = Console.ReadLine();
                        int.TryParse(readString, out orderId);
                        order = iBl.Order.UpdateSendOrderByManager(orderId);
                        Console.WriteLine(order);
                        break;
                    case OrderOptions.UPDATE_DELIVERY_ORDER:
                        Console.WriteLine("Enter id order:");
                        readString = Console.ReadLine();
                        int.TryParse(readString, out orderId);
                        order = iBl.Order.UpdateSupplyOrderByManager(orderId);
                        Console.WriteLine(order);
                        break;
                    case OrderOptions.ORDER_TRACKING:
                        Console.WriteLine("Enter id order:");
                        readString = Console.ReadLine();
                        int.TryParse(readString, out orderId);
                        orderTracking = iBl.Order.TrackingOrder(orderId);
                        Console.WriteLine(orderTracking);
                        break;
                    case OrderOptions.UPDATE_AMOUNT_OF_PRODUCT_IN_ORDER:
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
            catch (BO.BLDoesNotExistException ex)
            {
                Console.WriteLine(ex);
                Console.WriteLine(ex.InnerException?.ToString());
            }
            catch (BO.BLAlreadyExistException ex)
            {
                Console.WriteLine(ex);
                Console.WriteLine(ex.InnerException?.ToString());
            }
            catch (BO.BLImpossibleActionException ex)
            {
                Console.WriteLine(ex);
            }
            catch (BO.BLInvalidInputException ex)
            {
                Console.WriteLine(ex);
            }
            catch (BO.BLMistakeUpdateException ex)
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
                    case CartOptions.ADD:
                        Console.WriteLine("Enter product id:");
                        readString = Console.ReadLine();
                        int.TryParse(readString, out readInt);
                        currentCart = iBl.cart.AddProductToCart(currentCart, readInt);
                        Console.WriteLine(currentCart);
                        break;
                    case CartOptions.UPDATE:
                        Console.WriteLine("Enter product id: ");
                        readString = Console.ReadLine();
                        int.TryParse(readString, out readInt);
                        Console.WriteLine("Enter a new amount: ");
                        readString = Console.ReadLine();
                        int.TryParse(readString, out readInt1);
                        currentCart = iBl.cart.UpdateProductAmountInCart(currentCart, readInt, readInt1);
                        Console.WriteLine(currentCart);
                        break;
                    case CartOptions.MAKE_ORDER:
                        iBl.cart.MakeOrder(currentCart);
                        currentCart = new Cart();
                        break;
                    default:
                        break;
                }
            }
            catch (BO.BLDoesNotExistException ex)
            {
                Console.WriteLine(ex);
                Console.WriteLine(ex.InnerException?.ToString());
            }
            catch (BO.BLAlreadyExistException ex)
            {
                Console.WriteLine(ex);
                Console.WriteLine(ex.InnerException?.ToString());
            }
            catch (BO.BLImpossibleActionException ex)
            {
                Console.WriteLine(ex);
            }
            catch (BO.BLInvalidInputException ex)
            {
                Console.WriteLine(ex);
            }
            catch (BO.BLMistakeUpdateException ex)
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
            ManagementProgram managementProgram;
            Console.WriteLine("Shop menu: \n 0-exit \n 1-product \n 2-order\n 3-cart");
            string choice = Console.ReadLine();
            ManagementProgram.TryParse(choice, out managementProgram);

            while (managementProgram != ManagementProgram.EXIT)
            {
                switch (managementProgram)
                {

                    case ManagementProgram.PRODUCT:
                        ProductManagement();
                        break;
                    case ManagementProgram.ORDER:
                        OrdersManagement();
                        break;
                    case ManagementProgram.CART:
                        CartManagement();
                        break;
                    default:
                        break;
                }
                Console.WriteLine("Shop menu: \n 0-exit \n 1-product \n 2-order \n 3-cart ");
                choice = Console.ReadLine();
                ManagementProgram.TryParse(choice, out managementProgram);

            }
        }
    }
}