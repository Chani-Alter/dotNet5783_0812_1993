using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO;

public struct Cart
{
    public int ID { get; set; }
    public int UserID { get; set; }
    public override string ToString() => this.ToStringProperty();

}
