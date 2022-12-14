using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    /// <summary>
    /// A class for a logical entity: order tracking
    /// </summary>
    public class OrderTracking
    {
        public int ID { get; set; }
        public OrderStatus Status { get; set; }
        public List<Tuple<DateTime, string>> Tuples { set; get; }
        public override string ToString() => this.ToStringProperty();

    }
}
