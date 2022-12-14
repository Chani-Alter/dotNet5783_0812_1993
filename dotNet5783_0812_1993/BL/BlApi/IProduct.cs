using BO;

namespace BlApi
{
    /// <summary>
    /// Interface for a product logical entity
    /// </summary>
    public interface IProduct
    {
        /// <summary>
        /// Definition of a function that returns a list of all products for the manager
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ProductForList> GetProductListForManager();
        /// <summary>
        /// Definition of a function that returns a product by id for the manager
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Product GetProductByIdForManager(int id);
        /// <summary>
        /// Definition of a function that add a product
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public Product AddProduct(Product product);
        /// <summary>
        /// Definition of a function that delete a product
        /// </summary>
        /// <param name="id"></param>
        public void DeleteProduct(int id);
        /// <summary>
        /// Definition of a function that update a product
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public Product UpdateProduct(Product product);
        /// <summary>
        /// Definition of a function that returns a list of all products for the customer
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ProductItem> GetProductListForCustomer();
        /// <summary>
        /// Definition of a function that returns a product by id for the customer
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ProductItem GetProductByIdForCustomer(int id);
    }
}
