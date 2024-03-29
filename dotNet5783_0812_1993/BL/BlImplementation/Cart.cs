﻿using BlApi;
using BO;
using System.Runtime.CompilerServices;

namespace BlImplementation;

/// <summary>
/// A class that implements the icart interface
/// </summary>
internal class Cart : ICart
{
    #region PUBLIC MEMBERS

    [MethodImpl(MethodImplOptions.Synchronized)]
    /// <summary>
    ///Adding product to the cart
    /// </summary>
    /// <param name="cart"></param>
    /// <param name="productId"></param>
    /// <returns>the cart after adding</returns>
    /// <exception cref="BO.ImpossibleActionBlException"></exception>
    /// <exception cref="BO.DoesNotExistedBlException"></exception>
    public BO.Cart AddProductToCart(BO.Cart cart, int productId, int amount)
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

            int itemId = 0;

            if (cart.ID != 0)
                try
                {
                  itemId = dal.CartItem.Add(new DO.CartItem
                    {
                        Amount = amount,
                        CartID = cart.ID,
                        Price = product.Price,
                        ProductID = productId,
                    });

                }catch(DO.DuplicateDalException)
                {
                    throw new ImpossibleActionBlException("product exist in cart");
                }

            cart.Items.Add(
                new BO.OrderItem
                {
                    ID= itemId,
                    Name = product.Name,
                    ProductID = productId,
                    Amount = amount,
                    Price = product.Price,
                    TotalPrice = product.Price
                });
            cart.TotalPrice += product.Price * amount;

