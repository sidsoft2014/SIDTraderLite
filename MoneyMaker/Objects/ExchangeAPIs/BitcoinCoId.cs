﻿using System;
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

using Json.BitcoinCoId;


namespace Objects
{
    public class API_BitcoinCoId : IExchange
    {
        public API_BitcoinCoId(Exchange exchange)
        {
            this.Exchange = exchange;
            _marketList = new HashSet<Market>();

            this.MarketIds = new Dictionary<string, string>()
            {
                {"BTC/IDR", "btc_idr"},
                {"LTC/BTC", "ltc_btc"},
                {"DOGE/BTC", "doge_btc"}
            };

            foreach (var item in MarketIds)
            {
                string[] curCom = item.Value.ToUpper().Split('_');

                Market market = new Market(Exchange);
                market.PriceDecimals = 8;
                market.QuantDecimals = 8;
                market.MaxPrice = 0;
                market.MinPrice = 0.00000001;
                market.MinQuant = 0.00000001;

                Fee fee = new Fee
                {
                    Market = market,
                    MarketId = item.Value,
                    FeePercentage = 0.3,
                    Quantity = 0
                };
                market.Fees.Add(fee);

                MarketIdentity mi = new MarketIdentity(market)
                {
                    Commodity = curCom[0],
                    Currency = curCom[1],
                    MarketId = item.Value,
                    Name = item.Value,
                    StandardisedName = item.Key
                };
                market.MarketIdentity = mi;

                _marketList.Add(market);
            }
        }
        ~API_BitcoinCoId()
        {

        }

        private HashSet<Market> CombinedUpdate()
        {
            GetAllOrderBooks();
            GetAllTradeHistorys();

            return _marketList;
        }
        private void GetAllOrderBooks()
        {
            foreach (var m in _marketList)
            {
                m.OrderBook = GetSingleMarketOrders(m.MarketIdentity);
            }
        }
        private void GetAllTradeHistorys()
        {
            foreach (var m in _marketList)
            {
                m.TradeRecords = GetSingleMarketTradeHistory(m.MarketIdentity);
            }
        }

        #region IExchange
        public override string GetStandardisedName(string Name)
        {
            string[] parts = Name.ToUpper().Split('_');
            string result = string.Format("{0}/{1}", parts[0], parts[1]);
            return result;
        }
        public override HashSet<Market> GetAllMarketData()
        {
            return CombinedUpdate();
        }
        public override OrderBook GetSingleMarketOrders(MarketIdentity MarketIdent)
        {
            OrderBook oB = new OrderBook();
            string url = string.Format("https://vip.bitcoin.co.id/api/{0}/depth/", MarketIdent.MarketId);

            string json = tryDownload(url);

            if (!string.IsNullOrEmpty(json))
            {
                Dictionary<string, Depth> ordersList = new Dictionary<string, Depth>();

                var jObj = JObject.Parse(json);
                var asks = jObj["sell"];
                var bids = jObj["buy"];

                foreach (var ask in asks)
                {
                    oB.AskOrders.Add(new Order(OrderType.Ask) { Price = (double)ask[0], Quantity = (double)ask[1] });
                }
                foreach (var bid in bids)
                {
                    oB.BidOrders.Add(new Order(OrderType.Bid) { Price = (double)bid[0], Quantity = (double)bid[1] });
                }
            }
            return oB;
        }
        public override Stack<TradeRecord> GetSingleMarketTradeHistory(MarketIdentity MarketIdent)
        {
            Stack<TradeRecord> records = new Stack<TradeRecord>();

            string url = string.Format("https://vip.bitcoin.co.id/api/{0}/trades/", MarketIdent.MarketId);

            string json = tryDownload(url);

            if (!string.IsNullOrEmpty(json))
            {
                List<MarketTrades> trades = JsonConvert.DeserializeObject<List<MarketTrades>>(json);

                foreach (var trade in trades.OrderByDescending(p => p.date))
                {
                    DateTime time = GeneralTools.TimeStampToDateTimeUsingSeconds(trade.date);

                    OrderType type = OrderType.Ask;
                    if (trade.type == "bid")
                        type = OrderType.Bid;

                    records.Push(new TradeRecord() { Price = trade.price, Quantity = trade.amount, TradeTime = time, Type = type });
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
                    {"method", "openOrders"}
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
                    Dictionary<string, double> bals = jObject.balanceInfo.balance;
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

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://vip.bitcoin.co.id/tapi");
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
