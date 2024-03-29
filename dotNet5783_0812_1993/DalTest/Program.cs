﻿using DO;

namespace DalTest;

/// <summary>
/// Enum of the main menu input options
/// </summary>
enum MainMenu { EXIT, PRODUCT, ORDERITEM, ORDER }

/// <summary>
/// Enum of the secondary menu input options
/// </summary>
enum SecondaryMenu { ADD=1, GET_ALL,GET_BY_ID,UPDATE,DELETE,GET_BY_ORDERID,GET_BY_ORDER_PRODUCT}

/// <summary>
/// The class of the main program
/// </summary>
public class Program
{
    /// <summary>
    /// Instances of the data access classes
    /// </summary>
    static private DalApi.IDal? dal = DalApi.Factory.Get();

    /// <summary>
    ///A static private function, that is called by the main program
    ///when the user requests to perform operations on the product array
    /// </summary>
    static private void menuProduct()
    { 
        Console.WriteLine("order menu: \n 1-add \n 2-get all \n 3- get by id \n 4- update \n 5- delete");
        SecondaryMenu menuChoice;
        string? helpString = Console.ReadLine();
        SecondaryMenu.TryParse(helpString, out menuChoice);
        Product product = new Product();
        int helpInt;
        double helpDouble;
        
        try
        {
            switch (menuChoice)
            {
                case SecondaryMenu.ADD:
                    Console.WriteLine("enter product details:\n product id, product name, category , price ,amount");
                    helpString = Console.ReadLine();
                    int.TryParse(helpString, out helpInt);
                    product.ID=helpInt;
                    product.Name = Console.ReadLine();
                    helpString=Console.ReadLine();
                    int.TryParse(helpString, out helpInt);
                    product.Category = (Category)helpInt;
                    helpString = Console.ReadLine();
                    double.TryParse(helpString, out helpDouble);
                    product.Price = helpDouble;
                    helpString = Console.ReadLine();
                    int.TryParse(helpString, out helpInt);
                    product.InStock = helpInt;
                    int insertId = dal.Product.Add(product);
                    Console.WriteLine("insert id: " + insertId);
                    break;
                case SecondaryMenu.GET_ALL:
                    //Product[] products = dalProduct.GetAll();
                    IEnumerable<Product?> products = dal.Product.GetList();
                    Console.WriteLine("products: ");
                    foreach (Product? prod in products)
                        Console.WriteLine(prod);
                    break;
                case SecondaryMenu.GET_BY_ID:
                    Console.WriteLine("enter product id:");
                    helpString = Console.ReadLine();
                    int.TryParse(helpString, out helpInt);
                    //product = dalProduct.GetById(helpInt);
                    product = dal.Product.GetByCondition(prod=>prod?.ID == helpInt);
                    Console.WriteLine(product);
                    break;
                case SecondaryMenu.UPDATE:
                    Console.WriteLine("enter product id:");
                    helpString = Console.ReadLine();
                    int.TryParse(helpString, out helpInt);
                    //product = dalProduct.GetById(helpInt);
                    Console.WriteLine(product);
                    Console.WriteLine("enter product details:\n product name, category , arice ,amount");
                    helpString = Console.ReadLine();
                    if(helpString != "")
                        product.Name = helpString;
                    helpString = Console.ReadLine();
                    if (helpString != "")
                    {
                        int.TryParse(helpString, out helpInt);
                        product.Category = (Category)helpInt;
                    }
                    helpString = Console.ReadLine();
                    if (helpString != "")
                    {
                        int.TryParse(helpString, out helpInt);
                        product.Price = helpInt;
                    }
                    helpString = Console.ReadLine();
                    if (helpString != "")
                    {
                        int.TryParse(helpString, out helpInt);
                        product.InStock = helpInt;
                    }
                    dal.Product.Update(product);
                    //dalProduct.Update(product);
                    break;
                case SecondaryMenu.DELETE:
                    Console.WriteLine("enter product id to delete:");
                    helpString = Console.ReadLine();
                    int.TryParse(helpString, out helpInt);
                    //dalProduct.Delete(helpInt);
                    dal.Product.Delete(helpInt);
                    break;
                default:
                    Console.WriteLine("not valid choise");

                    break;
            }
        }
        catch (Exception exp)
        {
            Console.WriteLine("ERROR: " + exp.Message);
        }

    }

