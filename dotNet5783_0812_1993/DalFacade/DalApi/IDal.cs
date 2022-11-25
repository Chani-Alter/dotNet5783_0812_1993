using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;

namespace DalApi
{
    internal interface IDal
    {
        public IOrder Order { get; }
        public IOrderItem OrderItem { get; }
        public IProduct Product { get; }
    }
}
