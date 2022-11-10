using DO;
using static Dal.DataSource;

namespace Dal;

public class DalOrder
{
    public int Add(Order order)
    {
        int id = DataSource.Config.OrderItemId;
        order.ID= id;
        DataSource.OrderArray[DataSource.Config.IndexOrderArray++]= order;
        return id;
    }
}
