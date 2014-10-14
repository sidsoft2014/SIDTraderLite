using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Json.Bittrex
{
    /// <summary>
    /// Used to get the open and available trading markets at Bittrex along with other meta data.
    /// </summary>
    public class Json_GetMarkets
    {
        public string MarketCurrency { get; set; }
        public string BaseCurrency { get; set; }
        public string MarketCurrencyLong { get; set; }
        public string BaseCurrencyLong { get; set; }
        public string MinTradeSize { get; set; }
        public string MarketName { get; set; }
        public bool IsActive { get; set; }
        public string Created { get; set; }
    }
    /// <summary>
    /// Used to get the last 24 hour summary of all active exchanges
    /// </summary>
    public class Json_GetMarketSummaries
    {
        public string MarketName { get; set; }
        public string High { get; set; }
        public string Low { get; set; }
        public string Volume { get; set; }
        public string Last { get; set; }
        public string BaseVolume { get; set; }
        public string TimeStamp { get; set; }
        public string Bid { get; set; }
        public string Ask { get; set; }
        public string OpenBuyOrders { get; set; }
        public string OpenSellOrders { get; set; }
        public string PrevDay { get; set; }
        public string Created { get; set; }
        public string DisplayMarketName { get; set; }
    }
    /// <summary>
    /// Used to get retrieve the orderbook for a given market
    /// Parameters
    /// parameter	required	description
    /// market	    required	a string literal for the market (ex: BTC-LTC)
    /// type	    required	buy, sell or both to identify the type of orderbook to return.
    /// depth	    optional	defaults to 20 - how deep of an order book to retrieve. Max is 50
    /// </summary>
    public class Json_GetOrderBook
    {
        public Json_OrderBookEntry[] buy { get; set; }
        public Json_OrderBookEntry[] sell { get; set; }
    }
    /// <summary>
    /// OrderBook Entry Json
    /// </summary>
    public class Json_OrderBookEntry
    {
        public string Quantity { get; set; }
        public string Rate { get; set; }
    }
    /// <summary>
    /// Used to retrieve the latest trades that have occured for a specific market.
    /// Parameters
    /// parameter	required	description
    /// market	    required	a string literal for the market (ex: BTC-LTC)
    /// count	    optional	a number between 1-50 for the number of entries to return (default = 20)
    /// </summary>
    public class Json_GetMarketHistory
    {
        public string Id { get; set; }
        [JsonProperty("TimeStamp")]
        public string TimeStampString { get; set; }
        public DateTime Time
        {
            get
            {
                if (!string.IsNullOrEmpty(TimeStampString))
                {
                    DateTime dt;
                    string val = TimeStampString.Replace('T', ' ');
                    DateTime.TryParse(val, out dt);
                    return dt;
                }
                else return DateTime.Now;
            }
        }
        public string Quantity { get; set; }
        public string Price { get; set; }
        public string Total { get; set; }
        public string FillType { get; set; }
        public string OrderType { get; set; }
    }
    /// <summary>
    /// /market/buylimit
    /// /market/buymarket
    /// Used to place a buy order in a specific market.
    /// Use limit to place limit orders and market to place market orders.
    /// Make sure you have the proper permissions set on your API keys for this call to work
    /// Parameters
    /// parameter	required	description
    /// market	    required	a string literal for the market (ex: BTC-LTC)
    /// quantity	required	the amount to purchase
    /// rate	    required	the rate at which to place the order. this is not needed for market orders
    /// Request:
    /// https://bittrex.com/api/v1.1/market/buylimit?apikey=API_KEY&market=BTC-LTC&quantity=1.2&rate=1.3    
    /// Request:
    /// https://bittrex.com/api/v1.1/market/buymarket?apikey=API_KEY&market=BTC-LTC&quantity=1.2
    /// 
    /// Request:
    /// https://bittrex.com/api/v1.1/market/selllimit?apikey=API_KEY&market=BTC-LTC&quantity=1.2&rate=1.3    
    /// Request:
    /// https://bittrex.com/api/v1.1/market/sellmarket?apikey=API_KEY&market=BTC-LTC&quantity=1.2    
    /// </summary>
    public class Json_PlaceOrderResponse
    {
        [JsonProperty("uuid")]
        public string orderId { get; set; }
    }
    /// <summary>
    /// /market/cancel
    /// Used to cancel a buy or sell order.
    /// Parameters
    /// parameter	required	description
    /// uuid	    required	uuid of buy or sell order
    /// Request:
    /// https://bittrex.com/api/v1.1/market/cancel?apikey=API_KEY&uuid=ORDER_UUID
    /// </summary>
    public class Json_CancelOrderResponse
    {
        public bool success { get; set; }
        public string message { get; set; }
        public object result { get; set; }
    }
    /// <summary>
    /// /market/getopenorders
    /// Get all orders that you currently have opened. A specific market can be requested
    /// Parameters
    /// parameter	required	description
    /// market	optional	a string literal for the market (ie. BTC-LTC)
    /// Request:
    /// https://bittrex.com/api/v1.1/market/getopenorders?apikey=API_KEY&market=BTC-LTC    
    /// </summary>
    public class Json_GetOpenOrders
    {
        public string Uuid { get; set; }
        [JsonProperty("OrderUuid")]
        public string OrderId { get; set; }
        public string Exchange { get; set; }
        public string OrderType { get; set; }
        public string Quantity { get; set; }
        public string QuantityRemaining { get; set; }
        public string Limit { get; set; }
        public string CommissionPaid { get; set; }
        public string Price { get; set; }
        public string PricePerUnit { get; set; }
        public string Opened { get; set; }
        public string Closed { get; set; }
        public string CancelInitiated { get; set; }
        public string ImmediateOrCancel { get; set; }
        public string IsConditional { get; set; }
        public string Condition { get; set; }
        public string ConditionTarget { get; set; }
    }
    /// <summary>
    /// /account/getbalances
    /// Used to retrieve all balances from your account
    /// Parameters
    /// None
    /// Request:
    /// https://bittrex.com/api/v1.1/account/getbalances?apikey=API_KEY    
    /// </summary>
    public class Json_GetBalances
    {
        public string Currency { get; set; }
        public string Balance { get; set; }
        public string Available { get; set; }
        public string Pending { get; set; }
        public string CryptoAddress { get; set; }
        public string Requested { get; set; }
        public string Uuid { get; set; }
    }
    /// <summary>
    /// /account/getdepositaddress
    /// Used to retrieve or generate an address for a specific currency.
    /// If one does not exist, the call will fail and return ADDRESS_GENERATING until one is available.
    /// Parameters
    /// parameter	required	description
    /// currency	required	a string literal for the currency (ie. BTC)
    /// Request:
    /// https://bittrex.com/api/v1.1/account/getdepositaddress?apikey=API_KEY&currency=VTC
    /// </summary>
    public class Json_GetDepositAddress
    {
        public string Currency { get; set; }
        public string Address { get; set; }
    }
    /// <summary>
    /// /account/withdraw
    /// Used to withdraw funds from your account. note: please account for txfee.
    /// Parameters
    /// parameter	required	description
    /// currency	required	a string literal for the currency (ie. BTC)
    /// quantity	required	the quantity of coins to withdraw
    /// address	required	the address where to send the funds.
    /// paymentid	optional	used for CryptoNotes/BitShareX/Nxt optional field (memo/paymentid)
    /// Request:
    /// https://bittrex.com/api/v1.1/account/withdraw?apikey=API_KEY&currency=EAC&quantity=20.40&address=EAC_ADDRESS
    /// </summary>
    public class Json_WithdrawlResponse
    {
        public string uuid { get; set; }
    }
    /// <summary>
    /// /account/getorderhistory
    /// Used to retrieve your order history.
    /// Parameters
    /// parameter	required	description
    /// market	    optional	a string literal for the market (ie. BTC-LTC). If ommited, will return for all markets
    /// count	    optional	the number of records to return
    /// Request:
    /// https://bittrex.com/api/v1.1/account/getorderhistory&count=2
    /// </summary>
    public class Json_GetOrderHistory
    {
        public string OrderUuid { get; set; }
        public string Exchange { get; set; }
        public string TimeStamp { get; set; }
        public string OrderType { get; set; }
        public string Limit { get; set; }
        public string Quantity { get; set; }
        public string QuantityRemaining { get; set; }
        public string Commission { get; set; }
        public string Price { get; set; }
        public string PricePerUnit { get; set; }
        public string IsConditional { get; set; }
        public string Condition { get; set; }
        public string ConditionTarget { get; set; }
        public string ImmediateOrCancel { get; set; }
    }
    /// <summary>
    /// /account/getwithdrawalhistory
    /// Used to retrieve your withdrawal history.
    /// Parameters
    /// parameter	required	description
    /// currency	optional	a string literal for the currecy (ie. BTC). If omitted, will return for all currencies
    /// count	optional	the number of records to return
    /// Request:
    /// https://bittrex.com/api/v1.1/account/getwithdrawalhistory?currency=BTC&count=2
    /// 
    /// /account/getdeposithistory
    /// Used to retrieve your deposit history.
    /// Parameters
    /// parameter	required	description
    /// currency	optional	a string literal for the currecy (ie. BTC). If omitted, will return for all currencies
    /// count	optional	the number of records to return
    /// Request:
    /// https://bittrex.com/api/v1.1/account/getwithdrawalhistory?currency=BTC&count=1
    /// </summary>
    public class Json_GetDepositOrWithdrawlHistory
    {
        public string PaymentUuid { get; set; }
        public string Currency { get; set; }
        public string Amount { get; set; }
        public string Address { get; set; }
        public string Opened { get; set; }
        public string Authorized { get; set; }
        public string PendingPayment { get; set; }
        public string TxCost { get; set; }
        public string TxId { get; set; }
        public string Canceled { get; set; }
        public string InvalidAddress { get; set; }
    }
}
