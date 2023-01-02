using DalApi;
using DO;
using System;
using static Dal.DataSource;

namespace Dal;

/// <summary>
/// A department that performs operations: 
/// adding, updating, repeating and deleting on the product array
/// </summary>
internal class DalProduct : IProduct
{

    /// <summary>
    /// Add a product to the productArray
    /// </summary>
    /// <param name="product">the new product to add</param>
    /// <returns>the id of the new product</returns>
    public int Add(Product product)
    {
        var result = ProductList.FirstOrDefault(p => p?.ID == product.ID);
        if (result != null)
            throw new DuplicateDalException("product id already exists");
        ProductList.Add(product);

        return product.ID;
    }

    /// <summary>
    /// get list of product by condition
    /// </summary>
    /// <param name="predicate">the condition</param>
    /// <returns>the products list</returns>
    public IEnumerable<Product?> GetList(Func<Product?, bool>? predicate)
    {
        if (predicate == null)
            return ProductList.Select(product => product);
        return ProductList.Where(predicate);
    }


    /// <summary>
    /// get a spesific product by condition
    /// </summary>
    /// <param name="predicate">a condition</param>
    /// <returns>the product</returns>
    /// <exception cref="DoesNotExistedDalException">if the order does not exist</exception>
    public Product GetByCondition(Func<Product?, bool> predicate)
    {
        return ProductList.FirstOrDefault(predicate) ??
            throw new DoesNotExistedDalException("There is no order item that matches the condition");
    }

    /// <summary>
    /// delete a product 
    /// </summary>
    /// <param name="id">the id of the product thet need to be deleted</param>
    /// <exception cref="Exception">if the product didnt exist</exception>
    public void Delete(int id)
    {
        var result = ProductList.FirstOrDefault(ord => ord?.ID == id);

        if (result == null)
            throw new DoesNotExistedDalException("product is not exist");

        ProductList.Remove(result);

    }

    /// <summary>
    /// update a product
    /// </summary>
    /// <param name="product">the product new details</param>
    /// <exception cref="Exception">if the product didnt exist</exception>
    public void Update(Product product)
    {
        int index = ProductList.FindIndex(prod => prod?.ID == product.ID);
        if (index == -1)
            throw new DoesNotExistedDalException("order is not exist");

        ProductList[index] = product;

    }
}