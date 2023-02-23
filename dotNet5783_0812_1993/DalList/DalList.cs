using DalApi;
namespace Dal;

/// <summary>
/// a class how containes all the dal classes
/// </summary>
sealed internal class DalList : IDal
{
    #region PUBLIC MEMBERS
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

    public IOrder Order { get; } = new DalOrder();
    public IProduct Product { get; } = new DalProduct();
    public IOrderItem OrderItem { get; } = new DalOrderItem();

    public IUser User { get; } = new DalUser();

    public ICart Cart { get; } = new DalCart();

    public ICartItem CartItem { get; } = new DalCartItem();

    #endregion

    #region PRIVATE MEMBERS

    private static readonly object key = new();

    private static IDal? instance;

    private DalList() { }

    #endregion
}

