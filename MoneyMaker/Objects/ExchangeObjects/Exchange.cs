using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objects
{
    /// <summary>
    /// Top class for encompassing an exchange.
    /// All other Exchange Objects can be held within this.
    /// </summary>
    public class Exchange : IEquatable<Exchange>
    {
        public Exchange(ExchangeEnum Name)
        {
            this.Name = Name;
            this.Markets = new HashSet<Market>();
            this.ActiveOrders = new HashSet<ActiveOrder>();
            this.Balances = new HashSet<Balance>();
        }
        public ExchangeEnum Name { get; set; }
        public virtual Boolean CanTrade { get; internal set; }

        private static object mLock = new object();
        private HashSet<Market> _markets;
        public virtual HashSet<Market> Markets
        {
            get
            {
                HashSet<Market> temp = null;
                lock (mLock)
                {
                    if (this._markets == null)
                        this._markets = new HashSet<Market>();

                    temp = _markets;
                }
                return temp;
            }
            set
            {
                lock (mLock)
                {
                    _markets = value as HashSet<Market>;
                }
            }
        }

        private static object aLock = new object();
        private HashSet<ActiveOrder> _activeOrders;
        public virtual HashSet<ActiveOrder> ActiveOrders
        {
            get
            {
                HashSet<ActiveOrder> a = null;
                lock(aLock)
                {
                    if (this._activeOrders == null)
                        _activeOrders = new HashSet<ActiveOrder>();

                    a = _activeOrders;                    
                }
                return a;
            }
            set
            {
                lock(aLock)
                {
                    _activeOrders = value as HashSet<ActiveOrder>;
                }
            }
        }

        private static object bLock = new object();
        private HashSet<Balance> _balances;
        public virtual HashSet<Balance> Balances
        {
            get
            {
                HashSet<Balance> b = null;
                lock(bLock)
                {
                    if (this._balances == null)
                        this._balances = new HashSet<Balance>();
                    b = this._balances;
                }
                return b;
            }
            set
            {
                lock(bLock)
                {
                    this._balances = value as HashSet<Balance>;
                }
                return;
            }
        }

        private static object tLock = new object();
        private HashSet<Ticker> _tickers;
        public virtual HashSet<Ticker> Tickers
        {
            get
            {
                HashSet<Ticker> t = new HashSet<Ticker>();
                lock(tLock)
                {
                    if (this._tickers == null)
                        this._tickers = new HashSet<Ticker>();
                    t = this._tickers;
                }
                return t;
            }
            set
            {
                lock(tLock)
                {
                    this._tickers = value as HashSet<Ticker>;
                }
                return;
            }
        }

        public bool Equals(Exchange other)
        {
            return this.Name == other.Name;
        }
    }
    
}
