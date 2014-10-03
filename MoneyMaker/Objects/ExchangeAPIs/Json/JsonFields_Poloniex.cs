using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Json.Poloniex
{
    /// <summary>
    /// Returns the ticker for all markets.
    /// </summary>
    public class PoloniexMarketTickers
    {
        [JsonProperty("last")]
        public double LastTradePrice { get; set; }

        [JsonProperty("lowestAsk")]
        public double? TopAsk { get; set; }

        [JsonProperty("highestBid")]
        public double? TopBid { get; set; }

        [JsonProperty("percentChange")]
        public double? PercentChange { get; set; }

        [JsonProperty("baseVolume")]
        public double? BaseVolume { get; set; }

        [JsonProperty("quoteVolume")]
        public double? QuoteVolume { get; set; }

        [JsonProperty("isFrozen")]
        public int isFrozen { get; set; }
    }
    /// <summary>
    /// Returns the order book for a given market.
    /// </summary>
    public class PoloniexMarketOrderBook
    {
        [JsonProperty("asks")]
        public List<double[]> Asks { get; set; }

        [JsonProperty("bids")]
        public List<double[]> Bids { get; set; }
    }
    /// <summary>
    /// Returns the past 200 trades for a given market. 
    /// </summary>
    public class PoloniexTradeHistory
    {
        [JsonProperty("date")]
        public DateTime DateTime { get; set; }

        [JsonProperty("type")]
        public string TradeType { get; set; }

        [JsonProperty("rate")]
        public double Price { get; set; }

        [JsonProperty("amount")]
        public double Quantity { get; set; }

        [JsonProperty("total")]
        public double Total { get; set; }
    }
    /// <summary>
    /// Returns all of your balances
    /// </summary>
    public class PoloniexBalances
    {
        public double available { get; set; }
        public double onOrders { get; set; }
        public double btcValue { get; set; }
    }
    /// <summary>
    /// Returns your open orders for a given market,
    /// specified by the "currencyPair" POST parameter,
    /// e.g. "BTC_XCP". Set "currencyPair" to "all" to return open orders for all markets. 
    /// </summary>
    public class PoloniexOpenOrders
    {
        public string date { get; set; }
        public double rate { get; set; }
        public double amount { get; set; }
        public double total { get; set; }
        public string orderNumber { get; set; }
        public string type { get; set; }
    }
    /// <summary>
    /// Returns your trade history for a given market, specified by the "currencyPair" POST parameter.
    /// You may optionally specify a range via "start" and/or "end" POST parameters, given in UNIX timestamp format.
    /// </summary>
    public class PoloniexFilledOrders
    {
        public DateTime date { get; set; }
        public double rate { get; set; }
        public double amount { get; set; }
        public double total { get; set; }
        public string orderNumber { get; set; }
        public string type { get; set; }
    }
    /// <summary>
    /// Places a buy or sell order in a given market.
    /// Required POST parameters are 
    /// "currencyPair", 
    /// "rate", 
    /// "amount".
    /// If successful, the method will return the order number. 
    /// </summary>
    public class OrderResponse
    {
        public string orderNumber { get; set; }
    }
}
