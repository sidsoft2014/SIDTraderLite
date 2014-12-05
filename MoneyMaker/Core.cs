using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Toolbox;
using System.Security;

namespace Objects
{
    public class CoreEvents : EventArgs
    {
        public Tuple<CoreMessageHeader, string> CoreMessage { get; set; }
        public Tuple<ExchangeEnum, string> ExchangeMessage { get; set; }
        public Tuple<ExchangeEnum, bool> ApiStateChange { get; set; }
        public Tuple<ExchangeEnum, TickerViewTableRow> NewTickerRow { get; set; }
        public Tuple<ExchangeEnum, HashSet<Balance>> NewBalances { get; set; }
        public Tuple<ExchangeEnum, HashSet<ActiveOrder>> NewActiveOrders { get; set; }
        public Tuple<ExchangeEnum, Ticker> NewRates { get; set; }
        public List<TradeObject> NewConditionalOrderList { get; set; }
        public Tuple<OrderBook, Stack<TradeRecord>> NewOrderBook { get; set; }
    }

    public static class Core
    {
        #region Fields
        private static User CurrentUser;
        private static ConcurrentDictionary<ExchangeEnum, Exchange> Exchanges;
        private static ConcurrentDictionary<ExchangeEnum, ConcurrentDictionary<string, Ticker>> Rates;
        private static ConcurrentDictionary<ExchangeEnum, List<TradeObject>> _conditionalTrades;
        private static ConcurrentDictionary<ExchangeEnum, List<TradeObject>> ConditionalTrades
        {
            get
            {
                if (_conditionalTrades == null)
                {
                    _conditionalTrades = new ConcurrentDictionary<ExchangeEnum, List<TradeObject>>();

                    foreach (ExchangeEnum item in (ExchangeEnum[])Enum.GetValues(typeof(ExchangeEnum)))
                    {
                        _conditionalTrades.TryAdd(item, new List<TradeObject>());
                    }
                }
                return _conditionalTrades;
            }
            set
            {
                _conditionalTrades = value;
            }
        }
        private static List<ExchangeEnum> ActiveExchanges;
        private static PushTickerHitClient PusherClient;

        private static ConcurrentDictionary<ExchangeEnum, string[]> lastUpdateTimes;
        private static System.Timers.Timer actMktTimer;

        private static API_Bittrex Bittrex;
        private static API_Cryptsy Cryptsy;
        private static API_BTCe BTCe;
        private static API_Poloniex Poloniex;
        private static API_Kraken Kraken;
        private static API_BitcoinCoId BitcoinCoId;

        private static Market ActiveMarket;

        public static bool running;
        public static bool exited;
        #endregion

        #region Constructor
        /// <summary>
        /// Needs adjusting everytime a new exchange is added
        /// </summary>
        public static void InitialiseCore()
        {
            Exchanges = new ConcurrentDictionary<ExchangeEnum, Exchange>();
            Rates = new ConcurrentDictionary<ExchangeEnum, ConcurrentDictionary<string, Ticker>>();
            ActiveExchanges = new List<ExchangeEnum>();

            string dt = DateTime.Now.ToString();
            lastUpdateTimes = new ConcurrentDictionary<ExchangeEnum, string[]>();
            lastUpdateTimes.TryAdd(ExchangeEnum.BitCoinCoId, new string[] { "120", dt });
            lastUpdateTimes.TryAdd(ExchangeEnum.Bittrex, new string[] { "120", dt });
            lastUpdateTimes.TryAdd(ExchangeEnum.BTCe, new string[] { "120", dt });
            lastUpdateTimes.TryAdd(ExchangeEnum.Cryptsy, new string[] { "600", dt });
            lastUpdateTimes.TryAdd(ExchangeEnum.Kraken, new string[] { "300", dt });
            lastUpdateTimes.TryAdd(ExchangeEnum.MintPal, new string[] { "150", dt });
            lastUpdateTimes.TryAdd(ExchangeEnum.Poloniex, new string[] { "300", dt });
            lastUpdateTimes.TryAdd(ExchangeEnum.Vircurex, new string[] { "300", dt });

        }
        public static void Start()
        {
            string msg = String.Empty;
            CoreMessageHeader header = CoreMessageHeader.ProgressMessage;

            if (CurrentUser == null)
                CurrentUser = new User();
            msg = "Loading Markets";
            NotifyMessageSubscribers(msg, header);

            GetMarketData();
            msg = "Loaded Markets";
            NotifyMessageSubscribers(msg, header);

            FindBaseMarkets();

            if (CurrentUser != null)
            {
                msg = "Checking user info";
                NotifyMessageSubscribers(msg, header);
                UpdateInfo();
            }

            if (Cryptsy != null)
            {
                PusherClient = new Toolbox.PushTickerHitClient();
                PusherHandler(PusherClient);
                PusherClient.StartPusher(Cryptsy.MarketIds);
            }

            msg = "Starting ticker client";
            NotifyMessageSubscribers(msg, header);
            UpdateTickers();

            running = true;
            delayTimer();
        }
        #endregion

