using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objects
{
    public class MarketIdentity : IComparable<MarketIdentity>, IEquatable<MarketIdentity>
    {
        public MarketIdentity(Market Market)
        {
            this.Market = Market;
        }
        /// <summary>
        /// Market this identity belongs too
        /// </summary>
        /// <param name="Market"></param>
        /// <summary>
        public Market Market { get; protected set; }
        /// <summary>
        /// Enum for exchange the market belongs too.
        /// </summary>
        public ExchangeEnum ExchangeName
        {
            get
            {
                return Market.ExchangeName;
            }
        }
        /// <summary>
        /// The exchange-assigned Id for this market
        /// </summary>
        public string MarketId { get; set; }
        /// <summary>
        /// The exchange-assigned name for this market
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The market name set to application standard format
        /// </summary>
        public string StandardisedName { get; set; }
        /// <summary>
        /// The commodity on this market
        /// </summary>
        public string Commodity { get; set; }
        /// <summary>
        /// The currency for this market
        /// </summary>
        public string Currency { get; set; }

        public override string ToString()
        {
            string msg = String.Format("[Exchange: {0}, MarketName: {1}, MarketId: {2}]", ExchangeName, Name, MarketId);
            return msg;
        }
        public int CompareTo(MarketIdentity other)
        {
            if (this.Commodity == other.Commodity)
            {
                if (this.Currency == other.Currency)
                {
                    if (this.MarketId == other.MarketId)
                    {
                        return other.ExchangeName.CompareTo(this.ExchangeName);
                    }
                    return other.MarketId.CompareTo(this.MarketId);
                }
                return other.Currency.CompareTo(this.Currency);
            }
            return other.Commodity.CompareTo(this.Commodity);
        }

        public bool Equals(MarketIdentity other)
        {
            return this.StandardisedName == other.StandardisedName &&
                this.ExchangeName == other.ExchangeName;
        }
    }
}
