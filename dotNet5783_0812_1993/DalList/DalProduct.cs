using DO;
using System;
using static Dal.DataSource;

namespace Dal;

public class DalProduct
{
    public int Add(Product product)
    {
        //int i;
        //for (i = 0; i < ProductArray.Length|| ProductArray[i].ID!=product.ID; i++) ;
        //if (i < ProductArray.Length)
        //    throw new Exception("product id already exists");

        Random randNum = new();
        int id = randNum.Next(100000,1000000);
        int i;
        for (i = 0; i < ProductArray.Length|| ProductArray[i].ID!=product.ID; i++) ;
        while (i < ProductArray.Length)
        {
            id = randNum.Next(100000, 1000000);
            for (i = 0; i < ProductArray.Length || ProductArray[i].ID != product.ID; i++) ;
        }
        product.ID = id;   
        
        ProductArray[Config.IndexOrderItemArray]=product;
        return product.ID;

    }
    public Order GetById(int id)
    {
        for (int i = 0; i < OrderArray.Length; i++)
        {
            if (OrderArray[i].ID == id)
                return OrderArray[i];
        }
        throw new Exception("product is not exist");
    }
    public Product[] GetAll()
    {
        Product[] products = new Product[ProductArray.Length];
        for (int i = 0; i < ProductArray.Length; i++)
        {
            products[i] = ProductArray[i];
        }
        return products;
    }

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

    public void Update(Product product)
    {
        int i;
        for (i = 0; i < ProductArray.Length || ProductArray[i].ID != product.ID; i++) ;
        if (i == ProductArray.Length)
            throw new Exception("product is not exist");
        ProductArray[i] = product;
    }

}