        #region UI Bound Event Handles
        public static event EventHandler<CoreEvents> event_NewMessage;
        public static event EventHandler<CoreEvents> event_NewExchangeMessage;
        public static event EventHandler<CoreEvents> event_NewTickerRow;
        public static event EventHandler<CoreEvents> event_NewBalances;
        public static event EventHandler<CoreEvents> event_NewActiveOrders;
        public static event EventHandler<CoreEvents> event_NewOrderBook;
        public static event EventHandler<CoreEvents> event_RateChanged;
        public static event EventHandler<CoreEvents> event_ConditonalOrderListUpdated;
        public static event EventHandler<CoreEvents> event_AutoOrderPlaced;
        public static event EventHandler<CoreEvents> event_ApiStateChanged;
        #endregion

        #region Subscriber Notifiers
        private static void NotifyExchangeMessageSubscribers(ExchangeEnum exch, string message)
        {
            if (event_NewExchangeMessage != null)
            {
                CoreEvents ce = new CoreEvents();
                ce.ExchangeMessage = new Tuple<ExchangeEnum, string>(exch, message);
                event_NewExchangeMessage(null, ce);
            }
        }
        private static void NotifyMessageSubscribers(string msg, CoreMessageHeader header)
        {
            if (event_NewMessage != null)
            {
                CoreEvents ce = new CoreEvents();
                ce.CoreMessage = new Tuple<CoreMessageHeader, string>(header, msg);
                event_NewMessage(null, ce);
            }
        }
        private static void NotifyConditionalOrdersSubscribers()
        {
            if (event_ConditonalOrderListUpdated != null)
            {
                CoreEvents ce = new CoreEvents();
                ce.NewConditionalOrderList = new List<TradeObject>();
                foreach (var ex in ConditionalTrades)
                {
                    foreach (var ord in ex.Value)
                    {
                        ce.NewConditionalOrderList.Add(ord);
                    }
                }
                event_ConditonalOrderListUpdated(null, ce);
            }
        }
        private static void NotifyTickerSubscribers(Ticker input, TickerViewTableRow tRow)
        {
            if (event_NewTickerRow != null)
            {
                CoreEvents cE = new CoreEvents();
                cE.NewTickerRow = new Tuple<ExchangeEnum, TickerViewTableRow>(input.ExchangeName, tRow);
                event_NewTickerRow(null, cE);
            }
        }
        private static void NotifyOrderBookSubscribers(OrderBook ob, Stack<TradeRecord> tr)
        {
            if (event_NewOrderBook != null)
            {
                CoreEvents ce = new CoreEvents();
                ob.Market = null;
                ce.NewOrderBook = new Tuple<OrderBook, Stack<TradeRecord>>(ob, tr);
                event_NewOrderBook(null, ce);
            }
        }
        private static void NotifyRateSubscribers(Exchange exch, Market market)
        {
            if (event_RateChanged != null)
            {
                CoreEvents ce = new CoreEvents();
                ce.NewRates = new Tuple<ExchangeEnum, Ticker>(exch.Name, market.Ticker);
                event_RateChanged(null, ce);
            }
        }
        private static void NotifyBalanceSubscribers(ExchangeEnum exch, Exchange exRef)
        {
            if (event_NewBalances != null)
            {
                HashSet<Balance> balances = exRef.Balances as HashSet<Balance>;
                CoreEvents ce = new CoreEvents();
                ce.NewBalances = new Tuple<ExchangeEnum, HashSet<Balance>>(exch, balances);
                event_NewBalances(null, ce);
            }
        }
        private static void NotifyActiveOrderSubscribers(ExchangeEnum exch, HashSet<ActiveOrder> orders)
        {
            if (event_NewActiveOrders != null)
            {
                CoreEvents ce = new CoreEvents();
                ce.NewActiveOrders = new Tuple<ExchangeEnum, HashSet<ActiveOrder>>(exch, orders);
                event_NewActiveOrders(null, ce);
            }
        }
        #endregion

