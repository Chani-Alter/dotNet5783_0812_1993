using DalApi;
using BO;

namespace BlImplementation;

/// <summary>
/// A class that implements the iorder interface
/// </summary>
internal class Order : BlApi.IOrder
{
    private IDal dal = new DalList.DalList();

    /// <summary>
    /// function that returns all orders
    /// </summary>
    /// <returns>list of orders</returns>
    /// <exception cref="DoesNotExistedBlException"></exception>
    public IEnumerable<OrderForList> GetOrderList()
    {
        try
        {
            List<OrderForList> ordersForList = new List<OrderForList>();
            IEnumerable<DO.Order> orders = dal.Order.GetAll();
            double totalPrice = 0;
            int amount = 0;
            foreach (DO.Order order in orders)
            {
                OrderForList orderForList = new OrderForList();
                IEnumerable<DO.OrderItem> orderitems = dal.OrderItem.GetAllItemsByOrderId(order.ID);

                foreach (DO.OrderItem item in orderitems)
                {
                    totalPrice += item.Amount * item.Price;
                    amount += item.Amount;
                }

                orderForList.ID = order.ID;
                orderForList.CustomerName = order.CustomerName;
                orderForList.AmountOfItems = amount;
                orderForList.TotalPrice = totalPrice;

                if (order.DeliveryDate < DateTime.Now && order.DeliveryDate != DateTime.MinValue)
                    orderForList.Status = OrderStatus.PROVIDED_ORDER;
                else
                {
                    if (order.ShippingDate < DateTime.Now && order.ShippingDate != DateTime.MinValue)
                        orderForList.Status = OrderStatus.SEND_ORDER;
                    else
                        orderForList.Status = OrderStatus.CONFIRMED_ORDER;
                }

                ordersForList.Add(orderForList);
            }
            return ordersForList;
        }
        catch (DO.DoesNotExistedDalException ex)
        {
            throw new DoesNotExistedBlException("order doesnot exist", ex);
        }
    }

    /// <summary>
    /// function that returns order by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>order</returns>
    /// <exception cref="BO.DoesNotExistedBlException"></exception>
    public BO.Order GetOrderById(int id)
    {
        BO.Order order = new BO.Order();
        DO.Product product = new DO.Product();
        DO.Order orderDal = new DO.Order();

        List<OrderItem> orderitems = new List<OrderItem>();
        IEnumerable<DO.OrderItem> orderitemsDal;

        try
        {
            orderDal = dal.Order.GetById(id);
        }
        catch (DO.DoesNotExistedDalException ex)
        {
            throw new DoesNotExistedBlException("order doesnot exist", ex);
        }
        try
        {
            orderitemsDal = dal.OrderItem.GetAllItemsByOrderId(id);
        }
        catch (DO.DoesNotExistedDalException ex)
        {
            throw new DoesNotExistedBlException("order doesnot exist", ex);
        }

        double totalPrice = 0;

        foreach (DO.OrderItem item in orderitemsDal)
        {
            BO.OrderItem orderitem = new OrderItem();
            orderitem.ID = item.ID;
            orderitem.ProductID = item.ProductID;
            orderitem.Price = item.Price;
            orderitem.Amount = item.Amount;
            orderitem.TotalPrice = item.Amount * item.Price;

            try
            {
                product = dal.Product.GetById(orderitem.ProductID);
            }
            catch (DO.DoesNotExistedDalException ex)
            {
                throw new DoesNotExistedBlException("order does not exist", ex);
            }
            orderitem.Name = product.Name;
            orderitems.Add(orderitem);
            totalPrice += orderitem.TotalPrice;
        }

        order.ID = orderDal.ID;
        order.CustomerName = orderDal.CustomerName;
        order.CustomerAdress = orderDal.CustomerAdress;
        order.CustomerEmail = orderDal.CustomerEmail;
        order.CreateOrderDate = orderDal.CreateOrderDate;
        order.ShippingDate = orderDal.ShippingDate;
        order.DeliveryDate = orderDal.DeliveryDate;
        order.TotalPrice = totalPrice;
        order.Items = orderitems;

        if (orderDal.DeliveryDate < DateTime.Now && orderDal.DeliveryDate != DateTime.MinValue)
            order.Status = OrderStatus.PROVIDED_ORDER;
        else
        {
            if (order.ShippingDate < DateTime.Now && orderDal.ShippingDate != DateTime.MinValue)
                order.Status = OrderStatus.SEND_ORDER;
            else
                order.Status = OrderStatus.CONFIRMED_ORDER;
        }
        return order;
    }

