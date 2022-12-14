using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    /// <summary>
    /// A class for a logical entity: order
    /// </summary>
    public class Order
    {
        public int ID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerAdress { get; set; }
        public DateTime CreateOrderDate { get; set; }
        public DateTime ShippingDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public List<OrderItem> Items { get; set; }
        public double TotalPrice { get; set; }
        public OrderStatus Status { get; set; }
        public override string ToString() => this.ToStringProperty();

    }
}