        #region UI Requests
        public static void SetUser(User user)
        {
            if (user != CurrentUser)
                CurrentUser = user;
        }
        public static string GetUserName()
        {
            if (CurrentUser != null)
                return CurrentUser.ToString();
            else return "Guest";
        }
        public static bool ChangeUpdatePeriod(ExchangeEnum exch, int period)
        {
            if (period > 0)
            {
                lastUpdateTimes[exch][0] = period.ToString();
                return true;
            }
            else return false;
        }
        public static async Task<Market> SetActiveMarket(ExchangeEnum exch, string market)
        {
            bool firstRun = false;
            if (ActiveMarket == null)
                firstRun = true;

            Exchange ex;
            IExchange iex = SwitchExchange(exch, out ex);

            var q = from mk in ex.Markets
                    where mk.MarketIdentity.StandardisedName == market
                    select mk;

            if (q.Count() == 0)
            {
                q = from mk in ex.Markets
                    where mk.MarketIdentity.MarketId == market
                    select mk;
                if (q.Count() == 0)
                {
                    throw new Exception();
                }
            }

            ActiveMarket = q.First();

            ActiveMarket.TradeRecords.Clear();
            var lst = await Task.Run(() => iex.GetSingleMarketTradeHistory(ActiveMarket.MarketIdentity).ToList().OrderBy(p => p.TradeTime));
            foreach (var l in lst)
            {
                ActiveMarket.TradeRecords.Push(l);
            }

            ActiveMarket.OrderBook = await Task.Run(() => iex.GetSingleMarketOrders(ActiveMarket.MarketIdentity));

            if (firstRun)
            {
                StartActiveMarketTimer();
            }

            return ActiveMarket;
        }

        private static void StartActiveMarketTimer()
        {
            if (actMktTimer == null)
            {
                actMktTimer = new System.Timers.Timer();
                actMktTimer.Interval = 10000;
                actMktTimer.AutoReset = false;
                actMktTimer.Elapsed += actMktTimer_Elapsed;
                actMktTimer.Start();
            }
        }

        static void actMktTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            actMktTimer = null;

            if (ActiveMarket != null)
            {
                Exchange ex;
                IExchange ordEx = SwitchExchange(ActiveMarket.ExchangeName, out ex);

                ActiveMarket.TradeRecords = ordEx.GetSingleMarketTradeHistory(ActiveMarket.MarketIdentity);
                ActiveMarket.OrderBook = ordEx.GetSingleMarketOrders(ActiveMarket.MarketIdentity);

                NotifyOrderBookSubscribers(ActiveMarket.OrderBook, ActiveMarket.TradeRecords);

                StartActiveMarketTimer();
            }
        }
        /// <summary>
        /// Simplified order placing method.
        /// Only for recieving requests from UI.
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="Price"></param>
        /// <param name="Quantity"></param>
        /// <returns></returns>
        public static string[] PlaceOrderAsync(OrderType Type, double Price, double Quantity)
        {
            ExchangeEnum exName = ActiveMarket.ExchangeName;
            string[] result = PlaceOrder(exName, ActiveMarket.MarketIdentity, Type, Price, Quantity);
            return result;
        }
        public static string CancelOrderAsync(ExchangeEnum exName, ActiveOrder orderObj)
        {
            string result = CancelOrder(exName, orderObj);
            return result;
        }

        public static bool AddConditionalOrder(ExchangeEnum exch, TradeObject tO)
        {
            bool success = false;

            if (!ConditionalTrades[exch].Contains(tO))
            {
                ConditionalTrades[exch].Add(tO);
                success = true;

                NotifyConditionalOrdersSubscribers();
            }

            return success;
        }
        public static bool RemoveConditionalOrder(TradeObject tO)
        {
            if (tO != null)
            {
                var t = ConditionalTrades[tO.Market.ExchangeName].Find(p => p.Market.MarketId == tO.Market.MarketId);
                bool success = ConditionalTrades[tO.Market.ExchangeName].Remove(t);

                NotifyConditionalOrdersSubscribers();
                return success;
            }
            else return false;
        }
        public static async Task ForceUpdate(ExchangeEnum exName, string Method)
        {
            await Task.Run(() =>
            {
                switch (Method)
                {
                    case "Balances":
                        UpdateBalances(exName);
                        break;
                    case "ActiveOrders":
                        UpdateActiveOrders(exName);
                        break;
                    case "Markets":
                        GetMarketData();
                        UpdateTickers();
                        break;
                    default:
                        UpdateInfo(exName);
                        GetMarketData(exName);
                        UpdateTickers(exName);
                        break;
                }
            });
        }
        public static async Task ForceUpdate()
        {
            await Task.Run(() =>
            {
                UpdateInfo();
                GetMarketData();
                UpdateTickers();
            });
            return;
        }
        #endregion

