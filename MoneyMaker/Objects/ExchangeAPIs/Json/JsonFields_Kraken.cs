using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Json.Kraken
{
    public class BaseResponse
    {
        public List<object> error { get; set; }
        public Dictionary<string,dynamic> result { get; set; }
    }
    /// <summary>
    /// <pair_name>         = pair name
    /// altname             = alternate pair name
    /// aclass_base         = asset class of base component
    /// base                = asset id of base component
    /// aclass_quote        = asset class of quote component
    /// quote               = asset id of quote component
    /// lot                 = volume lot size
    /// pair_decimals       = scaling decimal places for pair
    /// lot_decimals        = scaling decimal places for volume
    /// lot_multiplier      = amount to multiply lot volume by to get currency volume
    /// leverage            = array of leverage amounts available
    /// fees                = fee schedule array in [volume, percent fee] tuples
    /// fee_volume_currency = volume discount currency
    /// margin_call         = margin call level
    /// margin_stop         = stop-out/liquidation margin level
    /// </summary>
    public class AssetPairs : BaseResponse
    {
        [JsonProperty("altname")]
        public string Altname { get; set; }
        [JsonProperty("aclass_base")]
        public string aclass_base { get; set; }
        [JsonProperty("base")]
        public string Base { get; set; }
        [JsonProperty("aclass_quote")]
        public string aclass_quote { get; set; }
        [JsonProperty("quote")]
        public string quote { get; set; }
        [JsonProperty("lot")]
        public string lot { get; set; }
        [JsonProperty("pair_decimals")]
        public int pair_decimals { get; set; }
        [JsonProperty("lot_decimals")]
        public int lot_decimals { get; set; }
        [JsonProperty("lot_multiplier")]
        public int lot_multiplier { get; set; }
        [JsonProperty("leverage")]
        public List<object> leverage { get; set; }
        [JsonProperty("fees")]
        public List<double[]> fees { get; set; }
        //public List<List<double>> fees { get; set; }
        [JsonProperty("fee_volume_currency")]
        public string fee_volume_currency { get; set; }
        [JsonProperty("margin_call")]
        public int margin_call { get; set; }
        [JsonProperty("margin_stop")]
        public int margin_stop { get; set; }
    }
    /// <summary>
    /// <asset_name>        = asset name
    /// altname             = alternate name
    /// aclass              = asset class
    /// decimals            = scaling decimal places for record keeping
    /// display_decimals    = scaling decimal places for output display
    /// </summary>
    public class AssetInfo : BaseResponse
    {
        public string aclass { get; set; }
        public string altname { get; set; }
        public int decimals { get; set; }
        public int display_decimals { get; set; }
    }
    /// <summary>
    /// https://api.kraken.com/0/public/Ticker?pair=XLTCXXRP
    /// Input:
    /// pair = comma delimited list of asset pairs to get info on
    /// 
    /// Result: array of pair names and their ticker info
    /// <pair_name> = pair name
    /// a = ask array(<price>, <lot volume>),
    /// b = bid array(<price>, <lot volume>),
    /// c = last trade closed array(<price>, <lot volume>),
    /// v = volume array(<today>, <last 24 hours>),
    /// p = volume weighted average price array(<today>, <last 24 hours>),
    /// t = number of trades array(<today>, <last 24 hours>),
    /// l = low array(<today>, <last 24 hours>),
    /// h = high array(<today>, <last 24 hours>),
    /// o = today's opening price
    /// </summary>
    public class TickerInfo : BaseResponse
    {
        [JsonProperty("a")]
        public double[] AskArray { get; set; }

        [JsonProperty("b")]
        public double[] BidArray { get; set; }

        [JsonProperty("c")]
        public double[] LastClosedArray { get; set; }

        [JsonProperty("v")]
        public double[] VolumeArray { get; set; }

        [JsonProperty("p")]
        public double[] WeightedVoumeArray { get; set; }

        [JsonProperty("t")]
        public double[] NumberTradesArray { get; set; }

        [JsonProperty("l")]
        public double[] LowArray { get; set; }

        [JsonProperty("h")]
        public double[] HighArray { get; set; }

        [JsonProperty("o")]
        public double OpeningPriceToday { get; set; }
    }
    /// <summary>
    /// URL: https://api.kraken.com/0/public/Depth?pair=XLTCXXRP
    /// 
    /// Input:
    /// pair = asset pair to get market depth for
    /// count = maximum number of asks/bids (optional)
    /// 
    /// Result: array of pair name and market depth
    /// <pair_name> = pair name
    /// asks = ask side array of array entries(<price>, <volume>, <timestamp>)
    /// bids = bid side array of array entries(<price>, <volume>, <timestamp>)
    /// </summary>
    public class OrderBookInfo : BaseResponse
    {
        public List<string[]> asks { get; set; }
        public List<string[]> bids { get; set; }
    }
    /// <summary>
    /// Get recent trades
    /// URL: https://api.kraken.com/0/public/Trades?pair=XLTCXXRP
    /// 
    /// Input:
    /// pair = asset pair to get trade data for
    /// since = return trade data since given id (optional.  exclusive)
    /// 
    /// Result: array of pair name and recent trade data
    /// <pair_name> = pair name
    /// array of array entries(<price>, <volume>, <time>, <buy/sell>, <market/limit>, <miscellaneous>)
    /// last = id to be used as 'since' variable when polling for new trade data
    /// </summary>
    public class GetRecentTrades
    {
        public List<object> error { get; set; }
        public Dictionary<string, Tuple<double, double, double, string, string, string>> result { get; set; }
        public string last { get; set; }
    }
    /// <summary>
    /// Array of asset names and balance amount
    /// </summary>
    public class TradeBalances
    {
        /// <summary>
        /// trade balance (combined balance of all currencies)
        /// </summary>
        public double tb {get; set;}
        /// <summary>
        /// initial margin amount of open positions
        /// </summary>
        public string m {get; set;} 
        /// <summary>
        /// unrealized net profit/loss of open positions
        /// </summary>
        public string n {get; set;}
        /// <summary>
        /// cost basis of open positions
        /// </summary>
        public string c {get;set;}
        /// <summary>
        /// current floating valuation of open positions
        /// </summary>
        public string v {get; set;}
        /// <summary>
        /// equity = trade balance + unrealized net profit/loss
        /// </summary>
        public string e {get;set;}
        /// <summary>
        /// free margin = equity - initial margin (maximum margin available to open new positions)
        /// </summary>
        public string mf {get;set;}
        /// <summary>
        /// margin level = (equity / initial margin) * 100
        /// </summary>
        public string ml {get;set;}
    }
    /// <summary>
    /// Array of order info in open array with txid as the key
    /// </summary>
    public class OpenOrderInfo
    {
        public object refid { get; set; }
        public object userref { get; set; }
        public string status { get; set; }
        public double opentm { get; set; }
        public double starttm { get; set; }
        public double expiretm { get; set; }
        public Descr descr { get; set; }
        public double vol { get; set; }
        public double vol_exec { get; set; }
        public double cost { get; set; }
        public double fee { get; set; }
        public double price { get; set; }
        public string misc { get; set; }
        public string oflags { get; set; }

        
    }
    public class Descr
    {
        public string pair { get; set; }
        public string type { get; set; }
        public string ordertype { get; set; }
        public double price { get; set; }
        public double price2 { get; set; }
        public string leverage { get; set; }
        public string order { get; set; }
    }

    public class MarketTradeHistory
    {
        public List<object> error { get; set; }
        public Tuple<String, string[], string> result { get; set; }
    }
}
