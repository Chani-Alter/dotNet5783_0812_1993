using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    /// <summary>
    /// A class for a logical entity:order for list
    /// </summary>
    public class OrderForList
    {
        public int ID { get; set; }
        public string CustomerName { get; set; }
        public OrderStatus Status { get; set; }
        public int AmountOfItems { get; set; }
        public double TotalPrice { get; set; }
        public override string ToString() => this.ToStringProperty();

    }
}