            return cart;
        }
        catch (DO.DoesNotExistedDalException ex)
        {
            throw new DoesNotExistedBlException("product dosent exsit", ex);
        }
        catch (DO.XMLFileNullExeption ex)
        {
            throw new DoesNotExistedBlException("product dosent exsit", ex);
        }
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
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
                throw new ImpossibleActionBlException("There are no items in the cart");

            var result = cart.Items.FirstOrDefault(item => item?.ProductID == productId);

            if (result == null)
                throw new ImpossibleActionBlException("This item is not in the cart");

            DO.Product product = dal.Product.GetByCondition(prod => prod?.ID == productId);

            if (product.InStock < amount)
                throw new ImpossibleActionBlException("product not exist in stock");

            cart.TotalPrice += (amount - result.Amount) * result.Price;

            if (amount == 0)
            {
                cart.Items.Remove(result);
                if (cart.ID != 0)
                    dal.CartItem.Delete(result.ID);
            }
            else
            {
                var cartItems = from item in cart.Items
                                let orderItem = item
                                select new BO.OrderItem
                                {
                                    ID = orderItem.ID,
                                    Name = orderItem.Name,
                                    ProductID = orderItem.ProductID,
                                    Price = orderItem.Price,
                                    Amount = orderItem?.ProductID == productId ? amount : orderItem.Amount,
                                    TotalPrice = orderItem?.ProductID == productId ? amount * orderItem.Price : orderItem.TotalPrice
                                };

                cart.Items = cartItems.ToList();
                if (cart.ID != 0)
                    dal.CartItem.Update(new DO.CartItem {
                        ID=result.ID,
                        Amount= result.Amount,
                        Price= result.Price,
                        CartID= cart.ID,
                        ProductID =result.ProductID
                    });
            }

            return cart;
        }
        catch (DO.DoesNotExistedDalException ex)
        {
            throw new DoesNotExistedBlException("product dosent exsit", ex);
        }
        catch (DO.XMLFileNullExeption ex)
        {
            throw new DoesNotExistedBlException("product dosent exsit", ex);
        }
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
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

            lock (dal)
            {
                int id = dal.Order.Add(
                    new DO.Order
                    {
                        CustomerAdress = cart.CustomerAdress,
                        CustomerEmail = cart.CustomerEmail,
                        CustomerName = cart.CustomerName,
                        CreateOrderDate = DateTime.Now,
                        ShippingDate = null,
                        DeliveryDate = null
                    });

                IEnumerable<DO.Product?> products = dal.Product.GetList();

                //return a list of tuples that everyone ave a orderItem to add and a updated product
                var result = from item in cart.Items
                             join prod in products on item.ProductID equals prod?.ID into empdept
                             from ed in empdept.DefaultIfEmpty()
                             let product = ed ?? throw new ImpossibleActionBlException("product does not exist")
                             let orderItem = item!
                             select new
                             {
                                 orderItem = new DO.OrderItem
                                 {
                                     OrderID = id,
                                     ProductID = item.ProductID,
                                     Amount = item.Amount > 0 ? item.Amount : throw new ImpossibleActionBlException("invalid amount"),
                                     Price = item.Price
                                 },
                                 prod = new DO.Product
                                 {
                                     ID = product.ID,
                                     Price = product.Price,
                                     Category = product.Category,
                                     InStock = product.InStock > item?.Amount ? product.InStock - item?.Amount ?? 0 : throw new ImpossibleActionBlException("amount not in stock "),
                                     Name = product.Name
                                 }
                             };

                //for each tuple we update the product list and add a orderItem to the order Item list
                result.ToList().ForEach(res =>
                {
                    try
                    {
                        dal.Product.Update(res.prod);
                    }
                    catch (DO.DoesNotExistedDalException ex)
                    {
                        throw new UpdateErrorBlException("product update failes", ex);
                    }
                    try
                    {
                        dal.OrderItem.Add(res.orderItem);
                    }
                    catch (DO.DuplicateDalException ex)
                    {
                        throw new BLAlreadyExistException("order Item alredy exist ", ex);
                    }
                });

                if (cart.ID != 0)
                {
                    var cartitems = dal.CartItem.GetList(item => item?.CartID == cart.ID);
                    cartitems.ToList().ForEach(res => dal.CartItem.Delete(((DO.CartItem)res!).ID));
                    dal.Cart.Delete(cart.ID);
                }
            }
        }
        catch (DO.DoesNotExistedDalException ex)
        {
            throw new DoesNotExistedBlException($"{ex.EntityName} dosent exsit", ex);
        }
        catch (DO.XMLFileNullExeption ex)
        {
            throw new DoesNotExistedBlException("product dosent exsit", ex);
        }
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public BO.Cart GetUserCart(int id)
    {
        if (id == 0)
            return new BO.Cart();
        else
        {
            try {
                var result = dal.Cart.GetList(c => c?.ID == id);
                DO.User user = dal.User.GetByCondition(u => u?.ID == id);
                if (result.Count() == 0)
                    return creatNewCart(user);
                DO.Cart? cart = result.FirstOrDefault();
                return creatNewCart((DO.Cart)cart!, user);
            }
            catch (DO.XMLFileNullExeption ex)
            {
                throw new DoesNotExistedBlException("cart doesnt exist " ,ex);
            }
         }
    }

    #endregion

    #region PRIVATE MEMBER

    /// <summary>
    /// An attribute that contains access to all the dallist data
    /// </summary>
    DalApi.IDal? dal = DalApi.Factory.Get();

    /// <summary>
    /// create new cart for a registered user
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    private BO.Cart creatNewCart(DO.User user)
    {
        int cartid = dal.Cart.Add(new DO.Cart { UserID = user.ID });
        return new BO.Cart
        {
            ID = cartid,
            CustomerAdress = user.CustomerAdress,
            CustomerName = user.CustomerName,
            CustomerEmail = user.CustomerEmail,
            Items = new(),
            TotalPrice = 0
        };
    }

    /// <summary>
    /// return the saved cart of a registered user
    /// </summary>
    /// <param name="cart"></param>
    /// <param name="user"></param>
    /// <returns></returns>
    private BO.Cart creatNewCart(DO.Cart cart, DO.User user)
    {
        IEnumerable<DO.CartItem?> cartItemsDal = dal.CartItem.GetList(item => item?.CartID == cart.ID);

        var cartItems = from item in cartItemsDal
                        let cartItem = (DO.CartItem)item
                        let product = dal.Product.GetByCondition(prod => prod?.ID == cartItem.ProductID)
                        select new BO.OrderItem
                        {
                            ID = cartItem.ID,
                            ProductID = cartItem.ProductID,
                            Price = cartItem.Price,
                            Amount = cartItem.Amount,
                            TotalPrice = cartItem.Amount * cartItem.Price,
                            Name = product.Name
                        };
        return new BO.Cart
        {
            ID = cart.ID,
            CustomerAdress = user.CustomerAdress,
            CustomerName = user.CustomerName,
            CustomerEmail = user.CustomerEmail,
            Items = cartItems.ToList(),
            TotalPrice = cartItems.Sum(item => item.TotalPrice)
        };
    }

    #endregion

}

