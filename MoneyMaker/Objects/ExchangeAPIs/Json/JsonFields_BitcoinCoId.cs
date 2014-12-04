using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Json.BitcoinCoId
{

    public class BaseResponse
    {
        [JsonProperty("Success")]
        public bool success { get; set; }
    }
    /// <summary>
    /// This method provides all the information about currently active pairs,
    /// such as the maximum number of digits after the decimal point in the auction,
    /// the minimum price, maximum price, minimum quantity purchase / sale,
    /// whether hidden pair and a pair of commission.
    /// 
    /// decimal_places :    number of decimal places allowed when bidding.
    /// min_price :         minimum price allowed by the tender.
    /// max_price :         the maximum price allowed by the tender.
    /// min_amount :        minimum quantity allowed for purchase / sale.
    /// Hidden :            hidden whether the pair, a value of 0 or 1.
    /// Fee :               Commission couples.
    /// </summary>
    public class GetExcahngeInfo
    {
        public Dictionary<string, JsonFields_BTCe_PairInfo> pairs { get; set; }
        public int server_time { get; set; }
    }
    public class JsonFields_BTCe_PairInfo
    {
        public int decimal_places { get; set; }
        public double min_price { get; set; }
        public int max_price { get; set; }
        public double min_amount { get; set; }
        public int hidden { get; set; }
        public double fee { get; set; }
    }
    /// <summary>
    /// It returns the information about the user's current balance,
    /// API key privileges, the number of transactions,
    /// the number of open orders and the server time.
    /// Parameters:
    /// None
    /// </summary>
    public class JsonFields_BTCe_GetInfoRoot : BaseResponse
    {
        [JsonProperty("return")]
        public GetInfo balanceInfo { get; set; }
    }
    public class GetInfo
    {
        public Dictionary<string, double> balance { get; set; }
        public int server_time { get; set; }
    }
    /// <summary>
    /// TransHistory
    /// It returns the transactions history.
    /// Parameters:
    /// parameter	oblig?	description	it takes on the values	standard value
    /// from	No	The ID of the transaction to start displaying with	numerical	0
    /// count	No	The number of transactions for displaying	numerical	1,000
    /// from_id	No	The ID of the transaction to start displaying with	numerical	0
    /// end_id	No	The ID of the transaction to finish displaying with	numerical	∞
    /// order	No	sorting	ASC or DESC	DESC
    /// since	No	When to start displaying?	UNIX time	0
    /// end	No	When to finish displaying?	UNIX time	∞
    /// Note: while using since or end parameters, the order parameter automatically take up ASC value.
    /// </summary>
    public class TransHistory
    {
        public int type { get; set; }
        public double amount { get; set; }
        public string currency { get; set; }
        public string desc { get; set; }
        public int status { get; set; }
        public int timestamp { get; set; }
    }
    /// <summary>
    /// TradeHistory
    /// It returns the trade history
    /// Parameters:
    /// parameter	oblig?	description	                                            It takes up the value	standard value
    /// from	    No	    the number of the transaction to start displaying with	numerical	            
    /// count	    No	    the number of transactions for displaying	            numerical	            1000
    /// from_id	    No	    the ID of the transaction to start displaying with	    numerical	            0
    /// end_id	    No	    the ID of the transaction to finish displaying with	    numerical	            ∞
    /// order	    No	    sorting	ASC or DESC	DESC
    /// since	    No	    when to start the displaying	                        UNIX time	            0
    /// end	        No	    when to finish the displaying	                        UNIX time	            ∞
    /// pair	    No	    the pair to show the transactions	                    btc_usd (example)	    all pairs
    /// Note: while using since or end parameters, order parameter automatically takes up ASC value.
    /// </summary>
    public class TradeHistoryRoot : BaseResponse
    {
        [JsonProperty("return")]
        public Dictionary<string, TradeHistory> TradeHistory { get; set; }
    }
    public class TradeHistory
    {
        public string pair { get; set; }
        public string type { get; set; }
        public double amount { get; set; }
        public double rate { get; set; }
        public int order_id { get; set; }
        public bool is_your_order { get; set; }
        public int timestamp { get; set; }
    }
    /// <summary>
    /// ActiveOrders
    /// Returns your open orders.
    /// Parameters:
    /// parameter	oblig?	description	                    it takes up values	standard value
    /// pair	    No	    the pair to display the orders	btc_usd (example)	all pairs
    /// </summary>
    public class ActiveOrdersRoot
    {
        [JsonProperty("success")]
        public int Success { get; set; }

        [JsonProperty("return")]
        public OrderList OpenOrders { get; set; }
    }
    public class OrderList
    {
        public List<Dictionary<string, string>> orders { get; set; }
    }
    /// <summary>
    /// Trade
    /// Trading is done according to this method.
    /// Parameters:
    /// parameter	oblig?	description	                                it takes up the values	standard value
    /// pair	    Yes	    pair	                                    btc_usd (example)	    -
    /// type	    Yes	    The transaction type	                    buy or sell	            -
    /// rate	    Yes	    The rate to buy/sell	                    numerical	            -
    /// amount	    Yes	    The amount which is necessary to buy/sell	numerical	            -
    /// </summary>
    public class TradeResponse
    {
        public int order_id { get; set; }
        public Dictionary<string, double> balance { get; set; }
    }
    /// <summary>
    /// BTCe Ticker JSON
    /// </summary>
    public class Ticker
    {
        public double high { get; set; }
        public double low { get; set; }
        public double avg { get; set; }
        public double last { get; set; }
        public double buy { get; set; }
        public double sell { get; set; }
        public int server_time { get; set; }
    }
    /// <summary>
    /// List of last 150 trades in a given market
    /// max trades returned is 2000
    /// </summary>
    public class MarketTrades
    {
        public string type { get; set; }
        public double price { get; set; }
        public double amount { get; set; }
        public int tid { get; set; }
        public int date { get; set; }
    }
    /// <summary>
    /// This method provides information on active warrants pair.
    /// Additionally takes an optional parameter GET- limit,
    /// which indicates how many orders you want to display (default 150).
    /// Takes a value less than 2000.
    /// </summary>
    public class Depth
    {
        public List<double[]> sell { get; set; }
        public List<double[]> buy { get; set; }
    }

}
