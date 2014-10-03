using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objects
{
    public class Ticker : IEquatable<Ticker>
    {
        public Ticker(Market market)
        {
            this.Market = market;
            this.TopAsk = 0D;
            this.TopBid = 0D;
            this.LastTrade = new TradeRecord();
        }
        public virtual Market Market { get; protected set; }
        public ExchangeEnum ExchangeName { get { return Market.ExchangeName; } }
        public virtual Exchange Exchange { get { return Market.Exchange; } }
        public MarketIdentity MarketIdentity { get { return Market.MarketIdentity; } }
        public double TopAsk { get; set; }
        public double TopBid { get; set; }
        public TradeRecord LastTrade { get; set; }

        public bool Equals(Ticker other)
        {
            return this.Market == other.Market;
        }
    }
}
