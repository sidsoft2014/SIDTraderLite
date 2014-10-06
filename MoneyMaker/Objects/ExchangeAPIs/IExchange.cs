using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Toolbox;

namespace Objects
{
    public class IExEvents : EventArgs
    {
        public bool Active { get; set; }
        public string BytesDownloaded { get; set; }
    }

    /// <summary>
    /// Common interface for comunicating with exchange APIs
    /// Is not responsible for holding any data in itself and should
    /// be used in conjunction with an Exchnage Object for data storage and manipulation.
    /// </summary>
    public abstract class IExchange
    {
        internal Exchange Exchange;
        protected internal string _publicKey;
        protected internal HMACSHA512 hmAcSha;
        internal bool? alive;

        internal HashSet<Market> _marketList;

        public bool ApiActive
        {
            get
            {
                if (null == alive)
                    alive = false;
                return (bool)alive;
            }
            protected set
            {
                if (value != alive)
                {
                    alive = value;
                    if (event_DeclaringApiState != null)
                    {
                        IExEvents exe = new IExEvents();
                        exe.Active = (bool)alive;
                        event_DeclaringApiState(this, exe);
                    }
                }
            }
        }

        public virtual bool SetKeys(SecureString pKey, SecureString sKey)
        {
            ClearKeys();

            this._publicKey = EncryptionTools.ConvertToUnsecureString(pKey);
            if (sKey != null)
            {
                string t = EncryptionTools.ConvertToUnsecureString(sKey);
                hmAcSha = new HMACSHA512(Encoding.ASCII.GetBytes(t));
                t = null;
                pKey = null;
                sKey = null;
            }

            return HasKeys;
        }
        public void ClearKeys()
        {
            this.hmAcSha = null;
            this._publicKey = null;
        }
        public virtual bool HasKeys
        {
            get
            {
                if (string.IsNullOrEmpty(_publicKey) || hmAcSha == null)
                {
                    try
                    {
                        Exchange.CanTrade = false;
                    }
                    catch (NullReferenceException) { }
                    return false;
                }

                else
                {
                    try
                    {
                        Exchange.CanTrade = true;
                    }
                    catch (NullReferenceException) { }
                    return true;
                }
            }
        }
        public bool KeysValid { get; protected set; }

        /// <summary>
        /// Public market name to market Id dictionary
        /// </summary>
        public abstract Dictionary<string, string> MarketIds { get; protected set; }
        /// <summary>
        /// Public exchange enum reference
        /// </summary>
        public abstract ExchangeEnum ExchangeName { get; }
        /// <summary>
        /// Public method to convert name given by market to a standardised form
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public abstract string GetStandardisedName(string Name);
        /// <summary>
        /// Returns a full set of market data for the exchange
        /// </summary>
        /// <returns></returns>
        public abstract HashSet<Market> GetAllMarketData();
        /// <summary>
        /// Returns the data set for a single market
        /// </summary>
        /// <param name="MarketIdent"></param>
        /// <returns></returns>
        public abstract Market GetSingleMarket(MarketIdentity MarketIdent);
        /// <summary>
        /// Get order book for a single market
        /// </summary>
        /// <param name="MarketIdent"></param>
        /// <returns></returns>
        public abstract OrderBook GetSingleMarketOrders(MarketIdentity MarketIdent);
        /// <summary>
        /// Returns the trade history for a single market
        /// </summary>
        /// <param name="MarketIdent"></param>
        /// <returns></returns>
        public abstract Stack<TradeRecord> GetSingleMarketTradeHistory(MarketIdentity MarketIdent);
        /// <summary>
        /// Returns ant currently active orders
        /// </summary>
        /// <returns></returns>
        public abstract HashSet<ActiveOrder> GetActiveOrders();
        /// <summary>
        /// Returns list of current balances
        /// </summary>
        /// <returns></returns>
        public abstract HashSet<Balance> GetBalances();
        /// <summary>
        /// Places an order
        /// </summary>
        /// <param name="MarketIdent">Market identity object, used to get relevent market name or id</param>
        /// <param name="Type">Enum for the type of order</param>
        /// <param name="Price">Price to buy\sell at</param>
        /// <param name="Quantity">Amount to buy\sell</param>
        /// <returns>Tuple - item1 = order id or null if error, item2 = message</returns>
        public abstract Tuple<string, string> PlaceBasicOrder(MarketIdentity MarketIdent, OrderType Type, double Price, double Quantity);
        /// <summary>
        /// Cancels a currently active order
        /// </summary>
        /// <param name="OrderId">Order id to cancel</param>
        /// <param name="MarketId">Market order is in</param>
        /// <returns>Result message</returns>
        public abstract string CancelOrder(string OrderId, string MarketId);
        
        internal async Task<string> WebClientController(string url)
        {
            string json = String.Empty;

            try
            {
                using (WebClient wc = new WebClient())
                {
                    wc.DownloadProgressChanged += wc_DownloadProgressChanged;
                    wc.DownloadStringCompleted += wc_DownloadStringCompleted;
                    json = await wc.DownloadStringTaskAsync(url);
                    wc.Dispose();
                }
                ApiActive = true;
            }
            catch (WebException we)
            {
                ApiActive = false;
                Logger.WriteEvent(we.InnerException);
            }
            catch (System.Net.Sockets.SocketException er)
            {
                Logger.WriteEvent(er.InnerException);
            }
            return json;
        }
        internal event EventHandler<IExEvents> event_DeclaringApiState;
        internal event EventHandler<IExEvents> event_ApiBytesDownloaded;
        internal void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            string pgs = string.Format("Downloaded {0} bytes.", e.BytesReceived);
            if (event_ApiBytesDownloaded != null)
            {
                IExEvents exe = new IExEvents();
                exe.BytesDownloaded = pgs;
                event_ApiBytesDownloaded(this, exe);
            }
        }
        internal void wc_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            string pgs = string.Format("Downloaded Completed: {0}", DateTime.Now);
            if (event_ApiBytesDownloaded != null)
            {
                IExEvents exe = new IExEvents();
                exe.BytesDownloaded = pgs;
                event_ApiBytesDownloaded(this, exe);
            }
        }
    }
}
