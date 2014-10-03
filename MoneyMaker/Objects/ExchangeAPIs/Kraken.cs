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
using Json.Kraken;

namespace Objects
{
    class API_Kraken : IExchange
    {
        public API_Kraken(Exchange exchange)
        {
            this.Exchange = exchange;
            this.MarketIds = new Dictionary<string, string>();
            this.cookieJar = new CookieContainer();
        }
        ~API_Kraken()
        {
            ClearKeys();
        }

        #region Fields
        private string _secret;
        private bool isDownloading = false;
        private CookieContainer cookieJar;
        private DateTime lastCallTime;
        #endregion

        #region Properties
        public override Dictionary<string, string> MarketIds { get; protected set; }
        public override ExchangeEnum ExchangeName { get { return ExchangeEnum.Kraken; } }
        #endregion

        #region Unique Methods
        private void GetAssetInfo()
        {
            if (null == _marketList)
                _marketList = new HashSet<Market>();

            string assets = Task.Run(async () => await PublicApi("AssetPairs")).Result;
            var jObj = JObject.Parse(assets).GetValue("result");
            var aList = jObj.ToObject<Dictionary<string, AssetPairs>>();

            foreach (var item in aList)
            {
                String sN = GetStandardisedName(item.Value.Altname);

                var i = item.Value;
                Market m = new Market(Exchange)
                {
                    PriceDecimals = i.pair_decimals,
                    QuantDecimals = i.lot_decimals
                };

                MarketIdentity mi = new MarketIdentity(m)
                {
                    Commodity = i.Base,
                    Currency = i.quote,
                    MarketId = item.Key,
                    StandardisedName = sN
                };
                m.MarketIdentity = mi;

                foreach (var f in i.fees)
                {
                    Fee fee = new Fee
                    {
                        Market = m,
                        MarketId = item.Key,
                        FeePercentage = f[1],
                        Quantity = f[0]
                    };
                    m.Fees.Add(fee);
                }

                var q = from mk in _marketList
                        where mk.MarketIdentity.StandardisedName == sN
                        select mk;

                if (q.Count() == 0)
                    _marketList.Add(m);
                else
                {
                    var mk = q.First();
                    mk = m;
                }

                if (!MarketIds.ContainsKey(sN))
                    MarketIds.Add(sN, item.Key);
            }
        }
        #endregion

