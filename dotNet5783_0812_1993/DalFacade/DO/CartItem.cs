using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO;

public struct CartItem
{
    public int ID { get; set; }

    /// <summary>
    ///  ID of the order
    /// </summary>
    public int CartID { get; set; }

    /// <summary>
    ///  ID of product
    /// </summary>
    public int ProductID { get; set; }

    /// <summary>
    /// price per unit
    /// </summary>
    public double Price { get; set; }

    /// <summary>
    /// InStock of ordered item
    /// </summary>
    public int Amount { get; set; }

    public override string ToString() => this.ToStringProperty();

}
