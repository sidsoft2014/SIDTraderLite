using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Json.MintPal
{
    public class JsonFields_Mint
    {
        /// <summary>
        /// Market Summary
        /// GET https://api.mintpal.com/v1/market/summary/
        /// Provides an overview of all our markets. Data refreshes every minute.
        /// GET https://api.mintpal.com/v1/market/summary/{EXCHANGE}
        /// Provides an overview of only BTC or LTC markets. Data refreshes every minute.
        /// Example: https://api.mintpal.com/v1/market/summary/BTC
        /// </summary>
        public class JsonFields_Mint_MarketSummary
        {
            [JsonProperty("market_id")]
            public string Marketid { get; set; }

            [JsonProperty("code")]
            public string Primarycode { get; set; }

            [JsonProperty("exchange")]
            public string Secondarycode { get; set; }

            [JsonProperty("last_price")]
            public double? LastPrice { get; set; }

            [JsonProperty("yesterday_price")]
            public double? YesterdayPrice { get; set; }

            [JsonProperty("change")]
            public string Change { get; set; }

            [JsonProperty("24hhigh")]
            public double? High { get; set; }

            [JsonProperty("24hlow")]
            public double? Low { get; set; }

            [JsonProperty("24hvol")]
            public string Volume { get; set; }

            [JsonProperty("top_bid")]
            public double? TopBid { get; set; }

            [JsonProperty("top_ask")]
            public double? TopAsk { get; set; }

            public string Time
            {
                get
                {
                    return DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString();
                }
            }
        }
        /// <summary>
        /// Fetches the 50 best priced orders of a given type for a given market.
        /// Example: https://api.mintpal.com/v1/market/orders/MINT/BTC/BUY
        /// </summary>
        public class JsonFields_Mint_MarketOrders
        {
            [JsonProperty("count")]
            public string Count { get; set; }

            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("orders")]
            public List<JsonFields_Mint_Orders> Orders { get; set; }

            public double TopBid
            {
                get
                {
                    if (Type == "BUY")
                    {
                        int i = 0;
                        while (Orders.ElementAt(i).Price > Orders.ElementAt(0).Price && Orders.ElementAt(i).Price > Orders.ElementAt(1).Price)
                        {
                            i++;
                        }
                        return Orders.ElementAt(i).Price;
                    }

                    return 0;
                }
            }

            public double TopAsk
            {
                get
                {
                    if (Type == "SELL")
                    {
                        int i = 0;
                        while (Orders.ElementAt(i).Price > Orders.ElementAt(0).Price && Orders.ElementAt(i).Price > Orders.ElementAt(1).Price)
                        {
                            i++;
                        }
                        return Orders.ElementAt(i).Price;
                    }

                    return 0;
                }
            }

            public double BidVol
            {
                get
                {
                    if (Type == "BUY")
                    {
                        if (Orders.Any())
                            return Orders.FirstOrDefault().Quantity;
                    }

                    return 0;
                }
            }

            public double AskVol
            {
                get
                {
                    if (Type == "SELL")
                    {
                        if (Orders.Any())
                            return Orders.FirstOrDefault().Quantity;
                    }

                    return 0;
                }
            }

            public class JsonFields_Mint_Orders
            {
                [JsonProperty("price")]
                public double Price { get; set; }

                [JsonProperty("amount")]
                public double Quantity { get; set; }

                [JsonProperty("total")]
                public double Total { get; set; }
            }
        }       
        /// <summary>
        /// Market Trades
        /// GET https://api.mintpal.com/v1/market/trades/{COIN}/{EXCHANGE
        /// Fetches the last 100 trades for a given market.
        /// Example: https://api.mintpal.com/v1/market/trades/MINT/BTC
        /// NOTE: Type 0 refers to a BUY and type 1 refers to a SELL.
        /// Time is specified as a unix timestamp with microseconds.
        /// </summary>
        public class JsonFields_Mint_MarketTrades
        {
            [JsonProperty("count")]
            public int Count { get; set; }
            
            [JsonProperty("trades")]
            public MintTrades Trades { get; set; }

            public partial class MintTrades
            {
                [JsonProperty("type")]
                public string Type
                {
                    get
                    {
                        if (Type == "0")
                            return "Buy";
                        else
                            return "Sell";
                    }
                    
                    
                }

                [JsonProperty("price")]
                public double Price { get; set; }

                [JsonProperty("amount")]
                public double Qunatity { get; set; }

                [JsonProperty("total")]
                public double Total { get; set; }

                [JsonProperty("time")]
                public string Time { get; set; }
            }
      

        }
    }
}

