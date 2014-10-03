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

using Json.BTCe;


namespace Objects
{
    public class API_BTCe : IExchange
    {
        internal CookieContainer cookieJar;
        UInt32 nonce = UnixTime.Now;
        UInt32 GetTheNonce()
        {
            return nonce++;
        }
        
        public API_BTCe(Exchange exchange)
        {
            this.Exchange = exchange;
            MarketIds = new Dictionary<string, string>();
            cookieJar = new CookieContainer();
        }
        ~API_BTCe()
        {
            ClearKeys();
        }

        private HashSet<Market> BTCeCombinedUpdate()
        {
            var tempMarketsList = new HashSet<Market>();
            BTCe_GetAllMarketSettings(ref tempMarketsList);
            BTCe_GetAllOrderBooks(ref tempMarketsList);
            BTCe_GetAllTradeHistorys(ref tempMarketsList);

            return tempMarketsList;
        }
        private void BTCe_GetAllMarketSettings(ref HashSet<Market> tempMarketsList)
        {
            string url = @"https://btc-e.com/api/3/info";
            string json =  tryDownload(url);

            if (!string.IsNullOrEmpty(json))
            {
                GetExcahngeInfo jObject =
                    JObject.Parse(json).ToObject<GetExcahngeInfo>();

                var marketList = jObject.pairs;
                foreach (var mkt in marketList)
                {
                    string standardName = GetStandardisedName(mkt.Key);
                    string[] curCom = mkt.Key.ToUpper().Split('_');

                    Market market = new Market(Exchange);
                    market.PriceDecimals = mkt.Value.decimal_places;
                    market.QuantDecimals = 8;
                    market.MaxPrice = mkt.Value.max_price;
                    market.MinPrice = mkt.Value.min_price;
                    market.MinQuant = mkt.Value.min_amount;
                    
                    Fee fee = new Fee
                    {
                        Market = market,
                        MarketId = mkt.Key,
                        FeePercentage = mkt.Value.fee,
                        Quantity = 0
                    };
                    market.Fees.Add(fee);

                    MarketIdentity mi = new MarketIdentity(market)
                    {
                        Commodity = curCom[0],
                        Currency = curCom[1],
                        MarketId = mkt.Key,
                        Name = mkt.Key,
                        StandardisedName = standardName
                    };
                    market.MarketIdentity = mi;

                    if (!MarketIds.ContainsKey(standardName))
                        MarketIds.Add(standardName, mkt.Key);
                    
                    tempMarketsList.Add(market);
                }
            }
        }
        private void BTCe_GetAllOrderBooks(ref HashSet<Market> tempMarketsList)
        {
            string url = @"https://btc-e.com/api/3/depth/";
            for (int ii = 0; ii < MarketIds.Count; ii++)
            {
                var kvp = MarketIds.ElementAt(ii);
                if (ii == 0)
                    url = url + kvp.Value;
                else
                    url = String.Format("{0}-{1}", url, kvp.Value);
            }

            string json = tryDownload(url);

            if (!string.IsNullOrEmpty(json))
            {
                Dictionary<string, Depth> ordersList =
                    JsonConvert.DeserializeObject<Dictionary<string, Depth>>(json);

                for (int ii = 0; ii < ordersList.Count; ii++)
                {
                    var kvp = ordersList.ElementAt(ii);
                    Depth jObject = kvp.Value;
                    string sName = GetStandardisedName(kvp.Key);
                    var m = tempMarketsList.First(s => s.MarketIdentity.StandardisedName == sName);
                    OrderBook oB = new OrderBook();
                    foreach (var ask in jObject.asks)
                    {
                        oB.AskOrders.Add(new Order(OrderType.Ask) { Price = ask[0], Quantity = ask[1] });
                    }
                    foreach (var bid in jObject.bids)
                    {
                        oB.BidOrders.Add(new Order(OrderType.Bid) { Price = bid[0], Quantity = bid[1] });
                    }
                    m.OrderBook = oB;
                }
            }
        }
        private void BTCe_GetAllTradeHistorys(ref HashSet<Market> tempMarketsList)
        {
            string url = @"https://btc-e.com/api/3/trades/";
            for (int ii = 0; ii < MarketIds.Count; ii++)
            {
                var kvp = MarketIds.ElementAt(ii);
                if (ii == 0)
                    url = url + kvp.Value;
                else
                    url = String.Format("{0}-{1}", url, kvp.Value);
            }

            string json = tryDownload(url);

            if (!string.IsNullOrEmpty(json))
            {
                Dictionary<string, List<MarketTrades>> jsonTradesList =
                    JsonConvert.DeserializeObject<Dictionary<string, List<MarketTrades>>>(json);

                for (int ii = 0; ii < jsonTradesList.Count; ii++)
                {
                    var kvp = jsonTradesList.ElementAt(ii);
                    string name = GetStandardisedName(kvp.Key);
                    Market m = tempMarketsList.First(s => s.MarketIdentity.StandardisedName == name);

                    List<MarketTrades> trades = kvp.Value;

                    trades.Reverse();

                    foreach (var trade in trades)
                    {
                        DateTime time = GeneralTools.TimeStampToDateTimeUsingSeconds(trade.timestamp);

                        OrderType type = OrderType.Ask;
                        if (trade.type == "bid")
                            type = OrderType.Bid;

                        m.TradeRecords.Push(new TradeRecord() { Price = trade.price, Quantity = trade.amount, TradeTime = time, Type = type });
                    }
                }
            }
        }

