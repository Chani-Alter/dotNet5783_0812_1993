using BO;

namespace BlImplementation;

/// <summary>
/// A class that implements the iorder interface
/// </summary>
internal class Order : BlApi.IOrder
{
    #region PUBLIC MEMBERS
    /// <summary>
    /// function that returns the orders
    /// </summary>
    /// <returns>list of orders</returns>
    /// <exception cref="DoesNotExistedBlException"></exception>
    public IEnumerable<OrderForList> GetOrderList()
    {
        try
        {
            IEnumerable<DO.Order?> orders = dal.Order.GetList();

            return from ord in orders
                   let order = (DO.Order)ord
                   let orderItems = dal.OrderItem.GetList(item => item?.OrderID == order.ID)
                   let amount = orderItems.Sum(item => ((DO.OrderItem)item!).Amount)
                   let totalPrice = orderItems.Sum(item => ((DO.OrderItem)item!).Price * ((DO.OrderItem)item!).Amount)
                   select new OrderForList
                   {
                       ID = order.ID,
                       CustomerName = order.CustomerName,
                       AmountOfItems = amount,
                       TotalPrice = totalPrice,
                       Status = order.DeliveryDate != null && order.DeliveryDate < DateTime.Now ? OrderStatus.ProvidedOrder :
                       order.ShippingDate != null && order.ShippingDate < DateTime.Now ? OrderStatus.SendOrder : OrderStatus.ConfirmedOrder

                   };
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

        try
        {
            DO.Order orderDal = dal.Order.GetByCondition(ord => ord?.ID == id);
            return returnBOOrder(orderDal);
        }
        catch (DO.DoesNotExistedDalException ex)
        {
            throw new DoesNotExistedBlException("order doesnot exist", ex);
        }
    }


    /// <summary>
    /// function that update the send order
    /// </summary>
    /// <param name="id"></param>
    /// <returns>updated order</returns>
    /// <exception cref="ImpossibleActionBlException"></exception>
    /// <exception cref="UpdateErrorBlException"></exception>
    /// <exception cref="DoesNotExistedBlException"></exception>
    public BO.Order UpdateSendOrderByManager(int id)
    {
        try
        {
            DO.Order orderDal = dal.Order.GetByCondition(ord => ord?.ID == id);
            if (orderDal.ShippingDate != null && orderDal.ShippingDate < DateTime.Now)
                throw new ImpossibleActionBlException("order send");
            orderDal.ShippingDate = DateTime.Now;
            try
            {
                dal.Order.Update(orderDal);
            }
            catch (DO.DoesNotExistedDalException ex)
            {
                throw new UpdateErrorBlException("order update failes", ex);
            }
            return returnBOOrder(orderDal);
        }
        catch (DO.DoesNotExistedDalException ex)
        {
            throw new DoesNotExistedBlException("order does not exist", ex);
        }

    }


    /// <summary>
    ///  function that update the supply order
    /// </summary>
    /// <param name="id"></param>
    /// <returns>updated order</returns>
    /// <exception cref="BO.ImpossibleActionBlException"></exception>
    /// <exception cref="UpdateErrorBlException"></exception>
    /// <exception cref="DoesNotExistedBlException"></exception>
    public BO.Order UpdateSupplyOrderByManager(int id)
    {
        try
        {
            DO.Order orderDal = dal.Order.GetByCondition(ord => ord?.ID == id);

            if (orderDal.DeliveryDate != null && orderDal.DeliveryDate < DateTime.Now)
                throw new BO.ImpossibleActionBlException("order Delivery");

            if (orderDal.ShippingDate == null || orderDal.ShippingDate > DateTime.Now)
                throw new BO.ImpossibleActionBlException("It is not possible to update a delivery date before a shipping date");

            orderDal.DeliveryDate = DateTime.Now;
            try
            {
                dal.Order.Update(orderDal);
            }
            catch (DO.DoesNotExistedDalException ex)
            {
                throw new UpdateErrorBlException("order update failes", ex);
            }
            return returnBOOrder(orderDal);

        }
        catch (DO.DoesNotExistedDalException ex)
        {
            throw new DoesNotExistedBlException("order does not exist", ex);
        }

    }


    /// <summary>
    /// function that tracks the order
    /// </summary>
    /// <param name="id"></param>
    /// <returns>order tracking</returns>
    /// <exception cref="BO.DoesNotExistedBlException"></exception>
    public OrderTracking TrackingOrder(int id)
    {
        try
        {
            DO.Order order = dal.Order.GetByCondition(ord => ord?.ID == id);

            List<Tuple<DateTime?, string?>?>? listTuples = new List<Tuple<DateTime?, string?>?>
            {
               new Tuple<DateTime?, string?>(order.CreateOrderDate, "the order has been created"),
               order.ShippingDate != null ? new Tuple<DateTime?, string?>(order.ShippingDate, "the order has been sent"): null,
               order.DeliveryDate != null?new Tuple<DateTime?, string?>(order.DeliveryDate, "the order provided"):null
            };

            return new OrderTracking
            {
                ID = order.ID,
                Status = order.DeliveryDate != null && order.DeliveryDate < DateTime.Now ? OrderStatus.ProvidedOrder :
                order.ShippingDate != null && order.ShippingDate < DateTime.Now ? OrderStatus.SendOrder :
                OrderStatus.ConfirmedOrder,
                Tuples = listTuples

            };
        }
        catch (DO.DoesNotExistedDalException ex)
        {
            throw new DoesNotExistedBlException("order doesnot exist", ex);
        }
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

        try
        {
            DO.OrderItem ordItem = dal.OrderItem.GetByCondition(item => ((DO.OrderItem)item!).OrderID == orderId && ((DO.OrderItem)item!).ProductID == productId);
            DO.Order orderDal = dal.Order.GetByCondition(ord => ord?.ID == orderId);
            DO.Product product = dal.Product.GetByCondition(prod => prod?.ID == productId);

            if (orderDal.ShippingDate != null && orderDal.ShippingDate < DateTime.Now)
                throw new ImpossibleActionBlException("It is not possible to update an order after it has been sent");

            if (amount < 0)
                throw new InvalidInputBlException("invalid amount");

            product.InStock -= amount - ordItem.Amount;
            ordItem.Amount = amount;

            try
            {
                dal.OrderItem.Update(ordItem);
                dal.Product.Update(product);
            }
            catch (DO.DoesNotExistedDalException ex)
            {
                throw new UpdateErrorBlException($"{ex.EntityName} update fails", ex);
            }

            if (amount == 0)
            {
                dal.OrderItem.Delete(ordItem.ID);
                throw new Exception("The product remove from the order");
            }

            return new BO.OrderItem
            {
                ID = ordItem.ID,
                Amount = amount,
                Price = ordItem.Price,
                TotalPrice = amount * ordItem.Price,
                ProductID = productId,
                Name = product.Name
            };

        }
        catch (DO.DoesNotExistedDalException ex)
        {
            throw new DoesNotExistedBlException($"{ex.EntityName} does not exist", ex);
        }
    }
    #endregion

    #region PRIVATE MEMMBER

    /// <summary>
    /// An attribute that contains access to all the dallist data
    /// </summary>
    DalApi.IDal? dal = DalApi.Factory.Get();

    /// <summary>
    /// convert a DO.Order to a BO.Order 
    /// </summary>
    /// <param name="orderDal">a DO.order</param>
    /// <returns>a BO.order</returns>
    /// <exception cref="DoesNotExistedBlException"></exception>
    private BO.Order returnBOOrder(DO.Order orderDal)
    {
        try
        {
            IEnumerable<DO.OrderItem?> orderItemsDal = dal.OrderItem.GetList(ord => ord?.OrderID == orderDal.ID);

            var orderItems = from item in orderItemsDal
                             let orderItem = (DO.OrderItem)item
                             let product = dal.Product.GetByCondition(prod => prod?.ID == orderItem.ProductID)
                             select new BO.OrderItem
                             {
                                 ID = orderItem.ID,
                                 ProductID = orderItem.ProductID,
                                 Price = orderItem.Price,
                                 Amount = orderItem.Amount,
                                 TotalPrice = orderItem.Amount * orderItem.Price,
                                 Name = product.Name
                             };

            return new BO.Order
            {
                ID = orderDal.ID,
                CustomerName = orderDal.CustomerName,
                CustomerAdress = orderDal.CustomerAdress,
                CustomerEmail = orderDal.CustomerEmail,
                CreateOrderDate = orderDal.CreateOrderDate,
                ShippingDate = orderDal.ShippingDate,
                DeliveryDate = orderDal.DeliveryDate,
                TotalPrice = orderItems.Sum(item => item.TotalPrice),
                Items = orderItems.ToList(),
                Status = orderDal.DeliveryDate != null && orderDal.DeliveryDate < DateTime.Now ? OrderStatus.ProvidedOrder :
                orderDal.ShippingDate != null && orderDal.ShippingDate < DateTime.Now ? OrderStatus.SendOrder : OrderStatus.ConfirmedOrder

            };
        }
        catch (DO.DoesNotExistedDalException ex)
        {
            throw new DoesNotExistedBlException("order items doesnot exist", ex);

        }

    }

    #endregion
}