        #region Internal Update and Report Methods
        private static void GetMarketData()
        {
            foreach (ExchangeEnum exc in (ExchangeEnum[])Enum.GetValues(typeof(ExchangeEnum)))
            {
                GetMarketData(exc);
            }
            return;
        }
        private static object mLock = new object();
        private static void GetMarketData(ExchangeEnum exc)
        {
            lock (mLock)
            {
                Exchange exRef;
                IExchange iex = SwitchExchange(exc, out exRef);

                if (iex != null)
                {
                    string msg = string.Format("Downloading market data: {0}", exc);
                    var header = CoreMessageHeader.ProgressMessage;
                    NotifyMessageSubscribers(msg, header);

                    if (exRef.Markets == null || exRef.Markets.Count == 0)
                    {
                        exRef.Markets = iex.GetAllMarketData();
                    }
                    else
                    {
                        var tempMarks = iex.GetAllMarketData();

                        for (int ii = 0; ii < tempMarks.Count; ii++)
                        {
                            var market = tempMarks.ElementAt(ii);

                            var mq = from mk in exRef.Markets
                                     where mk.MarketIdentity.StandardisedName == market.MarketIdentity.StandardisedName
                                     select mk;

                            if (mq.Count() > 0)
                            {
                                var m = mq.ElementAt(0);
                                market.OrderBook.Market = m;
                                m.OrderBook = market.OrderBook;
                                m.TradeRecords = market.TradeRecords;
                            }
                            else
                            {
                                exRef.Markets.Add(market);
                                MessageBox.Show(exc.ToString().ToUpper() + "\nNEW MARKET ADDED!\n" + market.MarketIdentity.StandardisedName + "\n");
                            }
                        }
                        tempMarks = null;
                    }
                }
                lastUpdateTimes[exRef.Name][1] = DateTime.Now.ToString();
            }
            return;
        }
        private static void UpdateTickers()
        {
            ///Select only exchanges with markets
            var query = from ex in Exchanges
                        where ex.Value.Markets.Count > 0
                        select ex.Value.Name;

            foreach (var exch in query)
            {
                UpdateTickers(exch);
            }
        }
        private static void UpdateTickers(ExchangeEnum exc)
        {
            Exchange exch = Exchanges[exc];
            for (int ii = 0; ii < exch.Markets.Count; ii++)
            {
                var market = exch.Markets.ElementAt(ii);
                var tObj = market.Ticker;
                SendTickerRow(tObj);
                if (ActiveMarket != null)
                {
                    if (ActiveMarket.MarketIdentity.ExchangeName == market.MarketIdentity.ExchangeName
                        && ActiveMarket.MarketIdentity.StandardisedName == market.MarketIdentity.StandardisedName)
                    {
                        if (exc == ExchangeEnum.Poloniex)
                        {
                            market.TradeRecords.Clear();
                            Exchange ex;
                            var iex = SwitchExchange(market.MarketIdentity.ExchangeName, out ex);
                            var lst = Task.Run(() => iex.GetSingleMarketTradeHistory(market.MarketIdentity).ToList().OrderBy(p => p.TradeTime)).Result;
                            foreach (var l in lst)
                            {
                                market.TradeRecords.Push(l);
                            }
                        }
                        ActiveMarket = market;
                        NotifyOrderBookSubscribers(market.OrderBook, market.TradeRecords);
                    }
                }
                if (Rates.ContainsKey(exch.Name))
                {
                    if (Rates[exch.Name].ContainsKey(market.MarketIdentity.StandardisedName))
                    {
                        Rates[exch.Name][market.MarketIdentity.StandardisedName] = market.Ticker;
                        NotifyRateSubscribers(exch, market);
                    }
                }
            }
        }
        private static void FindBaseMarkets()
        {
            var tempList = new List<string>();


            foreach (var ex in Exchanges)
            {
                if (ex.Value.Markets.Count > 0)
                {
                    var tempGroups = new Dictionary<string, List<MarketIdentity>>();

                    if (!Rates.ContainsKey(ex.Key))
                        Rates.TryAdd(ex.Key, new ConcurrentDictionary<string, Ticker>());

                    foreach (var m in ex.Value.Markets)
                    {
                        if (!tempList.Contains(m.MarketIdentity.Currency))
                            tempList.Add(m.MarketIdentity.Currency);
                    }
                    foreach (var m in ex.Value.Markets)
                    {
                        if (tempList.Contains(m.MarketIdentity.Commodity))
                        {
                            if (!Rates[ex.Key].ContainsKey(m.MarketIdentity.StandardisedName))
                                Rates[ex.Key].TryAdd(m.MarketIdentity.StandardisedName, m.Ticker);
                        }
                    }
                }
            }
        }

