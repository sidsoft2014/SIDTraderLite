using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objects
{
    public class TradeObject
    {
        public TradeObject(
            MarketIdentity market,
            OrderType type,
            Double price,
            Double quantity,
            ConditionalType? condition = null,
            Double? trigger = null,
            DateTime? expireTime = null)
        {
            this.Market = market;
            this.Type = type;
            this.Price = price;
            this.Quantity = quantity;

            if (trigger != null)
                this.TriggerPrice = trigger;
            if (expireTime != null)
                this.ExpireTime = expireTime;
            if (condition != null)
                this.Condition = condition;
        }

        public MarketIdentity Market { get; set; }
        public OrderType Type { get; set; }
        public double Price { get; set; }
        public double Quantity { get; set; }

        public ConditionalType? Condition { get; set; }
        public double? TriggerPrice { get; set; }
        public DateTime? ExpireTime { get; set; }

        public override string ToString()
        {
            string msg = string.Format("Market: {0}", Market.StandardisedName);            
            return msg;
        }
        public string ToStringVerbose()
        {
            string msg = string.Format("Exchange: {0}\nMarket: {1}\nPrice: {2}\nQuantity: {3}\nType: {4}", Market.ExchangeName, Market.StandardisedName, Price, Quantity, Type);
            return msg;
        }
    }
}
