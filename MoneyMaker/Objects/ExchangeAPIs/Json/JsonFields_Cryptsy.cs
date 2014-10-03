using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Json.Cryptsy
{
    public class BaseResponse
    {
        public int success { get; set; }
        public string error { get; set; }
        [JsonProperty("return")]
        public dynamic DataList { get; set; }
    }

    /// <summary>
    /// Outputs:            Array of last 1000 Trades for this Market, in Date Decending Order 
    /// tradeid	            A unique ID for the trade
    /// datetime	        Server datetime trade occurred
    /// tradeprice	        The price the trade occurred at
    /// quantity	        Quantity traded
    /// total	            Total value of trade (tradeprice * quantity)
    /// initiate_ordertype  The type of order which initiated this trade
    /// </summary>
    public class MarketTrades
    {
        public string tradeid { get; set; }
        public DateTime datetime { get; set; }
        public double? tradeprice { get; set; }
        public double? quantity { get; set; }
        public double? total { get; set; }
        public string initiate_ordertype { get; set; }
    }
    /// <summary>
    /// balances_available	Array of currencies and the balances availalbe for each
    /// balances_hold	    Array of currencies and the amounts currently on hold for open orders
    /// servertimestamp	    Current server timestamp
    /// servertimezone	    Current timezone for the server
    /// serverdatetime	    Current date/time on the server
    /// openordercount	    Count of open orders on your account
    /// </summary>
    public class BalanceInfo
    {
        public Dictionary<string, double> balances_available { get; set; }
        public Dictionary<string, double> balances_hold { get; set; }
        public string servertimestamp { get; set; }
        public string servertimezone { get; set; }
        public string serverdatetime { get; set; }
        public string openordercount { get; set; }
    }
    /// <summary>
    /// Method: allmyorders 
    /// Inputs: n/a 
    ///
    /// Outputs: Array of all open orders for your account. 
    ///
    /// orderid	        Order ID for this order
    /// marketid	    The Market ID this order was created for
    /// created	        Datetime the order was created
    /// ordertype	    Type of order (Buy/Sell)
    /// price	        The price per unit for this order
    /// quantity	    Quantity remaining for this order
    /// total	        Total value of order (price * quantity)
    /// orig_quantity	Original Total Order Quantity
    /// </summary>
    public class OpenOrders
    {
        public string orderid { get; set; }
        public string marketid { get; set; }
        public string created { get; set; }
        public string ordertype { get; set; }
        public double? price { get; set; }
        public double? quantity { get; set; }
        public double? total { get; set; }
        public double? orig_quantity { get; set; }
    }

    public class OrderResponse : BaseResponse
    {
        public string orderid { get; set; }
        public string moreinfo { get; set; }
    }
    public class BasicOrders
    {
        public string id { get; set; }
        public string time { get; set; }
        public double? price { get; set; }
        public double? quantity { get; set; }
    }
    public class BuyOrder
    {
        public double? buyprice { get; set; }
        public double? quantity { get; set; }
        public double? total { get; set; }
    }
    public class SellOrder
    {
        public double? sellprice { get; set; }
        public double? quantity { get; set; }
        public double? total { get; set; }
    }
    public class AnotherOrderFormat
    {
        public double? price { get; set; }
        public double? quantity { get; set; }
        public double? total { get; set; }
    }
    public class SingleMarketOrders
    {
        int ib = 0;
        int ia = 0;
        public string marketid { get; set; }
        public string label { get; set; }
        public string primaryname { get; set; }
        public string primarycode { get; set; }
        [JsonProperty("sellorders")]
        public List<AnotherOrderFormat> SellOrders { get; set; }
        [JsonProperty("buyorders")]
        public List<AnotherOrderFormat> BuyOrders { get; set; }
        public double? TopBid
        {
            get
            {
                if (BuyOrders.Count > 0 & SellOrders.Count > 0)
                {
                    try
                    {
                        int i = 0;
                        while (BuyOrders.ElementAt(i).price >= SellOrders.ElementAt(0).price && i < 3)
                        {
                            i++;
                        }

                        ib = i;
                        return BuyOrders.ElementAt(i).price;
                    }
                    catch (IndexOutOfRangeException)
                    {
                        return 0;
                    }
                }

                return 0;
            }
        }
        public double? BidVol
        {
            get
            {
                if (BuyOrders.Count > 0)
                {
                    try
                    {
                        return BuyOrders.ElementAt(ib).quantity;
                    }
                    catch (IndexOutOfRangeException)
                    {
                        return 0;
                    }
                }
                return 0;
            }
        }
        public double? BidTwo
        {
            get
            {
                if (BuyOrders.Count > 0 & SellOrders.Count > 0)
                {
                    try
                    {
                        return BuyOrders.ElementAt(ib + 1).price;
                    }
                    catch (IndexOutOfRangeException)
                    {
                        return 0;
                    }
                }

                return 0;
            }
        }
        public double? TopAsk
        {
            get
            {
                if (SellOrders.Count > 0 && BuyOrders.Count > 0)
                {
                    try
                    {
                        int i = 0;
                        while (SellOrders.ElementAt(i).price <= BuyOrders.ElementAt(0).price && i < 3)
                        {
                            i++;
                        }
                        ia = i;
                        return SellOrders.ElementAt(i).price;
                    }
                    catch (IndexOutOfRangeException)
                    {
                        return 0;
                    }
                }

                return 0;
            }
        }
        public double? AskVol
        {
            get
            {
                if (SellOrders.Count > 0)
                {
                    try
                    {
                        return SellOrders.ElementAt(ia).quantity;
                    }
                    catch (IndexOutOfRangeException)
                    {
                        return 0;
                    }
                }
                return 0;
            }
        }
        public double? AskTwo
        {
            get
            {
                if (SellOrders.Count > 0 && BuyOrders.Count > 0)
                {
                    try
                    {
                        return SellOrders.ElementAt(ia + 1).price;
                    }
                    catch (IndexOutOfRangeException)
                    {
                        return 0;
                    }
                }

                return 0;
            }
        }

    }
    public class FilledOrders
    {
        public string tradeid { get; set; }
        public string tradetype { get; set; }
        public string datetime { get; set; }
        public string marketid { get; set; }
        public double? tradeprice { get; set; }
        public double? quantity { get; set; }
        public double? fee { get; set; }
        public double? total { get; set; }
        public string initiate_ordertype { get; set; }
        public string order_id { get; set; }
    }


    /// <summary>
    /// Method: getmarkets
    /// Inputs: n/a
    /// Outputs: Array of Active Markets 
    /// 
    /// marketid	            stringeger value representing a market
    /// label	                Name for this market, for example: AMC/BTC
    /// primary_currency_code	Primary currency code, for example: AMC
    /// primary_currency_name	Primary currency name, for example: AmericanCoin
    /// secondary_currency_code	Secondary currency code, for example: BTC
    /// secondary_currency_name	Secondary currency name, for example: BitCoin
    /// current_volume	        24 hour trading quantity in this market
    /// last_trade	            Last trade price for this market
    /// high_trade	            24 hour highest trade price in this market
    /// low_trade	            24 hour lowest trade price in this market
    /// created	                Datetime (EST) the market was created
    /// </summary>
    public class PrivateAPIMarkets
    {
        [JsonProperty("marketid")]
        public string marketid { get; set; }
        [JsonProperty("label")]
        public string label { get; set; }
        [JsonProperty("primary_currency_code")]
        public string primarycode { get; set; }
        [JsonProperty("primary_currency_name")]
        public string primaryname { get; set; }
        [JsonProperty("secondary_currency_code")]
        public string secondarycode { get; set; }
        [JsonProperty("secondary_currency_name")]
        public string secondaryname { get; set; }
        [JsonProperty("current_volume")]
        public double? current_volume { get; set; }
        [JsonProperty("last_trade")]
        public double? lasttradeprice { get; set; }
        [JsonProperty("high_trade")]
        public double? high_trade { get; set; }
        [JsonProperty("low_trade")]
        public double? low_trade { get; set; }
        [JsonProperty("created")]
        public string created { get; set; }
    }
    
    public class MarketOverview
    {
        public class Details
        {
            public int ia = 0;
            public int ib = 0;

            public string marketid { get; set; }
            public string label { get; set; }
            public double? lasttradeprice { get; set; }
            public double? quantity { get; set; }
            public dynamic lasttradetime { get; set; }
            public string primaryname { get; set; }
            public string primarycode { get; set; }
            public string secondaryname { get; set; }
            public string secondarycode { get; set; }
            public List<Trades> recenttrades { get; set; }
            public List<Orders> sellorders { get; set; }
            public List<Orders> buyorders { get; set; }

            public double? TopBid
            {
                get
                {
                    if (buyorders != null && sellorders != null && buyorders.Count > 0)
                    {
                        try
                        {
                            int i = 0;
                            while (buyorders.ElementAt(i).price >= sellorders.ElementAt(0).price && i < 3)
                            {
                                i++;
                            }

                            ib = i;
                            return buyorders.ElementAt(i).price;
                        }
                        catch (IndexOutOfRangeException)
                        {
                            return 0;
                        }
                    }

                    return 0;
                }
            }
            public double? BidVol
            {
                get
                {
                    if (buyorders != null && sellorders != null && buyorders.Count > 0)
                    {
                        try
                        {
                            return buyorders.ElementAt(ib).quantity;
                        }
                        catch (IndexOutOfRangeException)
                        {
                            return 0;
                        }
                    }
                    return 0;
                }
            }
            public double? TopAsk
            {
                get
                {
                    if (sellorders != null && buyorders != null && sellorders.Count > 0)
                    {
                        try
                        {
                            int i = 0;
                            while (sellorders.ElementAt(i).price <= buyorders.ElementAt(0).price && i < 3)
                            {
                                i++;
                            }
                            ia = i;
                            return sellorders.ElementAt(i).price;
                        }
                        catch (IndexOutOfRangeException)
                        {
                            return 0;
                        }
                    }

                    return 0;
                }
            }
            public double? AskVol
            {
                get
                {
                    if (sellorders != null && buyorders != null && sellorders.Count > 0)
                    {
                        try
                        {
                            return sellorders.ElementAt(ia).quantity;
                        }
                        catch (IndexOutOfRangeException)
                        {
                            return 0;
                        }
                    }
                    return 0;
                }
            }

            public class Orders
            {
                public double? price { get; set; }
                public double? quantity { get; set; }
                public double? total { get; set; }
            }
            public class Trades
            {
                public string time { get; set; }
                public string type { get; set; }
                public string price { get; set; }
                public string quantity { get; set; }
                public string total { get; set; }
            }
        }
        
    }

    public class PusherTicker
    {
        public class Topsell
        {
            public double? price { get; set; }
            public double? quantity { get; set; }
        }

        public class Topbuy
        {
            public double? price { get; set; }
            public double? quantity { get; set; }
        }

        public class Ticker
        {
            public string timestamp { get; set; }
            public string datetime { get; set; }
            public string marketid { get; set; }
            public Topsell topsell { get; set; }
            public Topbuy topbuy { get; set; }
        }

        public class TickerRoot
        {
            public string channel { get; set; }
            public Ticker trade { get; set; }
        }
    }
    public class PusherTrade
    {
        public class PusherTradeRoot
        {
            public string channel { get; set; }
            public Trade trade { get; set; }
        }
        public class Trade
        {
            public string timestamp { get; set; }
            public string datetime { get; set; }
            public string marketid { get; set; }
            public string marketname { get; set; }
            public double? quantity { get; set; }
            public double? price { get; set; }
            public double? total { get; set; }
            public string type { get; set; }
        }
    }
}