    /// <summary>
    /// A static private function, that is called by the main program
    ///when the user requests to perform operations on the orderItem array
    /// </summary>
    static private void menuOrderItem()
    {
        Console.WriteLine("order menu: \n 1-add \n 2-get all \n 3- get by id \n 4- update \n 5- delete \n 6- getitems by order id \n 7- get by order id and product id");
        SecondaryMenu menuChoice;
        string? helpString = Console.ReadLine();
        SecondaryMenu.TryParse(helpString, out menuChoice);
        OrderItem orderItem = new OrderItem();
        int helpInt;
        double helpDouble;
        try
        {
            switch (menuChoice)
            {
                case SecondaryMenu.ADD:
                    Console.WriteLine("enter order item  details:\n order id, product id , price , amount");
                    helpString = Console.ReadLine();
                    int.TryParse(helpString, out helpInt);
                    orderItem.OrderID=helpInt;
                    helpString = Console.ReadLine();
                    int.TryParse(helpString, out helpInt);
                    orderItem.ProductID=helpInt;
                    helpString = Console.ReadLine();
                    double.TryParse(helpString, out helpDouble);
                    orderItem.Price=helpDouble;
                    helpString = Console.ReadLine();
                    int.TryParse(helpString, out helpInt);
                    orderItem.Amount=helpInt;
                    //int insertId = dalOrderItem.Add(orderItem);
                    int insertId = dal.OrderItem.Add(orderItem);
                    Console.WriteLine("insert id: " + insertId);
                    break;
                case SecondaryMenu.GET_ALL:
                    IEnumerable<OrderItem?> ordersItems = dal.OrderItem.GetList();
                    Console.WriteLine("orderItems: ");
                    foreach (OrderItem? ordItem in ordersItems)
                        Console.WriteLine(ordItem);
                    break;
                case SecondaryMenu.GET_BY_ID:
                    Console.WriteLine("enter order item id:");
                    helpString = Console.ReadLine();
                    int.TryParse(helpString, out helpInt);
                    orderItem = dal.OrderItem.GetByCondition(ord=>ord?.ID==helpInt); Console.WriteLine(orderItem);
                    break;
                case SecondaryMenu.UPDATE:
                    Console.WriteLine("enter order item id:");
                    helpString = Console.ReadLine();
                    int.TryParse(helpString, out helpInt);
                    //orderItem = dalOrderItem.GetById(helpInt);
                    Console.WriteLine(orderItem);
                    Console.WriteLine("enter order item details:\n order id, product id , price , amount");
                    helpString = Console.ReadLine();
                    if (helpString != "")
                    {
                        int.TryParse(helpString, out helpInt);
                        orderItem.OrderID = helpInt;
                    }
                    helpString = Console.ReadLine();
                    if (helpString != "")
                    {
                        int.TryParse(helpString, out helpInt);
                        orderItem.ProductID = helpInt;
                    }
                    helpString = Console.ReadLine();
                    if (helpString != "")
                    {
                        double.TryParse(helpString, out helpDouble);
                        orderItem.Price = helpDouble;
                    }
                    helpString = Console.ReadLine();
                    if (helpString != "")
                    {
                        int.TryParse(helpString, out helpInt);
                        orderItem.Amount = helpInt;
                    }
                    dal.OrderItem.Update(orderItem);
                    break;
                case SecondaryMenu.DELETE:
                    Console.WriteLine("enter order item id to delete:");
                    helpString = Console.ReadLine();
                    int.TryParse(helpString, out helpInt);
                    dal.OrderItem.Delete(helpInt);
                    break;
                case SecondaryMenu.GET_BY_ORDER_PRODUCT:
                    Console.WriteLine("enter order id:");
                    int orderId;
                    helpString = Console.ReadLine();
                    int.TryParse(helpString, out orderId);
                    Console.WriteLine("enter product id:");
                    int prodId;
                    helpString = Console.ReadLine();
                    int.TryParse(helpString, out prodId);
                    orderItem = dal.OrderItem.GetByCondition(ord=>ord?.OrderID==orderId&&ord?.ProductID== prodId);
                    Console.WriteLine(orderItem);
                    break;
                case SecondaryMenu.GET_BY_ORDERID:
                    Console.WriteLine("enter order id:");
                    helpString = Console.ReadLine();
                    int.TryParse(helpString, out helpInt);
                    IEnumerable<OrderItem?> orderItems = dal.OrderItem.GetList(ord=>ord?.OrderID==helpInt);
                    foreach (OrderItem? ordItem in orderItems)
                        Console.WriteLine(ordItem);
                    break;
                default:
                    Console.WriteLine("not valid choise");
                    break;
            }
        }
        catch (Exception exp)
        {
            Console.WriteLine("ERROR: " + exp.Message);
        }

    }

