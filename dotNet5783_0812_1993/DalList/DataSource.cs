using DO;
using System.Globalization;

namespace Dal;

static class DataSource
{
    private static readonly Random randNum = new();

    internal static class Config
    {
        private const int s_startOrderId = 1000;
        private const int s_startOrderItemId = 0;
        internal static int IndexProductArray { get; set; } = 0;
        internal static int IndexOrderArray { get; set; } = 0;
        internal static int IndexOrderItemArray { get; set; } = 0;
        private static int orderId = s_startOrderId;
        internal static int OrderId { get => ++orderId; }
        private static int orderItemId = s_startOrderItemId;
        internal static int OrderItemId { get => ++orderItemId; }
    }
  
    internal static Product[] ProductArray = new Product[50];
    internal static Order[] OrderArray = new Order[100];
    internal static OrderItem[] OrderItemArray = new OrderItem[200];

    static DataSource() => s_Initialize();

    static private void s_Initialize()
    {
        initProductArray();
        initOrderArray();
        initOrderItemArray();
    }

    static private void initProductArray()
    {
        string[] productName = new string[10];
        int[] orderId = new int[]
        {
            793154,885632,958742,125412,563258,745896,125458,458796,236974,012587
        };
        productName = new string[]{
         "Dell Vostro","Lenovo ThinkBook","Lenovo IdeaPad","Apple Mac mini","Lenovo legion","Dell 24 inch","Xiaomi 30 inch",
         "JBL tune 510","Apple AirPods","mouse and keyboard DOQO"
        };
        Category[] category = new Category[]
        {
             LAPTOP,LAPTOP,LAPTOP ,dESKTOP_COMPUTER,dESKTOP_COMPUTER,SCREENS,SCREENS,
            PERIPHERAL_EQUIPMENT,PERIPHERAL_EQUIPMENT,PERIPHERAL_EQUIPMENT
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
            ProductArray[i].ID = orderId[i];
            ProductArray[i].ProductName = productName[i];
            ProductArray[i].Category = category[i];
            ProductArray[i].Price = price[i];
            ProductArray[i].Amount = amountAvailable[i];
        }
    }
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
            OrderArray[i].ID = Config.OrderId;
            OrderArray[i].ClientName = clientName[i % 13];
            OrderArray[i].Email = email[i%13];
            OrderArray[i].Adress = address[i%13];
        }
    }
   
    static private void initOrderItemArray()

    {
        for (int i = 1; i <= 20; i++)
        {
            int helpA = randNum.Next(10);
            OrderItemArray[i].ID = Config.OrderItemId;
            OrderItemArray[i].OrderID = OrderArray[i].ID;
            OrderItemArray[i].ProductID = ProductArray[helpA].ID;
            OrderItemArray[i].Price = ProductArray[helpA].Price;
            OrderItemArray[i].Amount = randNum.Next(4);
            int helpB;
            do
                helpB = randNum.Next(10);
            while (helpA == helpB);
            OrderItemArray[i*2].ID = Config.OrderItemId;
            OrderItemArray[i*2].OrderID = OrderArray[i].ID;
            OrderItemArray[i * 2].ProductID = ProductArray[helpB].ID;
            OrderItemArray[i*2].Price = ProductArray[helpB].Price;
            OrderItemArray[i * 2].Amount = randNum.Next(4);


        }
    }
}
