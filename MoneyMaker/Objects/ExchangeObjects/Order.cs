using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objects
{
    public class Order
    {
        public Order(OrderType type)
        {
            this.Type = type;
        }
        internal OrderType Type { get; private set; }
        public double Price { get; set; }
        public double Quantity { get; set; }
    }
}
