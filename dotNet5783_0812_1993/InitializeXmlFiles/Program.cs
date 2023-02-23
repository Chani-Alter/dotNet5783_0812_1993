using DO;
using Dal;
using System.Xml.Serialization;

namespace IntilizeXmlFile;
public class Program
{

    static void Main()
    {
        //using reflection (call static constructor)
        Type staticClassInfo = typeof(DataSource);
        var staticClassConstructorInfo = staticClassInfo.TypeInitializer;
        staticClassConstructorInfo?.Invoke(null, null);

        List<Product?> PrdouctList = DataSource.ProductList;
        List<Order?> OrderList = DataSource.OrderList;
        List<OrderItem?> OrderItemList = DataSource.OrderItemList;
        List<User?> UserList = DataSource.UserList;
        List<Cart?> CartList = DataSource.CartList;
        List<CartItem? > CartItemList = DataSource.CartItemList;

        StreamWriter wProduct = new(@"..\..\..\..\xml\Product.xml");
        XmlSerializer serProduct = new(typeof(List<Product?>));
        serProduct.Serialize(wProduct, PrdouctList);
        wProduct.Close();

        StreamWriter wOrder = new(@"..\..\..\..\xml\Order.xml");
        XmlSerializer serOrder = new(typeof(List<Order?>));
        serOrder.Serialize(wOrder, OrderList);
        wOrder.Close();

        StreamWriter wOrderItem = new(@"..\..\..\..\xml\OrderItem.xml");
        XmlSerializer serOrderItem = new(typeof(List<OrderItem?>));
        serOrderItem.Serialize(wOrderItem, OrderItemList);
        wOrderItem.Close();

        StreamWriter wUser = new(@"..\..\..\..\xml\User.xml");
        XmlSerializer serUser = new(typeof(List<User?>));
        serUser.Serialize(wUser, UserList);
        wOrderItem.Close();

        StreamWriter wCart = new(@"..\..\..\..\xml\Cart.xml");
        XmlSerializer serCart = new(typeof(List<Cart?>));
        serCart.Serialize(wCart, CartList);
        wOrderItem.Close();

        StreamWriter wCartItem = new(@"..\..\..\..\xml\CartItem.xml");
        XmlSerializer serCartItem = new(typeof(List<CartItem?>));
        serCartItem.Serialize(wCartItem, CartItemList);
        wOrderItem.Close();

    }
}