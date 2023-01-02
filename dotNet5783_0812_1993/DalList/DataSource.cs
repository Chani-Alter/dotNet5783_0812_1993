using DO;
namespace Dal;

static class DataSource
{
    /// <summary>
    /// Random field for drawing numbers in the department
    /// </summary>
    private static readonly Random randNum = new Random();
   
        /// <summary>
        /// the starting id of the orders
        /// </summary>
        private const int s_startOrderId = 1000;

        /// <summary>
        /// the starting id of the orderItems
        /// </summary>
        private const int s_startOrderItemId = 0;

        ///// <summary>
        ///// the index of the free place in the product array
        ///// </summary>
        //internal static int IndexProductArray { get; set; } = 0;

        ///// <summary>
        ///// the index of the free place in the order array
        ///// </summary>
        //internal static int IndexOrderArray { get; set; } = 0;

        ///// <summary>
        ///// the index of the free place in the order item array
        ///// </summary>
        //internal static int IndexOrderItemArray { get; set; } = 0;

        /// <summary>
        /// the next order id
        /// </summary>
        private static int orderId = s_startOrderId;
        /// <summary>
        /// the propety get of the order id
        /// </summary>
        internal static int OrderId { get => ++orderId; }

        /// <summary>
        /// the next order item id
        /// </summary>
        private static int orderItemId = s_startOrderItemId;

        /// <summary>
        /// the get propety of the order item id
        /// </summary>
        internal static int OrderItemId { get => ++orderItemId; }
  
    /// <summary>
    /// the product array
    /// </summary>
    internal static List <Product?> ProductList= new List <Product?>();
    /// <summary>
    /// the orders array
    /// </summary>
    internal static List <Order?> OrderList = new List<Order?>() ;
    /// <summary>
    /// the order items array
    /// </summary>
    internal static List <OrderItem?> OrderItemList = new List <OrderItem?>() ;

    /// <summary>
    /// the static constractor how caled the s_Initialize function
    /// </summary>
    static DataSource() => s_Initialize();

    /// <summary>
    /// the function caled the initializations function
    /// </summary>
    static private void s_Initialize()
    {
        initProductArray();
        initOrderArray();
        initOrderItemArray();
    }
    /// <summary>
    /// Product array initialization
    /// </summary>
    static private void initProductArray()
    {
        
        int[] productId = new int[]
        {
            793154,885632,958742,125412,563258,745896,125458,458796,236974,125870
        };
        string[] Name = new string[]{
         "Dell Vostro","Lenovo ThinkBook","Lenovo IdeaPad","Apple Mac mini","Lenovo legion","Dell 24 inch","Xiaomi 30 inch",
         "JBL tune 510","Apple AirPods","mouse and keyboard DOQO"
        };
        Category[] category = new Category[]
        {
            Category.Laptop,Category.Laptop,Category.Laptop ,Category.DesktopComputer,Category.DesktopComputer,Category.Screens,Category.Screens,
            Category.PeripheralEquipment,Category.PeripheralEquipment,Category.PeripheralEquipment
        };
        int[] price = new int[]
        {
            4500,3800,3400,5000,3100,800,900,250,350,100
        };
        int[] InStockAvailable = new int[]
        {
            150,180,120,80,92,46,39,300,0,523
        };

        for (int i = 0; i < 10; i++)
        {
            ProductList.Add(new Product { ID = productId[i], Name = Name[i], Category = category[i], Price = price[i], InStock = InStockAvailable[i] });
        }
    }

    /// <summary>
    /// Product order initialization
    /// </summary>
    static private void initOrderArray()
    {
        String [] CustomerName = new String[13] {"Motti Weiss","Moshe Feld","Itzik Orlev","Ruli Dikman",
        "Ari Hill","Shuli Rand","Ishay Ribo","Beri Weber","Simcha Friedman","Avraam Fried","Mordechai Ben David",
        "Yaakov Shwekey","Naftali Kempeh"
        };  
        string[] CustomerEmail = new string[]
        {
            "MottiWeiss@gmail.com","MosheFeld@gmail.com","ItzikOrlev@gmail.com","RuliDikman@gmail.com",
        "AriHill@gmail.com","ShuliRand@gmail.com","IshayRibo@gmail.com","BeriWeber@gmail.com",
            "SimchaFriedman@gmail.com","AvraamFried@gmail.com","MordechaiBenDavid@gmail.com",
        "YaakovShwekey@gmail.com","NaftaliKempeh@gmail.com"
        };
        string[] address = new string[13]
        {
            "Chazon Ish 3 Beit Shemesh","Rabbi Akiva 170 Bnei Brak","Hakalanit 17 herzelia","Haturim 6 Jerusalem" ,
            "Malchei Israel 40 Jerusalem","miron 20 Bnei Brak","Hameiri 4 Jerusalem","begin 72 Naaria",
            "hagefen 22 Kfar Chabad", "hanurit 7 Ashdod","Shamgar 20 Jerusalem","Pnei Menachem 1 Petach Tikwa",
            "Hadekel 16 Tel Aviv"
        };
        
       
        for (int i = 0; i < 20; i++)
        {
            DateTime ShippingDate = new DateTime();
            DateTime DeliveryDate1 = new DateTime();
            DateTime helpE;
            do
            {
                helpE = new DateTime(randNum.Next(2000, 2022), randNum.Next(1, 13), randNum.Next(1, 29), randNum.Next(24), randNum.Next(60), randNum.Next(60));
            }
            while (helpE >= DateTime.Now);
            //OrderArray[IndexOrderArray].CreateOrderDate = helpE;
            TimeSpan helpC;
            if (i < 16)
            {
                helpC = new TimeSpan(randNum.Next(1, 10), 0, 0, 0, 0);
                //OrderArray[IndexOrderArray].ShippingDate = OrderArray[i].CreateOrderDate + helpC;
                ShippingDate = helpE + helpC;

            }
            if (i < 8)
            {
                helpC = new TimeSpan(randNum.Next(1, 10), 0, 0, 0, 0);
                //OrderArray[IndexOrderArray].DeliveryDate = OrderArray[i].DeliveryDate + helpC;
                DeliveryDate1 = helpE + helpC;

            }
            //IndexOrderArray++;
            OrderList.Add(new Order { ID = OrderId, CustomerName = CustomerName[i % 13],CustomerEmail = CustomerEmail[i % 13],CustomerAdress = address[i % 13], CreateOrderDate = helpE, ShippingDate = ShippingDate, DeliveryDate = DeliveryDate1 });
        }
        
    }

    /// <summary>
    /// item in order array initialization
    /// </summary>
    static private void initOrderItemArray()

    {
        foreach (Order? o in OrderList)
        {
            int randA = randNum.Next(9);
            double price = new double();
            double price2 = new double();
            foreach (Product? p in ProductList)
            {
                if (p?.ID == ProductList[randA]?.ID)
                {
                    price = p?.Price??0;
                    break;
                }
            }
            int randB;
            do
                randB = randNum.Next(9);
            while (randA == randB);
            foreach (Product? p in ProductList)
            {
                if (p?.ID == ProductList[randB]?.ID)
                {
                    price2 = p?.Price??0;
                    break;
                }
            }
            OrderItemList.Add(new OrderItem { ID = OrderItemId, OrderID = o?.ID ?? 0, ProductID = ProductList[randA]?.ID ?? 0, Price = price, Amount = randNum.Next(1, 5) });
            OrderItemList.Add(new OrderItem { ID = OrderItemId, OrderID = o?.ID??0, ProductID = ProductList[randB]?.ID??0, Price = price2, Amount = randNum.Next(1, 5) });

        }


    }
}
