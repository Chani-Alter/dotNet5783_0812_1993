using System;
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
    /// <param name="idProduct"></param>
    /// <returns>the cart after adding</returns>
    /// <exception cref="BO.ImpossibleActionBlException"></exception>
    /// <exception cref="BO.DoesNotExistedBlException"></exception>
    public BO.Cart AddProductToCart(BO.Cart cart, int idProduct)
    {
        try
        {
            DO.Product product = new DO.Product();
            BO.OrderItem orderItem1 = new BO.OrderItem();
            if (cart.Items != null)
            {
                foreach (BO.OrderItem orderItem in cart.Items)
                {
                    if (orderItem.ProductID == idProduct)
                        throw new BO.ImpossibleActionBlException("product exist in cart");
                }
            }
            else
            {
                cart.Items = new List<BO.OrderItem>();
            }
            product = dal.Product.GetById(idProduct);
            if (product.InStock <= 0)
                throw new BO.ImpossibleActionBlException("product not exist in stock");
            orderItem1.Name = product.Name;
            orderItem1.ProductID = idProduct;
            orderItem1.Amount = 1;
            orderItem1.Price = product.Price;
            orderItem1.TotalPrice = orderItem1.Price * orderItem1.Amount;
            cart.Items.Add(orderItem1);
            cart.TotalPrice += orderItem1.TotalPrice;
            return cart;
        }
        catch (DO.DoesNotExistedDalException ex)
        {
            throw new BO.DoesNotExistedBlException("product dosent exsit", ex);
        }
    }
    /// <summary>
    ///function that update the amount of product in the cart
    /// </summary>
    /// <param name="cart"></param>
    /// <param name="idProduct"></param>
    /// <param name="amount"></param>
    /// <returns>update curt</returns>
    /// <exception cref="BO.ImpossibleActionBlException"></exception>
    /// <exception cref="BO.InvalidInputBlException"></exception>
    /// <exception cref="ImpossibleActionBlException"></exception>
    /// <exception cref="BO.DoesNotExistedBlException"></exception>
    public BO.Cart UpdateProductAmountInCart(BO.Cart cart, int idProduct, int amount)
    {
        bool flag = false;
        try
        {
            DO.Product product = new DO.Product();
            product = dal.Product.GetById(idProduct);
            if (product.InStock < amount)
                throw new BO.ImpossibleActionBlException("product not exist in stock");
            if (cart.Items != null)
            {
                foreach (BO.OrderItem orderItem in cart.Items)
                {

                    if (orderItem.ProductID == idProduct)
                    {
                        flag = true;
                        if (amount < 0)
                        {
                            throw new BO.InvalidInputBlException("invalid amount");
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
                if (flag == false)
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
            throw new BO.DoesNotExistedBlException("product dosent exsit", ex);
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
                throw new BO.InvalidInputBlException("Invalid details");
            }
            if (cart.Items == null)
                throw new ImpossibleActionBlException("There are no items in the cart.");
            order.CreateOrderDate = DateTime.Now;
            order.ShippingDate = new DateTime();
            order.DeliveryDate = new DateTime();
            int id = dal.Order.Add(order);
            foreach (BO.OrderItem orderItem in cart.Items)
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
                    throw new BO.ImpossibleActionBlException("invalid amount");
                if (product.InStock < orderItem.Amount)
                    throw new BO.ImpossibleActionBlException("amount not in stock ");
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
                    throw new BO.DoesNotExistedBlException("product dosent exsit", ex);
                }
                try
                {
                    dal.OrderItem.Add(orderItem1);
                }
                catch (DO.DoesNotExistedDalException ex)
                {
                    throw new BO.DoesNotExistedBlException($"{ex.EntityName} dosent exsit", ex);
                }
            }
        }
        catch (DO.DoesNotExistedDalException ex)
        {
            throw new BO.DoesNotExistedBlException("product dosent exsit", ex);
        }

    }

}
