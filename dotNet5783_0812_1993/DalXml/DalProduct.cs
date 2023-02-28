using DalApi;
using DO;
using System.Xml.Linq;
using System.Runtime.CompilerServices;

namespace Dal;

/// <summary>
/// A department that performs operations: 
/// adding, updating, repeating and deleting on the product file
/// </summary>
internal class DalProduct : IProduct
{
    #region PUBLIC MEMBERS

    [MethodImpl(MethodImplOptions.Synchronized)]
    /// <summary>
    /// Add a product to the product file
    /// </summary>
    /// <param name="product">the new product to add</param>
    /// <returns>the id of the new product</returns>
    public int Add(Product product)
    {
        XElement productRoot = XmlTools.LoadListFromXmlElement(entityName);
        List<Product?> productList = XmlTools.CreateList<Product>(productRoot, entityName);

        var result = productList.FirstOrDefault(p => p?.ID == product.ID);
        if (result != null)
            throw new DuplicateDalException(product.ID, "product", "product id already exists");

        productRoot.Add(XmlTools.ConvertToXelement(product, entityName));

        XmlTools.SaveListForXmlElement(productRoot, entityName);

        return product.ID;
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    /// <summary>
    /// get list of product by condition
    /// </summary>
    /// <param name="predicate">the condition</param>
    /// <returns>the products list</returns>
    public IEnumerable<Product?> GetList(Func<Product?, bool>? predicate)
    {
        XElement productRoot = XmlTools.LoadListFromXmlElement(entityName);
        List<Product?> productList = XmlTools.CreateList<Product>(productRoot, entityName);

        if (predicate == null)
            return productList.Select(product => product);
        return productList.Where(predicate);
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    /// <summary>
    /// get a spesific product by condition
    /// </summary>
    /// <param name="predicate">a condition</param>
    /// <returns>the product</returns>
    /// <exception cref="DoesNotExistedDalException">if the order does not exist</exception>
    public Product GetByCondition(Func<Product?, bool> predicate)
    {
        XElement productRoot = XmlTools.LoadListFromXmlElement(entityName);
        List<Product?> productList = XmlTools.CreateList<Product>(productRoot, entityName);

        return productList.FirstOrDefault(predicate) ??
            throw new DoesNotExistedDalException("There is no order item that matches the condition");
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    /// <summary>
    /// delete a product 
    /// </summary>
    /// <param name="id">the id of the product thet need to be deleted</param>
    /// <exception cref="Exception">if the product didnt exist</exception>
    public void Delete(int id)
    {
        XElement productRoot = XmlTools.LoadListFromXmlElement(entityName);
        XElement product = (from prod in productRoot.Elements()
                            where (int?)prod.Element("ID") == id
                            select prod).FirstOrDefault() ?? throw new DoesNotExistedDalException(id, "product", "product is not exist");
        product.Remove();
        XmlTools.SaveListForXmlElement(productRoot , entityName);   


    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    /// <summary>
    /// update a product
    /// </summary>
    /// <param name="product">the product new details</param>
    /// <exception cref="Exception">if the product didnt exist</exception>
    public void Update(Product product)
    {
        Delete(product.ID);
        Add(product);   
    }

    #endregion

    #region PRIVATE MEMBER

    /// <summary>
    /// the entity name
    /// </summary>
    const string entityName = @"Product";

    #endregion

}