    /// <summary>
    /// function that update the send order
    /// </summary>
    /// <param name="id"></param>
    /// <returns>update order</returns>
    /// <exception cref="BO.DoesNotExistedBlException"></exception>
    /// <exception cref="BO.ImpossibleActionBlException"></exception>
    /// <exception cref="BO.UpdateErrorBlException"></exception>
    public BO.Order UpdateSendOrderByManager(int id)
    {
        BO.Order order = new BO.Order();
        DO.Product product = new DO.Product();
        DO.Order orderDal = new DO.Order();
        List<OrderItem> orderitems = new List<OrderItem>();
        IEnumerable<DO.OrderItem> orderitemsDal = new List<DO.OrderItem>();
        
        try
        {
            orderDal = dal.Order.GetById(id);
        }
        catch (DO.DoesNotExistedDalException ex)
        {
            throw new DoesNotExistedBlException("order does not exist", ex);
        }
        if (orderDal.ShippingDate < DateTime.Now && orderDal.ShippingDate != DateTime.MinValue)
            throw new ImpossibleActionBlException("order send");
        else
        {
            if (orderDal.ShippingDate == DateTime.MinValue)
                throw new UpdateErrorBlException("No shipping date");
            orderDal.ShippingDate = DateTime.Now;
            try
            {
                dal.Order.Update(orderDal);
            }
            catch (DO.DoesNotExistedDalException ex)
            {
                throw new DoesNotExistedBlException("order does not exist", ex);
            }
        }
        try
        {
            orderitemsDal = dal.OrderItem.GetAllItemsByOrderId(id);
        }
        catch (DO.DoesNotExistedDalException ex)
        {
            throw new DoesNotExistedBlException("order doesnot exist", ex);
        }
        double totalPrice = 0;

        foreach (DO.OrderItem item in orderitemsDal)
        {
            BO.OrderItem orderitem = new BO.OrderItem();
            orderitem.ID = item.ID;
            orderitem.ProductID = item.ProductID;
            orderitem.Price = item.Price;
            orderitem.Amount = item.Amount;
            orderitem.TotalPrice = item.Amount * item.Price;
            try
            {
                product = dal.Product.GetById(orderitem.ProductID);
            }
            catch (DO.DoesNotExistedDalException ex)
            {
                throw new BO.DoesNotExistedBlException("product doesnot exist", ex);
            }

            orderitem.Name = product.Name;
            orderitems.Add(orderitem);
            totalPrice += orderitem.TotalPrice;

        }
        order.ID = orderDal.ID;
        order.CustomerName = orderDal.CustomerName;
        order.CustomerAdress = orderDal.CustomerAdress;
        order.CustomerEmail = orderDal.CustomerEmail;
        order.CreateOrderDate = orderDal.CreateOrderDate;
        order.ShippingDate = orderDal.ShippingDate;
        order.DeliveryDate = orderDal.DeliveryDate;
        order.TotalPrice = totalPrice;
        order.Items = orderitems;
        order.Status = OrderStatus.SEND_ORDER;

        return order;
    }

    /// <summary>
    ///  function that update the supply order
    /// </summary>
    /// <param name="id"></param>
    /// <returns>update order</returns>
    /// <exception cref="DoesNotExistedBlException"></exception>
    /// <exception cref="ImpossibleActionBlException"></exception>
    public BO.Order UpdateSupplyOrderByManager(int id)
    {
        BO.Order order = new BO.Order();
        DO.Product product = new DO.Product();
        List<OrderItem> orderitems = new List<OrderItem>();
        DO.Order orderDal = new DO.Order();
        IEnumerable<DO.OrderItem> orderitemsDal = new List<DO.OrderItem>();

        try
        {
            orderDal = dal.Order.GetById(id);
        }
        catch (DO.DoesNotExistedDalException ex)
        {
            throw new DoesNotExistedBlException("order doesnot exist", ex);
        }
        if (orderDal.DeliveryDate < DateTime.Now && orderDal.DeliveryDate != DateTime.MinValue)
            throw new ImpossibleActionBlException("order Delivery");
        else
        {

            if (orderDal.ShippingDate > DateTime.Now)
                throw new ImpossibleActionBlException("It is not possible to update a delivery date before a shipping date");
            else
            {
                if (orderDal.DeliveryDate == DateTime.MinValue)
                    throw new ImpossibleActionBlException(" No delivery date");
            }
            orderDal.DeliveryDate = DateTime.Now;
            try
            {
                dal.Order.Update(orderDal);
            }
            catch (DO.DoesNotExistedDalException ex)
            {
                throw new DoesNotExistedBlException("order dosent exsit", ex);
            }
        }
        try
        {
            orderitemsDal = dal.OrderItem.GetAllItemsByOrderId(id);
        }
        catch (DO.DoesNotExistedDalException ex)
        {
            throw new DoesNotExistedBlException("order doesnot exist", ex);
        }
        double totalPrice = 0;
        foreach (DO.OrderItem item in orderitemsDal)
        {
            BO.OrderItem orderitem = new OrderItem();
            orderitem.ID = item.ID;
            orderitem.ProductID = item.ProductID;
            orderitem.Price = item.Price;
            orderitem.Amount = item.Amount;
            orderitem.TotalPrice = item.Amount * item.Price;
            try
            {
                product = dal.Product.GetById(orderitem.ProductID);
            }
            catch (DO.DoesNotExistedDalException ex)
            {
                throw new DoesNotExistedBlException("product does not exsit", ex);
            }
            orderitem.Name = product.Name;
            orderitems.Add(orderitem);
            totalPrice += orderitem.TotalPrice;

        }
        order.ID = orderDal.ID;
        order.CustomerName = orderDal.CustomerName;
        order.CustomerAdress = orderDal.CustomerAdress;
        order.CustomerEmail = orderDal.CustomerEmail;
        order.CreateOrderDate = orderDal.CreateOrderDate;
        order.ShippingDate = orderDal.ShippingDate;
        order.DeliveryDate = orderDal.DeliveryDate;
        order.TotalPrice = totalPrice;
        order.Items = orderitems;
        order.Status = OrderStatus.PROVIDED_ORDER;

        return order;
    }

