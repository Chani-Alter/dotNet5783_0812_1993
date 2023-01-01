using BlApi;
using DalApi;
using BO;

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
            if (cart.Items == null)
            {
                cart.Items = new List<BO.OrderItem?>();
            }
            else
            {
                var result = cart.Items.FirstOrDefault(item => item?.ProductID == productId);
                if (result != null)
                    throw new ImpossibleActionBlException("product exist in cart");
            }

            DO.Product product = dal.Product.GetByCondition(prod => prod?.ID == productId);
            if (product.InStock <= 0)
                throw new ImpossibleActionBlException("product not exist in stock");

            cart.Items.Add(
                new BO.OrderItem
                {
                    Name = product.Name,
                    ProductID = productId,
                    Amount = 1,
                    Price = product.Price,
                    TotalPrice = product.Price
                });
            cart.TotalPrice += product.Price;

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
        try
        {
            if (cart.Items == null)
                throw new BO.ImpossibleActionBlException("There are no items in the cart");
            var result = cart.Items.FirstOrDefault(item => item?.ProductID == productId);

            if (result == null)
                throw new ImpossibleActionBlException("This item is not in the cart");
            DO.Product product = dal.Product.GetByCondition(prod => prod?.ID == productId);

            if (product.InStock < amount)
                throw new ImpossibleActionBlException("product not exist in stock");
            cart.TotalPrice -= (amount - result.Amount) * result.Price;
            if (amount == 0)
            {
                cart.Items.Remove(result);
            }
            else
            {
                var cartItems = from item in cart.Items
                                let orderItem = (BO.OrderItem)item
                                select new BO.OrderItem
                                {
                                    ID = orderItem.ID,
                                    Name = orderItem.Name,
                                    ProductID = orderItem.ProductID,
                                    Price = orderItem.Price,
                                    Amount = orderItem?.ProductID == productId ? amount : orderItem.Amount,
                                    TotalPrice = orderItem?.ProductID == productId ? amount * orderItem.Price : orderItem.TotalPrice
                                };
                cart.Items = (List<BO.OrderItem?>)cartItems;
            }
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
            if (cart.CustomerName == "" || cart.CustomerEmail == "" || cart.CustomerAdress == "")
                throw new InvalidInputBlException("Invalid details");
            if (cart.Items == null)
                throw new ImpossibleActionBlException("There are no items in the cart.");
            int id = dal.Order.Add(
                new DO.Order
                {
                    CreateOrderDate = DateTime.Now,
                    ShippingDate = null,
                    DeliveryDate = null
                });
            IEnumerable<DO.Product?> products = dal.Product.GetList();

            var result = from item in cart.Items
                         join product in products on item.ProductID equals product?.ID into empdept
                         from ed in empdept.DefaultIfEmpty()
                         select new
                         {
                             orderItem = new DO.OrderItem
                             {
                                 OrderID = id,
                                 ProductID = item?.ProductID ?? 0,
                                 Amount = item?.Amount > 0 ? item?.Amount ?? 0 : throw new ImpossibleActionBlException("invalid amount"),
                                 Price = item?.Price ?? 0
                             },
                             prod = new DO.Product
                             {
                                 ID = ed == null ? throw new ImpossibleActionBlException("product does not exist") : item?.ProductID ?? 0,
                                 Price = ed?.Price ?? 0,
                                 Category = ed?.Category ?? 0,
                                 InStock = ed?.InStock > item?.Amount ? ed?.InStock - item?.Amount ?? 0 : throw new ImpossibleActionBlException("amount not in stock "),
                                 Name = ed?.Name
                             }
                         };

            result.ToList().ForEach(res =>
            {
                dal.Product.Update(res.prod);
                dal.OrderItem.Add(res.orderItem);
            });
        }
        catch (DO.DoesNotExistedDalException ex)
        {
            throw new DoesNotExistedBlException($"{ex.EntityName} dosent exsit", ex);
        }
    }
}