using DalApi;
using DO;
using static Dal.DataSource;

namespace Dal;

/// <summary>
/// A department that performs operations: 
/// adding, updating, repeating and deleting on the product array
/// </summary>
internal class DalProduct:IProduct
{

    /// <summary>
    /// Add a product to the productArray
    /// </summary>
    /// <param name="product">the new product to add</param>
    /// <returns>the id of the new product</returns>
    public int Add(Product product)
    {        foreach (Product p in ProductList)
        {
            if (p.ID == product.ID)
                throw new DuplicateDalException("product id already exists");
        }

        ProductList.Add(product);
        return product.ID;
    }
    
    /// <summary>
    /// get product by id
    /// </summary>
    /// <param name="id">the id of the requeses product</param>
    /// <returns>the product</returns>
    /// <exception cref="Exception">if the product didnt exist throw exeption</exception>
    public Product GetById(int id)
    {
        int index = search(id);
        if (index != -1)
            return ProductList[index];
        else
            throw new DoesNotExistedDalException("product is not exist");
    }

    /// <summary>
    /// get all products
    /// </summary>
    /// <returns>an array of all the products</returns>
    public IEnumerable <Product> GetAll()
    {
        List<Product> products = new List<Product>();
        for (int i = (ProductList.Count) - 1; i >= 0; i--)
        {
            products.Add(ProductList[i]);
        }
        return products;
    }

    /// <summary>
    /// delete a product 
    /// </summary>
    /// <param name="id">the id of the product thet need to be deleted</param>
    /// <exception cref="Exception">if the product didnt exist</exception>
    public void Delete(int id)
    {
        int index = search(id);
        if (index != -1)
        {
            ProductList.RemoveAt(index);
        }
        else
            throw new DoesNotExistedDalException("product is not exist");
    }

    /// <summary>
    /// update a product
    /// </summary>
    /// <param name="product">the product new details</param>
    /// <exception cref="Exception">if the product didnt exist</exception>
    public void Update(Product product)
    {
        int index = search(product.ID);
        if (index != -1)
            ProductList[index] = product;
        else
            throw new DoesNotExistedDalException("product is not exist");
    }
    private int search(int id)
    {
        for (int i = 0; i < ProductList.Count; i++)
        {
            if (ProductList[i].ID == id)
                return i;
        }
        return -1;
    }
}
