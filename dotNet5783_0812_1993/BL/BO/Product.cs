using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;

namespace BO
{
    /// <summary>
    /// A class for a logical entity: product
    /// </summary>
    public class Product
    {
        public int ID { get; set; }
        public string ProductName { get; set; }
        public double Price { get; set; }
        public Category Category { get; set; }
        public int InStock { get; set; }
        public override string ToString() => this.ToStringProperty();

    }
}