    /// <summary>
    /// function that tracks the order
    /// </summary>
    /// <param name="id"></param>
    /// <returns>order tracking</returns>
    /// <exception cref="BO.DoesNotExistedBlException"></exception>
    public OrderTracking TrackingOrder(int id)
    {
        DO.Order order = new DO.Order();
        OrderTracking orderTracking = new OrderTracking();

        try
        {
            order = dal.Order.GetById(id);
        }
        catch (DO.DoesNotExistedDalException ex)
        {
            throw new DoesNotExistedBlException("order doesnot exist", ex);
        }
        orderTracking.ID = order.ID;
        if (order.DeliveryDate < DateTime.Now && order.DeliveryDate != DateTime.MinValue)
            orderTracking.Status = OrderStatus.PROVIDED_ORDER;
        else
        {
            if (order.ShippingDate < DateTime.Now && order.ShippingDate != DateTime.MinValue)
                orderTracking.Status = OrderStatus.SEND_ORDER;
            else
                orderTracking.Status = OrderStatus.CONFIRMED_ORDER;
        }

        List<Tuple<DateTime, string>> listTuples = new List<Tuple<DateTime, string>>

            {
                new Tuple<DateTime, string>(order.CreateOrderDate, "the order has been created")
             };
        if (order.ShippingDate != DateTime.MinValue)
        {
            listTuples.Add(new Tuple<DateTime, string>(order.ShippingDate, "the order has been sent"));
        }
        if (order.DeliveryDate != DateTime.MinValue)
        {
            listTuples.Add(new Tuple<DateTime, string>(order.DeliveryDate, "the order provided"));
        }
        orderTracking.Tuples = listTuples;

        return orderTracking;
    }

    /// <summary>
    ///function that updates the quantity of a product in the order
    /// </summary>
    /// <param name="orderId"></param>
    /// <param name="productId"></param>
    /// <param name="amount"></param>
    /// <returns>update order</returns>
    /// <exception cref="DoesNotExistedBlException"></exception>
    /// <exception cref="ImpossibleActionBlException"></exception>
    /// <exception cref="InvalidInputBlException"></exception>
    /// <exception cref="Exception"></exception>
    public BO.OrderItem UpdateAmountOfOProductInOrder(int orderId, int productId, int amount)
    {
        DO.OrderItem item = new DO.OrderItem();
        try
        {
            item = dal.OrderItem.GetByOrderIdAndProductId(orderId, productId);
        }
        catch (DO.DoesNotExistedDalException ex)
        {
            throw new DoesNotExistedBlException($"{ex.EntityName}  are does not exist", ex);
        }
        DO.Order order = new DO.Order();
        try
        {
            order = dal.Order.GetById(orderId);
        }
        catch (DO.DoesNotExistedDalException ex)
        {
            throw new DoesNotExistedBlException("order does not exist", ex);
        }
        DO.Product product = new DO.Product();
        BO.OrderItem orderItem = new OrderItem();
        if (order.ShippingDate != DateTime.MinValue && order.ShippingDate < DateTime.Now)
        {
            throw new ImpossibleActionBlException("It is not possible to update an order after it has been sent");
        }
        if (amount < 0)
            throw new InvalidInputBlException("invalid amount");
        try
        {
            product = dal.Product.GetById(item.ProductID);
        }
        catch (DO.DoesNotExistedDalException ex)
        {
            throw new BO.DoesNotExistedBlException("product does not exist", ex);
        }
        product.InStock += item.Amount;
        item.Amount = amount;
        try
        {
            dal.OrderItem.Update(item);
        }
        catch (DO.DoesNotExistedDalException ex)
        {
            throw new DoesNotExistedBlException($"{ex.EntityName} does not exist", ex);
        }
        product.InStock -= amount;
        try
        {
            dal.Product.Update(product);
        }
        catch (DO.DoesNotExistedDalException ex)
        {
            throw new DoesNotExistedBlException(" product does not exist", ex);
        }
        if (amount == 0)
        {
            try
            {
                dal.OrderItem.Delete(item.ID);
            }
            catch (DO.DoesNotExistedDalException ex)
            {
                throw new DoesNotExistedBlException("id does not exist", ex);
            }
            throw new Exception("The product remove from the order");
        }
        orderItem.ID = item.ID;
        orderItem.Amount = amount;
        orderItem.Price = item.Price;
        orderItem.TotalPrice = amount * item.Price;
        orderItem.ProductID = productId;
        orderItem.Name = product.Name;

        return orderItem;
    }
}
