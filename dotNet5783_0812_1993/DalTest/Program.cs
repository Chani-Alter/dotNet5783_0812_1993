using Dal;
using DO;
using System;

namespace DalTest
{
    enum MainMenu { EXIT, PRODUCT, ORDERITEM, ORDER };
    enum SecondaryMenu { ADD=1, GET_ALL,GET_BY_ID,UPDATE,DELETE,GET_BY_ORDERID,GET_BY_ORDER_PRODUCT};
    class Program
    {   private DalOrder dalOrder = new DalOrder();
        private DalProduct dalProduct = new DalProduct(); 
        private DalOrderItem dalOrderItem = new DalOrderItem();
        void menuProduct()
        {

        }
         void menuOrderItem()
        {

        }
         void menuOrder()
        {
            Console.WriteLine("order menu: \n 1-add \n 2-get all \n 3- get by id \n 4- update \n 5- delete");
            SecondaryMenu menuChoice;
            string choice = Console.ReadLine();
            SecondaryMenu.TryParse(choice, out menuChoice);
            switch (menuChoice)
            {
                case SecondaryMenu.ADD:
                    Order order = new Order();
                    Console.WriteLine("enter order details:\n client name, email , adress");
                    order.ClientName = Console.ReadLine();
                    order.Email = Console.ReadLine();
                    order.Adress = Console.ReadLine();
                    order.CreateOrderDate=DateTime.Now;
                    int insertId = dalOrder.Add(order);
                    break;
                case SecondaryMenu.GET_ALL:
                    break;
                case SecondaryMenu.GET_BY_ID:
                    break;
                case SecondaryMenu.UPDATE:
                    break;
                case SecondaryMenu.DELETE:
                    break;
                default:
                    break;
            }

        }
        static void Main(string[] args)
        {
            MainMenu menuChoice;
            Console.WriteLine("Shop menu: \n 0-exit \n 1-product \n 2-order item \n 3-order.");
            string choice = Console.ReadLine();
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
                        break;
                }


                Console.WriteLine("Shop menu: \n 0-exit \n 1-product \n 2-order item \n 3-order.");
                choice = Console.ReadLine();
                MainMenu.TryParse(choice, out menuChoice);

            }

        }
    }
}
