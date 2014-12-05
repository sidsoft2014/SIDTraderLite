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
using Json.Poloniex;
using System.Threading;

namespace Objects
{
    class API_Poloniex : IExchange
    {
        public API_Poloniex(Exchange exchange)
        {
            this.Exchange = exchange;
        }
        ~API_Poloniex()
        {
            ClearKeys();
        }

        #region UniqueMethods
        private async Task<HashSet<Market>> PoloniexCombinedUpdate()
        {
            _marketList = new HashSet<Market>();

            await getTickerInfo();
            await getOrderBooks();
            return _marketList;
        }
        private async Task getTickerInfo()
        {
            string url = @"https://poloniex.com/public?command=returnTicker";
            string json = await tryDownload(url);

            if (!string.IsNullOrEmpty(json))
            {
                var jOb = JObject.Parse(json);
                var jsonObjects = new Dictionary<string, PoloniexMarketTickers>();
                foreach (var item in jOb)
                {
                    try
                    {
                        string key = item.Key.ToString();
                        PoloniexMarketTickers tick = item.Value.ToObject<PoloniexMarketTickers>();
                        jsonObjects.Add(key, tick);
                    }
                    catch (InvalidCastException) {}
                }

                foreach (var mkt in jsonObjects)
                {
                    Market m;
                    string sName = GetStandardisedName(mkt.Key);

                    /// If MarketsId list doesn't contain the market we need to create a new one.
                    if(!MarketIds.ContainsKey(sName))
                    {
                        MarketIds.Add(sName, mkt.Key);
                        string[] curCom = sName.Split('/');

                        m = new Market(this.Exchange)
                        {
                            PriceDecimals = 8,
                            QuantDecimals = 8,
                            MinPrice = 0.00000001,
                            MaxPrice = 0,
                            MinQuant = 0,
                        };
                        MarketIdentity mi = new MarketIdentity(m)
                        {
                            Commodity = curCom[0],
                            Currency = curCom[1],
                            MarketId = mkt.Key,
                            Name = mkt.Key,
                            StandardisedName = sName
                        };
                        m.MarketIdentity = mi;

                        Fee fee = new Fee
                        {
                            Market = m,
                            MarketId = mkt.Key,
                            FeePercentage = 0.2,
                            Quantity = 0
                        };
                        m.Fees.Add(fee);

                        _marketList.Add(m);
                    }
                    /// Else we want to update the existing market
                    else
                    {
                        var q = from mkts in _marketList
                                where mkts.MarketIdentity.StandardisedName == sName
                                select mkts;

                        if (q.Count() > 0)
                        {
                            m = q.First();
                        }
                        else m = null;
                    }

                    if (null != m)
                    {
                        //Poloniex trade records need to be downloaded individually
                        //also there is a cap on api requests. For these reasons we
                        //add a minimal trade object to the top of the trade stack to
                        //prevent null reference errors.
                        TradeRecord tr = new TradeRecord
                        {
                            Price = mkt.Value.LastTradePrice
                        };
                        m.TradeRecords.Push(tr);

                        m.Ticker.TopAsk = mkt.Value.TopAsk;
                        m.Ticker.TopBid = mkt.Value.TopBid;
                        m.Ticker.LastTrade.Price = mkt.Value.LastTradePrice;
                    }                    
                }
            }
        }
        private static object bookLocker = new object();
        private async Task getOrderBooks()
        {
            string url = @"http://poloniex.com/public?command=returnOrderBook&currencyPair=all&depth=100";

            string json = await tryDownload(url);

            lock (bookLocker)
            {
                if (!string.IsNullOrEmpty(json))
                {
                    var orders = JsonConvert.DeserializeObject<Dictionary<string, PoloniexMarketOrderBook>>(json);
                    foreach (var item in orders)
                    {
                        string sName = GetStandardisedName(item.Key);

                        var q = from m in _marketList where m.MarketIdentity.StandardisedName == sName select m;

                        if (q.Count() > 0)
                        {
                            var market = q.First();

                            if (market.OrderBook == null)
                                market.OrderBook = new OrderBook();

                            PoloniexMarketOrderBook mob = item.Value;
                            foreach (var ask in mob.Asks)
                            {
                                market.OrderBook.AskOrders.Add(new Order(OrderType.Ask) { Price = ask[0], Quantity = ask[1] });
                            }
                            foreach (var bid in mob.Bids)
                            {
                                market.OrderBook.BidOrders.Add(new Order(OrderType.Bid) { Price = bid[0], Quantity = bid[1] });
                            }
                        }
                    }
                }
            }
            return;
        }
        #endregion