        #region IExchange
        public override Dictionary<string, string> MarketIds { get; protected set; }
        public override ExchangeEnum ExchangeName { get { return ExchangeEnum.BTCe; } }
        public override string GetStandardisedName(string Name)
        {
            string[] parts = Name.ToUpper().Split('_');
            string result = string.Format("{0}/{1}", parts[0], parts[1]);
            return result;
        }        
        public override HashSet<Market> GetAllMarketData()
        {
            return BTCeCombinedUpdate();
        }
        public override Market GetSingleMarket(MarketIdentity MarketIdent)
        {
            var hs = BTCeCombinedUpdate();
            var querey = from mi in hs
                         where mi.MarketIdentity.StandardisedName == MarketIdent.StandardisedName
                         select mi;
            if (querey.Count() > 0)
                return querey.First();
            else return null;
        }
        public override OrderBook GetSingleMarketOrders(MarketIdentity MarketIdent)
        {
            OrderBook ob = new OrderBook();
            string url = String.Format("https://btc-e.com/api/3/depth/{0}", MarketIdent.MarketId);

            string json = tryDownload(url);

            if (!string.IsNullOrEmpty(json))
            {
                Dictionary<string, Depth> ordersList =
                    JsonConvert.DeserializeObject<Dictionary<string, Depth>>(json);

                for (int ii = 0; ii < ordersList.Count; ii++)
                {
                    var kvp = ordersList.ElementAt(ii);
                    Depth jObject = kvp.Value;
                    foreach (var ask in jObject.asks)
                    {
                        ob.AskOrders.Add(new Order(OrderType.Ask) { Price = ask[0], Quantity = ask[1] });
                    }
                    foreach (var bid in jObject.bids)
                    {
                        ob.BidOrders.Add(new Order(OrderType.Bid) { Price = bid[0], Quantity = bid[1] });
                    }
                }
            }
           
            return ob;
        }
        public override HashSet<TradeRecord> GetSingleMarketTradeHistory(MarketIdentity MarketIdent)
        {
            HashSet<TradeRecord> records = new HashSet<TradeRecord>();

            string url = String.Format("https://btc-e.com/api/3/trades/{0}", MarketIdent.MarketId);

            string json = tryDownload(url);

            if (!string.IsNullOrEmpty(json))
            {
                Dictionary<string, List<MarketTrades>> jsonTradesList =
                    JsonConvert.DeserializeObject<Dictionary<string, List<MarketTrades>>>(json);

                for (int ii = 0; ii < jsonTradesList.Count; ii++)
                {
                    var kvp = jsonTradesList.ElementAt(ii);
                    string name = GetStandardisedName(kvp.Key);

                    List<MarketTrades> trades = kvp.Value;
                    foreach (var trade in trades)
                    {
                        DateTime time = GeneralTools.TimeStampToDateTimeUsingSeconds(trade.timestamp);
                        records.Add(new TradeRecord() { Price = trade.price, Quantity = trade.amount, TradeTime = time });
                    }
                }
            }
            return records;
        }        
        public override HashSet<ActiveOrder> GetActiveOrders()
        {
            HashSet<ActiveOrder> orders = new HashSet<ActiveOrder>();
            if (ApiActive && HasKeys)
            {
                string json = null;
                var args = new Dictionary<string, string>()
                {
                    {"method", "ActiveOrders"}
                };

                json = AuthenticatedRequest(args);
                if (!string.IsNullOrEmpty(json))
                {
                    ActiveOrdersRoot jObject =
                        JObject.Parse(json).ToObject<ActiveOrdersRoot>();

                    if (jObject.Success)
                    {
                        Dictionary<string, ActiveOrders> jOutput = jObject.OpenOrders as Dictionary<string, ActiveOrders>;

                        for (int ii = 0; ii < jOutput.Count; ii++)
                        {
                            var joo = jOutput.ElementAt(ii);

                            var dt = GeneralTools.TimeStampToDateTimeUsingSeconds(joo.Value.timestamp_created);

                            OrderType type = OrderType.Bid;
                            if (joo.Value.type == "sell")
                                type = OrderType.Ask;

                            ActiveOrder row = new ActiveOrder(ExchangeName, joo.Value.pair)
                            {
                                Price = (double)joo.Value.rate,
                                Quantity = (double)joo.Value.amount,
                                Remaining = Double.NaN,
                                OrderId = joo.Key,
                                OrderType = type,
                                Created = dt
                            };
                            orders.Add(row);
                        }
                    }
                }
            }
            return orders;
        }
        public override HashSet<Balance> GetBalances()
        {
            var balances = new HashSet<Balance>();

            if (ApiActive && HasKeys)
            {
                var args = new Dictionary<string, string>()
                {
                    {"method", "getInfo"}
                };
                string json = AuthenticatedRequest(args);

                var jObject = JsonConvert.DeserializeObject<JsonFields_BTCe_GetInfoRoot>(json);

                if (jObject.success)
                {
                    Dictionary<string, double> bals = jObject.balanceInfo.funds;
                    foreach (var kvp in bals)
                    {
                        balances.Add(new Balance
                        {
                            Exchange = this.Exchange,
                            Name = kvp.Key.ToUpper(),
                            Available = kvp.Value
                        });
                    }
                }
            }
            return balances;
        }
        public override Tuple<string, string> PlaceBasicOrder(MarketIdentity MarketIdent, OrderType Type, double Price, double Quantity)
        {
            Tuple<string, string> result = null;

            if (ApiActive && HasKeys)
            {
                string type = "buy";
                if (Type == OrderType.Ask)
                    type = "sell";

                var args = new Dictionary<string, string>()
                {
                    { "method", "Trade" },
                    { "pair", MarketIdent.MarketId },
                    { "type", type },
                    { "rate", Price.ToString()},
                    { "amount", Quantity.ToString()}
                };

                string response = AuthenticatedRequest(args);
                if (response != null)
                {
                    var jObj = JObject.Parse(response);
                    if (jObj.GetValue("success").ToObject<Int16>() == 1)
                    {
                        var fields = jObj.GetValue("return").ToObject<TradeResponse>();
                        string msg = string.Format("Order placed: {0}", fields.order_id);
                        result = new Tuple<string, string>(fields.order_id.ToString(), msg);
                    }
                }
            }

            return result;
        }
        public override string CancelOrder(string OrderId, string MarketId = null)
        {
            string result = null;

            if (ApiActive && HasKeys)
            {
                var args = new Dictionary<string, string>()
                {
                    {"method", "CancelOrder"},
                    {"order_id", OrderId}
                };

                string response = AuthenticatedRequest(args);
                var jObj = JObject.Parse(response);
                int success = jObj.GetValue("success").ToObject<Int16>();

                if (success == 1)
                {
                    var ordId = jObj.GetValue("return").First().ToString();
                    string id = ordId.Split(':')[1];
                    result = string.Format("Order {0} has been cancelled", id);
                }
            }

            return result;
        }
        #endregion

