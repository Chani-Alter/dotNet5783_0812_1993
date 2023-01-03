using DalApi;
namespace Dal;

/// <summary>
/// a class how containes all the dal classes
/// </summary>
sealed internal class DalList : IDal
{
    private static readonly object key = new();

    private static IDal? instance;
    public static IDal? Instance
    {
        get{
            if (instance == null)
            {
                lock (key)
                {
                    if (instance == null)
                        instance = new DalList();
                }
            }
            return instance;
        }
    }

    private DalList() { }

    public IOrder Order { get; } = new DalOrder();
    public IProduct Product { get; } = new DalProduct();
    public IOrderItem OrderItem { get; } = new DalOrderItem();

}