        #region Methods
        public override string GetStandardisedName(string Name)
        {
            string[] parts = Name.Split('_');
            string sName = string.Format("{0}/{1}", parts[1], parts[0]);
            return sName;
        }
        public override HashSet<Market> GetAllMarketData()
        {
            Task<HashSet<Market>> t = Task.Run(async () => await PoloniexCombinedUpdate());
            HashSet<Market> r = t.Result;
            return r;
        }
        public override OrderBook GetSingleMarketOrders(MarketIdentity MarketIdent)
        {
            OrderBook ob = new OrderBook();
            string url = string.Format("http://poloniex.com/public?command=returnOrderBook&currencyPair={0}&depth=100", MarketIdent.MarketId);

            Task<String> t = Task.Run(async () => await tryDownload(url));
            string json = t.Result;

            if (!string.IsNullOrEmpty(json))
            {
                ///2 bools to check first if we can deserialise as an error
                ///and second to see if error has value.
                bool hasErrs = false;
                bool chkErr = false;
                try
                {
                    JToken err;
                    chkErr = JObject.Parse(json).TryGetValue("error", out err);
                    if (chkErr)
                        hasErrs = err.Parent.HasValues;
                }
                catch (JsonReaderException) { hasErrs = false; }
                catch (Exception) { hasErrs = false; }
                //If the parse failed, or error has no value we can continue.
                if (!chkErr || !hasErrs)
                {
                    var orderBooks = JsonConvert.DeserializeObject<PoloniexMarketOrderBook>(json);
                    foreach (var ask in orderBooks.Asks)
                    {
                        ob.AskOrders.Add(new Order(OrderType.Ask) { Price = ask[0], Quantity = ask[1] });
                    }
                    foreach (var bid in orderBooks.Bids)
                    {
                        ob.BidOrders.Add(new Order(OrderType.Bid) { Price = bid[0], Quantity = bid[1] });
                    }
                }
            }
            return ob;
        }
        public override Stack<TradeRecord> GetSingleMarketTradeHistory(MarketIdentity MarketIdent)
        {
            Stack<TradeRecord> trades = new Stack<TradeRecord>();

            string url = string.Format("https://poloniex.com/public?command=returnTradeHistory&currencyPair={0}", MarketIdent.MarketId);

            Task<String> t = Task.Run(async () => await tryDownload(url));
            string json = t.Result;

            if (!string.IsNullOrEmpty(json))
            {
                ///2 bools to check first if we can deserialise as an error
                ///and second to see if error has value.
                bool hasErrs = false;
                bool chkErr = false;
                try
                {
                    JToken err;
                    chkErr = JObject.Parse(json).TryGetValue("error", out err);
                    if(chkErr)
                        hasErrs = err.Parent.HasValues;
                }
                catch (JsonReaderException) { hasErrs = false; }
                catch (Exception) { hasErrs = false; }
                //If the parse failed, or error has no value we can continue.
                if (!chkErr || !hasErrs)
                {
                    var tradeHist = JsonConvert.DeserializeObject<List<PoloniexTradeHistory>>(json);

                    var sortedHist = tradeHist.OrderBy(p => p.DateTime).ToList();

                    foreach (var trade in sortedHist)
                    {
                        OrderType type = OrderType.Ask;
                        if (trade.TradeType == "buy")
                            type = OrderType.Bid;

                        trades.Push(new TradeRecord
                        {
                            Price = trade.Price,
                            Quantity = trade.Quantity,
                            Type = type,
                            TradeTime = trade.DateTime
                        });
                    }
                }
            }
            return trades;
        }
        public override HashSet<ActiveOrder> GetActiveOrders()
        {
            HashSet<ActiveOrder> activeOrders = new HashSet<ActiveOrder>();

            if (ApiActive && HasKeys)
            {
                Dictionary<string, string> paras = new Dictionary<string, string>();
                paras.Add("currencyPair", "all");

                string json = AuthenticatedRequest("returnOpenOrders", paras);
                if (!string.IsNullOrEmpty(json))
                {
                    var openOrds = JsonConvert.DeserializeObject<Dictionary<string, List<PoloniexOpenOrders>>>(json);
                    foreach (var orders in openOrds)
                    {
                        foreach (var order in orders.Value)
                        {
                            OrderType type = OrderType.Bid;
                            if (order.type == "sell")
                                type = OrderType.Ask;

                            string mId = MarketIds[GetStandardisedName(orders.Key)];
                            activeOrders.Add(new ActiveOrder(ExchangeName, mId)
                            {
                                OrderId = order.orderNumber,
                                Created = DateTime.Parse(order.date),
                                Price = order.rate,
                                Quantity = order.amount,
                                OrderType = type,
                                Remaining = order.amount,
                            });
                        }
                    }
                }
            }
            return activeOrders;
        }
        public override HashSet<Balance> GetBalances()
        {
            HashSet<Balance> _balances = new HashSet<Balance>();

            if (ApiActive && HasKeys)
            {
                string json = AuthenticatedRequest("returnCompleteBalances");

                if (!string.IsNullOrEmpty(json))
                {
                    var balanceList = JsonConvert.DeserializeObject<Dictionary<string, PoloniexBalances>>(json);

                    foreach (var kvp in balanceList)
                    {
                        _balances.Add(new Balance
                        {
                            Available = kvp.Value.available,
                            Held = kvp.Value.onOrders,
                            Name = kvp.Key,
                            Exchange = this.Exchange
                        });
                    }
                }
            }
            return _balances;
        }
        public override Tuple<string, string> PlaceBasicOrder(MarketIdentity MarketIdent, OrderType Type, double Price, double Quantity)
        {
            Tuple<string, string> result = null;

            if (ApiActive && HasKeys)
            {
                string orderType = "buy";
                if (Type == OrderType.Ask)
                    orderType = "sell";

                Dictionary<string, string> paramList = new Dictionary<string, string>();
                paramList.Add("currencyPair", MarketIdent.MarketId);
                paramList.Add("rate", Price.ToString());
                paramList.Add("amount", Quantity.ToString());
                string json = AuthenticatedRequest(orderType, paramList);

                result = new Tuple<string, string>(json, string.Format("Order placed. {0}", json));
            }

            return result;
        }
        public override string CancelOrder(ActiveOrder orderObj)
        {
            string json = null;

            if (ApiActive && HasKeys)
            {
                Dictionary<string, string> paramList = new Dictionary<string, string>();
                paramList.Add("currencyPair", orderObj.MarketId);
                paramList.Add("orderNumber", orderObj.OrderId);
                json = AuthenticatedRequest("cancelOrder", paramList);
            }

            return json;
        }
        #endregion