    /// <summary>
    /// A static private function, that is called by the main program
    ///when the user requests to perform operations on the order array
    /// </summary>
    static private void menuOrder()
    {
       
            Console.WriteLine("order menu: \n 1-add \n 2-get all \n 3- get by id \n 4- update \n 5- delete");
            SecondaryMenu menuChoice;
            string? helpString = Console.ReadLine();
            SecondaryMenu.TryParse(helpString, out menuChoice);
            Order order = new Order();
            int helpInt;
            DateTime helpDateTime;
        try
        {
            switch (menuChoice)
            {
                case SecondaryMenu.ADD:
                    Console.WriteLine("enter order details:\n client name, CustomerEmail , CustomerAdress");
                    order.CustomerName = Console.ReadLine();
                    order.CustomerEmail = Console.ReadLine();
                    order.CustomerAdress = Console.ReadLine();
                    order.CreateOrderDate = DateTime.Now;
                    order.ShippingDate=DateTime.MinValue;
                    order.DeliveryDate=DateTime.MinValue;
                    int insertId = dal.Order.Add(order);
                    Console.WriteLine("insert id: " + insertId);
                    break;
                case SecondaryMenu.GET_ALL:
                    IEnumerable<Order?> orders = dal.Order.GetList();
                    Console.WriteLine("orders: ");
                    foreach(Order? ord in orders)
                        Console.WriteLine(ord);
                    break;
                case SecondaryMenu.GET_BY_ID:
                    Console.WriteLine("enter order id:");
                    helpString = Console.ReadLine();
                    int.TryParse(helpString, out helpInt);
                    order = dal.Order.GetByCondition(ord=>ord?.ID ==helpInt);
                    Console.WriteLine(order);
                    break;
                case SecondaryMenu.UPDATE:
                    Console.WriteLine("enter order id:");
                    helpString = Console.ReadLine();
                    int.TryParse(helpString, out helpInt);
                    //order = dalOrder.GetById(helpInt);
                    Console.WriteLine(order);
                    Console.WriteLine("enter order details:\n client name, CustomerEmail , CustomerAdress,create date , shiping date ,delivery date");
                    helpString = Console.ReadLine();
                    if(helpString != "")
                        order.CustomerName = Console.ReadLine();
                    helpString = Console.ReadLine();
                    if (helpString != "")
                        order.CustomerEmail = helpString;
                    helpString = Console.ReadLine();
                    if (helpString != "")
                        order.CustomerAdress = helpString;
                    helpString = Console.ReadLine();
                    if (helpString != "")
                    {
                        DateTime.TryParse(helpString, out helpDateTime);
                        order.CreateOrderDate = helpDateTime;
                    }
                    helpString = Console.ReadLine();
                    if (helpString != "")
                    {
                        DateTime.TryParse(helpString, out helpDateTime);
                        order.ShippingDate = helpDateTime;
                    }
                    helpString = Console.ReadLine();
                    if (helpString != "")
                    {
                        DateTime.TryParse(helpString, out helpDateTime);
                        order.DeliveryDate = helpDateTime;
                    }
                    dal.Order.Update(order);
                    break;
                case SecondaryMenu.DELETE:
                    Console.WriteLine("enter order id to delete:");
                    helpString = Console.ReadLine();
                    int.TryParse(helpString, out helpInt);
                    dal.Order.Delete(helpInt);
                    break;
                default:
                    Console.WriteLine("not valid choise");

                    break;
            }
        }
        catch(Exception exp){
            Console.WriteLine("ERROR: " + exp.Message);
        }
    }

    /// <summary>
    /// The main program
    /// </summary>
    /// <param name="args"></param>
    static void Main(string[] args)
    {
        MainMenu menuChoice;
        Console.WriteLine("Shop menu: \n 0-exit \n 1-product \n 2-order item \n 3-order.");
        string? choice = Console.ReadLine();
        MainMenu.TryParse(choice, out menuChoice);
        while (menuChoice != MainMenu.EXIT)
        {
            switch (menuChoice)
            {
                case MainMenu.PRODUCT:
                    menuProduct();
                    break;
                case MainMenu.ORDERITEM:
                    menuOrderItem();
                    break;
                case MainMenu.ORDER:
                    menuOrder();
                    break;
                default:
                    Console.WriteLine("not valid choise");

                    break;
            }


            Console.WriteLine("Shop menu: \n 0-exit \n 1-product \n 2-order item \n 3-order.");
            choice = Console.ReadLine();
            MainMenu.TryParse(choice, out menuChoice);

        }

    }
}
