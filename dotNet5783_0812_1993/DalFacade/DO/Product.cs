namespace DO;

/// <summary>
///     Structure for product
/// </summary>
public class Product
{
    public Product()
    {
        /// <summary>
        /// Unique ID of product
        /// </summary>
        public int ID = { get; set;}

        /// <summary>
        /// the name of the product
        /// </summary>
        public string ProductName = { get; set;}

        /// <summary>
        /// the category of the product
        /// </summary>
        public Category Category = { get; set;}

        /// <summary>
        /// price per unit
        /// </summary>
        public double Price = { get; set;}

        /// <summary>
        /// amount available
        /// </summary>
        public int Amount = { get; set;}

        /// <summary>
        /// to string function to the product struct
        /// </summary>
        /// <returns>string with the ordered item details</returns>

        public override string ToString() => $@"
            product id:{ID},
            Product Name: {ProductName},
            Category: {Category},
            Price: {Price},
            amount: {Amount}
        ";
	}

}
