using DalApi;
using BO;
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
    public IEnumerable<ProductForList> GetProductListManager()
    {
        IEnumerable<DO.Product> products = dal.Product.GetAll();
        List<ProductForList> productsForList = new List<ProductForList>();

        foreach (DO.Product product in products)
        {
            ProductForList productForList = new ProductForList();
            productForList.ID = product.ID;
            productForList.Name = product.Name;
            productForList.Price = product.Price;
            productForList.Category = (Category)product.Category;
            productsForList.Add(productForList);
        }
        return productsForList;
    }

    /// <summary>
    /// function that returns a product by id for the manager
    /// </summary>
    /// <param name="id"></param>
    /// <returns>product</returns>
    /// <exception cref="BO.DoesNotExistedBlException"></exception>
    public BO.Product GetProductByIdManager(int id)
    {
        try
        {
            DO.Product product = dal.Product.GetById(id);
            BO.Product product1 = new BO.Product();
            product1.ID = product.ID;
            product1.Name = product.Name;
            product1.Price = product.Price;
            product1.Category = (Category)product.Category;
            product1.InStock = product.InStock;

            return product1;
        }
        catch (DO.DoesNotExistedDalException ex)
        {
            throw new DoesNotExistedBlException("product does not exist", ex);
        }
    }

    /// <summary>
    /// function that add a product
    /// </summary>
    /// <param name="product"></param>
    /// <returns>product</returns>
    /// <exception cref="BO.InvalidInputBlException"></exception>
    /// <exception cref="BO.BLAlreadyExistException"></exception>
    public BO.Product AddProduct(BO.Product product)
    {
        try
        {
            if (product.ID < 1 || product.Name == "" || product.Price < 1 || product.InStock < 0 || (int)product.Category > 5 || (int)product.Category < 0)
            {
                throw new InvalidInputBlException("Invalid input");
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
        catch (DO.DuplicateDalException ex)
        {
            throw new BLAlreadyExistException("product already exist", ex);
        }
    }

    /// <summary>
    /// function that delete a product
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="ImpossibleActionBlException"></exception>
    /// <exception cref="DoesNotExistedBlException"></exception>
    public void DeleteProduct(int id)
    {
        try
        {
            IEnumerable<DO.OrderItem> orderstems = dal.OrderItem.GetAll();

            foreach (DO.OrderItem orderItem in orderstems)
            {
                if (orderItem.ProductID == id)
                {
                    throw new ImpossibleActionBlException("product exist in order");
                }
            }
            dal.Product.Delete(id);
        }
        catch (DO.DoesNotExistedDalException ex)
        {
            throw new DoesNotExistedBlException("prouct does not exist", ex);
        }
    }

    /// <summary>
    /// function that update a product
    /// </summary>
    /// <param name="product"></param>
    /// <returns>update product</returns>
    /// <exception cref="InvalidInputBlException"></exception>
    /// <exception cref="DoesNotExistedBlException"></exception>
    public BO.Product UpdateProduct(BO.Product product)
    {
        try
        {
            if (product.ID < 1 || product.Name == "" || product.Price < 1 || product.InStock < 0 || (int)product.Category > 5 || (int)product.Category < 0)
                throw new InvalidInputBlException(" Invalid input");
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
        catch (DO.DoesNotExistedDalException ex)
        {
            throw new DoesNotExistedBlException("product does not exist", ex);
        }
    }

    /// <summary>
    /// function that returns a list of all products for the customer
    /// </summary>
    /// <returns>list of product</returns>
    public IEnumerable<ProductItem> GetProductListForCustomer()
    {
        IEnumerable<DO.Product> products = dal.Product.GetAll();
        List<ProductItem> productsItems = new List<ProductItem>();
        foreach (DO.Product product in products)
        {
            ProductItem productItem = new ProductItem();
            productItem.ID = product.ID;
            productItem.Name = product.Name;
            productItem.Price = product.Price;
            productItem.Category = (Category)product.Category;
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
    /// <exception cref="DoesNotExistedBlException"></exception>
    public ProductItem GetProductByIdCustomer(int id)
    {
        ProductItem productItem = new ProductItem();
        DO.Product product1 = new DO.Product();
        try
        {
            product1 = dal.Product.GetById(id);
        }
        catch (DO.DoesNotExistedDalException ex)
        {
            throw new DoesNotExistedBlException("product does not exist", ex);
        }

        productItem.ID = product1.ID;
        productItem.Name = product1.Name;
        productItem.Price = product1.Price;
        productItem.Category = (Category)product1.Category;
        productItem.Amount = product1.InStock;

        if (product1.InStock > 0)
            productItem.Instock = true;
        else
            productItem.Instock = false;

        return productItem;
    }
}

