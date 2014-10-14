using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Json.Cryptsy;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Toolbox;

namespace Objects
{
    /// <summary>
    /// Wrapper for the Cryptsy API
    /// </summary>
    public class API_Cryptsy : IExchange
    {
        internal static bool isDownloading = false;

        public API_Cryptsy(Exchange exchange)
        {
            this.Exchange = exchange;
        }
        ~API_Cryptsy() { ClearKeys(); }
        
        public override string GetStandardisedName(string Name)
        {
            return Name.ToUpper();
        }
        public override HashSet<Market> GetAllMarketData()
        {
            if (!isDownloading)
            {
                isDownloading = true;

                #region Get new market data
                HashSet<Market> markets = new HashSet<Market>();

                string json = null;
                try
                {
                    json = Task.Run(async () => await GetJsonString()).Result;
                }
                catch
                { }

                if (!string.IsNullOrEmpty(json))
                {
                    var jObj = JObject.Parse(json);
                    var checkResult = jObj.GetValue("return").ToString();

                    bool validBoolean;
                    if (!Boolean.TryParse(checkResult, out validBoolean))
                    {

                        var ret = jObj.GetValue("return").First().First();

                        var overviews = ret.ToObject<Dictionary<string, MarketOverview.Details>>();

                        foreach (var item in overviews)
                        {
                            var overview = item.Value;

                            Market mkt = new Market(Exchange);
                            mkt.MarketIdentity.MarketId = overview.marketid;
                            mkt.MarketIdentity.Name = overview.label;
                            mkt.MarketIdentity.StandardisedName = overview.label.ToUpper();

                            string[] curCom = overview.label.ToUpper().Split('/');
                            mkt.MarketIdentity.Commodity = curCom[0];
                            mkt.MarketIdentity.Currency = curCom[1];

                            mkt.PriceDecimals = 8;
                            mkt.QuantDecimals = 8;

                            mkt.MinPrice = 0.00000001;
                            mkt.MaxPrice = 0;
                            mkt.MinQuant = 0;

                            Fee fee = new Fee { Quantity = 0, FeePercentage = 0.25, Market = mkt, MarketId = mkt.MarketIdentity.MarketId };
                            mkt.Fees.Add(fee);

                            OrderBook tmpOb = new OrderBook();
                            if (overview.sellorders != null)
                            {
                                foreach (var order in overview.sellorders)
                                {
                                    double price = 0;
                                    if (order.price != null)
                                        price = (double)order.price;

                                    double quant = 0;
                                    if (order.quantity != null)
                                        quant = (double)order.quantity;

                                    Order aO = new Order(OrderType.Ask);
                                    aO.Price = price;
                                    aO.Quantity = quant;

                                    tmpOb.AskOrders.Add(aO);
                                }
                            }
                            if (overview.buyorders != null)
                            {
                                foreach (var order in overview.buyorders)
                                {
                                    double price = 0;
                                    if (order.price != null)
                                        price = (double)order.price;

                                    double quant = 0;
                                    if (order.quantity != null)
                                        quant = (double)order.quantity;

                                    Order bO = new Order(OrderType.Bid);
                                    bO.Price = price;
                                    bO.Quantity = quant;

                                    tmpOb.BidOrders.Add(bO);
                                }
                            }
                            mkt.OrderBook = tmpOb;

                            if (overview.recenttrades != null)
                            {
                                overview.recenttrades.Reverse();

                                foreach (var trade in overview.recenttrades)
                                {
                                    OrderType type = OrderType.Ask;
                                    if (trade.type == "Buy")
                                        type = OrderType.Bid;

                                    mkt.TradeRecords.Push(new TradeRecord()
                                    {
                                        Price = Convert.ToDouble(trade.price),
                                        Quantity = Convert.ToDouble(trade.quantity),
                                        TradeTime = DateTime.Parse(trade.time),
                                        Type = type
                                    });
                                }
                            }
                            markets.Add(mkt);
                        }
                        foreach (var item in markets)
                        {
                            if (!MarketIds.ContainsKey(item.MarketIdentity.Name))
                            {
                                MarketIds.Add(item.MarketIdentity.Name, item.MarketIdentity.MarketId);
                            }
                        }
                    }
                }
                #endregion

                isDownloading = false;
                this._marketList = markets;
                return markets;
            }
            else
            {
                return this._marketList;
            }
        }
        public override OrderBook GetSingleMarketOrders(MarketIdentity MarketIdent)
        {
            OrderBook oB = new OrderBook();
            string mId = MarketIdent.MarketId;
            
            string json = GetOrderBookAsync(mId).Result;
            
            if (!string.IsNullOrEmpty(json))
            {
                var jObj = JObject.Parse(json);

                var checkResult = jObj.GetValue("return").ToString();
                var ret = jObj.GetValue("return").First().First();
                var overviews = ret.ToObject<SingleMarketOrders>();

                if (overviews != null)
                {
                    var overview = overviews;
                    
                    if (overview.SellOrders != null)
                    {
                        foreach (var order in overview.SellOrders)
                        {
                            double price = 0;
                            if (order.price != null)
                                price = (double)order.price;

                            double quant = 0;
                            if (order.quantity != null)
                                quant = (double)order.quantity;

                            Order aO = new Order(OrderType.Ask);
                            aO.Price = price;
                            aO.Quantity = quant;

                            oB.AskOrders.Add(aO);
                        }
                    }
                    if (overview.BuyOrders != null)
                    {
                        foreach (var order in overview.BuyOrders)
                        {
                            double price = 0;
                            if (order.price != null)
                                price = (double)order.price;

                            double quant = 0;
                            if (order.quantity != null)
                                quant = (double)order.quantity;

                            Order bO = new Order(OrderType.Bid);
                            bO.Price = price;
                            bO.Quantity = quant;

                            oB.BidOrders.Add(bO);
                        }
                    }
                }
            }
            return oB;
        }