        #region Methods
        public override bool SetKeys(SecureString pKey, SecureString sKey)
        {
            ClearKeys();

            this._publicKey = EncryptionTools.ConvertToUnsecureString(pKey);
            if (sKey != null)
            {
                _secret = EncryptionTools.ConvertToUnsecureString(sKey);
                pKey = null;
                sKey = null;
            }
            return HasKeys;
        }
        public override bool HasKeys
        {
            get
            {
                if (string.IsNullOrEmpty(_publicKey) || string.IsNullOrEmpty(_secret))
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
        public override string GetStandardisedName(string Name)
        {
            int n = Name.Length + 1;
            StringBuilder sb = new StringBuilder();
            int pos = 0;
            for (int ii = 0; ii < n; ii++)
            {
                char c;
                if (ii == 3)
                {
                    c = '/';
                }
                else
                {
                    c = Name.ElementAt(pos);
                    pos++;
                }
                sb.Append(c);
            }
            return sb.ToString();
        }
        public override HashSet<Market> GetAllMarketData()
        {
            if (_marketList == null)
                GetAssetInfo();

            if (lastCallTime == null || lastCallTime < DateTime.Now.AddSeconds(-60))
            {
                HashSet<Market> marketList = new HashSet<Market>();
                StringBuilder sb = new StringBuilder();
                sb.Append(MarketIds.ElementAt(0).Value);
                for (int ii = 1; ii < MarketIds.Count; ii++)
                {
                    sb.Append(',' + MarketIds.ElementAt(ii).Value);
                }
                string reqString = sb.ToString();

                string json = Task.Run(async () => await PublicApi("Ticker", reqString)).Result;
                lastCallTime = DateTime.Now;

                if (json != null)
                {
                    var jObj = JObject.Parse(json).GetValue("result");
                    var tickers = jObj.ToObject<Dictionary<string, TickerInfo>>();

                    foreach (var t in tickers)
                    {
                        var m = _marketList.First(p => p.MarketIdentity.MarketId == t.Key);
                        Order a = new Order(OrderType.Ask)
                        {
                            Price = t.Value.AskArray[0],
                            Quantity = t.Value.AskArray[1]
                        };
                        Order b = new Order(OrderType.Bid)
                        {
                            Price = t.Value.BidArray[0],
                            Quantity = t.Value.BidArray[1]
                        };
                        TradeRecord tr = new TradeRecord
                        {
                            Price = t.Value.LastClosedArray[0],
                            Quantity = t.Value.LastClosedArray[1]
                        };
                        m.OrderBook.AskOrders.Add(a);
                        m.OrderBook.BidOrders.Add(b);
                        m.TradeRecords.Push(tr);

                        marketList.Add(m);
                    }

                    if (marketList.Count > 0)
                        ApiActive = true;
                    else
                        ApiActive = false;

                    _marketList = marketList;
                    return marketList;
                }
                else return _marketList as HashSet<Market>;
            }
            else return _marketList as HashSet<Market>;
        }
        public override Market GetSingleMarket(MarketIdentity MarketIdent)
        {
            Market m = new Market(Exchange);

            return m;
        }
        public override OrderBook GetSingleMarketOrders(MarketIdentity MarketIdent)
        {
            OrderBook ob = new OrderBook();

            try
            {
                string json = Task.Run(async()=> await PublicApi("Depth", MarketIdent.MarketId)).Result;
                var jObj = JObject.Parse(json).GetValue("result").First().First();
                var orders = jObj.ToObject<OrderBookInfo>();
                foreach (var o in orders.asks)
                {
                    ob.AskOrders.Add(new Order(OrderType.Ask) { Price = Convert.ToDouble(o[0]), Quantity = Convert.ToDouble(o[1]) });
                }
                foreach (var o in orders.bids)
                {
                    ob.BidOrders.Add(new Order(OrderType.Bid) { Price = Convert.ToDouble(o[0]), Quantity = Convert.ToDouble(o[1]) });
                }
            }
            catch(Exception e)
            {
                Logger.WriteEvent(e.InnerException);
            }

            return ob;
        }
        public override HashSet<TradeRecord> GetSingleMarketTradeHistory(MarketIdentity MarketIdent)
        {
            HashSet<TradeRecord> tr = new HashSet<TradeRecord>();

            string json = Task.Run(async () => await PublicApi("Trades", MarketIdent.MarketId)).Result;

            if(!string.IsNullOrEmpty(json))
            {
                var jObj = JObject.Parse(json).GetValue("result");
                var xs = jObj.ToString();
                var tList = jObj.First().First().ToObject<List<string[]>>();

                foreach (var item in tList)
                {
                    OrderType type = OrderType.Bid;
                    if (item[3] == "s")
                        type = OrderType.Ask;

                    var timestamp = Convert.ToDouble((string)item[2]);

                    DateTime dt = UnixTime.ConvertToDateTime(timestamp);
                    
                    tr.Add(new TradeRecord
                        {
                            Price = Convert.ToDouble(item[0]),
                            Quantity = Convert.ToDouble(item[1]),
                            TradeTime = dt,
                            Type = type        
                        });
                }
            }

            return tr;
        }        
        public override HashSet<ActiveOrder> GetActiveOrders()
        {
            HashSet<ActiveOrder> ao = new HashSet<ActiveOrder>();

            if(HasKeys)
            {
                string json = AuthenticatedRequest("OpenOrders");
                if (!JObject.Parse(json).GetValue("error").HasValues)
                {
                    var jObj = JObject.Parse(json).GetValue("result").First().First();
                    var data = jObj.ToObject<Dictionary<string, OpenOrderInfo>>();

                    foreach (var o in data)
                    {
                        String sN = GetStandardisedName(o.Value.descr.pair);
                        Market m = _marketList.First(p => p.MarketIdentity.StandardisedName == sN);

                        OrderType type = OrderType.Ask;
                        if (o.Value.descr.type == "buy")
                            type = OrderType.Bid;

                        ao.Add(new ActiveOrder(ExchangeEnum.Kraken, m.MarketIdentity.MarketId)
                        {
                            Created = UnixTime.ConvertToDateTime(o.Value.opentm),
                            OrderId = o.Key,
                            OrderType = type,
                            Price = o.Value.descr.price,
                            Quantity = o.Value.vol,
                            Remaining = o.Value.vol = o.Value.vol_exec
                        });
                    }
                }
                else
                {
                    string msg = JObject.Parse(json).GetValue("error").ToString();
                    Logger.WriteEvent("Kraken: " + msg);
                }
            }

            return ao;
        }
        public override HashSet<Balance> GetBalances()
        {
            HashSet<Balance> balances = new HashSet<Balance>();

            if (HasKeys)
            {
                try
                {
                    string json = AuthenticatedRequest("Balance");
                    if (!JObject.Parse(json).GetValue("error").HasValues)
                    {
                        var jObj = JObject.Parse(json).GetValue("result");
                        var bals = jObj.ToObject<Dictionary<String, Double>>();

                        foreach (var b in bals)
                        {
                            balances.Add(new Balance
                            {
                                Exchange = this.Exchange,
                                Name = b.Key,
                                Available = b.Value
                            });
                        }
                    }
                    else
                    {
                        string msg = JObject.Parse(json).GetValue("error").ToString();
                        Logger.WriteEvent("Kraken: " + msg);
                    }
                }
                catch
                {

                }
            }

            return balances;
        }
        public override Tuple<string, string> PlaceBasicOrder(MarketIdentity MarketIdent, OrderType Type, double Price, double Quantity)
        {
            Tuple<string, string> tp;

            if (HasKeys)
            {
                string pair = MarketIdent.MarketId;
                string type = "buy";
                if (Type == OrderType.Ask)
                    type = "sell";
                string ordertype = "limit";


                string reqs = string.Format("&pair={0}&type={1}&ordertype={2}&volume={3}&price={4}", pair, type, ordertype, Quantity, Price);
                string json = AuthenticatedRequest("AddOrder", reqs);
                if (!JObject.Parse(json).GetValue("error").HasValues)
                {

                    var jObj = JObject.Parse(json).GetValue("result");
                    List<string> txids = jObj.Last().First().ToObject<List<string>>();
                    string msg = jObj.First().First().First().First().ToString();

                    tp = new Tuple<string, string>(txids[0], msg);
                }
                else
                {
                    string msg = JObject.Parse(json).GetValue("error").ToString();
                    Logger.WriteEvent("Kraken: " + msg);
                    tp = new Tuple<string, string>("Error: ", msg);
                }
            }
            else
            {
                tp = new Tuple<string, string>("Error", "No keys saved for this exchange.");
            }
            
            return tp;
        }
        public override string CancelOrder(string OrderId = null, string MarketId = null)
        {
            if (HasKeys)
            {
                string req = string.Format("&txid={0}", OrderId);
                var json = AuthenticatedRequest("CancelOrder", req);
                var jObj = JObject.Parse(json);
                var isError = jObj.GetValue("error");
                if (isError.HasValues)
                {
                    string result = string.Format("Error: {0}", jObj.GetValue("error").First);
                    return result;
                }
                else
                {
                    return string.Format("Order {0} cancelled", OrderId);
                }
            }
            else
                return "Error - No keys saved for this exchange.";
        }
        #endregion

        #region Calls
        private async Task<string> PublicApi(string method, string pair = null)
        {
            await Task.Delay(300);

            string url;
            string json = String.Empty;
            
            if (pair != null)
                url = String.Format("https://api.kraken.com/0/public/{0}?pair={1}", method, pair);
            else
                url = String.Format("https://api.kraken.com/0/public/{0}", method);

            using (WebClient wc = new WebClient())
            {
                try
                {
                    wc.DownloadStringCompleted += wc_DownloadStringCompleted;
                    wc.DownloadProgressChanged += wc_DownloadProgressChanged;
                    json = await wc.DownloadStringTaskAsync(url);
                }
                catch (WebException e)
                {
                    Logger.WriteEvent("Public API" + e.InnerException);
                    ApiActive = false;
                }
                catch (System.Net.Sockets.SocketException er)
                {
                    Logger.WriteEvent("Public API" + er.InnerException);
                }
            }
            return json;
        }
        private static readonly object locker = new object();
        private string AuthenticatedRequest(string method, string props = null)
        {
            #region Rate Limiter
            while (isDownloading)
            {
                using (Task t = Task.Delay(3000))
                {
                    Task.WaitAll(t);
                    t.Dispose();
                }
            }
            #endregion

            string result;
            lock (locker)
            {
                result = String.Empty;
                isDownloading = true;

                UInt64 nonce = (ulong)DateTime.Now.Ticks;
                props = "nonce=" + nonce + props;
                var np = nonce + Convert.ToChar(0) + props;

                string postData = String.Format("/0/private/{0}", method);
                byte[] messagebyte = Encoding.UTF8.GetBytes(postData);
                string address = String.Format("https://api.kraken.com/0/private/{0}", method);

                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(address);
                webRequest.ContentType = "application/x-www-form-urlencoded";
                webRequest.Method = "POST";
                webRequest.Headers.Add("API-Key", _publicKey);

                byte[] base64DecodedSecret = Convert.FromBase64String(_secret);

                var hash256Bytes = sha256_hash(np);
                var z = new byte[messagebyte.Count() + hash256Bytes.Count()];
                messagebyte.CopyTo(z, 0);
                hash256Bytes.CopyTo(z, messagebyte.Count());

                var signature = getHash(base64DecodedSecret, z);

                webRequest.Headers.Add("API-Sign", Convert.ToBase64String(signature));

                //Make the request
                try
                {
                    if (props != null)
                    {

                        using (var writer = new StreamWriter(webRequest.GetRequestStream()))
                        {
                            writer.Write(props);
                        }


                        using (WebResponse webResponse = webRequest.GetResponse())
                        {
                            using (Stream str = webResponse.GetResponseStream())
                            {
                                using (StreamReader sr = new StreamReader(str))
                                {
                                    result = sr.ReadToEnd();
                                }
                            }
                        }
                    }
                }
                catch (WebException wex)
                {
                    Logger.WriteEvent("Private API" + wex.InnerException);
                }
                catch (System.Net.Sockets.SocketException er)
                {
                    Logger.WriteEvent("Private API" + er.InnerException);
                }
                finally
                {
                    isDownloading = false;
                }
            }
            return result;
        }
        private byte[] sha256_hash(String value)
        {
            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                Byte[] result = hash.ComputeHash(enc.GetBytes(value));
                return result;
            }
        }
        private byte[] getHash(byte[] keyByte, byte[] messageBytes)
        {
            using (var hmacsha512 = new HMACSHA512(keyByte))
            {
                Byte[] result = hmacsha512.ComputeHash(messageBytes);
                return result;
            }
        }
        #endregion
    }
}
