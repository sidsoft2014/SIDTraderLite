using Json.Bittrex;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Toolbox;

namespace Objects
{
    public class API_Bittrex : IExchange
    {
        internal static bool isDownloading = false;
        private string _secret;

        public API_Bittrex(Exchange exchange)
        {
            this.Exchange = exchange;
        }
        ~API_Bittrex() { ClearKeys(); }

        public override string GetStandardisedName(string Name)
        {
            var parts = Name.Split('-');
            string nom = String.Format("{0}/{1}", parts[1], parts[0]);
            return nom;
        }

        public override HashSet<Market> GetAllMarketData()
        {
            Task a = Task.Run(async () => await GetMarkets());
            Task.WaitAll(a);
            Task b = Task.Run(async () => await GetSummaries());
            Task.WaitAll(b);
            return _marketList;
        }

        public override OrderBook GetSingleMarketOrders(MarketIdentity MarketIdent)
        {
            OrderBook ob = new OrderBook();

            string command = string.Format("getorderbook?market={0}&type=both&depth=50", MarketIdent.MarketId);
            string json = Task.Run(async () => await tryDownload(command)).Result;

            if (!string.IsNullOrEmpty(json))
            {
                JToken jOb;
                if (JObject.Parse(json).TryGetValue("result", out jOb))
                {
                    var orders = jOb.ToObject<Json_GetOrderBook>();
                    foreach (var item in orders.buy)
                    {
                        double prc = Double.NaN;
                        double qty = Double.NaN;
                        Double.TryParse(item.Rate, out prc);
                        Double.TryParse(item.Quantity, out qty);

                        ob.BidOrders.Add(new Order(OrderType.Bid) { Price = prc, Quantity = qty });
                    }
                    foreach (var item in orders.sell)
                    {
                        double prc = Double.NaN;
                        double qty = Double.NaN;
                        Double.TryParse(item.Rate, out prc);
                        Double.TryParse(item.Quantity, out qty);

                        ob.AskOrders.Add(new Order(OrderType.Ask) { Price = prc, Quantity = qty });
                    }
                }
            }

            return ob;
        }

        public override Stack<TradeRecord> GetSingleMarketTradeHistory(MarketIdentity MarketIdent)
        {
            Stack<TradeRecord> hist = new Stack<TradeRecord>();

            string command = string.Format("getmarkethistory?market={0}&count=4", MarketIdent.MarketId);
            string json = Task.Run(async () => await tryDownload(command)).Result;

            if(!string.IsNullOrEmpty(json))
            {
                JToken jOb;
                if(JObject.Parse(json).TryGetValue("result", out jOb))
                {
                    var trades = jOb.ToObject<List<Json_GetMarketHistory>>();
                    {
                        foreach (var item in trades.OrderBy(p => p.Time))
                        {
                            double prc = Double.NaN;
                            double qty = Double.NaN;
                            Double.TryParse(item.Price, out prc);
                            Double.TryParse(item.Quantity, out qty);
                            OrderType oType = OrderType.Bid;
                            if(item.OrderType == "SELL")
                                oType = OrderType.Ask;

                            hist.Push(new TradeRecord { Price = prc, Quantity = qty, TradeTime = item.Time, Type = oType });
                        }
                    }
                }
            }
            return hist;
        }

        public override HashSet<ActiveOrder> GetActiveOrders()
        {
            HashSet<ActiveOrder> ords = new HashSet<ActiveOrder>();
            if (ApiActive && HasKeys)
            {
                Dictionary<string, string> paramList = new Dictionary<string, string>()
                {
                    {"method", "market"},
                    {"command", "getopenorders"}
                };

                string json = AuthenticatedRequest(paramList);

                if(!string.IsNullOrEmpty(json))
                {
                    JToken jTok;
                    if(JObject.Parse(json).TryGetValue("result", out jTok))
                    {
                        var ordList = jTok.ToObject<List<Json_GetOpenOrders>>();

                        foreach (var o in ordList)
                        {
                            var q = from mk in _marketList
                                    where mk.MarketIdentity.MarketId == o.Exchange
                                    select mk;

                            if(q.Count() > 0)
                            {
                                Market m = q.First();
                                string dts = o.Opened.Replace('T', ' ');
                                DateTime dt;
                                DateTime.TryParse(dts, out dt);

                                double prc = Double.NaN;
                                double qty = Double.NaN;
                                double rem = Double.NaN;
                                Double.TryParse(o.Limit, out prc);
                                Double.TryParse(o.Quantity, out qty);
                                Double.TryParse(o.QuantityRemaining, out rem);

                                OrderType tp = OrderType.Ask;
                                if (o.OrderType.Contains("BUY"))
                                    tp = OrderType.Bid;
                                ords.Add(new ActiveOrder(m)
                                {
                                    Created = dt,
                                    ExchangeName = ExchangeEnum.Bittrex,
                                    Market = m,
                                    MarketId = m.MarketIdentity.MarketId,
                                    OrderId = o.OrderId,
                                    Price = prc,
                                    Quantity = qty,
                                    Remaining = rem,
                                    OrderType = tp
                                });
                            }
                        }
                    }
                }
            }
            return ords;
        }

