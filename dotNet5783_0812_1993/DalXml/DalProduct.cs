using DalApi;
using DO;
using System.Security.Cryptography;
using System.Xml.Linq;

namespace Dal;

/// <summary>
/// A department that performs operations: 
/// adding, updating, repeating and deleting on the product array
/// </summary>
internal class DalProduct : IProduct
{
    #region PUBLIC MEMBERS

    /// <summary>
    /// Add a product to the productArray
    /// </summary>
    /// <param name="product">the new product to add</param>
    /// <returns>the id of the new product</returns>
    public int Add(Product product)
    {
        XElement productRoot = XmlTools.LoadListFromXmlElement(entityName);
        List<Product?> productList = XmlTools.createList<Product>(productRoot, entityName);

        var result = productList.FirstOrDefault(p => p?.ID == product.ID);
        if (result != null)
            throw new DuplicateDalException(product.ID, "product", "product id already exists");

        productRoot.Add(XmlTools.convertToXelement(product, entityName));

        XmlTools.SaveListForXmlElement(productRoot, entityName);

        return product.ID;
    }

    /// <summary>
    /// get list of product by condition
    /// </summary>
    /// <param name="predicate">the condition</param>
    /// <returns>the products list</returns>
    public IEnumerable<Product?> GetList(Func<Product?, bool>? predicate)
    {
        XElement productRoot = XmlTools.LoadListFromXmlElement(entityName);
        List<Product?> productList = XmlTools.createList<Product>(productRoot, entityName);

        if (predicate == null)
            return productList.Select(product => product);
        return productList.Where(predicate);
    }

    /// <summary>
    /// get a spesific product by condition
    /// </summary>
    /// <param name="predicate">a condition</param>
    /// <returns>the product</returns>
    /// <exception cref="DoesNotExistedDalException">if the order does not exist</exception>
    public Product GetByCondition(Func<Product?, bool> predicate)
    {
        XElement productRoot = XmlTools.LoadListFromXmlElement(entityName);
        List<Product?> productList = XmlTools.createList<Product>(productRoot, entityName);

        return productList.FirstOrDefault(predicate) ??
            throw new DoesNotExistedDalException("There is no order item that matches the condition");
    }

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


    /// <summary>
    /// update a product
    /// </summary>
    /// <param name="product">the product new details</param>
    /// <exception cref="Exception">if the product didnt exist</exception>
    public void Update(Product product)
    {
        List<Product?> productList = XmlTools.LoadListFromXmlSerializer<Product>(entityName);

        int index = productList.FindIndex(prod => prod?.ID == product.ID);
        if (index == -1)
            throw new DoesNotExistedDalException(product.ID, "product", "product is not exist");

        productList[index] = product;
        XmlTools.SaveListForXmlSerializer(productList, entityName);

    }

    #endregion

    #region PRIVATE MEMBER
    const string entityName = @"Product";
    #endregion

}