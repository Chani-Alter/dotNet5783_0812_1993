using DO;
using System.Drawing;
using static Dal.DataSource;

namespace Dal;

public class DalOrder
{
    public int Add(Order order)
    {
        int id = Config.OrderItemId;
        order.ID= id;
        order.ShippingDate= DateTime.MinValue;
        order.DeliveryDate=DateTime.MinValue; 
        OrderArray[Config.IndexOrderArray++]= order;
        return id;
    }
    public Order GetById(int id)
    {
        for(int i = 0; i < OrderArray.Length; i++)
        {
           if( OrderArray[i].ID == id)
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
        for ( i = 0; i < OrderArray.Length || OrderArray[i].ID != id; i++) ;
        if(i== OrderArray.Length)
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
        OrderArray[i]= order;
    }
    


}
