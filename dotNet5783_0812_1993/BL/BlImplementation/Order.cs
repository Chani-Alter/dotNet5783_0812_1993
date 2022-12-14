using System;
using BlApi;
using DalApi;
using System.Collections.Generic;
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
    /// <exception cref="BO.BLDoesNotExistException"></exception>
    public IEnumerable<BO.OrderForList> GetOrderList()
    {
        try
        {
            List<BO.OrderForList> ordersForList = new List<BO.OrderForList>();
            IEnumerable<DO.Order> orders = dal.Order.GetAll();
            double totalPrice = 0;
            int amount = 0;
            foreach (DO.Order order in orders)
            {
                BO.OrderForList orderForList = new BO.OrderForList();

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


                if (order.DeliveryrDate < DateTime.Now && order.DeliveryrDate != DateTime.MinValue)
                    orderForList.Status = BO.OrderStatus.PROVIDED_ORDER;
                else
                {
                    if (order.ShipDate < DateTime.Now && order.ShipDate != DateTime.MinValue)
                        orderForList.Status = BO.OrderStatus.SEND_ORDER;
                    else
                        orderForList.Status = BO.OrderStatus.CONFIRMED_ORDER;
                }
                ordersForList.Add(orderForList);
            }

            return ordersForList;
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BLDoesNotExistException("order doesnot exist", ex);
        }
    }
    /// <summary>
    /// function that returns order by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>order</returns>
    /// <exception cref="BO.BLDoesNotExistException"></exception>
    public BO.Order GetOrderById(int id)
    {

        BO.Order order = new BO.Order();
        DO.Product product = new DO.Product();
        DO.Order orderDal = new DO.Order();
        List<BO.OrderItem> orderitems = new List<BO.OrderItem>();
        IEnumerable<DO.OrderItem> orderitemsDal;
        try
        {
            orderDal = dal.Order.GetById(id);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BLDoesNotExistException("order doesnot exist", ex);
        }
        try
        {
            orderitemsDal = dal.OrderItem.GetAllItemsByOrderId(id);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BLDoesNotExistException("order doesnot exist", ex);
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
            catch (DO.DalDoesNotExistException ex)
            {
                throw new BO.BLDoesNotExistException("order does not exist", ex);
            }
            orderitem.Name = product.Name;
            orderitems.Add(orderitem);
            totalPrice += orderitem.TotalPrice;

        }
        order.ID = orderDal.ID;
        order.CustomerName = orderDal.CustomerName;
        order.CustomerAddress = orderDal.CustomerAdress;
        order.CustomerEmail = orderDal.CustomerEmail;
        order.OrderDate = orderDal.OrderDate;
        order.ShipDate = orderDal.ShipDate;
        order.DeliveryDate = orderDal.DeliveryrDate;
        order.TotalPrice = totalPrice;
        order.Items = orderitems;
        if (orderDal.DeliveryrDate < DateTime.Now && orderDal.DeliveryrDate != DateTime.MinValue)
            order.Status = BO.OrderStatus.PROVIDED_ORDER;
        else
        {
            if (order.ShipDate < DateTime.Now && orderDal.ShipDate != DateTime.MinValue)
                order.Status = BO.OrderStatus.SEND_ORDER;
            else
                order.Status = BO.OrderStatus.CONFIRMED_ORDER;
        }
        return order;



    }
    /// <summary>
    /// function that update the send order
    /// </summary>
    /// <param name="id"></param>
    /// <returns>update order</returns>
    /// <exception cref="BO.BLDoesNotExistException"></exception>
    /// <exception cref="BO.BLImpossibleActionException"></exception>
    /// <exception cref="BO.BLMistakeUpdateException"></exception>
    public BO.Order UpdateSendOrderByManager(int id)
    {
        BO.Order order = new BO.Order();
        DO.Product product = new DO.Product();
        DO.Order orderDal = new DO.Order();
        List<BO.OrderItem> orderitems = new List<BO.OrderItem>();
        IEnumerable<DO.OrderItem> orderitemsDal = new List<DO.OrderItem>();
        try
        {
            orderDal = dal.Order.GetById(id);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BLDoesNotExistException("order does not exist", ex);
        }
        if (orderDal.ShipDate < DateTime.Now && orderDal.ShipDate != DateTime.MinValue)
            throw new BO.BLImpossibleActionException("order send");
        else
        {
            if (orderDal.ShipDate == DateTime.MinValue)
                throw new BO.BLMistakeUpdateException("No shipping date");
            orderDal.ShipDate = DateTime.Now;
            try
            {
                dal.Order.Update(orderDal);
            }
            catch (DO.DalDoesNotExistException ex)
            {
                throw new BO.BLDoesNotExistException("order does not exist", ex);
            }
        }
        try
        {
            orderitemsDal = dal.OrderItem.GetAllItemsByOrderId(id);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BLDoesNotExistException("order doesnot exist", ex);
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
            catch (DO.DalDoesNotExistException ex)
            {
                throw new BO.BLDoesNotExistException("product doesnot exist", ex);
            }

            orderitem.Name = product.Name;
            orderitems.Add(orderitem);
            totalPrice += orderitem.TotalPrice;

        }
        order.ID = orderDal.ID;
        order.CustomerName = orderDal.CustomerName;
        order.CustomerAddress = orderDal.CustomerAdress;
        order.CustomerEmail = orderDal.CustomerEmail;
        order.OrderDate = orderDal.OrderDate;
        order.ShipDate = orderDal.ShipDate;
        order.DeliveryDate = orderDal.DeliveryrDate;
        order.TotalPrice = totalPrice;
        order.Items = orderitems;
        order.Status = BO.OrderStatus.SEND_ORDER;
        return order;
    }
    /// <summary>
    ///  function that update the supply order
    /// </summary>
    /// <param name="id"></param>
    /// <returns>update order</returns>
    /// <exception cref="BO.BLDoesNotExistException"></exception>
    /// <exception cref="BO.BLImpossibleActionException"></exception>
    public BO.Order UpdateSupplyOrderByManager(int id)
    {
        BO.Order order = new BO.Order();
        DO.Product product = new DO.Product();
        List<BO.OrderItem> orderitems = new List<BO.OrderItem>();
        DO.Order orderDal = new DO.Order();
        IEnumerable<DO.OrderItem> orderitemsDal = new List<DO.OrderItem>();

        try
        {
            orderDal = dal.Order.GetById(id);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BLDoesNotExistException("order doesnot exist", ex);
        }
        if (orderDal.DeliveryrDate < DateTime.Now && orderDal.DeliveryrDate != DateTime.MinValue)
            throw new BO.BLImpossibleActionException("order Delivery");
        else
        {

            if (orderDal.ShipDate > DateTime.Now)
                throw new BO.BLImpossibleActionException("It is not possible to update a delivery date before a shipping date");
            else
            {
                if (orderDal.DeliveryrDate == DateTime.MinValue)
                    throw new BO.BLImpossibleActionException(" No delivery date");
            }
            orderDal.DeliveryrDate = DateTime.Now;
            try
            {
                dal.Order.Update(orderDal);
            }
            catch (DO.DalDoesNotExistException ex)
            {
                throw new BO.BLDoesNotExistException("order dosent exsit", ex);
            }
        }
        try
        {
            orderitemsDal = dal.OrderItem.GetAllItemsByOrderId(id);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BLDoesNotExistException("order doesnot exist", ex);
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
            catch (DO.DalDoesNotExistException ex)
            {
                throw new BO.BLDoesNotExistException("product does not exsit", ex);
            }
            orderitem.Name = product.Name;
            orderitems.Add(orderitem);
            totalPrice += orderitem.TotalPrice;

        }
        order.ID = orderDal.ID;
        order.CustomerName = orderDal.CustomerName;
        order.CustomerAddress = orderDal.CustomerAdress;
        order.CustomerEmail = orderDal.CustomerEmail;
        order.OrderDate = orderDal.OrderDate;
        order.ShipDate = orderDal.ShipDate;
        order.DeliveryDate = orderDal.DeliveryrDate;
        order.TotalPrice = totalPrice;
        order.Items = orderitems;
        order.Status = BO.OrderStatus.PROVIDED_ORDER;
        return order;

    }
    /// <summary>
    /// function that tracks the order
    /// </summary>
    /// <param name="id"></param>
    /// <returns>order tracking</returns>
    /// <exception cref="BO.BLDoesNotExistException"></exception>
    public BO.OrderTracking TrackingOrder(int id)
    {
        DO.Order order = new DO.Order();
        BO.OrderTracking orderTracking = new BO.OrderTracking();
        try
        {
            order = dal.Order.GetById(id);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BLDoesNotExistException("order doesnot exist", ex);
        }
        orderTracking.ID = order.ID;
        if (order.DeliveryrDate < DateTime.Now && order.DeliveryrDate != DateTime.MinValue)
            orderTracking.Status = BO.OrderStatus.PROVIDED_ORDER;
        else
        {
            if (order.ShipDate < DateTime.Now && order.ShipDate != DateTime.MinValue)
                orderTracking.Status = BO.OrderStatus.SEND_ORDER;
            else
                orderTracking.Status = BO.OrderStatus.CONFIRMED_ORDER;
        }
        List<Tuple<DateTime, string>> tList = new List<Tuple<DateTime, string>>
            {
                new Tuple<DateTime, string>(order.OrderDate, "the order has been created")
             };
        if (order.ShipDate != DateTime.MinValue)
        {
            tList.Add(new Tuple<DateTime, string>(order.ShipDate, "the order has been sent"));
        }
        if (order.DeliveryrDate != DateTime.MinValue)
        {
            tList.Add(new Tuple<DateTime, string>(order.DeliveryrDate, "the order provided"));
        }
        orderTracking.Tuples = tList;
        return orderTracking;

    }
    /// <summary>
    /// Bonus:function that updates the quantity of a product in the order
    /// </summary>
    /// <param name="idOrder"></param>
    /// <param name="idProduct"></param>
    /// <param name="amount"></param>
    /// <returns>update order</returns>
    /// <exception cref="BO.BLDoesNotExistException"></exception>
    /// <exception cref="BO.BLImpossibleActionException"></exception>
    /// <exception cref="BO.BLInvalidInputException"></exception>
    /// <exception cref="Exception"></exception>
    public BO.OrderItem UpdateAmountOfOProductInOrder(int idOrder, int idProduct, int amount)
    {
        DO.OrderItem item = new DO.OrderItem();
        try
        {
            item = dal.OrderItem.GetByOrderIdAndProductId(idOrder, idProduct);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BLDoesNotExistException($"{ex.EntityName}  are does not exist", ex);
        }
        DO.Order order = new DO.Order();
        try
        {
            order = dal.Order.GetById(idOrder);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BLDoesNotExistException("order does not exist", ex);
        }
        DO.Product product = new DO.Product();
        BO.OrderItem orderItem = new BO.OrderItem();
        if (order.ShipDate != DateTime.MinValue && order.ShipDate < DateTime.Now)
        {
            throw new BO.BLImpossibleActionException("It is not possible to update an order after it has been sent");
        }
        if (amount < 0)
            throw new BO.BLInvalidInputException("invalid amount");
        try
        {
            product = dal.Product.GetById(item.ProductID);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BLDoesNotExistException("product does not exist", ex);
        }
        product.InStock += item.Amount;
        item.Amount = amount;
        try
        {
            dal.OrderItem.Update(item);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BLDoesNotExistException($"{ex.EntityName} does not exist", ex);
        }
        product.InStock -= amount;
        try
        {
            dal.Product.Update(product);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BLDoesNotExistException(" product does not exist", ex);
        }
        if (amount == 0)
        {
            try
            {
                dal.OrderItem.Delete(item.ID);
            }
            catch (DO.DalDoesNotExistException ex)
            {
                throw new BO.BLDoesNotExistException("id does not exist", ex);
            }
            throw new Exception("The product remove from the order");
        }
        orderItem.ID = item.ID;
        orderItem.Amount = amount;
        orderItem.Price = item.Price;
        orderItem.TotalPrice = amount * item.Price;
        orderItem.ProductID = idProduct;
        orderItem.Name = product.Name;
        return orderItem;
    }
}
