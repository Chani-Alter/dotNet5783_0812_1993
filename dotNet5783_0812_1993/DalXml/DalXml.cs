using DalApi;

namespace Dal;

/// <summary>
/// a class how containes all the dal classes
/// </summary>
sealed internal class DalXml : IDal
{
    #region PUBLIC MEMBERS
    /// <summary>
    /// the get property of the instance
    /// </summary>
    public static DalXml Instance { get => GetInstance(); }

    /// <summary>
    /// the get function that return the instance and create instance if doesnt exist
    /// </summary>
    /// <returns></returns>
    public static DalXml GetInstance()
    {
        lock (instance)
        {
            if (instance == null)
                instance = new Lazy<DalXml>(() => new DalXml());
            return instance.Value;
        }
    }

    /// <summary>
    /// the instance of the DalOrder class
    /// </summary>
    public IOrder Order { get; } = new DalOrder();

    /// <summary>
    /// the instance of the DalProduct class
    /// </summary>
    public IProduct Product { get; } = new DalProduct();

    /// <summary>
    /// the instance of the DalOrderItem class
    /// </summary>
    public IOrderItem OrderItem { get; } = new DalOrderItem();

    /// <summary>
    /// the instance of the DalOUser class
    /// </summary>
    public IUser User { get; } = new DalUser();

    /// <summary>
    /// the instance of the DalCart class
    /// </summary>
    public ICart Cart { get; } = new DalCart();

    /// <summary>
    /// the instance of the DalCartItem class
    /// </summary>
    public ICartItem CartItem { get; } = new DalCartItem();


    #endregion

    #region PRIVATE MEMBERS

    /// <summary>
    /// the instance of the class 
    /// </summary>
    private static Lazy<DalXml> instance = new Lazy<DalXml>(() => new DalXml());

    /// <summary>
    /// a private ctor for the single ton class
    /// </summary>
    private DalXml() { }

    #endregion
}

