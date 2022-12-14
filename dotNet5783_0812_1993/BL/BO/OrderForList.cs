namespace BO;

/// <summary>
/// A class for an order for list
/// </summary>
public class OrderForList
{
    public int ID { get; set; }
    public string CustomerName { get; set; }
    public OrderStatus Status { get; set; }
    public int AmountOfItems { get; set; }
    public double TotalPrice { get; set; }
    public override string ToString() => this.ToStringProperty();

}
