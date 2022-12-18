using BlApi;
using BO;
using DalApi;

namespace BlImplementation;

/// <summary>
/// A class that implements the icart interface
/// </summary>
internal class Cart : ICart
{
    /// <summary>
    /// An attribute that contains access to all the dallist data
    /// </summary>
    private IDal dal = new DalList.DalList();

    /// <summary>
    ///Adding product to the cart
    /// </summary>
    /// <param name="cart"></param>
    /// <param name="productId"></param>
    /// <returns>the cart after adding</returns>
    /// <exception cref="BO.ImpossibleActionBlException"></exception>
    /// <exception cref="BO.DoesNotExistedBlException"></exception>
    public BO.Cart AddProductToCart(BO.Cart cart, int productId)
    {
        try
        {
            DO.Product product = new DO.Product();
            OrderItem orderItem = new OrderItem();
            if (cart.Items != null)
            {
                foreach (OrderItem item in cart.Items)
                {
                    if (item.ProductID == productId)
                        throw new ImpossibleActionBlException("product elredy exist in cart");
                }
            }
            else
            {
                cart.Items = new List<OrderItem>();
            }

            product = dal.Product.GetById(productId);
            if (product.InStock <= 0)
                throw new ImpossibleActionBlException("product dos'nt exist in stock");
            
            orderItem.Name = product.Name;
            orderItem.ProductID = productId;
            orderItem.Amount = 1;
            orderItem.Price = product.Price;
            orderItem.TotalPrice = orderItem.Amount * orderItem.Price;
            cart.Items.Add(orderItem);
            cart.TotalPrice += orderItem.TotalPrice;

            return cart;

        }
        catch (DO.DoesNotExistedDalException ex)
        {
            throw new DoesNotExistedBlException("product dosent exsit", ex);
        }
    }

    /// <summary>
    ///function that update the amount of product in the cart
    /// </summary>
    /// <param name="cart"></param>
    /// <param name="productId"></param>
    /// <param name="amount"></param>
    /// <returns>update curt</returns>
    /// <exception cref="BO.ImpossibleActionBlException"></exception>
    /// <exception cref="BO.InvalidInputBlException"></exception>
    /// <exception cref="ImpossibleActionBlException"></exception>
    /// <exception cref="BO.DoesNotExistedBlException"></exception>
    public BO.Cart UpdateProductAmountInCart(BO.Cart cart, int productId, int amount)
    {
        bool boolean = false;
        try
        {
            DO.Product product = new DO.Product();
            product = dal.Product.GetById(productId);

            if (product.InStock < amount)
                throw new ImpossibleActionBlException("product not exist in stock");
            
            if (cart.Items != null)
            {
                foreach (OrderItem orderItem in cart.Items)
                {

                    if (orderItem.ProductID == productId)
                    {
                        boolean = true;
                        if (amount < 0)
                        {
                            throw new InvalidInputBlException("invalid amount");
                        }
                        else
                        {
                            if (amount == 0)
                            {
                                cart.TotalPrice -= orderItem.TotalPrice;
                                cart.Items.Remove(orderItem);
                            }
                            else
                            {
                                cart.TotalPrice -= orderItem.TotalPrice;
                                orderItem.Amount = amount;
                                orderItem.TotalPrice = orderItem.Price * amount;
                                cart.TotalPrice += orderItem.TotalPrice;
                            }
                        }
                    }
                }
                if (boolean == false)
                {
                    throw new ImpossibleActionBlException("This item is not in the cart");
                }
            }
            else
                throw new ImpossibleActionBlException("There are no items in the cart");
            return cart;
        }
        catch (DO.DoesNotExistedDalException ex)
        {
            throw new DoesNotExistedBlException("product dosent exsit", ex);
        }
    }

    /// <summary>
    /// function that confirms an order
    /// </summary>
    /// <param name="cart"></param>
    /// <exception cref="BO.InvalidInputBlException"></exception>
    /// <exception cref="ImpossibleActionBlException"></exception>
    /// <exception cref="BO.DoesNotExistedBlException"></exception>
    /// <exception cref="BO.ImpossibleActionBlException"></exception>
    public void MakeOrder(BO.Cart cart)
    {
        try
        {
            DO.Order order = new DO.Order();
            DO.Product product = new DO.Product();
            if (cart.CustomerName == "" || cart.CustomerEmail == "" || cart.CustomerAdress == "")
            {
                throw new InvalidInputBlException("Invalid details");
            }
            if (cart.Items == null)
                throw new ImpossibleActionBlException("There are no items in the cart.");
            order.CreateOrderDate = DateTime.Now;
            order.ShippingDate = new DateTime();
            order.DeliveryDate = new DateTime();
            int id = dal.Order.Add(order);
            foreach (OrderItem orderItem in cart.Items)
            {
                try
                {
                    product = dal.Product.GetById(orderItem.ProductID);
                }
                catch (DO.DoesNotExistedDalException ex)
                {
                    throw new BO.DoesNotExistedBlException("product dosent exsit", ex);
                }
                if (orderItem.Amount <= 0)
                    throw new ImpossibleActionBlException("invalid amount");
                if (product.InStock < orderItem.Amount)
                    throw new ImpossibleActionBlException("amount not in stock ");
                DO.OrderItem orderItem1 = new DO.OrderItem();
                orderItem1.OrderID = id;
                orderItem1.ProductID = orderItem.ProductID;
                orderItem1.Amount = orderItem.Amount;
                orderItem1.Price = orderItem.Price;
                product.InStock -= orderItem.Amount;
                try
                {
                    dal.Product.Update(product);
                }
                catch (DO.DoesNotExistedDalException ex)
                {
                    throw new DoesNotExistedBlException("product dosent exsit", ex);
                }
                try
                {
                    dal.OrderItem.Add(orderItem1);
                }
                catch (DO.DoesNotExistedDalException ex)
                {
                    throw new DoesNotExistedBlException($"{ex.EntityName} dosent exsit", ex);
                }
            }
        }
        catch (DO.DoesNotExistedDalException ex)
        {
            throw new DoesNotExistedBlException("product dosent exsit", ex);
        }
    }
}