        private static void UpdateInfo()
        {
            foreach (ExchangeEnum exc in (ExchangeEnum[])Enum.GetValues(typeof(ExchangeEnum)))
            {
                Exchange exRef;
                IExchange iex = SwitchExchange(exc, out exRef);
                if (iex != null)
                {
                    UpdateActiveOrders(exc);
                    UpdateBalances(exc);
                }
            }
        }
        private static void UpdateInfo(ExchangeEnum exch)
        {
            UpdateActiveOrders(exch);
            UpdateBalances(exch);
        }
        private static bool UpdateBalances(ExchangeEnum exch)
        {
            bool success = false;

            Exchange exRef;
            IExchange iex = SwitchExchange(exch, out exRef);
            if (iex != null)
            {
                var bals = iex.GetBalances();
                if (bals != null)
                {
                    exRef.Balances = bals;

                    NotifyBalanceSubscribers(exch, exRef);
                    success = true;
                }
            }
            return success;
        }
        private static bool UpdateActiveOrders(ExchangeEnum exch)
        {
            bool success = false;

            Exchange exRef;
            IExchange iex = SwitchExchange(exch, out exRef);
            if (iex != null)
            {
                var ords = iex.GetActiveOrders();
                if (ords != null)
                {
                    exRef.ActiveOrders = ords;
                    HashSet<ActiveOrder> orders = exRef.ActiveOrders as HashSet<ActiveOrder>;

                    NotifyActiveOrderSubscribers(exch, orders);
                    success = true;
                }
            }
            return success;
        }
        /// <summary>
        /// DOES THIS NEED TO CHANGE!
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static async void delayTimer()
        {
            while (running)
            {
                await Task.Delay(2000);
                for (int ii = 0; ii < lastUpdateTimes.Count; ii++)
                {
                    var item = lastUpdateTimes.ElementAt(ii);
                    if (ActiveExchanges.Contains(item.Key))
                    {
                        string gapString = string.Format("-{0}", item.Value[0]);
                        double gap = Convert.ToDouble(gapString);
                        var lu = DateTime.Parse(item.Value[1]);

                        var dt = DateTime.Now.AddSeconds(gap);

                        if (lu < dt)
                        {
                            GetMarketData(item.Key);
                            UpdateInfo(item.Key);
                            UpdateTickers(item.Key);
                        }
                    }
                }
            }
            exited = true;
        }

        public static void CloseCore()
        {
            if (actMktTimer != null)
                actMktTimer = null;
            if (running)
                running = false;
            if (!exited)
                exited = true;
            if (ActiveMarket != null)
                ActiveMarket = null;
            if (ActiveExchanges != null)
                ActiveExchanges = null;
            if (Exchanges != null)
                Exchanges = null;
        }
        #endregion

        #region Unit Event Handlers
        private static void PusherHandler(Toolbox.PushTickerHitClient pusher)
        {
            pusher.Event_PusherDataIn += pusher_Event_PusherDataIn;
        }
        internal static void pusher_Event_PusherDataIn(object sender, Toolbox.PusherEventArgs e)
        {
            var pd = e.pusherData;

            var mktQ = from d in Exchanges[ExchangeEnum.Cryptsy].Markets
                       where d.MarketIdentity.StandardisedName == pd["Market"].ToUpper()
                       select d;

            Market mkt = null;
            if (mktQ.Count() > 0)
            {
                mkt = mktQ.First();
            }

            if (mkt != null)
            {
                if (pd["Type"] == "Ticker")
                {
                    OrderBook tempOb = new OrderBook(mkt);
                    double aP = Convert.ToDouble(e.pusherData["AskPrice"]);
                    double aQ = Convert.ToDouble(e.pusherData["AskQuant"]);
                    double bP = Convert.ToDouble(e.pusherData["BidPrice"]);
                    double bQ = Convert.ToDouble(e.pusherData["BidQuant"]);
                    tempOb.AskOrders.Add(new Order(OrderType.Ask) { Price = aP, Quantity = aQ });
                    tempOb.BidOrders.Add(new Order(OrderType.Bid) { Price = bP, Quantity = bQ });

                    foreach (var ask in mkt.OrderBook.AskOrders)
                    {
                        if (ask.Price > aP)
                            tempOb.AskOrders.Add(ask);
                    }
                    foreach (var bid in mkt.OrderBook.BidOrders)
                    {
                        if (bid.Price < bP)
                            tempOb.BidOrders.Add(bid);
                    }
                    mkt.OrderBook = tempOb;

                    if (Rates[ExchangeEnum.Cryptsy].ContainsKey(mkt.MarketIdentity.StandardisedName))
                    {
                        Rates[ExchangeEnum.Cryptsy][mkt.MarketIdentity.StandardisedName] = mkt.Ticker;
                        NotifyRateSubscribers(Exchanges[ExchangeEnum.Cryptsy], mkt);
                    }
                }
                else if (pd["Type"] == "Trade")
                {
                    double price = Convert.ToDouble(pd["LastTradePrice"]);
                    double quant = Convert.ToDouble(pd["Quantity"]);
                    DateTime time = DateTime.Parse(pd["Time"]);

                    OrderType type = OrderType.Ask;
                    if (pd["Type"] == "Buy")
                        type = OrderType.Bid;
                    mkt.TradeRecords.Push(new TradeRecord { Price = price, Quantity = quant, TradeTime = time, Type = type });
                }

                SendTickerRow(mkt.Ticker);

                if (ActiveMarket != null)
                {
                    if (mkt.MarketIdentity.ExchangeName == ActiveMarket.MarketIdentity.ExchangeName
                        && mkt.MarketIdentity.StandardisedName == ActiveMarket.MarketIdentity.StandardisedName)
                    {
                        mkt.OrderBook = Cryptsy.GetSingleMarketOrders(ActiveMarket.MarketIdentity);
                        NotifyOrderBookSubscribers(mkt.OrderBook, mkt.TradeRecords);
                    }
                }
            }
        }
        #endregion


