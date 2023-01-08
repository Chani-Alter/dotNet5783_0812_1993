namespace BO;

/// <summary>
/// the product categorys enum
/// </summary>
public enum Category
{
    Laptop = 1, Desktop_Computer, Screens, Peripheral_Equipment , All
}
/// <summary>
/// The order statuses enum
/// </summary>
public enum OrderStatus
{
    ConfirmedOrder, SendOrder, ProvidedOrder
}

public enum Filter
{
    FilterByCategory, FilterByBiggerThanPrice, FilterBySmallerThanPrice, None
}