        private async Task<string> GetOrderBookAsync(string mId)
        {
            string result = await GetJsonString(mId, false);
            return result;
        }
        public override Stack<TradeRecord> GetSingleMarketTradeHistory(MarketIdentity MarketIdent)
        {
            Stack<TradeRecord> TradeRecords = new Stack<TradeRecord>();
            string mId = MarketIdent.MarketId;
            
            string json = null;
            try
            {
                json = Task.Run(async () => await GetJsonString(mId)).Result;
            }
            catch
            { }

            if (!string.IsNullOrEmpty(json))
            {
                var jObj = JObject.Parse(json);

                var checkResult = jObj.GetValue("return").ToString();
                var ret = jObj.GetValue("return").First().First();
                var overviews = ret.ToObject<Dictionary<string, MarketOverview.Details>>();

                var item = overviews.ElementAt(0);
                if (item.Value != null)
                {
                    var overview = item.Value;
                    
                    if (overview.recenttrades != null)
                    {

                        var sortedHist = overview.recenttrades.OrderBy(p => p.time).ToList();

                        foreach (var trade in sortedHist)
                        {
                            OrderType type = OrderType.Ask;
                            if (trade.type == "Buy")
                                type = OrderType.Bid;

                            TradeRecords.Push(new TradeRecord()
                            {
                                Price = Convert.ToDouble(trade.price),
                                Quantity = Convert.ToDouble(trade.quantity),
                                TradeTime = DateTime.Parse(trade.time),
                                Type = type
                            });
                        }
                    }
                }
            }
            return TradeRecords;
        }        
        public override HashSet<ActiveOrder> GetActiveOrders()
        {
            HashSet<ActiveOrder> ords = new HashSet<ActiveOrder>();
            if (ApiActive && HasKeys)
            {
                try
                {
                    var json = GetPrivateString("allmyorders");

                    if (!string.IsNullOrEmpty(json.Trim()))
                    {
                        JObject jObj = JObject.Parse(json);
                        var jTok = jObj.GetValue("return");
                        var ordList = jTok.ToObject<List<OpenOrders>>();

                        if (ordList != null && ordList.Count > 0)
                        {
                            foreach (var order in ordList)
                            {
                                var q = from mk in Exchange.Markets
                                        where mk.MarketIdentity.MarketId == order.marketid
                                        select mk;

                                if (q.Count() > 0)
                                {
                                    Market refMkt = q.First();
                                    OrderType ordType;
                                    if (order.ordertype == "Buy")
                                        ordType = OrderType.Bid;
                                    else
                                        ordType = OrderType.Ask;

                                    DateTime dt;
                                    if (!DateTime.TryParse(order.created, out dt))
                                        dt = GeneralTools.TimeStampToDateTimeUsingMilliseconds(Convert.ToInt64(order.created));

                                    ords.Add(new ActiveOrder(refMkt)
                                    {
                                        OrderId = order.orderid,
                                        Price = (double)order.price,
                                        Quantity = (double)order.orig_quantity,
                                        Remaining = (double)order.quantity,
                                        OrderType = ordType,
                                        Created = dt
                                    });
                                }
                            }
                        }
                    }
                }
                catch(Exception e)
                {
                    Logger.WriteEvent(e.InnerException);
                }
            }
            return ords;
        }
        public override HashSet<Balance> GetBalances()
        {
            HashSet<Balance> balances = new HashSet<Balance>();

            if (ApiActive && HasKeys)
            {
                try
                {
                    string method = "getinfo";
                    string json = GetPrivateString(method);

                    if (!string.IsNullOrEmpty(json.Trim()))
                    {

                        var jObj = JObject.Parse(json);
                        var jTok = jObj.GetValue("return");
                        BalanceInfo balanceData = jTok.ToObject<BalanceInfo>();


                        if (balanceData.balances_available != null)
                        {
                            foreach (var balance in balanceData.balances_available)
                            {
                                balances.Add(new Balance
                                {
                                    Exchange = this.Exchange,
                                    Name = balance.Key,
                                    Available = balance.Value
                                });
                            }
                        }
                        if (balanceData.balances_hold != null)
                        {
                            foreach (var held in balanceData.balances_hold)
                            {
                                var query = from b in balances
                                            where b.Name == held.Key
                                            select b;

                                if (query.Count() > 0)
                                    query.ElementAt(0).Held = held.Value;
                                else
                                {
                                    balances.Add(new Balance
                                    {
                                        Exchange = this.Exchange,
                                        Name = held.Key,
                                        Held = held.Value
                                    });
                                }
                            }
                        }
                    }
                }
                catch(Exception e)
                {
                    Logger.WriteEvent(e.InnerException);
                }
            }
            return balances;
        }
        public override Tuple<string, string> PlaceBasicOrder(
            MarketIdentity MarketIdent,
            OrderType Type,
            double Price,
            double Quantity)
        {
            Tuple<string, string> result = null;
            if (ApiActive && HasKeys)
            {
                string price = String.Format("{0:N8}", Price);
                string qty = String.Format("{0:N8}", Quantity);

                string orderType = null;

                if (Type == OrderType.Ask)
                    orderType = "Sell";
                else
                    orderType = "Buy";

                string method = "createorder";
                string orderinfo = String.Format("marketid={0}&ordertype={1}&quantity={2}&price={3}", MarketIdent.MarketId, orderType, qty, price);

                try
                {
                    string json = GetPrivateString(method, orderinfo);

                    var jObj = JObject.Parse(json);

                    var response = jObj.ToObject<OrderResponse>();
                    if (response.success == 1)
                    {
                        string moreInfo = response.moreinfo;
                        string msg = String.Format("Order Placed - Cryptsy\r\nMore Info: {0}", moreInfo);
                        result = new Tuple<string, string>(response.orderid, msg);
                    }
                    else
                    {
                        string errorInfo = jObj.GetValue("error").ToString();
                        string msg = String.Format("Error: {0}", errorInfo);
                        result = new Tuple<string, string>(null, msg);
                    }
                }
                catch(Exception e)
                {
                    Logger.WriteEvent(e.InnerException);
                }
            }
            return result;
        }
        public override String CancelOrder(string OrderId, string MarketId = null)
        {
            string request = null;
            string result = null;

            if (ApiActive && HasKeys)
            {
                if (OrderId == "All")
                {
                    request = "cancelallorders";
                }
                else
                {
                    request = String.Format("cancelorder&orderid={0}", OrderId);
                }
                try
                {
                    string json = GetPrivateString(request);
                    if (!string.IsNullOrEmpty(json))
                    {
                        try
                        {
                            var job = JsonConvert.DeserializeObject<BaseResponse>(json);
                            result = job.DataList.ToString();
                        }
                        catch
                        {
                            result = "Success";
                        }
                    }
                    else result = "Error";
                }
                catch(Exception e)
                {
                    Logger.WriteEvent(e.InnerException);
                }
            }
            return result;
        }        

