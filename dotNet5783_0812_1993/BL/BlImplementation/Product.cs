using BO;
using IProduct = BlApi.IProduct;
using System.Runtime.CompilerServices;

namespace BlImplementation;

/// <summary>
/// A class that implements the iproduct interface
/// </summary>
internal class Product : IProduct
{
    #region PUBLIC MEMBER

    [MethodImpl(MethodImplOptions.Synchronized)]
    /// <summary>
    /// Definition of a function that returns a list of product by category for the manager
    /// </summary>
    /// <param name="category">the category value</param>
    /// <returns>the product filtered list</returns>
    public IEnumerable<ProductForList> GetProductListForManagerByCategory(Category? category)
    {
        return getProductListManager(Filter.FilterByCategory, category);
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    /// <summary>
    /// Definition of a function that returns All list of product for manager
    /// </summary>
    /// <returns></returns>
    public IEnumerable<ProductForList> GetAllProductListForManager()
    {
        return getProductListManager();
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
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
            return castProduct<BO.Product, DO.Product>(dal.Product.GetByCondition(prod => prod?.ID == id));
        }
        catch(BlNullValueException ex)
        {
            throw new DoesNotExistedBlException("product does not exist", ex);
        }
        catch (DO.DoesNotExistedDalException ex)
        {
            throw new DoesNotExistedBlException("product does not exist", ex);
        }
        catch (DO.XMLFileNullExeption ex)
        {
            throw new DoesNotExistedBlException("product dosent exsit", ex);
        }

    }

    [MethodImpl(MethodImplOptions.Synchronized)]
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

