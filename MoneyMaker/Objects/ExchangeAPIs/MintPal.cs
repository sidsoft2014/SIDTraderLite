using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Toolbox;
using Json.MintPal;

namespace Objects
{
    class API_MintPal : IExchange
    {
        public API_MintPal(Exchange exchange)
        {
            this.Exchange = exchange;
            this.MarketIds = new Dictionary<string, string>();
            this.cookieJar = new CookieContainer();
        }
        ~API_MintPal()
        {
            ClearKeys();
        }

        #region Fields
        private CookieContainer cookieJar;
        #endregion

        #region Properties
        public override Dictionary<string, string> MarketIds { get; protected set; }
        public override ExchangeEnum ExchangeName { get { return ExchangeEnum.MintPal; } }
        #endregion

        #region Methods
        public override string GetStandardisedName(string Name)
        {
            throw new NotImplementedException();
        }
        public override HashSet<Market> GetAllMarketData()
        {
            throw new NotImplementedException();
        }
        public override Market GetSingleMarket(MarketIdentity MarketIdent)
        {
            Market m = new Market(Exchange);

            return m;
        }
        public override OrderBook GetSingleMarketOrders(MarketIdentity MarketIdent)
        {
            OrderBook ob = new OrderBook();

            return ob;
        }
        public override HashSet<TradeRecord> GetSingleMarketTradeHistory(MarketIdentity MarketIdent)
        {
            HashSet<TradeRecord> tr = new HashSet<TradeRecord>();

            return tr;
        }
        public override HashSet<ActiveOrder> GetActiveOrders()
        {
            HashSet<ActiveOrder> ao = new HashSet<ActiveOrder>();

            return ao;
        }
        public override HashSet<Balance> GetBalances()
        {
            HashSet<Balance> b = new HashSet<Balance>();

            return b;
        }
        public override Tuple<string, string> PlaceBasicOrder(MarketIdentity MarketIdent, OrderType Type, double Price, double Quantity)
        {
            Tuple<string, string> tp = new Tuple<string, string>("", "");

            return tp;
        }
        public override string CancelOrder(string OrderId = null, string MarketId = null)
        {
            return "Error - Not implemented";
        }
        #endregion
    }
}
