using DO;
using System.Drawing;
using static Dal.DataSource;

namespace Dal;

/// <summary>
/// A department that performs operations: 
/// adding, updating, repeating and deleting on the order array
/// </summary>
public class DalOrder
{
    /// <summary>
    /// add a order to the order array
    /// </summary>
    /// <param name="order">the new order</param>
    /// <returns>the insert new order id</returns>
    public int Add(Order order)
    {
        int id = OrderItemId;
        order.ID = id; 
        OrderArray[IndexOrderArray++]= order;
        return id;
    }

    /// <summary>
    /// get order by id
    /// </summary>
    /// <param name="id">the order id</param>
    /// <returns>the order</returns>
    /// <exception cref="Exception">if the order doesnt exist</exception>
    public Order GetById(int id)
    {
        for(int i = 0; i < IndexOrderArray; i++)
        {
           if( OrderArray[i].ID == id)
                return OrderArray[i];
        }
        throw new Exception("Order is not exist");
    }

    /// <summary>
    /// get all the orders
    /// </summary>
    /// <returns>an array of orders</returns>
    public Order[] GetAll()
    {
        Order[] orders = new Order[IndexOrderArray];  
        for (int i = 0; i < IndexOrderArray; i++)
        {
            orders[i] = OrderArray[i];
        }
            return orders;
    }

    /// <summary>
    /// delete an order
    /// </summary>
    /// <param name="id">the id of the order</param>
    /// <exception cref="Exception">if the order didnt exist</exception>
    public void Delete(int id)
    {
        int i;
        for ( i = 0; i < IndexOrderArray && OrderArray[i].ID != id; i++) ;
        if(i== IndexOrderArray)
            throw new Exception("order is not exist");
        i++;
        for (; i < IndexOrderArray; i++)
            OrderArray[i - 1] = OrderArray[i];
        IndexOrderArray--;

    }

    /// <summary>
    /// update an order
    /// </summary>
    /// <param name="order">the updated order details</param>
    /// <exception cref="Exception">if the order doesnt exist</exception>
    public void Update(Order order)
    {
        int i;
        for (i = 0; i < IndexOrderArray && OrderArray[i].ID != order.ID; i++) ;
        if (i == IndexOrderArray)
            throw new Exception("order is not exist");
        OrderArray[i]= order;
    }
    


}
