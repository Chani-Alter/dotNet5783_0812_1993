namespace DO;

/// <summary>
///     Structure for product
/// </summary>

public struct Product
{
    /// <summary>
    /// Unique ID of product
    /// </summary>
    public int ID { get; set; }

    /// <summary>
    /// the name of the product
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// the category of the product
    /// </summary>
    public Category Category { get; set; }

    /// <summary>
    /// price per unit
    /// </summary>
    public double Price { get; set; }

    /// <summary>
    /// InStock available
    /// </summary>
    public int InStock { get; set; }

    /// <summary>
    /// to string function to the product struct
    /// </summary>
    /// <returns>string with the ordered item details</returns>

    public override string ToString() => $@"
            product id:{ID},
            Product Name: {Name},
            Category: {Category},
            Price: {Price},
            InStock: {InStock}
        ";

}
