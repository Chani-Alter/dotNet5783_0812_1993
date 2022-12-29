
namespace DalApi;

/// <summary>
/// A main interface that centers all the interfaces of the data layer
/// </summary>
public interface IDal
{
    /// <summary>
    /// A property that returns the IOrder entity
    /// </summary>
    public IOrder Order { get; }

    /// <summary>
    /// A property that returns the IOrderItem entity
    /// </summary>
    public IOrderItem OrderItem { get; }

    /// <summary>
    /// A property that returns the IProduct entity
    /// </summary>
    public IProduct Product { get; }
}
