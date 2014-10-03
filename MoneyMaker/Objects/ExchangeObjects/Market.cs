using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toolbox;

namespace Objects
{
    /// <summary>
    /// Holds data for a single market.
    /// </summary>
    public class Market : IComparable<Market>, IEquatable<Market>
    {
        #region Navigational Propeties
        public Market(Exchange exchange)
        {
            this.Exchange = exchange;
            this.MarketIdentity = new MarketIdentity(this);

            this.Fees = new List<Fee>();
        }
        public virtual Exchange Exchange { get; set; }
        public ExchangeEnum ExchangeName
        {
            get
            {
                return Exchange.Name;
            }
        }
        public MarketIdentity MarketIdentity { get; set; }
        public Ticker Ticker 
        {
            get
            {
                double aor = double.NaN;
                double bor = double.NaN;
                TradeRecord tor = null;

                if (null != OrderBook)
                {
                    if (OrderBook.AskOrders != null && OrderBook.AskOrders.Count > 0)
                    {
                        aor = OrderBook.AskOrders.FirstOrDefault().Price;
                    }
                    if (OrderBook.BidOrders != null && OrderBook.BidOrders.Count > 0)
                    {
                        bor = OrderBook.BidOrders.FirstOrDefault().Price;
                    }
                }
                if (TradeRecords != null && TradeRecords.Count > 0)
                {
                    tor = tradeRecords.Peek();
                }
                return new Ticker(this)
                {
                    LastTrade = tor,
                    TopAsk = aor, 
                    TopBid = bor
                };
            }
        }
        #endregion

        #region Properties
        public int PriceDecimals { get; set; }
        public int QuantDecimals { get; set; }
        public double MinPrice { get; set; }
        public double MaxPrice { get; set; }
        public double MinQuant { get; set; }
        public double Spread
        {
            get
            {
                double sprd = 0;
                if (OrderBook.AskOrders.Count > 0 && OrderBook.BidOrders.Count > 0)
                {
                    var p1 = OrderBook.AskOrders.First().Price;
                    var p2 = OrderBook.BidOrders.First().Price;
                    var delta = p2 / p1 * 100;
                    sprd = Math.Round(100 - delta, 2);
                }
                return sprd;
            }
        }
        #endregion

        #region Tables
        private OrderBook orderBook;
        private Stack<TradeRecord> tradeRecords;
        public virtual OrderBook OrderBook
        {
            get 
            {
                if (this.orderBook == null)
                    orderBook = new OrderBook(this);

                return this.orderBook;
            }
            set
            {
                //Set orderbook to new version
                this.orderBook = value;
            }
        }
        public virtual Stack<TradeRecord> TradeRecords
        {
            get
            {
                if (null == this.tradeRecords)
                    this.tradeRecords = new Stack<TradeRecord>();
                return this.tradeRecords;
            }
            set
            {
                this.tradeRecords = value;
            }
        }
        public virtual HashSet<ActiveOrder> ActiveOrders
        {
            get
            {
                var orders = from ord in Exchange.ActiveOrders
                        where ord.Market.MarketIdentity == this.MarketIdentity
                        select ord;

                HashSet<ActiveOrder> aO = new HashSet<ActiveOrder>();
                foreach(var a in orders)
                {
                    aO.Add(a);
                }

                return aO;
            }
        }
        public virtual HashSet<Balance> Balances
        {
            get 
            {
                HashSet<Balance> bals = new HashSet<Balance>();

                var curBal = from b in Exchange.Balances
                          where b.Name == this.MarketIdentity.Currency
                          select b;
                if (curBal.Count() == 0)
                {
                    Balance cur = new Balance() { Name = this.MarketIdentity.Currency, Available = 0, Held = 0 };
                    bals.Add(cur);
                }
                else
                {
                    bals.Add(curBal.First());
                }

                var comBal = from b in Exchange.Balances
                          where b.Name == this.MarketIdentity.Commodity
                          select b;
                if (comBal.Count() == 0)
                {
                    Balance com = new Balance() { Name = this.MarketIdentity.Commodity, Available = 0, Held = 0 };
                    bals.Add(com);
                }
                else
                {
                    bals.Add(comBal.First());
                }
                return bals;
            }
        }
        public virtual HashSet<Market> PairedMarkets
        {
            get
            {
                var q = from m in Exchange.Markets
                        where m.MarketIdentity.Commodity == this.MarketIdentity.Commodity
                        select m;

                HashSet<Market> temp = new HashSet<Market>();
                foreach (var p in q)
                {
                    if (p.MarketIdentity.StandardisedName != this.MarketIdentity.StandardisedName)
                        temp.Add(p);
                }
                return temp;
            }
        }
        public virtual List<Fee> Fees { get; set; }
        #endregion

        #region Methods
        public double GetFee(double Orderquantity)
        {
            if (Fees.Count * Orderquantity > 0)
            {
                if (Fees.Count > 1)
                {
                    return Fees.Last(p => p.Quantity < Orderquantity).FeePercentage;
                }
                else return Fees.First().FeePercentage;
            }
            else return 0;
        }
        public int CompareTo(Market other)
        {
            return other.MarketIdentity.CompareTo(this.MarketIdentity);
        }
        public bool Equals(Market other)
        {
            return this.MarketIdentity == other.MarketIdentity;
        }
        #endregion
    }
}
