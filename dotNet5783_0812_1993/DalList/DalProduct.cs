using DO;
using System;
using static Dal.DataSource;

namespace Dal;

/// <summary>
/// A department that performs operations: 
/// adding, updating, repeating and deleting on the product array
/// </summary>
public class DalProduct
{
    /// <summary>
    /// Add a product to the productArray
    /// </summary>
    /// <param name="product">the new product to add</param>
    /// <returns>the id of the new product</returns>
    public int Add(Product product)
    {
        int i;
        for (i = 0; i < ProductArray.Length || ProductArray[i].ID != product.ID; i++) ;
        if (i < ProductArray.Length)
            throw new Exception("product id already exists");
        ProductArray[Config.IndexOrderItemArray]=product;
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
        for (int i = 0; i < ProductArray.Length; i++)
        {
            if (ProductArray[i].ID == id)
                return ProductArray[i];
        }
        throw new Exception("product is not exist");
    }

    /// <summary>
    /// get all products
    /// </summary>
    /// <returns>an array of all the products</returns>
    public Product[] GetAll()
    {
        Product[] products = new Product[ProductArray.Length];
        for (int i = 0; i < ProductArray.Length; i++)
        {
            products[i] = ProductArray[i];
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
        int i;
        for (i = 0; i < ProductArray.Length || ProductArray[i].ID != id; i++) ;
        if (i == ProductArray.Length)
            throw new Exception("product is not exist");
        i++;
        for (; i < ProductArray.Length; i++)
            ProductArray[i - 1] = ProductArray[i];
        Config.IndexProductArray--;

    }

    /// <summary>
    /// update a product
    /// </summary>
    /// <param name="product">the product new details</param>
    /// <exception cref="Exception">if the product didnt exist</exception>
    public void Update(Product product)
    {
        int i;
        for (i = 0; i < ProductArray.Length || ProductArray[i].ID != product.ID; i++) ;
        if (i == ProductArray.Length)
            throw new Exception("product is not exist");
        ProductArray[i] = product;
    }

}
