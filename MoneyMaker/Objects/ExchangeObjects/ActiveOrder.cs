using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objects
{
    public class ActiveOrder : IEquatable<ActiveOrder>
    {
        private ExchangeEnum? _exchangeName;
        public ExchangeEnum ExchangeName
        { 
            get
            {
                if (_exchangeName == null)
                    _exchangeName = Market.ExchangeName;
                return (ExchangeEnum)_exchangeName;
            } 
            set
            {
                _exchangeName = value;
            }
        }
        private string _marketId;
        public string MarketId
        {
            get
            {
                if (_marketId == null)
                    _marketId = Market.MarketIdentity.MarketId;
                return _marketId;
            }
            set
            {
                _marketId = value;
            }
        }
        public virtual Market Market { get; set; }

        public string OrderId { get; set; }
        public OrderType OrderType { get; set; }
        public double Price { get; set; }
        public double Quantity { get; set; }
        public double Remaining { get; set; }
        public double PercentFilled
        {
            get
            {
                if (Remaining > 0 && Quantity > 0)
                    return Math.Round(100 - ((Remaining / Quantity) * 100), 2);
                else return 0;
            }
        }
        public DateTime Created { get; set; }

        public ActiveOrder(Market market)
        {
            this.Market = market;
        }
        public ActiveOrder(ExchangeEnum exchange, string marketId)
        {
            this.ExchangeName = exchange;
            this.MarketId = marketId;
        }
        public bool Equals(ActiveOrder other)
        {
            return this.OrderId == other.OrderId;
        }
    }
}