        public override HashSet<Balance> GetBalances()
        {
            HashSet<Balance> bals = new HashSet<Balance>();
            if (ApiActive && HasKeys)
            {
                Dictionary<string, string> paramList = new Dictionary<string, string>()
                {
                    {"method", "account"},
                    {"command", "getbalances"}
                };

                string json = AuthenticatedRequest(paramList);

                if(!string.IsNullOrEmpty(json))
                {
                    JToken jTok;
                    if(JObject.Parse(json).TryGetValue("result", out jTok))
                    {
                        var balList = jTok.ToObject<List<Json_GetBalances>>();
                        foreach (var b in balList)
                        {
                            double avail = 0;
                            Double.TryParse(b.Available, out avail);
                            double pend = 0;
                            Double.TryParse(b.Pending, out pend);
                            bals.Add(new Balance { Exchange = this.Exchange, Available = avail, Name = b.Currency, Held = pend });
                        }
                    }
                }
            }
            return bals;
        }

        public override Tuple<string, string> PlaceBasicOrder(MarketIdentity MarketIdent, OrderType Type, double Price, double Quantity)
        {
            if (ApiActive && HasKeys)
            {
                string json = String.Empty;

                string command = "selllimit";
                if (Type == OrderType.Bid)
                    command = "buylimit";

                Dictionary<string, string> paramList = new Dictionary<string, string>()
                {
                    {"method", "market"},
                    {"command", command},
                    {"market", MarketIdent.MarketId},
                    {"quantity", Quantity.ToString("N8")},
                    {"rate", Price.ToString("N8")}
                };

                json = AuthenticatedRequest(paramList);
                
                JToken jTok;
                if(JObject.Parse(json).TryGetValue("result", out jTok))
                {
                    var result = jTok.ToObject<Json_PlaceOrderResponse>();
                    return new Tuple<string, string>(result.orderId, "Success");
                }
                else return new Tuple<string, string>("", "Error");
            }
            else return new Tuple<string, string>("", "Error");
        }

        public override string CancelOrder(string OrderId, string MarketId)
        {
            string json = String.Empty;
            if (ApiActive && HasKeys)
            {
                Dictionary<string, string> paramList = new Dictionary<string, string>()
                {
                    {"method", "market"},
                    {"command", "cancel"},
                    {"uuid", OrderId}
                };

                json = AuthenticatedRequest(paramList);
                try
                {
                    var jTok = JObject.Parse(json);
                    if ((bool)jTok["success"])
                    {
                        return OrderId + " has been canceled.";
                    }
                    else
                    {
                        return "Error canceling " + OrderId + " : " + (string)jTok["message"];
                    }
                }
                catch (Newtonsoft.Json.JsonReaderException) { return json; }
            }
            else return json;
        }


