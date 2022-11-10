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
        //return product.ID;
        Random randNum = new();
        int id = randNum.Next(100000,1000000);
    }
    public Order GetById(int id)
    {
        for (int i = 0; i < OrderArray.Length; i++)
        {
            if (OrderArray[i].ID == id)
                return OrderArray[i];
        }
        throw new Exception("Order is not exist");
    }
    public Order[] GetAll()
    {
        Order[] orders = new Order[OrderArray.Length];
        for (int i = 0; i < OrderArray.Length; i++)
        {
            orders[i] = OrderArray[i];
        }
        return orders;
    }

    public void Delete(int id)
    {
        int i;
        for (i = 0; i < OrderArray.Length || OrderArray[i].ID != id; i++) ;
        if (i == OrderArray.Length)
            throw new Exception("order is not exist");
        i++;
        for (; i < OrderArray.Length; i++)
            OrderArray[i - 1] = OrderArray[i];
        Config.IndexOrderArray--;

    }

    public void Update(Order order)
    {
        int i;
        for (i = 0; i < OrderArray.Length || OrderArray[i].ID != order.ID; i++) ;
        if (i == OrderArray.Length)
            throw new Exception("order is not exist");
        OrderArray[i] = order;
    }

}
