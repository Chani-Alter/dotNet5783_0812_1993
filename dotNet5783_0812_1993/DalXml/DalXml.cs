using DalApi;
namespace Dal;

/// <summary>
/// a class how containes all the dal classes
/// </summary>
sealed internal class DalXml : IDal
{
    #region PUBLIC MEMBERS
    public static IDal? Instance
    {
        get
        {
            if (instance == null)
            {
                lock (key)
                {
                    if (instance == null)
                        instance = new DalXml();
                }
            }
            return instance;
        }
    }

    public IOrder Order { get; } = new DalOrder();
    public IProduct Product { get; } = new DalProduct();
    public IOrderItem OrderItem { get; } = new DalOrderItem();

    #endregion

    #region PRIVATE MEMBERS

    private static readonly object key = new();

    private static IDal? instance;

    private DalXml() { }

    #endregion
}

