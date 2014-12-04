using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Equin.ApplicationFramework;
using Toolbox;
using Objects;
using StockCharts;
using System.Globalization;

namespace Objects
{
    public partial class MainForm : Form
    {
        #region internal fields and parameters
        int dgvY;
        int rateButtonsY;
        bool showZeroBalances;
        double commodityBalance;
        double currencyBalance;
        #endregion

        #region Common UI Settings
        Color colourError = Color.Tomato;
        Color colourOk = Color.Teal;
        Color colourTick = Color.Yellow;
        Color colourTickDefault = Color.White;

        Font HeaderFont = new System.Drawing.Font("Arimo", 11.99F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        Font LabelFont = new System.Drawing.Font("Arimo", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        #endregion

        #region Lists
        Dictionary<TickerViewTable, BindingSource> TickerTables;
        Dictionary<ExchangeEnum, HashSet<Balance>> BalanceTables;
        Dictionary<ExchangeEnum, HashSet<ActiveOrder>> ActiveOrderTables;
        List<ExchangeEnum> ActiveExchanges;
        Dictionary<ExchangeEnum, Dictionary<string, Ticker>> MainRates;
        List<string> highlightList;

        private static readonly Dictionary<ExchangeEnum, string> defaultGaps = new Dictionary<ExchangeEnum, string>()
        {
            {ExchangeEnum.BitCoinCoId, "120"},
            {ExchangeEnum.Bittrex, "120"},
            {ExchangeEnum.BTCe, "120"},
            {ExchangeEnum.Cryptsy, "600"},
            {ExchangeEnum.Kraken, "300"},
            {ExchangeEnum.MintPal, "150"},
            {ExchangeEnum.Poloniex, "300"},
            {ExchangeEnum.Vircurex, "300"},
        };
        #endregion

        #region Active Objects
        private static ExchangeEnum ActiveExchangeName;
        private static Market ActiveMarket;
        private static ActiveOrder SelectedActiveOrder;

        private BindingSource ConditionalOrderListBinder;
        private BindingSource TradeHistoryBinder;
        #endregion

        #region Constructor
        /// <summary>
        /// Start units, create dynmaic UI elements and set base settings
        /// </summary>
        /// <param name="Guest"></param>
        public MainForm(bool Guest)
        {
            InitializeComponent();

            TickerTables = new Dictionary<TickerViewTable, BindingSource>();
            BalanceTables = new Dictionary<ExchangeEnum, HashSet<Balance>>();
            ActiveOrderTables = new Dictionary<ExchangeEnum, HashSet<ActiveOrder>>();
            ActiveExchanges = new List<ExchangeEnum>();
            MainRates = new Dictionary<ExchangeEnum, Dictionary<string, Ticker>>();
            highlightList = new List<string>();
            setButtons(false);

            Core.InitialiseCore();
            CoreEventHandler();

            if (Guest)
            {
                this.Text = "MainPage - Guest User";
                toolStripButton_EditKeys.Enabled = false;
                toolStripButton_EditKeys.ToolTipText = "Not available as guest.";
            }
            else
            {
                string welMsg = string.Format("MainPage - Authorised User ({0})", Core.GetUserName());
                this.Text = welMsg;
            }

            Task.Run(() => Core.Start());
        }

        /// <summary>
        /// FINISH ME -> Get if authorised or not, decide what that kills\enables.
        /// Load default values
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Load(object sender, System.EventArgs e)
        {
            ActiveExchangeName = ActiveExchanges.FirstOrDefault();
        }
        #endregion

        #region DataGrid Creation and Linking
        private void CreateTickerViews()
        {
            ///Set initial Y position
            dgvY = 35;

            ///Clear any controls currently on panel
            if (panel_Tickers.Controls.Count > 0)
                panel_Tickers.Controls.Clear();

            ///Create new set of ticker views
            foreach (var item in ActiveExchanges)
            {
                CreateTickerView(item);
            }
        }
        private void CreateTickerView(ExchangeEnum exch)
        {
            #region Layout
            string nom = exch.ToString();
            short period = Convert.ToInt16(defaultGaps[exch]);
            TickerPanel.Ticker ticker = new TickerPanel.Ticker();
            ticker.SetNameAndPeriod(nom, period);
            ticker.event_PeriodChanged += ticker_event_PeriodChanged;
            this.panel_Tickers.Controls.Add(ticker);
            ticker.Location = new System.Drawing.Point(5, dgvY);

            DataGridView dgv = ticker.dgv;
            dgv.Name = exch.ToString();
            dgv.CellClick += dgv_CellClick;
            dgv.VirtualMode = true;
            dgvY = dgvY + 305;
            #endregion

            ///Link data
            TickerViewTable tickTab = new TickerViewTable(exch.ToString());
            BindingSource binder = new BindingSource(tickTab, null);
            TickerTables.Add(tickTab, binder);
            binder.DataSource = tickTab;
            binder.ListChanged += tickerBinder_ListChanged;
            dgv.DataSource = binder;

            return;
        }

        private void LinkActiveOrders()
        {
            dataGridView_ActiveOrders.DataSource = ActiveOrderTables[ActiveExchangeName].ToList();
        }
        private void LinkBalances()
        {
            if (BalanceTables != null && BalanceTables.ContainsKey(ActiveExchangeName))
            {
                List<Balance> balances;
                if (showZeroBalances == true)
                {
                    balances = BalanceTables[ActiveExchangeName].ToList();
                }
                else
                {
                    balances = BalanceTables[ActiveExchangeName].Where(p => p.Total > 0).ToList();
                }
                dataGridView_Balances.SuspendLayout();
                dataGridView_Balances.DataSource = balances;
                dataGridView_Balances.Refresh();
                dataGridView_Balances.ResumeLayout();
            }
        }
        private void LinkOrderBook()
        {
            BindingList<Order> askBindList = new BindingList<Order>(ActiveMarket.OrderBook.AskOrders.ToList());
            BindingList<Order> bidBindList = new BindingList<Order>(ActiveMarket.OrderBook.BidOrders.ToList());
            BindingSource askBindSource = new BindingSource();
            BindingSource bidBindSource = new BindingSource();
            askBindSource.DataSource = askBindList;
            bidBindSource.DataSource = bidBindList;
            dataGridView_OrderBookAsks.DataSource = askBindSource;
            dataGridView_OrderBookBids.DataSource = bidBindSource;

            BindingList<TradeRecord> tradeList = new BindingList<TradeRecord>(ActiveMarket.TradeRecords.ToList());
            TradeHistoryBinder = new BindingSource();
            TradeHistoryBinder.DataSource = tradeList;
            dataGridView_TradeRecs.DataSource = TradeHistoryBinder;
        }
        #endregion

        #region DataGrid Actions
        private async void dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridView dg = sender as DataGridView;

                bool exchangeSwaped = false;
                if (dataGridView_ActiveOrders.Rows.Count == 0 || ActiveExchangeName.ToString() != dg.Name)
                    exchangeSwaped = true;

                ActiveExchangeName = Toolbox.GeneralTools.ParseEnum<ExchangeEnum>(dg.Name);

                if (exchangeSwaped)
                {
                    LinkActiveOrders();
                    LinkBalances();
                }

                string sN = dg.Rows[e.RowIndex].Cells["Market"].Value.ToString();

                ActiveMarket = await Core.SetActiveMarket(ActiveExchangeName, sN);
                label_MarketID.Text = ActiveMarket.MarketIdentity.MarketId;
                checkButtonState();

                var exN = Toolbox.GeneralTools.ParseEnum<StockCharts.ExchangeEnum>(ActiveExchangeName.ToString());
                quickChart1.LoadChart(exN, ActiveMarket.MarketIdentity.MarketId);

                LinkOrderBook();
                GetBalances();
                
                SetDecimalsOnGrids();
                SetSellingParameters();
                SetBuyingParameters();
                UpdateSettingsLabels();
                UpdateRateButtons();
            }
        }
        private void dgv_OrderBookAsks_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridView view = sender as DataGridView;
                decimal prc = Convert.ToDecimal(view.CurrentRow.Cells["dgv_Asks_Price"].Value);
                decimal qty = Convert.ToDecimal(view.CurrentRow.Cells["dgv_Asks_Quantity"].Value);
                try
                {
                    textBox_BuyPrice.Value = prc;
                    textBox_BuyQuant.Value = qty;
                    SetBuyingTotalAndFee(prc, qty);
                }
                catch (ArgumentOutOfRangeException) { }
            }
        }
        private void dgv_OrderBookBids_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridView view = sender as DataGridView;
                decimal prc = Convert.ToDecimal(view.CurrentRow.Cells["dgv_Bids_Price"].Value);
                decimal qty = Convert.ToDecimal(view.CurrentRow.Cells["dgv_Bids_Quantity"].Value);
                try
                {
                    textBox_SellPrice.Value = prc;
                    textBox_SellQuant.Value = qty;
                    SetSellingTotalAndFees(prc, qty);
                }
                catch (ArgumentOutOfRangeException) { }
            }
        }
        private void dataGridView_ActiveOrders_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var orderId = dataGridView_ActiveOrders.Rows[e.RowIndex].Cells[0].Value.ToString();
                SelectedActiveOrder = ActiveOrderTables[ActiveExchangeName].First(p => p.OrderId == orderId);
                textBox_CancelOrderId.Text = orderId;
            }
        }
        #endregion

        #region Text Boxes
        private void textBox_BuyPrice_ValueChanged(object sender, EventArgs e)
        {
            SetBuyingTotalAndFee(textBox_BuyPrice.Value, textBox_BuyQuant.Value);
        }
        private void textBox_SellPrice_ValueChanged(object sender, EventArgs e)
        {
            SetSellingTotalAndFees(textBox_SellPrice.Value, textBox_SellQuant.Value);
        }
        #endregion

        #region Buttons
        private void checkButtonState()
        {
            if (this.InvokeRequired && !this.IsDisposed)
            {
                try
                {
                    MethodInvoker d = new MethodInvoker(checkButtonState);
                    Invoke(d, new object[] { });
                }
                catch (ObjectDisposedException) { }
            }
            else
            {
                bool active;
                if (ActiveMarket != null)
                {
                    active = ActiveMarket.Exchange.CanTrade;
                }
                else active = false;

                setButtons(active);
            }
        }
        private void setButtons(bool active)
        {
            button_Buy.Enabled = active;
            button_CancelOrder.Enabled = active;
            button_CondOrdersCancel.Enabled = active;
            button_CondOrdersSave.Enabled = active;
            button_Sell.Enabled = active;
        }
        private void placeOrderIfValid(OrderType type)
        {
            double max;
            double tot = 0;
            double prc = 0;
            string balText = null;
            
            switch (type)
            {
                case OrderType.Ask:
                    tot = Convert.ToDouble(textBox_SellQuant.Value);
                    prc = Convert.ToDouble(textBox_SellPrice.Value);
                    balText = label_CommodityBalance.Text;
                    break;
                case OrderType.Bid:
                    if (!Double.TryParse(textBox_BuyTotal.Text, out tot))
                        UpdateStatusLabel("Unkown error occured trying to place order.");
                    prc = Convert.ToDouble(textBox_BuyPrice.Value);
                    balText = label_CurrencyBalance.Text;
                    break;
            }
            double mP = ActiveMarket.MaxPrice;
            if(ActiveMarket.MaxPrice == 0)
                mP = 999999999999;

            if (tot > 0)
            {
                if (prc > ActiveMarket.MinPrice && prc < mP)
                {
                    if (Double.TryParse(balText, out max))
                    {
                        if (tot <= max)
                            PlaceBasicOrder(type);
                        else
                            MessageBox.Show("Insufficent funds for this transaction", "Sorry");
                    }
                    else
                        UpdateStatusLabel("Unkown error occured trying to place order.");
                }
                else
                    MessageBox.Show("Order value must be between min and max values.");
            }
            else
                MessageBox.Show("Invalid order amount.\nOrder value must be greater than 0");
        }
        private void button_Buy_Click(object sender, EventArgs e)
        {
            placeOrderIfValid(OrderType.Bid);
        }
        private void button_Sell_Click(object sender, EventArgs e)
        {
            placeOrderIfValid(OrderType.Ask);
        }
        private void button_CancelOrder_Click(object sender, EventArgs e)
        {
            string OrderId = null;

            if (!string.IsNullOrEmpty(textBox_CancelOrderId.Text))
            {
                OrderId = textBox_CancelOrderId.Text;
            }
            else
            {
                MessageBox.Show("No order selected to cancel.");
            }

            string result = CancelOrder(ActiveExchangeName, OrderId, SelectedActiveOrder.MarketId);
            UpdateStatusLabel(result);
            textBox_CancelOrderId.Text = "";
            GetBalances();
        }
        private void button_HideZeros_Click(object sender, EventArgs e)
        {
            if (showZeroBalances == false)
            {
                showZeroBalances = true;
                button_HideZeros.Text = "Hide Zeros";
            }
            else
            {
                showZeroBalances = false;
                button_HideZeros.Text = "Show Zeros";
            }

            LinkBalances();
        }
        #endregion

        #region UI Info Updating
        delegate void statusLabelDelegate(string message, bool rep);
        private void UpdateStatusLabel(string message, bool repeat = false)
        {

            if (this.InvokeRequired)
            {
                statusLabelDelegate d = new statusLabelDelegate(UpdateStatusLabel);
                try
                {
                    Invoke(d, new object[] { message, repeat });
                }
                catch (ObjectDisposedException)
                { }
            }
            else
            {
                toolStripLabel_Status.Text = message;
                if (!repeat)
                {
                    Timer t = new Timer();
                    t.Interval = 3000;
                    t.Tick += (sender, e) =>
                    {
                        Timer ti = sender as Timer;
                        ti.Stop();
                        ti.Dispose();
                        string msg = String.Format("Last action: {0}", DateTime.Now.ToString());
                        UpdateStatusLabel(msg, true);
                    };
                    t.Start();
                }
            }
        }

        private void label_CommodityBalance_Click(object sender, EventArgs e)
        {
            ///As this is a check on selling all the commodity, the quantity
            ///is simply the balance available for the commodity
            double quant = commodityBalance;

            if (dataGridView_OrderBookBids.RowCount > 0 && quant > 0)
            {
                ///First get price of top bid incase no other price is set
                decimal prc;
                Decimal.TryParse(dataGridView_OrderBookBids.Rows[0].Cells[0].Value.ToString(), out prc);
                
                ///Check if a price has been set in text box
                ///and if so switch the price to this one
                if (textBox_SellPrice.Value > 0)
                    prc = textBox_SellPrice.Value;

                /// Check both price and quantity are above 0 to prevent errors
                if (prc > 0 && quant > 0)
                {
                    ///Run the calculator
                    decimal fee;
                    var feePct = ActiveMarket.GetFee(quant);
                    double total;

                    total = Convert.ToDouble(Calcs.Sell_HowMuch(Convert.ToDecimal(quant), feePct, Convert.ToDecimal(prc), out fee));

                    ///Set the textboxes to new values
                    textBox_SellPrice.Text = prc.ToString("N8");
                    textBox_SellQuant.Text = quant.ToString("N8");
                    textBox_SellFee.Text = String.Format("{0:N8}", fee);
                    textBox_SellTotal.Text = total.ToString("N8");
                }
            }
        }
        private void label_CurrencyBalance_Click(object sender, EventArgs e)
        {
            decimal bal = Convert.ToDecimal(currencyBalance);

            if (dataGridView_OrderBookAsks.RowCount > 0 && bal > 0)
            {
                ///First check if user has set a price,
                ///if not grab best price from asks order book.                
                decimal prc;
                if (textBox_BuyPrice.Value > 0)
                    prc = textBox_BuyPrice.Value;
                else
                    prc = Convert.ToDecimal(dataGridView_OrderBookAsks.Rows[0].Cells[1].Value);

                if (prc > 0)
                {
                    double apxAmt = Convert.ToDouble(bal / prc);

                    var feePct = ActiveMarket.GetFee(apxAmt);
                    ///Check price is above 0 to prevent divide by zero errors
                    if (prc > 0)
                    {
                        decimal quant;
                        ///Calculate maximum volume

                        quant = Calcs.Buy_HowMany(bal, 100M, feePct, prc);

                        ///Calculate total cost and fee
                        decimal fee;
                        decimal total = Calcs.Buy_HowMuch(Convert.ToDecimal(quant), 0.25M, prc, out fee);

                        ///Set text boxes
                        textBox_BuyQuant.Value = quant;
                        textBox_BuyPrice.Value = prc;
                        textBox_BuyFee.Text = String.Format("{0:N8}", fee);
                        textBox_BuyTotal.Text = total.ToString("N8");
                    }
                }
            }
        }

        private void UpdateBalanceLabels()
        {
            if (this.InvokeRequired)
            {
                MethodInvoker d = new MethodInvoker(UpdateBalanceLabels);
                try
                {
                    Invoke(d, new object[] { });
                }
                catch (ObjectDisposedException)
                { }
            }
            else
            {
                label_CommodityBalance.Text = commodityBalance.ToString("R");
                label_CurrencyBalance.Text = currencyBalance.ToString("R");
            }
        }
        private void UpdateRateButtons()
        {
            if (this.InvokeRequired)
            {
                MethodInvoker d = new MethodInvoker(UpdateRateButtons);
                try
                {
                    Invoke(d, new object[] { });
                }
                catch (ObjectDisposedException)
                { }
            }
            else
            {
                if (MainRates.ContainsKey(ActiveExchangeName) && MainRates[ActiveExchangeName].Count > 0)
                {

                    string labelText = string.Format("Main market rates: {0}", ActiveExchangeName);

                    ///Check if exchange name matches name on invisible lable
                    ///and if it doesn't clear all buttons and reset Y locator value.
                    if (label_RatesExchangeName.Text != labelText)
                    {
                        label_RatesExchangeName.Text = labelText;

                        if (tabControl1.TabPages[3].Controls.Count > 0)
                            tabControl1.TabPages[3].Controls.Clear();

                        rateButtonsY = 25;
                    }

                    foreach (var item in MainRates[ActiveExchangeName])
                    {
                        ///This block catches null values
                        double ltp = Double.NaN;
                        if (item.Value.LastTrade != null)
                            try
                            {
                                ltp = item.Value.LastTrade.Price;
                            }
                            catch (NullReferenceException) { }

                        ///Create string to display on rate button
                        string s = string.Format("{0}: {1}", item.Key, ltp);

                        ///If a button doesn't already exist for this market, create one
                        if (!tabControl1.TabPages[3].Controls.ContainsKey(item.Key))
                        {
                            Button bt = new Button();
                            bt.Name = item.Key;
                            bt.Text = s;
                            bt.Size = new System.Drawing.Size(327, 35);
                            tabControl1.TabPages[3].Controls.Add(bt);
                            bt.Location = new Point(7, rateButtonsY);
                            rateButtonsY = rateButtonsY + 38;
                        }
                        ///Else check new value against old and colour button accordingly
                        else
                        {
                            string oldValString = tabControl1.TabPages[3].Controls[item.Key].Text.Split(':')[1].Replace(" ", "");
                            double oldVal;
                            Double.TryParse(oldValString, out oldVal);
                            Color butCol = Color.LightGray;
                            if (oldVal != Double.NaN && ltp != Double.NaN)
                            {
                                if (ltp > oldVal)
                                    butCol = Color.Green;
                                else if (ltp < oldVal)
                                    butCol = Color.Red;
                                else
                                    butCol = Color.Aqua;
                            }

                            tabControl1.TabPages[3].Controls[item.Key].Text = s;
                            tabControl1.TabPages[3].Controls[item.Key].BackColor = butCol;
                        }
                    }
                }
            }
        }
        private void UpdateSettingsLabels()
        {
            if (this.InvokeRequired)
            {
                MethodInvoker d = new MethodInvoker(UpdateSettingsLabels);
                Invoke(d, new object[] { });
            }
            else
            {
                string fmt = string.Format("N{0}", ActiveMarket.PriceDecimals);
                label_Market.Text = ActiveMarket.MarketIdentity.StandardisedName;
                label_Spread.Text = string.Format("{0}%", ActiveMarket.Spread);

                textBox_MinPrice.Text = ActiveMarket.MinPrice.ToString(fmt);
                textBoxMaxPrice.Text = ActiveMarket.MaxPrice.ToString(fmt);
                textBox_MinVolume.Text = ActiveMarket.MinQuant.ToString(fmt);
                textBox_Decimals.Text = ActiveMarket.PriceDecimals.ToString();
            }
        }

        private void UpdateListViewInfo(ExchangeEnum ex, string status, string msg)
        {
            try
            {
                if (!listView1.Items.ContainsKey(ex.ToString()))
                {
                    string[] data = new string[] { ex.ToString(), status, msg };
                    ListViewItem lvi = new ListViewItem(data);
                    lvi.Tag = ex.ToString();
                    lvi.Name = ex.ToString();
                    listView1.Items.Add(lvi);
                }
                else
                {
                    ListViewItem lvi = listView1.Items.Find(ex.ToString(), true).First();
                    lvi.SubItems[1].Text = status;
                    lvi.SubItems[2].Text = msg;
                }
            }
            catch (ArgumentOutOfRangeException) { }
        }
        #endregion

        #region Helpers
        private void SetDecimalsOnGrids()
        {
            string priceFormat = String.Format("N{0}", ActiveMarket.PriceDecimals);
            string quantFormat = String.Format("N{0}", ActiveMarket.QuantDecimals);

            var dataGridViewCellStyle_Price = new System.Windows.Forms.DataGridViewCellStyle();
            dataGridViewCellStyle_Price.Format = priceFormat;
            var dataGridViewCellStyle_Quant = new System.Windows.Forms.DataGridViewCellStyle();
            dataGridViewCellStyle_Quant.Format = quantFormat;

            ///Price Fields
            this.dgv_Asks_Price.DefaultCellStyle = dataGridViewCellStyle_Price;
            this.dgv_Bids_Price.DefaultCellStyle = dataGridViewCellStyle_Price;
            ///Quantity Fields
            this.dgv_Bids_Quantity.DefaultCellStyle = dataGridViewCellStyle_Quant;
            this.dgv_Asks_Quantity.DefaultCellStyle = dataGridViewCellStyle_Quant;
        }
        private void SetBuyingParameters()
        {
            decimal priceIncrement = GetIncrementAmount(ActiveMarket.PriceDecimals);
            decimal quantIncrement = GetIncrementAmount(ActiveMarket.QuantDecimals);

            textBox_BuyPrice.Minimum = Convert.ToDecimal(ActiveMarket.MinPrice);
            textBox_BuyPrice.Maximum = Convert.ToDecimal(ActiveMarket.MaxPrice);
            if (textBox_BuyPrice.Maximum <= 0)
                textBox_BuyPrice.Maximum = 999999999;
            textBox_BuyPrice.DecimalPlaces = ActiveMarket.PriceDecimals;
            textBox_BuyPrice.Increment = priceIncrement;
            textBox_BuyPrice.Value = textBox_BuyPrice.Minimum;

            textBox_BuyQuant.Minimum = Convert.ToDecimal(ActiveMarket.MinQuant);
            textBox_BuyQuant.Maximum = 9999999999999999999;
            textBox_BuyQuant.DecimalPlaces = ActiveMarket.QuantDecimals;
            textBox_BuyQuant.Increment = quantIncrement;
            textBox_BuyQuant.Value = textBox_BuyQuant.Minimum;

            numericUpDown_CondOrderPrice.Increment = priceIncrement;
            numericUpDown_CondOrderPrice.Minimum = textBox_BuyPrice.Minimum;
            numericUpDown_CondOrderPrice.Maximum = textBox_BuyPrice.Maximum;
            numericUpDown_CondOrderPrice.DecimalPlaces = textBox_BuyPrice.DecimalPlaces;

            numericUpDown_CondTrigger.Increment = priceIncrement;
            numericUpDown_CondTrigger.Minimum = textBox_BuyPrice.Minimum;
            numericUpDown_CondTrigger.Maximum = textBox_BuyPrice.Maximum;
            numericUpDown_CondTrigger.DecimalPlaces = textBox_BuyPrice.DecimalPlaces;

            numericUpDown_CondQuant.Increment = quantIncrement;
            numericUpDown_CondQuant.Minimum = textBox_BuyQuant.Minimum;
            numericUpDown_CondQuant.Maximum = textBox_BuyQuant.Maximum;
            numericUpDown_CondQuant.DecimalPlaces = textBox_BuyQuant.DecimalPlaces;
        }
        private void SetSellingParameters()
        {
            decimal priceIncrement = GetIncrementAmount(ActiveMarket.PriceDecimals);
            decimal quantIncrement = GetIncrementAmount(ActiveMarket.QuantDecimals);

            textBox_SellPrice.Minimum = Convert.ToDecimal(ActiveMarket.MinPrice);
            textBox_SellPrice.Maximum = Convert.ToDecimal(ActiveMarket.MaxPrice);
            if (textBox_SellPrice.Maximum <= 0)
                textBox_SellPrice.Maximum = 999999999;
            textBox_SellPrice.DecimalPlaces = ActiveMarket.PriceDecimals;
            textBox_SellPrice.Increment = priceIncrement;
            textBox_SellPrice.Value = textBox_SellPrice.Minimum;

            textBox_SellQuant.Minimum = Convert.ToDecimal(ActiveMarket.MinQuant);
            textBox_SellQuant.Maximum = 9999999999999999999;
            textBox_SellQuant.DecimalPlaces = ActiveMarket.QuantDecimals;
            textBox_SellQuant.Increment = quantIncrement;
            textBox_SellQuant.Value = textBox_SellQuant.Minimum;
        }
        private decimal GetIncrementAmount(int input)
        {
            int length = input + 2;
            char[] getInc = new char[length];

            for (int ii = 0; ii < length; ii++)
            {
                if (ii == 0)
                {
                    getInc[ii] = '0';
                }
                else if (ii == 1)
                {
                    getInc[ii] = '.';
                }
                else if (ii == length - 1)
                    getInc[ii] = '1';
                else
                    getInc[ii] = '0';
            }
            string places = String.Empty;
            foreach (var c in getInc)
            {
                places = String.Format("{0}{1}", places, c);
            }
            decimal increment = Convert.ToDecimal(places);
            return increment;
        }
        private void SetBuyingTotalAndFee(decimal prc, decimal qty)
        {
            decimal fee;
            double currentFee = GetCurrentFee(qty);
            decimal tot = 0;

            tot = Calcs.Buy_HowMuch(qty, currentFee, prc, out fee);

            textBox_BuyFee.Text = String.Format("{0:G}", fee);
            textBox_BuyTotal.Text = tot.ToString("G");
        }
        private void SetSellingTotalAndFees(decimal prc, decimal qty)
        {
            decimal fee = 0;
            double currentFee = GetCurrentFee(qty);
            decimal tot = 0;

            tot = Calcs.Sell_HowMuch(qty, currentFee, prc, out fee);

            textBox_SellFee.Text = String.Format("{0:G}", (decimal)fee);
            textBox_SellTotal.Text = tot.ToString("G");
        }
        public double GetCurrentFee(dynamic quantity)
        {
            if (ActiveMarket != null)
            {
                var safeQ = Convert.ToDouble(quantity);
                return ActiveMarket.GetFee(safeQ);
            }
            else return 0;
        }
        #endregion

        #region Exchange Actions
        /// <summary>
        /// Updates balance labels from internal data.
        /// Does not call for an update as that is handled by Core.
        /// </summary>
        private void GetBalances()
        {
            if (null != ActiveMarket)
            {
                var comQ = from cM in BalanceTables[ActiveMarket.ExchangeName]
                           where cM.Name == ActiveMarket.MarketIdentity.Commodity
                           select cM;
                var curQ = from cR in BalanceTables[ActiveMarket.ExchangeName]
                           where cR.Name == ActiveMarket.MarketIdentity.Currency
                           select cR;

                commodityBalance = 0;
                currencyBalance = 0;

                if (comQ.Count() > 0)
                    commodityBalance = comQ.ElementAt(0).Available;
                if (curQ.Count() > 0)
                    currencyBalance = curQ.ElementAt(0).Available;

                UpdateBalanceLabels();
            }
        }
        private void GetActiveOrders()
        {
            Task.Factory.StartNew(async () => await Core.ForceUpdate(ActiveExchangeName, "ActiveOrders"));
        }
        private void PlaceBasicOrder(OrderType basicOrderEnum)
        {
            double price = 0;
            double quant = 0;
            switch (basicOrderEnum)
            {
                case OrderType.Ask:
                    price = Convert.ToDouble(textBox_SellPrice.Value);
                    quant = Convert.ToDouble(textBox_SellQuant.Value);
                    break;
                case OrderType.Bid:
                    price = Convert.ToDouble(textBox_BuyPrice.Value);

                    if(ActiveExchangeName == ExchangeEnum.BitCoinCoId)
                        quant = Convert.ToDouble(textBox_BuyTotal.Text);
                    else
                        quant = Convert.ToDouble(textBox_BuyQuant.Value);

                    break;
            }
            string resp = Core.PlaceOrderAsync(basicOrderEnum, price, quant)[1];
            UpdateStatusLabel(resp);

            ///We are only updating the balance labels here.
            ///Updating of balance tables and active orders is
            ///done from Core and sent via an event
            GetBalances();
        }
        private string CancelOrder(ExchangeEnum exch, string OrderId, string marketName)
        {
            string result = Core.CancelOrderAsync(exch, OrderId, marketName);
            return result;
        }
        #endregion

        #region Event Handlers
        delegate void coreEventDelegate(object sender, CoreEvents e);
        internal void CoreEventHandler()
        {
            Core.event_NewMessage += Core_event_NewMessage;
            Core.event_NewExchangeMessage += Core_event_NewExchangeMessage;
            Core.event_ApiStateChanged += Core_event_ApiStateChanged;
            Core.event_NewTickerRow += Core_event_NewTickerRow;
            Core.event_NewActiveOrders += Core_event_NewActiveOrders;
            Core.event_NewBalances += Core_event_NewBalances;
            Core.event_NewOrderBook += Core_event_NewOrderBook;
            Core.event_RateChanged += Core_event_RateChanged;
            Core.event_AutoOrderPlaced += Core_event_AutoOrderPlaced;
            Core.event_ConditonalOrderListUpdated += Core_event_ConditonalOrderListUpdated;
        }

        internal void Core_event_NewExchangeMessage(object sender, CoreEvents e)
        {
            if (this.InvokeRequired)
            {
                try
                {
                    coreEventDelegate d = new coreEventDelegate(Core_event_NewExchangeMessage);
                    Invoke(d, new object[] { sender, e });
                }
                catch (ObjectDisposedException) { }
            }
            else
            {
                ExchangeEnum ex = e.ExchangeMessage.Item1;
                string status = "Active";
                string msg = e.ExchangeMessage.Item2;

                UpdateListViewInfo(ex, status, msg);
            }
        }
        internal void Core_event_NewMessage(object sender, CoreEvents e)
        {
            if (this.InvokeRequired)
            {
                try
                {
                    coreEventDelegate d = new coreEventDelegate(Core_event_NewMessage);
                    Invoke(d, new object[] { sender, e });
                }
                catch (ObjectDisposedException) { }
            }
            else
            {
                switch (e.CoreMessage.Item1)
                {
                    case CoreMessageHeader.Message:
                        {
                            break;
                        }
                    case CoreMessageHeader.Error:
                        {
                            break;
                        }
                    case CoreMessageHeader.Success:
                        {
                            break;
                        }
                    case CoreMessageHeader.ProgressMessage:
                        {
                            if (e.CoreMessage.Item2 == "Loaded Markets")
                            {
                                CreateTickerViews();
                            }
                            break;
                        }
                    case CoreMessageHeader.ExchangeName:
                        {
                            break;
                        }
                    case CoreMessageHeader.MarketName:
                        {
                            break;
                        }
                    default:
                        break;
                }
                UpdateStatusLabel(e.CoreMessage.Item2);
            }
        }
        internal void Core_event_ApiStateChanged(object sender, CoreEvents e)
        {
            if (this.InvokeRequired)
            {
                try
                {
                    coreEventDelegate d = new coreEventDelegate(Core_event_ApiStateChanged);
                    Invoke(d, new object[] { sender, e });
                }
                catch (ObjectDisposedException) { }
            }
            else
            {
                switch (e.ApiStateChange.Item1)
                {
                    case ExchangeEnum.Bittrex:
                        {
                            if (e.ApiStateChange.Item2 == true)
                            {
                                ApiIndicator_Bittrex.BackColor = colourOk;
                            }
                            else
                            {
                                ApiIndicator_Bittrex.BackColor = colourError;
                            }
                            break;
                        }
                    case ExchangeEnum.BTCe:
                        {
                            if (e.ApiStateChange.Item2 == true)
                            {
                                ApiIndicator_BTCe.BackColor = colourOk;
                            }
                            else
                            {
                                ApiIndicator_BTCe.BackColor = colourError;
                            }
                            break;
                        }
                    case ExchangeEnum.Cryptsy:
                        {
                            if (e.ApiStateChange.Item2 == true)
                            {
                                ApiIndicator_Cryptsy.BackColor = colourOk;
                            }
                            else
                            {
                                ApiIndicator_Cryptsy.BackColor = colourError;
                            }
                            break;
                        }
                    case ExchangeEnum.Kraken:
                        {
                            if (e.ApiStateChange.Item2 == true)
                            {
                                ApiIndicator_Kraken.BackColor = colourOk;
                            }
                            else
                            {
                                ApiIndicator_Kraken.BackColor = colourError;
                            }
                            break;
                        }
                    case ExchangeEnum.MintPal:
                        {
                            if (e.ApiStateChange.Item2 == true)
                            {
                                ApiIndicator_MintPal.BackColor = colourOk;
                            }
                            else
                            {
                                ApiIndicator_MintPal.BackColor = colourError;
                            }
                            break;
                        }
                    case ExchangeEnum.Poloniex:
                        {
                            if (e.ApiStateChange.Item2 == true)
                            {
                                ApiIndicator_Poloniex.BackColor = colourOk;
                            }
                            else
                            {
                                ApiIndicator_Poloniex.BackColor = colourError;
                            }
                            break;
                        }
                    default:
                        break;
                }
                if (e.ApiStateChange.Item2 == true)
                {
                    if (!this.ActiveExchanges.Contains(e.ApiStateChange.Item1))
                        ActiveExchanges.Add(e.ApiStateChange.Item1);

                    string msg = string.Format("Connected: {0}", DateTime.Now.ToString());
                    UpdateListViewInfo(e.ApiStateChange.Item1, "Active", msg);
                }
                else
                {
                    if (this.ActiveExchanges.Contains(e.ApiStateChange.Item1))
                        ActiveExchanges.Remove(e.ApiStateChange.Item1);

                    string msg = string.Format("Down from: {0}", DateTime.Now.ToString());
                    UpdateListViewInfo(e.ApiStateChange.Item1, "Down", msg);
                }
            }
        }
        internal void Core_event_RateChanged(object sender, CoreEvents e)
        {
            if (this.InvokeRequired)
            {
                coreEventDelegate d = new coreEventDelegate(Core_event_RateChanged);
                try
                {
                    Invoke(d, new object[] { sender, e });
                }
                catch (ObjectDisposedException)
                { }
            }
            else
            {
                //If exchange isn't in rates list, add it.
                if (!MainRates.ContainsKey(e.NewRates.Item1))
                    MainRates.Add(e.NewRates.Item1, new Dictionary<string, Ticker>());

                //If market isn't in exchanges rate list, add it
                if (!MainRates[e.NewRates.Item1].ContainsKey(e.NewRates.Item2.MarketIdentity.StandardisedName))
                {
                    MainRates[e.NewRates.Item1].Add(e.NewRates.Item2.MarketIdentity.StandardisedName, e.NewRates.Item2);
                }
                //Otherwise adjust figure
                else
                {
                    MainRates[e.NewRates.Item1][e.NewRates.Item2.MarketIdentity.StandardisedName] = e.NewRates.Item2;
                }
                UpdateRateButtons();
            }
        }
        internal void Core_event_NewOrderBook(object sender, CoreEvents e)
        {
            if (this.InvokeRequired)
            {
                coreEventDelegate d = new coreEventDelegate(Core_event_NewOrderBook);
                try
                {
                    Invoke(d, new object[] { sender, e });
                }
                catch (ObjectDisposedException) { }
            }
            else
            {
                if (ActiveMarket != null)
                {
                    ActiveMarket.OrderBook = e.NewOrderBook.Item1;
                    ActiveMarket.TradeRecords = e.NewOrderBook.Item2;
                    LinkOrderBook();
                    UpdateSettingsLabels();
                    UpdateBalanceLabels();
                }
            }
        }
        internal void Core_event_NewBalances(object sender, CoreEvents e)
        {
            if (this.InvokeRequired)
            {
                coreEventDelegate d = new coreEventDelegate(Core_event_NewBalances);
                try
                {
                    Invoke(d, new object[] { sender, e });
                }
                catch (ObjectDisposedException) { }
            }
            else
            {
                var exRef = e.NewBalances.Item1;
                if (!BalanceTables.ContainsKey(exRef))
                    BalanceTables.Add(exRef, new HashSet<Balance>());

                BalanceTables[exRef] = e.NewBalances.Item2;
                UpdateStatusLabel("Balances Updated: " + exRef);

                if (exRef == ActiveExchangeName)
                {
                    LinkBalances();
                    GetBalances();
                }
            }
        }
        internal void Core_event_NewActiveOrders(object sender, CoreEvents e)
        {
            if (this.InvokeRequired)
            {
                coreEventDelegate d = new coreEventDelegate(Core_event_NewActiveOrders);
                try
                {
                    Invoke(d, new object[] { sender, e });
                }
                catch (ObjectDisposedException) { }
            }
            else
            {
                var exRef = e.NewActiveOrders.Item1;
                if (!ActiveOrderTables.ContainsKey(exRef))
                    ActiveOrderTables.Add(exRef, new HashSet<ActiveOrder>());

                ActiveOrderTables[exRef] = e.NewActiveOrders.Item2;
                UpdateStatusLabel("Active Orders Updated: " + exRef);
                if (exRef == ActiveExchangeName)
                    LinkActiveOrders();
            }
        }
        internal void Core_event_NewTickerRow(object sender, CoreEvents e)
        {
            try
            {
                if (this.InvokeRequired && !this.Disposing)
                {
                    coreEventDelegate d = new coreEventDelegate(Core_event_NewTickerRow);
                    try
                    {
                        Invoke(d, new object[] { sender, e });
                    }
                    catch (ObjectDisposedException)
                    { }
                }
                else
                {
                    TickerPanel.Ticker tick = panel_Tickers.Controls.OfType<TickerPanel.Ticker>().First(p => p.ExchangeName == e.NewTickerRow.Item1.ToString());
                    bool isChanged = false;

                    var tabQ = from t in TickerTables
                               where t.Key.TableName == e.NewTickerRow.Item1.ToString()
                               select t;

                    TickerViewTable tickTab = null;
                    if (tabQ.Count() > 0)
                        tickTab = tabQ.First().Key;

                    if (tickTab == null)
                    {
                        BindingSource binder = new BindingSource();
                        tickTab = new TickerViewTable(e.NewTickerRow.Item1.ToString());
                        binder.DataSource = tickTab;
                        TickerTables.Add(tickTab, binder);
                        tick.dgv.DataSource = binder;
                    }
                    else
                    {
                        if (!tickTab.Rows.Contains(e.NewTickerRow.Item2.Market))
                        {
                            TickerViewTableRow row = tickTab.GetNewRow();
                            row.BeginEdit();
                            row.ItemArray = e.NewTickerRow.Item2.ItemArray as object[];
                            row.EndEdit();
                            tickTab.Rows.Add(row);
                            tickTab.AcceptChanges();
                        }
                        else
                        {
                            TickerViewTableRow row = tickTab
                                .Rows
                                .Find(e.NewTickerRow.Item2.Market)
                                as TickerViewTableRow;

                            for (int ii = 0; ii < row.Table.Columns.Count; ii++)
                            {
                                if (row.Table.Columns[ii].ColumnName != "Exchange"
                                    && row.Table.Columns[ii].ColumnName != "Market"
                                    && row.Table.Columns[ii].ColumnName != "LastUpdated")
                                {
                                    if (row[ii].ToString() != e.NewTickerRow.Item2[ii].ToString())
                                    {
                                        row.BeginEdit();
                                        row[ii] = e.NewTickerRow.Item2[ii];
                                        row.EndEdit();
                                        isChanged = true;
                                    }
                                }
                            }
                            if (isChanged)
                            {
                                row.AcceptChanges();
                            }
                        }
                    }
                }
            }
            catch (InvalidOperationException) { }
        }
        internal void Core_event_AutoOrderPlaced(object sender, CoreEvents e)
        {
            if (this.InvokeRequired)
            {
                try
                {
                    coreEventDelegate d = new coreEventDelegate(Core_event_AutoOrderPlaced);
                    Invoke(d, new object[] { sender, e });
                }
                catch (ObjectDisposedException)
                {
                    MessageBox.Show("You are trying to quit while an order is being placed.");
                }
            }
            else
            {
                string msg = string.Format("Order Placed: {0}. OrderId {1}", e.ExchangeMessage.Item1, e.ExchangeMessage.Item2);
                UpdateStatusLabel(msg);
            }
        }
        internal void Core_event_ConditonalOrderListUpdated(object sender, CoreEvents e)
        {
            if (this.InvokeRequired)
            {
                try
                {
                    coreEventDelegate d = new coreEventDelegate(Core_event_ConditonalOrderListUpdated);
                    Invoke(d, new object[] { sender, e });
                }
                catch (ObjectDisposedException) { }
            }
            else
            {
                ConditionalOrderListBinder.DataSource = e.NewConditionalOrderList;
            }
        }

        private void tickerBinder_ListChanged(object sender, ListChangedEventArgs e)
        {
            BindingSource binder = sender as BindingSource;
            DataGridView dgv = panel_Tickers.Controls.OfType<TickerPanel.Ticker>().First(p => p.dgv.DataSource == binder).dgv;
            if (e.NewIndex >= 0)
            {
                FlashDgvCell(e, dgv);
            }
        }
        /// <summary>
        /// CHEWS LOTS OF CPU
        /// </summary>
        private void FlashDgvCell(ListChangedEventArgs e, DataGridView dgv)
        {
            string key = dgv.Name + e.NewIndex.ToString();
            if (!highlightList.Contains(key) && !this.Disposing)
            {
                highlightList.Add(key);

                var dRow = dgv.Rows[e.NewIndex];

                dgv.BeginInvoke((MethodInvoker)delegate()
                {
                    Timer tim = new Timer();
                    tim.Interval = 250;
                    tim.Tick += (s, ea) =>
                    {
                        int fRi = dgv.FirstDisplayedCell.RowIndex;
                        int topBound = fRi + 8;
                        int lowBound = fRi - 3;

                        if (dRow.Index < topBound && dRow.Index > lowBound)
                        {
                            if (dRow.DefaultCellStyle.BackColor == colourTick)
                                dRow.DefaultCellStyle.BackColor = colourTickDefault;
                            else
                                dRow.DefaultCellStyle.BackColor = colourTick;
                        }
                    };
                    Timer timTim = new Timer();
                    timTim.Interval = 3000;
                    timTim.Tick += (s2, ea2) =>
                    {
                        tim.Stop();
                        timTim.Stop();
                        tim.Dispose();
                        timTim.Dispose();
                        highlightList.Remove(key);
                        dRow.DefaultCellStyle.BackColor = colourTickDefault;
                    };
                    tim.Start();
                    timTim.Start();
                });
            }
        }
        #endregion

        #region Change Update Period
        void ticker_event_PeriodChanged(object sender, TickerPanel.TickerEvent e)
        {
            ExchangeEnum ex = GeneralTools.ParseEnum<ExchangeEnum>(e.updateChanged[0]);
            int period = Convert.ToInt32(e.updateChanged[1]);
            ChangeUpdatePeriod(ex, period);
        }
        void tb_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                var tb = sender as TextBox;
                string[] parts = tb.Name.Split('_');
                var enu = GeneralTools.ParseEnum<ExchangeEnum>(parts[1]);
                try
                {
                    int period = Int32.Parse(tb.Text);
                    ChangeUpdatePeriod(enu, period);
                }
                catch
                {
                    MessageBox.Show("That is an invalid number.");
                    tb.Text = defaultGaps[enu];
                }
            }
        }
        internal void ChangeUpdatePeriod(ExchangeEnum exch, int period)
        {
            String msg = String.Empty;
            bool result = Core.ChangeUpdatePeriod(exch, period);

            if (result)
                msg = String.Format("Update period is now {0} seconds.", period);
            else
                msg = "Number not valid.";

            UpdateStatusLabel(msg);
        }
        #endregion

        #region Coin Settings
        private void textBox_MinPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                ActiveMarket.MinPrice = Convert.ToDouble(textBox_MinPrice.Text);
        }
        private void textBox_MinVolume_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                ActiveMarket.MinQuant = Convert.ToDouble(textBox_MinVolume.Text);
        }
        private void textBoxMaxPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                ActiveMarket.MaxPrice = Convert.ToDouble(textBoxMaxPrice.Text);
        }
        private void textBox_Decimals_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                ActiveMarket.PriceDecimals = Convert.ToInt32(textBox_Decimals.Text);
        }
        #endregion

        #region Conditional Orders
        private void listBox_CondOrders_SelectedIndexChanged(object sender, EventArgs e)
        {
            button_CondOrdersCancel.Enabled = true;
        }

        private void updateCondOrders(object sender, EventArgs e)
        {
            CalculateConditionalOrderTotal();
            GenerateConditionDescription();
        }
        private void updateCondDescrip(object sender, EventArgs e)
        {
            GenerateConditionDescription();
        }
        
        private void CalculateConditionalOrderTotal()
        {
            double quant = Convert.ToDouble(numericUpDown_CondQuant.Value);
            decimal prc = numericUpDown_CondOrderPrice.Value;
            decimal fee;
            double currentFee = GetCurrentFee(quant);
            decimal tot = 0;

            if (radioButton_CondBuy.Checked)
            {
                tot = Calcs.Buy_HowMuch(quant, currentFee, prc, out fee);
            }
            else
            {
                tot = Calcs.Sell_HowMuch(quant, currentFee, prc);
            }

            string labTxt = string.Format("Total: {0:N8}", tot);
            label_CondTot.Text = labTxt;
        }
        private void GenerateConditionDescription()
        {
            string odrType = string.Empty;
            string cond = string.Empty;

            if (radioButton_CondBuy.Checked)
                odrType = "buy order";
            else if (radioButton_CondSell.Checked)
                odrType = "sell order";

            if (radioButton_PriceAbove.Checked)
                cond = "raises above";
            else if (radioButton_PriceBelow.Checked)
                cond = "drops below";

            string msg = string.Format("If price {0} {1}, place {2} for quantity of {3} at a price of {4}.", cond, numericUpDown_CondTrigger.Value, odrType, numericUpDown_CondQuant.Value, numericUpDown_CondOrderPrice.Value);

            richTextBox_OrderDecrip.Text = msg;
        }
        private void button_CondOrdersSave_Click(object sender, EventArgs e)
        {
            double availBal = 0;
            double wanted = 0;
            bool possible = false;
            switch (radioButton_CondBuy.Checked)
            {
                case true:
                    string num = label_CondTot.Text.Split(':')[1].Trim();
                    availBal = Convert.ToDouble(label_CurrencyBalance.Text);
                    wanted = Convert.ToDouble(num);
                    break;
                case false:
                    availBal = Convert.ToDouble(label_CommodityBalance.Text);
                    wanted = Convert.ToDouble(numericUpDown_CondQuant.Value);
                    break;
                default:
                    break;
            }
            if (wanted <= availBal)
                possible = true;

            if (possible)
            {
                #region Save Order
                if (ActiveMarket != null)
                {
                    ///Check all boxes have values
                    if (numericUpDown_CondOrderPrice.Value * numericUpDown_CondQuant.Value * numericUpDown_CondTrigger.Value > 0)
                    {

                        ///Lazy loading of conditional order binder to save loading in demo mode
                        if (ConditionalOrderListBinder == null)
                        {
                            ConditionalOrderListBinder = new BindingSource();
                            listBox_CondOrders.DataSource = ConditionalOrderListBinder;
                        }

                        TradeObject tO;

                        double price;
                        double quant;
                        double trigger;
                        OrderType type;
                        ConditionalType cType;

                        price = Convert.ToDouble(numericUpDown_CondOrderPrice.Value);
                        quant = Convert.ToDouble(numericUpDown_CondQuant.Value);
                        trigger = Convert.ToDouble(numericUpDown_CondTrigger.Value);

                        if (radioButton_CondBuy.Checked)
                            type = OrderType.Bid;
                        else if (radioButton_CondSell.Checked)
                            type = OrderType.Ask;
                        else
                        {
                            UpdateStatusLabel("No order type selected.");
                            return;
                        }

                        if (radioButton_PriceAbove.Checked)
                            cType = ConditionalType.LimitHigh;
                        else if (radioButton_PriceBelow.Checked)
                            cType = ConditionalType.LimitLow;
                        else
                        {
                            UpdateStatusLabel("No condition selected.");
                            return;
                        }
                        tO = new TradeObject(ActiveMarket.MarketIdentity, type, price, quant, cType, trigger);
                        bool success = Core.AddConditionalOrder(ActiveExchangeName, tO);

                        if (success)
                            UpdateStatusLabel("Order stored.");
                        else
                            UpdateStatusLabel("Conflicting order already exists.");

                        return;
                    }
                    else
                    {
                        UpdateStatusLabel("Please make sure a price, quantity and trigger are set.");
                        return;
                    }
                }
                #endregion
            }
            else MessageBox.Show("You do not have the required funds to make this transaction.");
        }
        private void button_CondOrdersCancel_Click(object sender, EventArgs e)
        {
            var tO = listBox_CondOrders.SelectedItem as TradeObject;
            String msg = String.Format("{0}\n{1}", tO.Market.ExchangeName, tO.Market.StandardisedName);

            DialogResult dr = MessageBox.Show(msg, "Remove Order?", MessageBoxButtons.YesNo);
            switch (dr)
            {
                case DialogResult.Yes:
                    {
                        try
                        {
                            Core.RemoveConditionalOrder(tO);
                            msg = "Order cancelled";
                            button_CondOrdersCancel.Enabled = false;
                        }
                        catch
                        {
                            msg = "Error removing order";
                        }
                        break;
                    }
                case DialogResult.No:
                    {
                        msg = "Cancelled the cancelling";
                        break;
                    }
            }
            UpdateStatusLabel(msg);
        }
        #endregion

        #region Toolbar buttons
        private void button_EditKeys_Click(object sender, EventArgs e)
        {
            Core.EditUserKeys();
        }
        private async void ApiIndicator_Cryptsy_Click(object sender, EventArgs e)
        {
            try
            {
                await Core.ForceUpdate(ExchangeEnum.Cryptsy, "All");
            }
            catch
            {
                UpdateStatusLabel("Can't update Cryptsy.");
            }
        }
        private async void ApiIndicator_BTCe_Click(object sender, EventArgs e)
        {
            try
            {
                await Core.ForceUpdate(ExchangeEnum.BTCe, "All");
            }
            catch
            {
                UpdateStatusLabel("Can't update BTCe.");
            }
        }
        private async void ApiIndicator_Kraken_Click(object sender, EventArgs e)
        {
            try
            {
                await Core.ForceUpdate(ExchangeEnum.Kraken, "All");
            }
            catch
            {
                UpdateStatusLabel("Can't update Kraken.");
            }
        }
        private async void ApiIndicator_MintPal_Click(object sender, EventArgs e)
        {
            try
            {
                await Core.ForceUpdate(ExchangeEnum.MintPal, "All");
            }
            catch
            {
                UpdateStatusLabel("Can't update MintPal.");
            }
        }
        private async void ApiIndicator_Poloniex_Click(object sender, EventArgs e)
        {
            try
            {
                await Core.ForceUpdate(ExchangeEnum.Poloniex, "All");
            }
            catch
            {
                UpdateStatusLabel("Can't update Poloniex.");
            }
        }
        private async void ApiIndicator_Bittrex_Click(object sender, EventArgs e)
        {
            try
            {
                await Core.ForceUpdate(ExchangeEnum.Bittrex, "All");
            }
            catch
            {
                UpdateStatusLabel("Can't update Bittrex.");
            }
        }
        #endregion

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Core.running = false;
            toolStripLabel_Status.Text = "Closing";
            Core.CloseCore();
        }

    }
}
