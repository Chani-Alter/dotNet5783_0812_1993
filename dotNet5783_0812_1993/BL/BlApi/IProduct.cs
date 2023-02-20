using BO;

namespace BlApi;

/// <summary>
/// Interface to the product entity
/// </summary>
public interface IProduct
{
    /// <summary>
    /// Definition of a function that returns the list of product for manager
    /// </summary>
    /// <returns></returns>
    public IEnumerable<ProductForList?> GetAllProductListForManager();

    /// <summary>
    /// Definition of a function that returns a list of product by category for the manager
    /// </summary>
    /// <param name="category"></param>
    /// <returns></returns>
    public IEnumerable<ProductForList?> GetProductListForManagerByCategory(BO.Category? category);


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
    public IEnumerable<ProductItem?> GetProductListForCustomer(Category category);

    /// <summary>
    /// A function Defination for return a list of all products for the customer
    /// </summary>
    /// <returns></returns>
    public IEnumerable<ProductItem?> GetPopularProductListForCustomer();

    /// <summary>
    /// A function Defination for return a list of all products for the customer
    /// </summary>
    /// <returns></returns>
    public IEnumerable<ProductItem?> GetcheapestProductListForCustomer();

    /// <summary>
    /// A function Defination for return a product by id for the customer
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public ProductItem GetProductByIdCustomer(int id);
}
