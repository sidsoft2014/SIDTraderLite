using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objects
{
    class Template
    {
        internal static bool isDownloading = false;
        private string _secret;

        public Template(Exchange exchange)
        {
            this.Exchange = exchange;
        }
        ~Template() { ClearKeys(); }

        public override string GetStandardisedName(string Name)
        {
            var parts = Name.Split('-');
            string nom = String.Format("{0}/{1}", parts[1], parts[0]);
            return nom;
        }
        
        public override HashSet<Market> GetAllMarketData()
        {
            if (_marketList == null)
                _marketList = new HashSet<Market>();

            return _marketList;
        }

        public override Market GetSingleMarket(MarketIdentity MarketIdent)
        {
            Market mkt = new Market(this.Exchange);

            return mkt;
        }

        public override OrderBook GetSingleMarketOrders(MarketIdentity MarketIdent)
        {
            OrderBook ob = new OrderBook();

            if(_marketList != null && _marketList.Count >0)
            {

            }

            return ob;
        }

        public override Stack<TradeRecord> GetSingleMarketTradeHistory(MarketIdentity MarketIdent)
        {
            Stack<TradeRecord> hist = new Stack<TradeRecord>();

            return hist;
        }

        public override HashSet<ActiveOrder> GetActiveOrders()
        {
            HashSet<ActiveOrder> ords = new HashSet<ActiveOrder>();
            if (ApiActive && HasKeys)
            {

            }
            return ords;
        }

        public override HashSet<Balance> GetBalances()
        {
            HashSet<Balance> bals = new HashSet<Balance>();
            if(ApiActive && HasKeys)
            {

            }
            return bals;
        }

        public override Tuple<string, string> PlaceBasicOrder(MarketIdentity MarketIdent, OrderType Type, double Price, double Quantity)
        {
            return new Tuple<string, string>("","");
        }

        public override string CancelOrder(string OrderId, string MarketId)
        {
            string json = String.Empty;

            return json;
        }

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
                    wc.Dispose();
                }
            }
            catch (WebException) { }
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

                    var postData = String.Format("{0}/{1}/{2}?apikey={0}&nonce={1}", url, paramList["method"], paramList["command"], DateTime.Now.Ticks);

                    foreach (var p in paramList)
                    {
                        if(p.Key != "method" && p.Key != "command")
                        {
                            postData += "&" + p.Key + "=" + p.Value;
                        }
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
            }
            return json;
        }
        #endregion
    }
}