        /// <summary>
        /// NEEDS RE-ORGANISING
        /// </summary>
        /// <param name="exEnum"></param>
        /// <param name="exOut"></param>
        /// <returns></returns>
        #region Internal Methods
        ///The switch statement in this method needs updating for each new exchange addeed
        private static IExchange SwitchExchange(ExchangeEnum exEnum, out Exchange exOut)
        {
            exOut = new Exchange(exEnum);
            if (!Exchanges.ContainsKey(exEnum))
            {
                Exchanges.TryAdd(exEnum, exOut);
            }
            else
            {
                exOut = Exchanges[exEnum];
            }

            IExchange iex = null;
            switch (exEnum)
            {
                case ExchangeEnum.Bittrex:
                    {
                        if (Bittrex == null)
                        {
                            Bittrex = new API_Bittrex(exOut);
                            Bittrex.event_DeclaringApiState += iEx_event_DeclaringApiState;
                            Bittrex.event_ApiBytesDownloaded += iEx_event_ApiBytesDownloaded;

                            if (CurrentUser.EncryptedKeys.ContainsKey(ExchangeEnum.Bittrex))
                            {
                                var items = CurrentUser.EncryptedKeys[ExchangeEnum.Bittrex];
                                SecureString sspk = EncryptionTools.UserDecrypt(items[0], CurrentUser);
                                SecureString sssk = EncryptionTools.UserDecrypt(items[1], CurrentUser);
                                Bittrex.SetKeys(sspk, sssk);
                            }
                        }
                        iex = Bittrex;
                        break;
                    }
                case ExchangeEnum.BitCoinCoId:
                    {
                        if (BitcoinCoId == null)
                        {
                            BitcoinCoId = new API_BitcoinCoId(exOut);
                            BitcoinCoId.event_DeclaringApiState += iEx_event_DeclaringApiState;
                            BitcoinCoId.event_ApiBytesDownloaded += iEx_event_ApiBytesDownloaded;

                            if (CurrentUser.EncryptedKeys.ContainsKey(ExchangeEnum.BitCoinCoId))
                            {
                                var items = CurrentUser.EncryptedKeys[ExchangeEnum.BitCoinCoId];
                                SecureString sspk = EncryptionTools.UserDecrypt(items[0], CurrentUser);
                                SecureString sssk = EncryptionTools.UserDecrypt(items[1], CurrentUser);
                                BitcoinCoId.SetKeys(sspk, sssk);
                            }
                        }
                        iex = BitcoinCoId;
                        break;
                    }
                case ExchangeEnum.BTCe:
                    {
                        if (BTCe == null)
                        {
                            BTCe = new API_BTCe(exOut);
                            BTCe.event_DeclaringApiState += iEx_event_DeclaringApiState;
                            BTCe.event_ApiBytesDownloaded += iEx_event_ApiBytesDownloaded;

                            if (CurrentUser.EncryptedKeys.ContainsKey(ExchangeEnum.BTCe))
                            {
                                var items = CurrentUser.EncryptedKeys[ExchangeEnum.BTCe];
                                SecureString sspk = EncryptionTools.UserDecrypt(items[0], CurrentUser);
                                SecureString sssk = EncryptionTools.UserDecrypt(items[1], CurrentUser);
                                BTCe.SetKeys(sspk, sssk);
                            }
                        }
                        iex = BTCe;
                        break;
                    }
                case ExchangeEnum.Cryptsy:
                    {
                        if (Cryptsy == null)
                        {
                            Cryptsy = new API_Cryptsy(exOut);
                            Cryptsy.event_DeclaringApiState += iEx_event_DeclaringApiState;
                            Cryptsy.event_ApiBytesDownloaded += iEx_event_ApiBytesDownloaded;

                            if (CurrentUser.EncryptedKeys.ContainsKey(ExchangeEnum.Cryptsy))
                            {
                                var items = CurrentUser.EncryptedKeys[ExchangeEnum.Cryptsy];
                                SecureString sspk = EncryptionTools.UserDecrypt(items[0], CurrentUser);
                                SecureString sssk = EncryptionTools.UserDecrypt(items[1], CurrentUser);
                                Cryptsy.SetKeys(sspk, sssk);
                            }
                        }
                        iex = Cryptsy;
                        break;
                    }
                case ExchangeEnum.MintPal:
                    {
                        break;
                    }
                case ExchangeEnum.Poloniex:
                    {
                        if (Poloniex == null)
                        {
                            Poloniex = new API_Poloniex(exOut);
                            Poloniex.event_DeclaringApiState += iEx_event_DeclaringApiState;
                            Poloniex.event_ApiBytesDownloaded += iEx_event_ApiBytesDownloaded;

                            if (CurrentUser.EncryptedKeys.ContainsKey(ExchangeEnum.Poloniex))
                            {
                                var items = CurrentUser.EncryptedKeys[ExchangeEnum.Poloniex];
                                SecureString sspk = EncryptionTools.UserDecrypt(items[0], CurrentUser);
                                SecureString sssk = EncryptionTools.UserDecrypt(items[1], CurrentUser);
                                Poloniex.SetKeys(sspk, sssk);
                            }
                        }
                        iex = Poloniex;
                        break;
                    }
                case ExchangeEnum.Kraken:
                    {
                        if (Kraken == null)
                        {
                            Kraken = new API_Kraken(exOut);
                            Kraken.event_DeclaringApiState += iEx_event_DeclaringApiState;
                            Kraken.event_ApiBytesDownloaded += iEx_event_ApiBytesDownloaded;

                            if (CurrentUser.EncryptedKeys.ContainsKey(ExchangeEnum.Kraken))
                            {
                                var items = CurrentUser.EncryptedKeys[ExchangeEnum.Kraken];
                                SecureString sspk = EncryptionTools.UserDecrypt(items[0], CurrentUser);
                                SecureString sssk = EncryptionTools.UserDecrypt(items[1], CurrentUser);
                                Kraken.SetKeys(sspk, sssk);
                            }
                        }
                        iex = Kraken;
                        break;
                    }
                default:
                    break;
            }
            return iex;
        }
        private static void iEx_event_ApiBytesDownloaded(object sender, IExEvents e)
        {
            var ex = sender as IExchange;
            NotifyExchangeMessageSubscribers(ex.ExchangeName, e.BytesDownloaded);
        }

