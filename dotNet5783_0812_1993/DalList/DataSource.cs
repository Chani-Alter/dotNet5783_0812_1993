using DO;
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
        for(int i = 0; i < 10; i++)
        {
        }
    }
    static private void initOrderArray()
    {
        for (int i = 0; i < 10; i++)
        {
        }
    }

    static private void initOrderItemArray()
    {
        for (int i = 0; i < 10; i++)
        {
        }
    }

}
