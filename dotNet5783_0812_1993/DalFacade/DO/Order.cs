
namespace DO;

/// <summary>
///     Structure for order
/// </summary>

public struct Order
{
    /// <summary>
    /// Unique ID of order
    /// </summary>
    public int ID { get; set; }

    /// <summary>
    /// the name of the ordering
    /// </summary>
    public string? CustomerName { get; set; }

    /// <summary>
    /// the CustomerEmail of the client
    /// </summary>
    public string? CustomerEmail { get; set; }


    /// <summary>
    /// mailing CustomerAdress 
    /// </summary>
    public string? CustomerAdress { get; set; }

    /// <summary>
    /// the create order date
    /// </summary>
    public DateTime? CreateOrderDate { get; set; }

    /// <summary>
    /// the shipping date
    /// </summary>
    public DateTime? ShippingDate { get; set; }

    /// <summary>
    /// the delivery date
    /// </summary>
    public DateTime? DeliveryDate { get; set; }


    /// <summary>
    /// to string function to the order struct
    /// </summary>
    /// <returns>string with the order details</returns>

    public override string ToString() => this.ToStringProperty();
}