        private async Task<string> GetJsonString(string MarketId = null, bool General = true, int Server = 1)
        {
            string json = String.Empty;

            if (MarketId == null || MarketIds.ContainsValue(MarketId))
            {

                string url = string.Empty;

                switch (MarketId)
                {
                    case null:
                        {
                            if (General)
                                url = string.Format("http://pubapi{0}.cryptsy.com/api.php?method=marketdatav2", Server);
                            else
                                url = string.Format("http://pubapi{0}.cryptsy.com/api.php?method=orderdatav2", Server);
                            break;
                        }
                    default:
                        {
                            if (General)
                                url = string.Format("http://pubapi{0}.cryptsy.com/api.php?method=singlemarketdata&marketid={1}", Server, MarketId);
                            else
                                url = string.Format("http://pubapi{0}.cryptsy.com/api.php?method=singleorderdata&marketid={1}", Server, MarketId);
                            break;
                        }
                }

                try
                {
                    using (WebClient wc = new WebClient())
                    {
                        wc.DownloadProgressChanged += wc_DownloadProgressChanged;
                        wc.DownloadStringCompleted += wc_DownloadStringCompleted;
                        json = await wc.DownloadStringTaskAsync(url);
                        wc.Dispose();
                    }
                }
                catch (WebException we)
                {
                    Logger.WriteEvent("Cryptsy API Error: " + we.InnerException);
                }
                catch (System.Net.Sockets.SocketException er)
                {
                    Logger.WriteEvent(er.InnerException);
                }


                bool isBoolean = false;

                if (!string.IsNullOrEmpty(json))
                {
                    var tester = JObject.Parse(json).GetValue("return").ToString();
                    Boolean.TryParse(tester, out isBoolean);
                }

                if (json == null || isBoolean)
                {
                    if (Server == 2)
                        Server = 1;
                    else
                        Server = 2;

                    if (ApiActive)
                        ApiActive = false;
                }
                else
                {
                    if (!ApiActive)
                        ApiActive = true;
                }
            }
            return json;
        }
        private string GetPrivateString(string method, string reqInfo = null)
        {
            string postData = String.Format("method={0}&nonce={1}", method, DateTime.Now.Ticks);

            if (reqInfo != null)
            {
                string makeString = String.Format("{0}&{1}", postData, reqInfo);
                postData = makeString;
            }
            byte[] messagebyte = Encoding.ASCII.GetBytes(postData);
            
            byte[] hashmessage = hmAcSha.ComputeHash(messagebyte);
            string sign = BitConverter.ToString(hashmessage);
            sign = sign.Replace("-", "");

            string json = String.Empty;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.cryptsy.com/api");
            request.CookieContainer = cookieJar;
            request.UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; Trident/6.0)";
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

                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        using (StreamReader postReader = new StreamReader(response.GetResponseStream()))
                        {
                            json = postReader.ReadToEnd();
                            ApiActive = true;
                        };
                        response.Close();
                    };
                };
            }
            catch (WebException ex)
            {
                Logger.WriteEvent(ex.Message);
                ApiActive = false;
            }
            catch (System.Net.Sockets.SocketException er)
            {
                Logger.WriteEvent(er.InnerException);
            }
            return json;
        }
    }
}