        #region Calls
        private string tryDownload(string url)
        {
            string json = String.Empty;
            try
            {
                json = Task.Run(async () => await WebClientController(url)).Result;
            }
            catch
            {
                if (ApiActive)
                {
                    ApiActive = false;
                }
            }
            return json;
        }
        private string AuthenticatedRequest(Dictionary<string, string> orderinfo = null, int maxRetryCount = 3)
        {
            orderinfo.Add("nonce", GetTheNonce().ToString());
            string postData = BuildPostData(orderinfo);
            byte[] messagebyte = Encoding.ASCII.GetBytes(postData);
            
            byte[] hashmessage = hmAcSha.ComputeHash(messagebyte);
            string sign = BitConverter.ToString(hashmessage);
            sign = sign.Replace("-", "");

            string json = String.Empty;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://btc-e.com/tapi");
            request.CookieContainer = cookieJar;
            request.UserAgent = "Mozilla/5.0 (.NET CLR 3.5.30729)";
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
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (StreamReader postReader = new StreamReader(response.GetResponseStream()))
                    {
                        json = postReader.ReadToEnd();
                    }
                    response.Close();
                }
                ApiActive = true;
            }
            catch (WebException ex)
            {
                System.Threading.Thread.Sleep(250);
                if (--maxRetryCount == 0)
                {
                    ApiActive = false;
                    throw ex;
                }
            }
            catch (System.Net.Sockets.SocketException er)
            {
                Logger.WriteEvent(er.InnerException);
            }
            return json;
        }
        static string BuildPostData(Dictionary<string, string> paramList)
        {
            StringBuilder s = new StringBuilder();
            foreach (var item in paramList)
            {
                s.AppendFormat("{0}={1}", item.Key, HttpUtility.UrlEncode(item.Value));
                s.Append("&");
            }
            if (s.Length > 0) s.Remove(s.Length - 1, 1);
            return s.ToString();
        }
        #endregion
    }
}
