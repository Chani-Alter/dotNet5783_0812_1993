using DO;
namespace Dal;

static class DataSource
{
    /// <summary>
    /// Random field for drawing numbers in the department
    /// </summary>
    private static readonly Random randNum = new();

    /// <summary>
    /// A class is responsible for keeping the indexes of the arrays as well as the Hertz ID
    /// </summary>
    internal static class Config
    {
        /// <summary>
        /// the starting id of the orders
        /// </summary>
        private const int s_startOrderId = 1000;

        /// <summary>
        /// the starting id of the orderItems
        /// </summary>
        private const int s_startOrderItemId = 0;

        /// <summary>
        /// the index of the free place in the product array
        /// </summary>
        internal static int IndexProductArray { get; set; } = 0;

        /// <summary>
        /// the index of the free place in the order array
        /// </summary>
        internal static int IndexOrderArray { get; set; } = 0;

        /// <summary>
        /// the index of the free place in the order item array
        /// </summary>
        internal static int IndexOrderItemArray { get; set; } = 0;

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
        static Config() => ProductArray[0] = new Product();
    }
  
    /// <summary>
    /// the product array
    /// </summary>
    internal static Product[] ProductArray = new Product[50];
    /// <summary>
    /// the orders array
    /// </summary>
    internal static Order[] OrderArray = new Order[100];
    /// <summary>
    /// the order items array
    /// </summary>
    internal static OrderItem[] OrderItemArray = new OrderItem[200];

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
        
        int[] orderId = new int[]
        {
            793154,885632,958742,125412,563258,745896,125458,458796,236974,012587
        };
        string[] productName = new string[]{
         "Dell Vostro","Lenovo ThinkBook","Lenovo IdeaPad","Apple Mac mini","Lenovo legion","Dell 24 inch","Xiaomi 30 inch",
         "JBL tune 510","Apple AirPods","mouse and keyboard DOQO"
        };
        Category[] category = new Category[]
        {
            Category.LAPTOP,Category.LAPTOP,Category.LAPTOP ,Category.dESKTOP_COMPUTER,Category.dESKTOP_COMPUTER,Category.SCREENS,Category.SCREENS,
            Category.PERIPHERAL_EQUIPMENT,Category.PERIPHERAL_EQUIPMENT,Category.PERIPHERAL_EQUIPMENT
        };
        int[] price = new int[]
        {
            4500,3800,3400,5000,3100,800,900,250,350,100
        };
        int[] amountAvailable = new int[]
        {
            150,180,120,80,92,46,39,300,0,523
        };

        for (int i = 0; i < 10; i++)
        {
            ProductArray[i].ID = orderId[Config.IndexProductArray];
            ProductArray[i].ProductName = productName[Config.IndexProductArray];
            ProductArray[i].Category = category[Config.IndexProductArray];
            ProductArray[i].Price = price[Config.IndexProductArray];
            ProductArray[i].Amount = amountAvailable[Config.IndexProductArray];
            Config.IndexProductArray++;
        }
    }
    /// <summary>
    /// Product order initialization
    /// </summary>
    static private void initOrderArray()
    {
        String [] clientName = new String[13] {"Motti Weiss","Moshe Feld","Itzik Orlev","Ruli Dikman",
        "Ari Hill","Shuli Rand","Ishay Ribo","Beri Weber","Simcha Friedman","Avraam Fried","Mordechai Ben David",
        "Yaakov Shwekey","Naftali Kempeh"
        };  
        string[] email = new string[]
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
            OrderArray[Config.IndexOrderArray].ID = Config.OrderId;
            OrderArray[Config.IndexOrderArray].ClientName = clientName[i % 13];
            OrderArray[Config.IndexOrderArray].Email = email[i%13];
            OrderArray[Config.IndexOrderArray].Adress = address[i%13];
            DateTime helpE;
            do
            {
                helpE = new DateTime(randNum.Next(2000, 2022), randNum.Next(1, 13), randNum.Next(1, 29), randNum.Next(24), randNum.Next(60), randNum.Next(60));
            }
            while (helpE >= DateTime.Now);
            OrderArray[Config.IndexOrderArray].CreateOrderDate = helpE;
            TimeSpan helpC;
            if (i < 16)
            {
                helpC = new TimeSpan(randNum.Next(1, 10), 0, 0, 0, 0);
                OrderArray[Config.IndexOrderArray].ShippingDate = OrderArray[i].CreateOrderDate + helpC;

            }
            if (i < 8)
            {
                helpC = new TimeSpan(randNum.Next(1, 10), 0, 0, 0, 0);
                OrderArray[Config.IndexOrderArray].DeliveryDate = OrderArray[i].DeliveryDate + helpC;
            }
            Config.IndexOrderArray++;
        }
        
    }

    /// <summary>
    /// item in order array initialization
    /// </summary>
    static private void initOrderItemArray()

    {
        for (int i = 1; i <= 20; i++)
        {
            int helpA = randNum.Next(10);
            OrderItemArray[Config.IndexOrderItemArray].ID = Config.OrderItemId;
            OrderItemArray[Config.IndexOrderItemArray].OrderID = OrderArray[i].ID;
            OrderItemArray[Config.IndexOrderItemArray].ProductID = ProductArray[helpA].ID;
            OrderItemArray[Config.IndexOrderItemArray].Price = ProductArray[helpA].Price;
            OrderItemArray[Config.IndexOrderItemArray].Amount = randNum.Next(4);
            Config.IndexOrderItemArray++;
            int helpB;
            do
                helpB = randNum.Next(10);
            while (helpA == helpB);
            OrderItemArray[Config.IndexOrderItemArray].ID = Config.OrderItemId;
            OrderItemArray[Config.IndexOrderItemArray].OrderID = OrderArray[i].ID;
            OrderItemArray[Config.IndexOrderItemArray].ProductID = ProductArray[helpB].ID;
            OrderItemArray[Config.IndexOrderItemArray].Price = ProductArray[helpB].Price;
            OrderItemArray[Config.IndexOrderItemArray].Amount = randNum.Next(4);
            Config.IndexOrderItemArray++;

        }
    }
}