        private static void SendTickerRow(Ticker input)
        {
            CheckConditionalOrders(input);

            using (TickerViewTable tempTab = new TickerViewTable())
            {
                TickerViewTableRow tRow = tempTab.GetNewRow();

                tRow.Exchange = input.ExchangeName;
                tRow.Market = input.MarketIdentity.StandardisedName;
                tRow.LastUpdated = DateTime.Now;

                double ask = 0;
                if (input.TopAsk.ToString() != null)
                    ask = input.TopAsk;
                tRow.Ask = ask;

                double bid = 0;
                if (input.TopBid.ToString() != null)
                    bid = input.TopBid;
                tRow.Bid = bid;

                double ltp = 0;
                if (input.LastTrade != null)
                    ltp = input.LastTrade.Price;
                tRow.LastTradePrice = ltp;

                NotifyTickerSubscribers(input, tRow);
            }
        }
        private static void CheckConditionalOrders(Ticker input)
        {
            Task.Run(() =>
            {
                if (ConditionalTrades != null && ConditionalTrades.Count > 0)
                {
                    var tradeList = from cT in ConditionalTrades[input.ExchangeName]
                                    where cT.Market.StandardisedName == input.Market.MarketIdentity.StandardisedName
                                    && cT.Market.ExchangeName == input.Market.MarketIdentity.ExchangeName
                                    select cT;

                    if (tradeList.Count() > 0)
                    {
                        foreach (TradeObject t in tradeList)
                        {
                            if (t.Condition != null)
                            {
                                bool triggered = false;
                                double relTrig;

                                if (t.Type == OrderType.Ask)
                                    relTrig = input.TopBid;
                                else
                                    relTrig = input.TopAsk;

                                var coni = (ConditionalType)t.Condition;
                                switch (coni)
                                {
                                    case ConditionalType.LimitHigh:
                                        {
                                            if (relTrig >= t.TriggerPrice)
                                                triggered = true;
                                            break;
                                        }
                                    case ConditionalType.LimitLow:
                                        {
                                            if (relTrig <= t.TriggerPrice)
                                                triggered = true;
                                            break;
                                        }
                                    case ConditionalType.TimeDelayed:
                                        {
                                            if ((DateTime)t.ExpireTime < DateTime.Now)
                                                triggered = true;
                                            break;
                                        }
                                    case ConditionalType.Tracking:
                                        {
                                            break;
                                        }
                                    default:
                                        break;
                                }

                                if (triggered)
                                {
                                    ConditionalTrades[input.ExchangeName].Remove(t);
                                    NotifyConditionalOrdersSubscribers();
                                    PlaceOrder(t.Market.ExchangeName, t.Market, t.Type, t.Price, t.Quantity);
                                }
                            }
                        }
                    }
                }
                return;
            });
        }

