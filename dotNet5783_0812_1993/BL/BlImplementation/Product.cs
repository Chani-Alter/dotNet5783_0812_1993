using DalApi;
using IProduct = BlApi.IProduct;
namespace BlImplementation;

/// <summary>
/// A class that implements the iproduct interface
/// </summary>
internal class Product : IProduct
{
    private IDal dal = new DalList.DalList();
    /// <summary>
    /// function that returns a product by id for the manager
    /// </summary>
    /// <returns>list of product</returns>
    public IEnumerable<BO.ProductForList> GetProductListForManager()
    {
        IEnumerable<DO.Product> products = dal.Product.GetAll();
        List<BO.ProductForList> productsForList = new List<BO.ProductForList>();
        foreach (DO.Product product in products)
        {
            BO.ProductForList productForList = new BO.ProductForList();
            productForList.ID = product.ID;
            productForList.Name = product.Name;
            productForList.Price = product.Price;
            productForList.Category = (BO.Category)product.Category;
            productsForList.Add(productForList);
        }
        return productsForList;
    }
    /// <summary>
    /// function that returns a product by id for the manager
    /// </summary>
    /// <param name="id"></param>
    /// <returns>product</returns>
    /// <exception cref="BO.BLDoesNotExistException"></exception>
    public BO.Product GetProductByIdForManager(int id)
    {
        try
        {
            DO.Product product = dal.Product.GetById(id);
            BO.Product product1 = new BO.Product();
            product1.ID = product.ID;
            product1.Name = product.Name;
            product1.Price = product.Price;
            product1.Category = (BO.Category)product.Category;
            product1.InStock = product.InStock;
            return product1;
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BLDoesNotExistException("product does not exist", ex);
        }

    }
    /// <summary>
    /// function that add a product
    /// </summary>
    /// <param name="product"></param>
    /// <returns>product</returns>
    /// <exception cref="BO.BLInvalidInputException"></exception>
    /// <exception cref="BO.BLAlreadyExistException"></exception>
    public BO.Product AddProduct(BO.Product product)
    {
        try
        {
            if (product.ID < 1 || product.Name == "" || product.Price < 1 || product.InStock < 0 || (int)product.Category > 5 || (int)product.Category < 0)
            {
                throw new BO.BLInvalidInputException("Invalid input");
            }
            DO.Product product1 = new DO.Product();
            product1.ID = product.ID;
            product1.Name = product.Name;
            product1.Price = product.Price;
            product1.Category = (DO.Category)product.Category;
            product1.InStock = product.InStock;
            dal.Product.Add(product1);
            return product;
        }
        catch (DO.DalAlreadyExistException ex)
        {
            throw new BO.BLAlreadyExistException("product already exist", ex);
        }
    }
    /// <summary>
    /// function that delete a product
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="BO.BLImpossibleActionException"></exception>
    /// <exception cref="BO.BLDoesNotExistException"></exception>
    public void DeleteProduct(int id)
    {
        try
        {
            IEnumerable<DO.OrderItem> orderstems = dal.OrderItem.GetAll();
            foreach (DO.OrderItem orderItem in orderstems)
            {
                if (orderItem.ProductID == id)
                {
                    throw new BO.BLImpossibleActionException("product exist in order");
                }
            }
            dal.Product.Delete(id);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BLDoesNotExistException("prouct does not exist", ex);
        }
    }
    /// <summary>
    /// function that update a product
    /// </summary>
    /// <param name="product"></param>
    /// <returns>update product</returns>
    /// <exception cref="BO.BLInvalidInputException"></exception>
    /// <exception cref="BO.BLDoesNotExistException"></exception>
    public BO.Product UpdateProduct(BO.Product product)
    {
        try
        {
            if (product.ID < 1 || product.Name == "" || product.Price < 1 || product.InStock < 0 || (int)product.Category > 5 || (int)product.Category < 0)
                throw new BO.BLInvalidInputException(" Invalid input");
            {
                DO.Product product1 = new DO.Product();
                product1.ID = product.ID;
                product1.Name = product.Name;
                product1.Price = product.Price;
                product1.InStock = product.InStock;
                product1.Category = (DO.Category)product.Category;
                dal.Product.Update(product1);
            }
            return product;
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BLDoesNotExistException("product does not exist", ex);
        }
    }
    /// <summary>
    /// function that returns a list of all products for the customer
    /// </summary>
    /// <returns>list of product</returns>
    public IEnumerable<BO.ProductItem> GetProductListForCustomer()
    {
        IEnumerable<DO.Product> products = dal.Product.GetAll();
        List<BO.ProductItem> productsItems = new List<BO.ProductItem>();
        foreach (DO.Product product in products)
        {
            BO.ProductItem productItem = new BO.ProductItem();
            productItem.ID = product.ID;
            productItem.Name = product.Name;
            productItem.Price = product.Price;
            productItem.Category = (BO.Category)product.Category;
            productItem.Amount = product.InStock;
            if (product.InStock > 0)
            {
                productItem.Instock = true;
            }
            else
            {
                productItem.Instock = false;
            }
            productsItems.Add(productItem);
        }
        return productsItems;
    }
    /// <summary>
    /// function that returns a product by id for the customer
    /// </summary>
    /// <param name="id"></param>
    /// <returns>product</returns>
    /// <exception cref="BO.BLDoesNotExistException"></exception>
    public BO.ProductItem GetProductByIdForCustomer(int id)
    {
        BO.ProductItem productItem = new BO.ProductItem();
        DO.Product product1 = new DO.Product();
        try
        {
            product1 = dal.Product.GetById(id);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BLDoesNotExistException("product does not exist", ex);
        }
        productItem.ID = product1.ID;
        productItem.Name = product1.Name;
        productItem.Price = product1.Price;
        productItem.Category = (BO.Category)product1.Category;
        productItem.Amount = product1.InStock;
        if (product1.InStock > 0)
            productItem.Instock = true;
        else
            productItem.Instock = false;
        return productItem;
    }
}

