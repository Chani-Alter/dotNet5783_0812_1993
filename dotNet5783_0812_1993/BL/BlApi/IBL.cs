namespace BlApi;

/// <summary>
/// An interface that contains all the secondary interfaces
/// </summary>
public interface IBl
{
    /// <summary>
    /// returns the Iproduct
    /// </summary>
    public IProduct Product { get; }

    /// <summary>
    ///  returns the IOrder 
    /// </summary>
    public IOrder Order { get; }

    /// <summary>
    /// returns the ICart 
    /// </summary>
    public ICart cart { get; }

    public IUser User { get; }

}