        /// <summary>
        /// Internal method for placing an order when result message and order id are needed
        /// </summary>
        /// <param name="exName"></param>
        /// <param name="Type"></param>
        /// <param name="Price"></param>
        /// <param name="Quantity"></param>
        /// <param name="OrderId"></param>
        /// <returns>Array [OrderId, Message]</returns>
        private static string[] PlaceOrder(ExchangeEnum exName, MarketIdentity Identity, OrderType Type, double Price, double Quantity)
        {
            if (Price * Quantity > 0)
            {
                Exchange ex;
                IExchange ordEx = SwitchExchange(exName, out ex);

                var result = ordEx.PlaceBasicOrder(Identity, Type, Price, Quantity);
                string OrderId = result.Item1;

                NotifyExchangeMessageSubscribers(exName, OrderId);

                Task.Factory.StartNew(() => PostOrderUpdate(Identity.MarketId, ex, ordEx));

                return new string[] { result.Item1, result.Item2 };
            }
            else return new string[] { "Error", "Order amounts invalid" };
        }

        private static async void PostOrderUpdate(String MarketId, Exchange ex, IExchange ordEx)
        {
            var q = from mkt in ex.Markets
                    where mkt.MarketIdentity.MarketId == MarketId
                    select mkt;

            if (q.Count() > 0)
            {
                /// wait for 1 second before updating to try and ensure order has been processed
                await Task.Delay(1000);

                ///Get count of active orders before update.
                ///Once update is done this number should have changed.
                ///If it hasn't the exchange probably still hadn't updated and we should wait then perform the update again.
                int aO = ex.ActiveOrders.Count;

                UpdateInfo(ex.Name);

                while (aO == ex.ActiveOrders.Count)
                {
                    await Task.Delay(3000);
                    UpdateInfo(ex.Name);
                }

                Market m = q.First();
                m.TradeRecords = ordEx.GetSingleMarketTradeHistory(m.MarketIdentity);
                m.OrderBook = ordEx.GetSingleMarketOrders(m.MarketIdentity);

                if (ActiveMarket != null
                    && m.MarketIdentity.StandardisedName == ActiveMarket.MarketIdentity.StandardisedName
                    && m.MarketIdentity.ExchangeName == ActiveMarket.MarketIdentity.ExchangeName)
                {
                    ActiveMarket = m;
                }
                NotifyOrderBookSubscribers(m.OrderBook, m.TradeRecords);

                await Task.Delay(5000);
                await ForceUpdate(ex.Name, "All");
            }
        }

        private static string CancelOrder(ExchangeEnum exName, ActiveOrder orderObj)
        {
            string result = "No Order To Cancel";
            if (orderObj != null)
            {
                Exchange ex;
                IExchange ordEx = SwitchExchange(exName, out ex);

                result = ordEx.CancelOrder(orderObj);

                Task.Factory.StartNew(() => PostOrderUpdate(orderObj.MarketId, ex, ordEx));
            }
            return result;
        }
        #endregion


        #region Exchange Events
        private static void iEx_event_DeclaringApiState(object sender, IExEvents e)
        {
            IExchange ex = sender as IExchange;
            if (e.Active)
            {
                if (!ActiveExchanges.Contains(ex.ExchangeName))
                    ActiveExchanges.Add(ex.ExchangeName);
            }
            else
            {
                if (ActiveExchanges.Contains(ex.ExchangeName))
                    ActiveExchanges.Remove(ex.ExchangeName);
            }

            if (event_ApiStateChanged != null)
            {
                CoreEvents ce = new CoreEvents();
                ce.ApiStateChange = new Tuple<ExchangeEnum, bool>(ex.ExchangeName, e.Active);
                event_ApiStateChanged(null, ce);
            }
        }
        #endregion

        internal static void EditUserKeys()
        {
            KeySetter kS = new KeySetter(CurrentUser);
            kS.FormClosed += (s, e) =>
            {
                Task.Factory.StartNew(()=>ApplyUserKeys());
            };
            kS.Show();
        }
        private static void ApplyUserKeys()
        {
            foreach (var exch in CurrentUser.EncryptedKeys)
            {
                Exchange ex;
                IExchange iE = SwitchExchange(exch.Key, out ex);
                if (iE != null)
                {
                    if (!iE.HasKeys || exch.Value[2] == "true")
                    {
                        SecureString sspk = EncryptionTools.UserDecrypt(exch.Value[0], CurrentUser);
                        SecureString sssk = EncryptionTools.UserDecrypt(exch.Value[1], CurrentUser);

                        iE.SetKeys(sspk, sssk);
                        CurrentUser.NotNew(exch.Key);
                    }
                }
            }
            UpdateInfo();
        }
    }
}