            dal.Product.Add(castBOToDO(product));
            return product;
        }
        catch (DO.DuplicateDalException ex)
        {
            throw new BLAlreadyExistException("product already exist", ex);
        }
        catch (DO.XMLFileNullExeption ex)
        {
            throw new DoesNotExistedBlException("product file doesnt exsit", ex);
        }

    }

    [MethodImpl(MethodImplOptions.Synchronized)]
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
        catch (DO.XMLFileNullExeption ex)
        {
            throw new DoesNotExistedBlException("product dosent exsit", ex);
        }

    }

    [MethodImpl(MethodImplOptions.Synchronized)]
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

            dal.Product.Update(castBOToDO(product));

            return product;
        }
        catch (DO.DoesNotExistedDalException ex)
        {
            throw new UpdateErrorBlException("product update fails", ex);
        }
        catch (DO.XMLFileNullExeption ex)
        {
            throw new UpdateErrorBlException("product update fails", ex);

        }
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    /// <summary>
    /// function that returns a list of all products for the customer
    /// </summary>
    /// <returns>list of product</returns>
    public IEnumerable<ProductItem> GetProductListForCustomer(Category category)
    {
        IEnumerable<DO.Product?> products = dal.Product.GetList(product => product?.Category == (category != Category.All ? (DO.Category)category : product?.Category));

        return from item in products
               where item != null 
               let product = (DO.Product)item
               select castProduct<ProductItem, DO.Product>(product);
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
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
            return castProduct<ProductItem, DO.Product>(product);


        }
        catch (DO.DoesNotExistedDalException ex)
        {
            throw new DoesNotExistedBlException("product does not exist", ex);
        }
        catch (DO.XMLFileNullExeption ex)
        {
            throw new DoesNotExistedBlException("product dosent exsit", ex);
        }


    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public IEnumerable<ProductItem?> GetPopularProductListForCustomer()
    {
        IEnumerable<DO.Product?> products = dal.Product.GetList();
        IEnumerable<DO.OrderItem?> orderItems = dal.OrderItem.GetList();
        var result =
            from oi in orderItems
            let orderItem = (DO.OrderItem)oi
            join p in products on orderItem.ProductID equals ((DO.Product)p).ID
            where p != null
            group new { OrderItem = oi, Product = p } by p?.Category into g
            select new
            {
                Product = (from op in g
                           group op by op.Product into pg
                           orderby pg.Count() descending
                           select pg.Key).FirstOrDefault()
            };
            return  result.Select(x => new BO.ProductItem
            {
                ID=((DO.Product)x.Product!).ID,
                Name = ((DO.Product)x.Product!).Name,
                Price = ((DO.Product)x.Product!).Price,
                Category =(BO.Category) ((DO.Product)x.Product!).Category,
                Amount = ((DO.Product)x.Product!).InStock,
                InStock = ((DO.Product)x.Product!).InStock>0?true:false
            });
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public IEnumerable<ProductItem?> GetcheapestProductListForCustomer()
    {
        IEnumerable<DO.Product?> products = dal.Product.GetList();

       var cheapestProducts = from p in products
                              group p by p?.Category into g
                              select g.OrderBy(p => p?.Price).FirstOrDefault();
        return cheapestProducts.Select(x => new BO.ProductItem
        {
            ID = ((DO.Product)x!).ID,
            Name = ((DO.Product)x!).Name,
            Price = ((DO.Product)x!).Price,
            Category = (BO.Category)((DO.Product)x!).Category,
            Amount = ((DO.Product)x!).InStock,
            InStock = ((DO.Product)x!).InStock > 0 ? true : false
        });
    }

    #endregion

    #region PRIVATE MEMBERS

    /// <summary>
    /// An attribute that contains access to all the dallist data
    /// </summary>
    DalApi.IDal? dal = DalApi.Factory.Get();

    /// <summary>
    /// cast from BO prodact to a DO product
    /// </summary>
    /// <param name="pBO"></param>
    /// <returns></returns>
    private DO.Product castBOToDO(BO.Product pBO)
    {

        DO.Product pDO = Utils.cast<DO.Product, BO.Product>(pBO);
        pDO.Category =(DO.Category) pBO.Category;
        return pDO;
    }

    /// <summary>
    /// cast from DO prodact to all kinds af bo products
    /// </summary>
    /// <typeparam name="S"></typeparam>
    /// <typeparam name="T"></typeparam>
    /// <param name="t"></param>
    /// <returns></returns>
    /// <exception cref="BlNullValueException"></exception>
    private S castProduct<S, T>(T t) where S : new()
    {
        S s = Utils.cast<S, T>(t);
        var value = t?.GetType().GetProperty("Category")?.GetValue(t, null) ?? throw new BlNullValueException();
        s?.GetType().GetProperty("Category")?.SetValue(s, (Category?)(int)value);
        if(s?.GetType().Name == "Product")
        {
            var val = t?.GetType()?.GetProperty("InStock")?.GetValue(t, null) ?? throw new BlNullValueException();
            s?.GetType().GetProperty("InStock")?.SetValue(s, val);
        }
        if (s?.GetType().Name== "ProductItem")
        {
                var val1 = t?.GetType()?.GetProperty("InStock")?.GetValue(t, null) ?? throw new BlNullValueException();
                s?.GetType().GetProperty("Amount")?.SetValue(s, val1);
                s?.GetType().GetProperty("InStock")?.SetValue(s, ((int)val1 > 0) ? true : false);
        }
        return s;
    }

    /// <summary>
    /// A private function that return a list of product by a spesific filter
    /// </summary>
    /// <param name="filter">the kind  of the filter</param>
    /// <param name="filterValue">the filter value</param>
    /// <returns>the list after filter</returns>
    private IEnumerable<ProductForList> getProductListManager(Filter filter = Filter.None, object? filterValue = null)
    {
        IEnumerable<DO.Product?> products;

        switch (filter)
        {
            case Filter.FilterByCategory:
                products = dal.Product.GetList(product => product?.Category == (filterValue != null ? (DO.Category)filterValue : product?.Category));
                break;
            case Filter.FilterByBiggerThanPrice:
                products = dal.Product.GetList(product => ((DO.Product)product!).Price > (double)filterValue!);
                break;
            case Filter.FilterBySmallerThanPrice:
                products = dal.Product.GetList(product => ((DO.Product)product!).Price < (double)filterValue!);
                break;
            case Filter.None:
                products = dal.Product.GetList();
                break;
            default:
                products = dal.Product.GetList();
                break;
        }

        return from item in products
               where item != null
               let product = (DO.Product)item
               select castProduct<ProductForList,DO.Product>(product);
    }


    #endregion

}