        #region Internal Methods
        internal async Task GetMarkets()
        {
            if (_marketList == null)
                _marketList = new HashSet<Market>();

            string json = await tryDownload("getmarkets");

            if (!string.IsNullOrEmpty(json))
            {
                var job = JObject.Parse(json).GetValue("result");
                var markets = job.ToObject<List<Json_GetMarkets>>();

                foreach (var market in markets.Where(p => p.IsActive == true))
                {
                    string sName = GetStandardisedName(market.MarketName);

                    /// If MarketsId list doesn't contain the market we need to create a new one.
                    if (!MarketIds.ContainsKey(sName))
                    {
                        Market m = new Market(this.Exchange)
                        {
                            MinQuant = Convert.ToDouble(market.MinTradeSize),
                            PriceDecimals = 8,
                            QuantDecimals = 8,
                            MinPrice = 0.00000001,
                            MaxPrice = 0,
                        };

                        MarketIdentity mi = new MarketIdentity(m);
                        mi.Name = market.MarketName;
                        mi.MarketId = market.MarketName;
                        mi.StandardisedName = sName;
                        mi.Commodity = market.MarketCurrency;
                        mi.Currency = market.BaseCurrency;
                        m.MarketIdentity = mi;                        

                        m.Fees = new List<Fee>();
                        m.Fees.Add(new Fee { FeePercentage = 0.25, Market = m, MarketId = market.MarketName, Quantity = 0 });

                        try
                        {
                            _marketList.Add(m);
                            MarketIds.Add(sName, market.MarketName);
                        }
                        catch (Exception) { }
                    }
                }
            }
        }
        internal async Task GetSummaries()
        {
            string json = await tryDownload("getmarketsummaries");

            if (!string.IsNullOrEmpty(json))
            {
                List<Json_GetMarketSummaries> mktSumList = new List<Json_GetMarketSummaries>();
                try
                {
                    var jOb = JObject.Parse(json).GetValue("result");
                    mktSumList = jOb.ToObject<List<Json_GetMarketSummaries>>();
                }
                catch (Newtonsoft.Json.JsonReaderException) { }

                if (mktSumList.Count > 0)
                {
                    foreach (var item in mktSumList)
                    {
                        var q = from mkt in _marketList
                                where mkt.MarketIdentity.MarketId == item.MarketName
                                select mkt;

                        if (q.Count() > 0)
                        {
                            Market m = q.First();

                            /// Like Poloniex we can't grab all markets trade records at once.
                            /// So we are using the last trade again to add a minimal trade history.
                            double prc = Double.NaN;
                            Double.TryParse(item.Last, out prc);
                            double ask = Double.NaN;
                            Double.TryParse(item.Ask, out ask);
                            double bid = Double.NaN;
                            Double.TryParse(item.Bid, out bid);
                            m.TradeRecords.Push(new TradeRecord { Price = prc });

                            /// Neither can we get all markets orderbooks in one. So we add minimal 
                            /// orders to the books aswell.
                            m.OrderBook.AskOrders.Add(new Order(OrderType.Ask) { Price = ask });
                            m.OrderBook.BidOrders.Add(new Order(OrderType.Bid) { Price = bid });
                        }
                    }
                }
            }
        }
        #endregion

        #region Calls
        private async Task<string> tryDownload(string command)
        {
            string url = string.Format("https://bittrex.com/api/v1.1/public/{0}", command);
            string json = String.Empty;
            try
            {
                using (WebClient wc = new WebClient())
                {
                    wc.DownloadProgressChanged += wc_DownloadProgressChanged;
                    wc.DownloadStringCompleted += wc_DownloadStringCompleted;
                    json = await wc.DownloadStringTaskAsync(url);
                    ApiActive = true;
                    wc.Dispose();
                }
            }
            catch (WebException) { ApiActive = false; }
            return json;
        }
        private static readonly object apiLock = new object();
        private string AuthenticatedRequest(Dictionary<string, string> paramList)
        {
            String json = String.Empty;
            if (paramList.ContainsKey("method"))
            {
                lock (apiLock)
                {
                    string url = @"https://bittrex.com/api/v1.1";
                    json = String.Empty;

                    var postData = String.Format("{0}/{1}/{2}?apikey={3}&nonce={4}", url, paramList["method"], paramList["command"], _publicKey ,DateTime.Now.Ticks);

                    foreach (var p in paramList)
                    {
                        if (p.Key != "method" && p.Key != "command")
                        {
                            postData += "&" + p.Key + "=" + p.Value;
                        }
                    }

                    url = postData;

                    var messagebyte = Encoding.ASCII.GetBytes(postData);
                    var hashmessage = hmAcSha.ComputeHash(messagebyte);
                    var sign = BitConverter.ToString(hashmessage);
                    sign = sign.Replace("-", "");

                    using(WebClient wc = new WebClient())
                    {
                        wc.Headers.Add("apisign", sign.ToLower());
                        json = wc.DownloadString(url);
                    }
                }
            }
            return json;
        }
        #endregion
    }
}
