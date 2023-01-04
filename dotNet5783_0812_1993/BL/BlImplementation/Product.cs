using BO;
using IProduct = BlApi.IProduct;

using AutoMapper;

namespace BlImplementation;

/// <summary>
/// A class that implements the iproduct interface
/// </summary>
internal class Product : IProduct
{

    /// <summary>
    /// An attribute that contains access to all the dallist data
    /// </summary>
    DalApi.IDal? dal = DalApi.Factory.Get();
    Mapper mapper = initializeAutomapper();

    /// <summary>
    /// Definition of a function that returns a list of product by category for the manager
    /// </summary>
    /// <param name="category">the category value</param>
    /// <returns>the product filtered list</returns>
    public IEnumerable<ProductForList> GetProductListForManagerByCategory(Category? category)
    {
        return getProductListManager(Filter.FilterByCategory, category);
    }


    /// <summary>
    /// Definition of a function that returns All list of product for manager
    /// </summary>
    /// <returns></returns>
    public IEnumerable<ProductForList> GetAllProductListForManager()
    {
        return getProductListManager();
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

            return mapper.Map<BO.Product>(product);

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

            dal.Product.Add(mapper.Map<DO.Product>(product));

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

            DO.Product product1 = mapper.Map<DO.Product>(product);

            dal.Product.Update(product1);

            return product;
        }
        catch (DO.DoesNotExistedDalException ex)
        {
            throw new UpdateErrorBlException("product update fails", ex);
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
               where item != null
               let product = (DO.Product)item
               select mapper.Map<ProductItem>(product);
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
            return mapper.Map<ProductItem>(product);

        }
        catch (DO.DoesNotExistedDalException ex)
        {
            throw new DoesNotExistedBlException("product does not exist", ex);
        }

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
               select mapper.Map<ProductForList>(products);
    }

    static private Mapper initializeAutomapper()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<DO.Product, BO.Product>()
            .ForMember(dest => dest.Category, act => act.MapFrom(src => (Category)src.Category));
            cfg.CreateMap<BO.Product, DO.Product>()
            .ForMember(dest => dest.Category, act => act.MapFrom(src => (DO.Category)src.Category));
            cfg.CreateMap<DO.Product, ProductItem>()
            .ForMember(dest => dest.Category, act => act.MapFrom(src => (Category)src.Category))
            .ForMember(dest => dest.Amount, act => act.MapFrom(src => src.InStock))
            .ForMember(dest => dest.Instock, act => act.MapFrom(src => src.InStock > 0));
            cfg.CreateMap<DO.Product, ProductForList>()
            .ForMember(dest => dest.Category, act => act.MapFrom(src => (Category)src.Category));
        });


        var mapper = new Mapper(config);
        return mapper;
    }

}