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
        IEnumerable<DO.Product?> products = dal.Product.GetList();
        return from item in products
               let product = (DO.Product)item
               select new ProductForList
               {
                   ID = product.ID,
                   Name = product.Name,
                   Price = product.Price,
                   Category = (Category)product.Category,

               };
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
            DO.Product product = dal.Product.GetByCondition(prod => prod?.ID == id);
            return new BO.Product
            {
                ID = product.ID,
                Name = product.Name,
                Price = product.Price,
                Category = (Category)product.Category,
                InStock = product.InStock
            };

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
                throw new InvalidInputBlException("Invalid input");

            dal.Product.Add(new DO.Product
            {
                ID = product.ID,
                Name = product.Name,
                Price = product.Price,
                Category = (DO.Category)product.Category,
                InStock = product.InStock
            });

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
            IEnumerable<DO.OrderItem?> orderItems = dal.OrderItem.GetList();

            var result = orderItems.FirstOrDefault(orderItem => orderItem?.ProductID == id);
            if (result == null)
                throw new ImpossibleActionBlException("product exist in order");

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
        IEnumerable<DO.Product?> products = dal.Product.GetList();
        return from item in products
               let product = (DO.Product)item
               select new ProductItem
               {
                   ID = product.ID,
                   Name = product.Name,
                   Price = product.Price,
                   Category = (Category)product.Category,
                   Amount = product.InStock,
                   Instock = product.InStock > 0

               };
    }

    /// <summary>
    /// function that returns a product by id for the customer
    /// </summary>
    /// <param name="id"></param>
    /// <returns>product</returns>
    /// <exception cref="DoesNotExistedBlException"></exception>
    public ProductItem GetProductByIdCustomer(int id)
    {
        try
        {
            DO.Product product = dal.Product.GetByCondition(prod => prod?.ID == id);
            return new ProductItem
            {
                ID = product.ID,
                Name = product.Name,
                Price = product.Price,
                Category = (Category)product.Category,
                Amount = product.InStock,
                Instock = product.InStock > 0
            };

        }
        catch (DO.DoesNotExistedDalException ex)
        {
            throw new DoesNotExistedBlException("product does not exist", ex);
        }

    }
}