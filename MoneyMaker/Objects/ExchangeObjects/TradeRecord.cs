using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objects
{
    public class TradeRecord : IEquatable<TradeRecord>
    {
        public DateTime TradeTime { get; set; }
        public OrderType Type { get; set; }
        public double Price { get; set; }
        public double Quantity { get; set; }

        public bool Equals(TradeRecord other)
        {
            return this.TradeTime == other.TradeTime &&
                this.Price == other.Price &&
                this.Quantity == other.Quantity;
        }
    }
}
