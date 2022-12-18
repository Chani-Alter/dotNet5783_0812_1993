using BO;

namespace BlApi;

/// <summary>
/// Interface to the product entity
/// </summary>
public interface IProduct
{
    /// <summary>
    /// A function Defination that returns a list of all products for the manager
    /// </summary>
    /// <returns></returns>
    public IEnumerable<ProductForList> GetProductListManager();

    /// <summary>
    ///A function Defination for return a product by id for manager
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Product GetProductByIdManager(int id);

    /// <summary>
    /// A function Defination for adding a product
    /// </summary>
    /// <param name="product"></param>
    /// <returns></returns>
    public Product AddProduct(Product product);

    /// <summary>
    /// A function Defination for deleteing a product
    /// </summary>
    /// <param name="id"></param>
    public void DeleteProduct(int id);

    /// <summary>
    /// A function Defination for updateing a product
    /// </summary>
    /// <param name="product"></param>
    /// <returns></returns>
    public Product UpdateProduct(Product product);

    /// <summary>
    /// A function Defination for return a list of all products for the customer
    /// </summary>
    /// <returns></returns>
    public IEnumerable<ProductItem> GetProductListForCustomer();

    /// <summary>
    /// A function Defination for return a product by id for the customer
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public ProductItem GetProductByIdCustomer(int id);
}
