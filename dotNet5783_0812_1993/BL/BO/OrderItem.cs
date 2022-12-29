namespace BO;

/// <summary>
/// A class for an order item
/// </summary>
public class OrderItem
{
    public int ID { get; set; }
    public string? Name { get; set; }
    public int ProductID { get; set; }
    public double Price { get; set; }
    public int Amount { get; set; }
    public double TotalPrice { get; set; }
    public override string ToString() => this.ToStringProperty();

}
