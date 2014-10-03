using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objects
{
    public class OrderBook
    {
        public OrderBook(Market sender = null)
        {
            this.Market = sender;

            this.AskOrders = new HashSet<Order>();
            this.BidOrders = new HashSet<Order>();
        }
        public virtual Market Market { get; set; }
        private HashSet<Order> _asks;
        private HashSet<Order> _bids;
        public virtual HashSet<Order> AskOrders
        {
            get
            {
                if (null == _asks)
                    _asks = new HashSet<Order>();
                return _asks;
            }
            set
            {
                _asks = value;
            }
        }
        public virtual HashSet<Order> BidOrders
        {
            get
            {
                if (_bids == null)
                    _bids = new HashSet<Order>();
                return _bids;
            }
            set
            {
                _bids = value;
            }
        }
    }
}