        #region Calls
        private async Task<string> tryDownload(string url)
        {
            string json = String.Empty;
            ///Make thread wait for 300ms to prevent
            ///exceeding rate limit.
            ///We do this at the start to prevent making the data old.
            await Task.Delay(300);
            try
            {
                json = await WebClientController(url);
                ApiActive = true;
            }
            catch (WebException)
            {
                ApiActive = false;
            }
            return json;
        }
        private static readonly object apiLock = new object();
        private string AuthenticatedRequest(string method, Dictionary<string, string> paramList = null)
        {
            String json;
            lock (apiLock)
            {
                json = String.Empty;

                var postData = String.Format("command={0}&nonce={1}", method, DateTime.Now.Ticks);
                if (paramList != null)
                {
                    postData = paramList.Aggregate(postData, (current, pair) => current + String.Format("&{0}={1}", pair.Key, HttpUtility.UrlEncode(pair.Value)));
                }

                var messagebyte = Encoding.ASCII.GetBytes(postData);
                var hashmessage = hmAcSha.ComputeHash(messagebyte);
                var sign = BitConverter.ToString(hashmessage);
                sign = sign.Replace("-", "");

                var request = (HttpWebRequest)WebRequest.Create("https://poloniex.com/tradingApi");
                request.CookieContainer = cookieJar;
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = messagebyte.Length;
                request.Method = "POST";
                request.Headers.Add("Key", _publicKey);
                request.Headers.Add("Sign", sign.ToLower());

                try
                {
                    using (var stream = request.GetRequestStream())
                    {
                        stream.Write(messagebyte, 0, messagebyte.Length);
                        stream.Close();
                    }
                    using (var response = (HttpWebResponse)request.GetResponse())
                    {
                        using (var postreqreader = new StreamReader(response.GetResponseStream()))
                        {
                            json = postreqreader.ReadToEnd();
                        }
                        response.Close();
                    }
                    ApiActive = true;
                }
                catch (WebException)
                {
                    ApiActive = false;
                }
            }
            return json;
        }
        #endregion
    }
}
