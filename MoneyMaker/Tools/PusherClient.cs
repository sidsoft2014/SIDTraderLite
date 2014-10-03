using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Pusher;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Data;
using System.Threading.Tasks;
using Json.Cryptsy;
using Objects;

namespace Toolbox
{
    public class PusherEventArgs : EventArgs
    {
        public string pusherError { get; set; }
        public Dictionary<string, string> pusherData { get; set; }
    }

    public class PushTickerHitClient
    {
        Dictionary<string, string> idToMarket { get; set; }
        Dictionary<string, string> marketToId { get; set; }
        Dictionary<string, Channel> Channels_ticker { get; set; }
        Dictionary<string, Channel> Channels_trade { get; set; }
        
        public PushTickerHitClient()
        {
            idToMarket = new Dictionary<string, string>();
            marketToId = new Dictionary<string, string>();
            Channels_ticker = new Dictionary<string, Channel>();
            Channels_trade = new Dictionary<string, Channel>();
        }
        public void StartPusher(Dictionary<string, string> marketToId)
        {
            this.marketToId = marketToId;
            idToMarket = new Dictionary<string, string>();

            var pusher = new Pusher.Pusher(new Pusher.Connections.Net.WebSocketConnectionFactory(), "cb65d0a7a72cd94adf1f", new Options
                {
                    Scheme = WebServiceScheme.Secure
                });
            Task t = Task.Run(async()=>await pusher.ConnectAsync());
            Task.WaitAll(t);

            foreach (var item in marketToId)
            {
                idToMarket.Add(item.Value, item.Key);
                var channel = pusher.SubscribeToChannelAsync("ticker." + item.Value).Result;
                channel.EventEmitted += (sender, e) =>
                    {
                        PusherDecodeTicker(e.Data);
                    };
                var channelTrade = pusher.SubscribeToChannelAsync("trade." + item.Value).Result;
                channelTrade.EventEmitted += (sender, e) =>
                    {
                        PusherDecodeTrade(e.Data);
                    };
            }
        }
        string lastDt;
        private void PusherDecodeTrade(string json)
        {
            var trade = JsonConvert.DeserializeObject<PusherTrade.PusherTradeRoot>(json);
            
            if (trade.channel != null && trade.trade != null)
            {
                string dt = trade.trade.datetime.Remove(19);
                
                if (lastDt == null || lastDt != dt)
                {
                    lastDt = dt;
                    
                    string mktName = idToMarket[trade.trade.marketid.ToString()];
                    string id = trade.trade.marketid.ToString();

                    double tradePrice = 0;
                    if (trade.trade.price != null)
                        tradePrice = (double)trade.trade.price;

                    double tradeQuant = 0;
                    if (trade.trade.quantity != null)
                        tradeQuant = (double)trade.trade.quantity;

                    if (tradePrice * tradeQuant > 0 && Event_PusherDataIn != null)
                    {
                        PusherEventArgs pea = new PusherEventArgs();
                        pea.pusherData = new Dictionary<string, string>();
                        pea.pusherData.Add("Market", mktName);
                        pea.pusherData.Add("Type", "Trade");
                        pea.pusherData.Add("LastTradePrice", tradePrice.ToString("N8"));
                        pea.pusherData.Add("Quantity", tradeQuant.ToString("N8"));
                        pea.pusherData.Add("Time", dt);
                        Event_PusherDataIn(this, pea);
                    }
                }
            }
        }
        private void PusherDecodeTicker(string json)
        {
            var ticker = JsonConvert.DeserializeObject<PusherTicker.TickerRoot>(json);
            if (ticker.channel != null && ticker.trade != null)
            {
                string mktName = idToMarket[ticker.trade.marketid.ToString()];

                double ask = 0;
                double askVol = 0;
                if (ticker.trade.topsell.price != null)
                {
                    ask = (double)ticker.trade.topsell.price;
                    askVol = (double)ticker.trade.topsell.quantity;
                }

                double bid = 0;
                double bidVol = 0;
                if (ticker.trade.topbuy.price != null)
                {
                    bid = (double)ticker.trade.topbuy.price;
                    bidVol = (double)ticker.trade.topbuy.quantity;
                }

                if(Event_PusherDataIn != null)
                {
                    PusherEventArgs pea = new PusherEventArgs();
                    pea.pusherData = new Dictionary<string, string>();
                    pea.pusherData.Add("Market", mktName);
                    pea.pusherData.Add("Type", "Ticker");
                    pea.pusherData.Add("AskPrice", ask.ToString("N8"));
                    pea.pusherData.Add("AskQuant", askVol.ToString("N8"));
                    pea.pusherData.Add("BidPrice", bid.ToString("N8"));
                    pea.pusherData.Add("BidQuant", bidVol.ToString("N8"));
                    Event_PusherDataIn(this, pea);
                }
            }
        }

        public event EventHandler<PusherEventArgs> Event_PusherDataIn;
    }
}
