using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;

namespace BO
{
    /// <summary>
    /// A class for a logical entity: product for list
    /// </summary>
    public class ProductForList
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public Category Category { get; set; }
        public override string ToString() => this.ToStringProperty();

    }
}
