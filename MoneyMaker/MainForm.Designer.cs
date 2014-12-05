namespace Objects
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.toolStripContainer_Main = new System.Windows.Forms.ToolStripContainer();
            this.toolStrip_Status = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel_Status = new System.Windows.Forms.ToolStripLabel();
            this.tableLayoutPanel_ChartArea = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label_MinPrice = new System.Windows.Forms.Label();
            this.textBox_MinPrice = new System.Windows.Forms.TextBox();
            this.textBox_MinVolume = new System.Windows.Forms.TextBox();
            this.textBoxMaxPrice = new System.Windows.Forms.TextBox();
            this.textBox_Decimals = new System.Windows.Forms.TextBox();
            this.label_MinVolume = new System.Windows.Forms.Label();
            this.label_MaxPrice = new System.Windows.Forms.Label();
            this.label_Decimals = new System.Windows.Forms.Label();
            this.label_Spread = new System.Windows.Forms.Label();
            this.label_Market = new System.Windows.Forms.Label();
            this.quickChart1 = new StockCharts.QuickChart();
            this.dataGridView_TradeRecs = new System.Windows.Forms.DataGridView();
            this.Column_TradeHistory_Time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_TradeHistory_Price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_TradeHistory_Quant = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_TradeHistory_Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel_Tickers = new System.Windows.Forms.Panel();
            this.panel_Main = new System.Windows.Forms.Panel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.dataGridView_OrderBookBids = new System.Windows.Forms.DataGridView();
            this.dgv_Bids_Price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgv_Bids_Quantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bidOrderBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.button_Buy = new System.Windows.Forms.Button();
            this.button_Sell = new System.Windows.Forms.Button();
            this.textBox_BuyPrice = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_SellPrice = new System.Windows.Forms.NumericUpDown();
            this.textBox_BuyQuant = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_SellQuant = new System.Windows.Forms.NumericUpDown();
            this.textBox_BuyFee = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_SellFee = new System.Windows.Forms.TextBox();
            this.textBox_BuyTotal = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox_SellTotal = new System.Windows.Forms.TextBox();
            this.label_CurrencyBalance = new System.Windows.Forms.Label();
            this.label_CommodityBalance = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label_MarketID = new System.Windows.Forms.Label();
            this.dataGridView_OrderBookAsks = new System.Windows.Forms.DataGridView();
            this.dgv_Asks_Quantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgv_Asks_Price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.askOrderBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.button_HideZeros = new System.Windows.Forms.Button();
            this.dataGridView_Balances = new System.Windows.Forms.DataGridView();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.availableDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.heldDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.totalDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.balanceBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.textBox_CancelOrderId = new System.Windows.Forms.TextBox();
            this.button_CancelOrder = new System.Windows.Forms.Button();
            this.dataGridView_ActiveOrders = new System.Windows.Forms.DataGridView();
            this.orderIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.priceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.quantityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.orderTypeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.remainingDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.percentFilledDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.activeOrderBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.groupBox_CondOrders = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.radioButton_CondSell = new System.Windows.Forms.RadioButton();
            this.radioButton_CondBuy = new System.Windows.Forms.RadioButton();
            this.richTextBox_OrderDecrip = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDown_CondTrigger = new System.Windows.Forms.NumericUpDown();
            this.button_CondOrdersCancel = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.button_CondOrdersSave = new System.Windows.Forms.Button();
            this.numericUpDown_CondOrderPrice = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown_CondQuant = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.radioButton_PriceBelow = new System.Windows.Forms.RadioButton();
            this.radioButton_PriceAbove = new System.Windows.Forms.RadioButton();
            this.listBox_CondOrders = new System.Windows.Forms.ListBox();
            this.label_CondTot = new System.Windows.Forms.Label();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.label_RatesExchangeName = new System.Windows.Forms.Label();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.listView1 = new System.Windows.Forms.ListView();
            this.lvC_Name = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lvC_Status = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lvC_Info = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.donationForm1 = new Donate.DonationForm();
            this.toolStrip_Settings = new System.Windows.Forms.ToolStrip();
            this.toolStripButton_EditKeys = new System.Windows.Forms.ToolStripButton();
            this.toolStrip_Apis = new System.Windows.Forms.ToolStrip();
            this.ApiIndicator_Bittrex = new System.Windows.Forms.ToolStripButton();
            this.ApiIndicator_BTCe = new System.Windows.Forms.ToolStripButton();
            this.ApiIndicator_Cryptsy = new System.Windows.Forms.ToolStripButton();
            this.ApiIndicator_Kraken = new System.Windows.Forms.ToolStripButton();
            this.ApiIndicator_BitCoinCoId = new System.Windows.Forms.ToolStripButton();
            this.ApiIndicator_Poloniex = new System.Windows.Forms.ToolStripButton();
            this.toolStripContainer_Main.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainer_Main.ContentPanel.SuspendLayout();
            this.toolStripContainer_Main.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer_Main.SuspendLayout();
            this.toolStrip_Status.SuspendLayout();
            this.tableLayoutPanel_ChartArea.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_TradeRecs)).BeginInit();
            this.panel_Main.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_OrderBookBids)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bidOrderBindingSource)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textBox_BuyPrice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBox_SellPrice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBox_BuyQuant)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBox_SellQuant)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_OrderBookAsks)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.askOrderBindingSource)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Balances)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.balanceBindingSource)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_ActiveOrders)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.activeOrderBindingSource)).BeginInit();
            this.tabPage3.SuspendLayout();
            this.groupBox_CondOrders.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_CondTrigger)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_CondOrderPrice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_CondQuant)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tabPage6.SuspendLayout();
            this.toolStrip_Settings.SuspendLayout();
            this.toolStrip_Apis.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripContainer_Main
            // 
            // 
            // toolStripContainer_Main.BottomToolStripPanel
            // 
            this.toolStripContainer_Main.BottomToolStripPanel.Controls.Add(this.toolStrip_Status);
            // 
            // toolStripContainer_Main.ContentPanel
            // 
            this.toolStripContainer_Main.ContentPanel.Controls.Add(this.tableLayoutPanel_ChartArea);
            this.toolStripContainer_Main.ContentPanel.Controls.Add(this.panel_Tickers);
            this.toolStripContainer_Main.ContentPanel.Controls.Add(this.panel_Main);
            this.toolStripContainer_Main.ContentPanel.Size = new System.Drawing.Size(1036, 680);
            this.toolStripContainer_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer_Main.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer_Main.Name = "toolStripContainer_Main";
            this.toolStripContainer_Main.Size = new System.Drawing.Size(1036, 730);
            this.toolStripContainer_Main.TabIndex = 0;
            this.toolStripContainer_Main.Text = "toolStripContainer1";
            // 
            // toolStripContainer_Main.TopToolStripPanel
            // 
            this.toolStripContainer_Main.TopToolStripPanel.Controls.Add(this.toolStrip_Settings);
            this.toolStripContainer_Main.TopToolStripPanel.Controls.Add(this.toolStrip_Apis);
            // 
            // toolStrip_Status
            // 
            this.toolStrip_Status.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip_Status.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel_Status});
            this.toolStrip_Status.Location = new System.Drawing.Point(3, 0);
            this.toolStrip_Status.Name = "toolStrip_Status";
            this.toolStrip_Status.Size = new System.Drawing.Size(51, 25);
            this.toolStrip_Status.TabIndex = 0;
            // 
            // toolStripLabel_Status
            // 
            this.toolStripLabel_Status.Name = "toolStripLabel_Status";
            this.toolStripLabel_Status.Size = new System.Drawing.Size(39, 22);
            this.toolStripLabel_Status.Text = "Status";
            // 
            // tableLayoutPanel_ChartArea
            // 
            this.tableLayoutPanel_ChartArea.ColumnCount = 3;
            this.tableLayoutPanel_ChartArea.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 65F));
            this.tableLayoutPanel_ChartArea.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel_ChartArea.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel_ChartArea.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel_ChartArea.Controls.Add(this.quickChart1, 0, 1);
            this.tableLayoutPanel_ChartArea.Controls.Add(this.dataGridView_TradeRecs, 0, 2);
            this.tableLayoutPanel_ChartArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel_ChartArea.Location = new System.Drawing.Point(400, 0);
            this.tableLayoutPanel_ChartArea.Name = "tableLayoutPanel_ChartArea";
            this.tableLayoutPanel_ChartArea.RowCount = 3;
            this.tableLayoutPanel_ChartArea.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel_ChartArea.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 65F));
            this.tableLayoutPanel_ChartArea.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel_ChartArea.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel_ChartArea.Size = new System.Drawing.Size(636, 430);
            this.tableLayoutPanel_ChartArea.TabIndex = 2;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset;
            this.tableLayoutPanel2.ColumnCount = 5;
            this.tableLayoutPanel_ChartArea.SetColumnSpan(this.tableLayoutPanel2, 3);
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.Controls.Add(this.label_MinPrice, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.textBox_MinPrice, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.textBox_MinVolume, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.textBoxMaxPrice, 3, 1);
            this.tableLayoutPanel2.Controls.Add(this.textBox_Decimals, 4, 1);
            this.tableLayoutPanel2.Controls.Add(this.label_MinVolume, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.label_MaxPrice, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.label_Decimals, 4, 0);
            this.tableLayoutPanel2.Controls.Add(this.label_Spread, 2, 1);
            this.tableLayoutPanel2.Controls.Add(this.label_Market, 2, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(7, 7);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(7);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(622, 50);
            this.tableLayoutPanel2.TabIndex = 2;
            // 
            // label_MinPrice
            // 
            this.label_MinPrice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label_MinPrice.AutoSize = true;
            this.label_MinPrice.Location = new System.Drawing.Point(5, 6);
            this.label_MinPrice.Name = "label_MinPrice";
            this.label_MinPrice.Size = new System.Drawing.Size(116, 13);
            this.label_MinPrice.TabIndex = 3;
            this.label_MinPrice.Text = "Min Price";
            this.label_MinPrice.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBox_MinPrice
            // 
            this.textBox_MinPrice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_MinPrice.Location = new System.Drawing.Point(2, 26);
            this.textBox_MinPrice.Margin = new System.Windows.Forms.Padding(0);
            this.textBox_MinPrice.Name = "textBox_MinPrice";
            this.textBox_MinPrice.Size = new System.Drawing.Size(122, 20);
            this.textBox_MinPrice.TabIndex = 0;
            this.textBox_MinPrice.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_MinPrice_KeyPress);
            // 
            // textBox_MinVolume
            // 
            this.textBox_MinVolume.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_MinVolume.Location = new System.Drawing.Point(126, 26);
            this.textBox_MinVolume.Margin = new System.Windows.Forms.Padding(0);
            this.textBox_MinVolume.Name = "textBox_MinVolume";
            this.textBox_MinVolume.Size = new System.Drawing.Size(122, 20);
            this.textBox_MinVolume.TabIndex = 0;
            this.textBox_MinVolume.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_MinVolume_KeyPress);
            // 
            // textBoxMaxPrice
            // 
            this.textBoxMaxPrice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxMaxPrice.Location = new System.Drawing.Point(374, 26);
            this.textBoxMaxPrice.Margin = new System.Windows.Forms.Padding(0);
            this.textBoxMaxPrice.Name = "textBoxMaxPrice";
            this.textBoxMaxPrice.Size = new System.Drawing.Size(122, 20);
            this.textBoxMaxPrice.TabIndex = 0;
            this.textBoxMaxPrice.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxMaxPrice_KeyPress);
            // 
            // textBox_Decimals
            // 
            this.textBox_Decimals.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_Decimals.Location = new System.Drawing.Point(498, 26);
            this.textBox_Decimals.Margin = new System.Windows.Forms.Padding(0);
            this.textBox_Decimals.Name = "textBox_Decimals";
            this.textBox_Decimals.Size = new System.Drawing.Size(122, 20);
            this.textBox_Decimals.TabIndex = 0;
            this.textBox_Decimals.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_Decimals_KeyPress);
            // 
            // label_MinVolume
            // 
            this.label_MinVolume.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label_MinVolume.AutoSize = true;
            this.label_MinVolume.Location = new System.Drawing.Point(129, 6);
            this.label_MinVolume.Name = "label_MinVolume";
            this.label_MinVolume.Size = new System.Drawing.Size(116, 13);
            this.label_MinVolume.TabIndex = 3;
            this.label_MinVolume.Text = "Min Volume";
            this.label_MinVolume.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_MaxPrice
            // 
            this.label_MaxPrice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label_MaxPrice.AutoSize = true;
            this.label_MaxPrice.Location = new System.Drawing.Point(377, 6);
            this.label_MaxPrice.Name = "label_MaxPrice";
            this.label_MaxPrice.Size = new System.Drawing.Size(116, 13);
            this.label_MaxPrice.TabIndex = 3;
            this.label_MaxPrice.Text = "Max Price";
            this.label_MaxPrice.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_Decimals
            // 
            this.label_Decimals.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label_Decimals.AutoSize = true;
            this.label_Decimals.Location = new System.Drawing.Point(501, 6);
            this.label_Decimals.Name = "label_Decimals";
            this.label_Decimals.Size = new System.Drawing.Size(116, 13);
            this.label_Decimals.TabIndex = 3;
            this.label_Decimals.Text = "Price Decimals";
            this.label_Decimals.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_Spread
            // 
            this.label_Spread.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label_Spread.AutoSize = true;
            this.label_Spread.Location = new System.Drawing.Point(253, 30);
            this.label_Spread.Name = "label_Spread";
            this.label_Spread.Size = new System.Drawing.Size(116, 13);
            this.label_Spread.TabIndex = 3;
            this.label_Spread.Text = "Spread";
            this.label_Spread.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_Market
            // 
            this.label_Market.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label_Market.AutoSize = true;
            this.label_Market.Location = new System.Drawing.Point(253, 6);
            this.label_Market.Name = "label_Market";
            this.label_Market.Size = new System.Drawing.Size(116, 13);
            this.label_Market.TabIndex = 3;
            this.label_Market.Text = "Market";
            this.label_Market.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // quickChart1
            // 
            this.tableLayoutPanel_ChartArea.SetColumnSpan(this.quickChart1, 3);
            this.quickChart1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.quickChart1.Location = new System.Drawing.Point(3, 67);
            this.quickChart1.Name = "quickChart1";
            this.quickChart1.Size = new System.Drawing.Size(630, 273);
            this.quickChart1.TabIndex = 11;
            // 
            // dataGridView_TradeRecs
            // 
            this.dataGridView_TradeRecs.AllowUserToAddRows = false;
            this.dataGridView_TradeRecs.AllowUserToDeleteRows = false;
            this.dataGridView_TradeRecs.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView_TradeRecs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_TradeRecs.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column_TradeHistory_Time,
            this.Column_TradeHistory_Price,
            this.Column_TradeHistory_Quant,
            this.Column_TradeHistory_Type});
            this.tableLayoutPanel_ChartArea.SetColumnSpan(this.dataGridView_TradeRecs, 3);
            this.dataGridView_TradeRecs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_TradeRecs.Location = new System.Drawing.Point(3, 346);
            this.dataGridView_TradeRecs.MultiSelect = false;
            this.dataGridView_TradeRecs.Name = "dataGridView_TradeRecs";
            this.dataGridView_TradeRecs.ReadOnly = true;
            this.dataGridView_TradeRecs.RowHeadersVisible = false;
            this.dataGridView_TradeRecs.ShowEditingIcon = false;
            this.dataGridView_TradeRecs.Size = new System.Drawing.Size(630, 81);
            this.dataGridView_TradeRecs.TabIndex = 12;
            // 
            // Column_TradeHistory_Time
            // 
            this.Column_TradeHistory_Time.DataPropertyName = "TradeTime";
            dataGridViewCellStyle1.Format = "g";
            dataGridViewCellStyle1.NullValue = null;
            this.Column_TradeHistory_Time.DefaultCellStyle = dataGridViewCellStyle1;
            this.Column_TradeHistory_Time.HeaderText = "Time";
            this.Column_TradeHistory_Time.Name = "Column_TradeHistory_Time";
            this.Column_TradeHistory_Time.ReadOnly = true;
            // 
            // Column_TradeHistory_Price
            // 
            this.Column_TradeHistory_Price.DataPropertyName = "Price";
            dataGridViewCellStyle2.Format = "N8";
            this.Column_TradeHistory_Price.DefaultCellStyle = dataGridViewCellStyle2;
            this.Column_TradeHistory_Price.HeaderText = "Price";
            this.Column_TradeHistory_Price.Name = "Column_TradeHistory_Price";
            this.Column_TradeHistory_Price.ReadOnly = true;
            // 
            // Column_TradeHistory_Quant
            // 
            this.Column_TradeHistory_Quant.DataPropertyName = "Quantity";
            dataGridViewCellStyle3.Format = "N8";
            this.Column_TradeHistory_Quant.DefaultCellStyle = dataGridViewCellStyle3;
            this.Column_TradeHistory_Quant.HeaderText = "Quant";
            this.Column_TradeHistory_Quant.Name = "Column_TradeHistory_Quant";
            this.Column_TradeHistory_Quant.ReadOnly = true;
            // 
            // Column_TradeHistory_Type
            // 
            this.Column_TradeHistory_Type.DataPropertyName = "Type";
            this.Column_TradeHistory_Type.HeaderText = "Type";
            this.Column_TradeHistory_Type.Name = "Column_TradeHistory_Type";
            this.Column_TradeHistory_Type.ReadOnly = true;
            // 
            // panel_Tickers
            // 
            this.panel_Tickers.AutoScroll = true;
            this.panel_Tickers.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel_Tickers.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel_Tickers.Location = new System.Drawing.Point(0, 0);
            this.panel_Tickers.Name = "panel_Tickers";
            this.panel_Tickers.Size = new System.Drawing.Size(400, 430);
            this.panel_Tickers.TabIndex = 1;
            // 
            // panel_Main
            // 
            this.panel_Main.Controls.Add(this.tableLayoutPanel3);
            this.panel_Main.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel_Main.Location = new System.Drawing.Point(0, 430);
            this.panel_Main.Name = "panel_Main";
            this.panel_Main.Size = new System.Drawing.Size(1036, 250);
            this.panel_Main.TabIndex = 0;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 66F));
            this.tableLayoutPanel3.Controls.Add(this.groupBox1, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.tabControl1, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.Size = new System.Drawing.Size(1036, 250);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanel4);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(355, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(678, 244);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Order Book";
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 3;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel4.Controls.Add(this.dataGridView_OrderBookBids, 2, 0);
            this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel1, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.dataGridView_OrderBookAsks, 0, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 2;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(672, 225);
            this.tableLayoutPanel4.TabIndex = 0;
            // 
            // dataGridView_OrderBookBids
            // 
            this.dataGridView_OrderBookBids.AllowUserToAddRows = false;
            this.dataGridView_OrderBookBids.AllowUserToDeleteRows = false;
            this.dataGridView_OrderBookBids.AllowUserToResizeRows = false;
            this.dataGridView_OrderBookBids.AutoGenerateColumns = false;
            this.dataGridView_OrderBookBids.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView_OrderBookBids.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_OrderBookBids.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgv_Bids_Price,
            this.dgv_Bids_Quantity});
            this.dataGridView_OrderBookBids.DataSource = this.bidOrderBindingSource;
            this.dataGridView_OrderBookBids.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_OrderBookBids.Location = new System.Drawing.Point(472, 3);
            this.dataGridView_OrderBookBids.MultiSelect = false;
            this.dataGridView_OrderBookBids.Name = "dataGridView_OrderBookBids";
            this.dataGridView_OrderBookBids.ReadOnly = true;
            this.dataGridView_OrderBookBids.RowHeadersVisible = false;
            this.tableLayoutPanel4.SetRowSpan(this.dataGridView_OrderBookBids, 2);
            this.dataGridView_OrderBookBids.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridView_OrderBookBids.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView_OrderBookBids.ShowEditingIcon = false;
            this.dataGridView_OrderBookBids.Size = new System.Drawing.Size(197, 219);
            this.dataGridView_OrderBookBids.TabIndex = 2;
            this.dataGridView_OrderBookBids.TabStop = false;
            this.dataGridView_OrderBookBids.VirtualMode = true;
            this.dataGridView_OrderBookBids.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_OrderBookBids_CellClick);
            // 
            // dgv_Bids_Price
            // 
            this.dgv_Bids_Price.DataPropertyName = "Price";
            dataGridViewCellStyle4.Format = "N8";
            this.dgv_Bids_Price.DefaultCellStyle = dataGridViewCellStyle4;
            this.dgv_Bids_Price.HeaderText = "Price";
            this.dgv_Bids_Price.Name = "dgv_Bids_Price";
            this.dgv_Bids_Price.ReadOnly = true;
            // 
            // dgv_Bids_Quantity
            // 
            this.dgv_Bids_Quantity.DataPropertyName = "Quantity";
            dataGridViewCellStyle5.Format = "N8";
            this.dgv_Bids_Quantity.DefaultCellStyle = dataGridViewCellStyle5;
            this.dgv_Bids_Quantity.HeaderText = "Quantity";
            this.dgv_Bids_Quantity.Name = "dgv_Bids_Quantity";
            this.dgv_Bids_Quantity.ReadOnly = true;
            // 
            // bidOrderBindingSource
            // 
            this.bidOrderBindingSource.DataSource = typeof(Objects.Order);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel1.Controls.Add(this.button_Buy, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.button_Sell, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.textBox_BuyPrice, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label2, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.textBox_SellPrice, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.textBox_BuyQuant, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label3, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.textBox_SellQuant, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.textBox_BuyFee, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label4, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.textBox_SellFee, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.textBox_BuyTotal, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.label5, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.textBox_SellTotal, 2, 4);
            this.tableLayoutPanel1.Controls.Add(this.label_CurrencyBalance, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.label_CommodityBalance, 2, 5);
            this.tableLayoutPanel1.Controls.Add(this.label7, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.label_MarketID, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(204, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.19544F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.28665F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.28665F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.28665F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.28665F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.65798F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(262, 174);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // button_Buy
            // 
            this.button_Buy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Buy.Location = new System.Drawing.Point(3, 6);
            this.button_Buy.Name = "button_Buy";
            this.button_Buy.Size = new System.Drawing.Size(98, 23);
            this.button_Buy.TabIndex = 0;
            this.button_Buy.Text = "Buy";
            this.button_Buy.UseVisualStyleBackColor = true;
            this.button_Buy.Click += new System.EventHandler(this.button_Buy_Click);
            // 
            // button_Sell
            // 
            this.button_Sell.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Sell.Location = new System.Drawing.Point(159, 6);
            this.button_Sell.Name = "button_Sell";
            this.button_Sell.Size = new System.Drawing.Size(100, 23);
            this.button_Sell.TabIndex = 2;
            this.button_Sell.Text = "Sell";
            this.button_Sell.UseVisualStyleBackColor = true;
            this.button_Sell.Click += new System.EventHandler(this.button_Sell_Click);
            // 
            // textBox_BuyPrice
            // 
            this.textBox_BuyPrice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_BuyPrice.Location = new System.Drawing.Point(1, 36);
            this.textBox_BuyPrice.Margin = new System.Windows.Forms.Padding(1);
            this.textBox_BuyPrice.Name = "textBox_BuyPrice";
            this.textBox_BuyPrice.Size = new System.Drawing.Size(102, 20);
            this.textBox_BuyPrice.TabIndex = 3;
            this.textBox_BuyPrice.ValueChanged += new System.EventHandler(this.textBox_BuyPrice_ValueChanged);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(107, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Price";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBox_SellPrice
            // 
            this.textBox_SellPrice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_SellPrice.Location = new System.Drawing.Point(157, 36);
            this.textBox_SellPrice.Margin = new System.Windows.Forms.Padding(1);
            this.textBox_SellPrice.Name = "textBox_SellPrice";
            this.textBox_SellPrice.Size = new System.Drawing.Size(104, 20);
            this.textBox_SellPrice.TabIndex = 5;
            this.textBox_SellPrice.ValueChanged += new System.EventHandler(this.textBox_SellPrice_ValueChanged);
            // 
            // textBox_BuyQuant
            // 
            this.textBox_BuyQuant.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_BuyQuant.Location = new System.Drawing.Point(1, 64);
            this.textBox_BuyQuant.Margin = new System.Windows.Forms.Padding(1);
            this.textBox_BuyQuant.Name = "textBox_BuyQuant";
            this.textBox_BuyQuant.Size = new System.Drawing.Size(102, 20);
            this.textBox_BuyQuant.TabIndex = 6;
            this.textBox_BuyQuant.ValueChanged += new System.EventHandler(this.textBox_BuyPrice_ValueChanged);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(107, 70);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Volume";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBox_SellQuant
            // 
            this.textBox_SellQuant.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_SellQuant.Location = new System.Drawing.Point(157, 64);
            this.textBox_SellQuant.Margin = new System.Windows.Forms.Padding(1);
            this.textBox_SellQuant.Name = "textBox_SellQuant";
            this.textBox_SellQuant.Size = new System.Drawing.Size(104, 20);
            this.textBox_SellQuant.TabIndex = 8;
            this.textBox_SellQuant.ValueChanged += new System.EventHandler(this.textBox_SellPrice_ValueChanged);
            // 
            // textBox_BuyFee
            // 
            this.textBox_BuyFee.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_BuyFee.Location = new System.Drawing.Point(3, 94);
            this.textBox_BuyFee.Name = "textBox_BuyFee";
            this.textBox_BuyFee.Size = new System.Drawing.Size(98, 20);
            this.textBox_BuyFee.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(107, 98);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Fee";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBox_SellFee
            // 
            this.textBox_SellFee.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_SellFee.Location = new System.Drawing.Point(159, 94);
            this.textBox_SellFee.Name = "textBox_SellFee";
            this.textBox_SellFee.Size = new System.Drawing.Size(100, 20);
            this.textBox_SellFee.TabIndex = 11;
            // 
            // textBox_BuyTotal
            // 
            this.textBox_BuyTotal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_BuyTotal.Location = new System.Drawing.Point(3, 122);
            this.textBox_BuyTotal.Name = "textBox_BuyTotal";
            this.textBox_BuyTotal.Size = new System.Drawing.Size(98, 20);
            this.textBox_BuyTotal.TabIndex = 12;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(107, 126);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Total";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBox_SellTotal
            // 
            this.textBox_SellTotal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_SellTotal.Location = new System.Drawing.Point(159, 122);
            this.textBox_SellTotal.Name = "textBox_SellTotal";
            this.textBox_SellTotal.Size = new System.Drawing.Size(100, 20);
            this.textBox_SellTotal.TabIndex = 14;
            // 
            // label_CurrencyBalance
            // 
            this.label_CurrencyBalance.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label_CurrencyBalance.AutoSize = true;
            this.label_CurrencyBalance.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label_CurrencyBalance.Location = new System.Drawing.Point(3, 154);
            this.label_CurrencyBalance.Name = "label_CurrencyBalance";
            this.label_CurrencyBalance.Size = new System.Drawing.Size(98, 13);
            this.label_CurrencyBalance.TabIndex = 15;
            this.label_CurrencyBalance.Text = "0.00";
            this.label_CurrencyBalance.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label_CurrencyBalance.Click += new System.EventHandler(this.label_CurrencyBalance_Click);
            // 
            // label_CommodityBalance
            // 
            this.label_CommodityBalance.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label_CommodityBalance.AutoSize = true;
            this.label_CommodityBalance.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label_CommodityBalance.Location = new System.Drawing.Point(159, 154);
            this.label_CommodityBalance.Name = "label_CommodityBalance";
            this.label_CommodityBalance.Size = new System.Drawing.Size(100, 13);
            this.label_CommodityBalance.TabIndex = 17;
            this.label_CommodityBalance.Text = "0.00";
            this.label_CommodityBalance.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label_CommodityBalance.Click += new System.EventHandler(this.label_CommodityBalance_Click);
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(107, 154);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(46, 13);
            this.label7.TabIndex = 16;
            this.label7.Text = "Balance";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_MarketID
            // 
            this.label_MarketID.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label_MarketID.AutoSize = true;
            this.label_MarketID.Location = new System.Drawing.Point(107, 11);
            this.label_MarketID.Name = "label_MarketID";
            this.label_MarketID.Size = new System.Drawing.Size(46, 13);
            this.label_MarketID.TabIndex = 1;
            this.label_MarketID.Text = "ID";
            this.label_MarketID.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dataGridView_OrderBookAsks
            // 
            this.dataGridView_OrderBookAsks.AllowUserToAddRows = false;
            this.dataGridView_OrderBookAsks.AllowUserToDeleteRows = false;
            this.dataGridView_OrderBookAsks.AllowUserToResizeRows = false;
            this.dataGridView_OrderBookAsks.AutoGenerateColumns = false;
            this.dataGridView_OrderBookAsks.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView_OrderBookAsks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_OrderBookAsks.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgv_Asks_Quantity,
            this.dgv_Asks_Price});
            this.dataGridView_OrderBookAsks.DataSource = this.askOrderBindingSource;
            this.dataGridView_OrderBookAsks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_OrderBookAsks.Location = new System.Drawing.Point(3, 3);
            this.dataGridView_OrderBookAsks.MultiSelect = false;
            this.dataGridView_OrderBookAsks.Name = "dataGridView_OrderBookAsks";
            this.dataGridView_OrderBookAsks.ReadOnly = true;
            this.dataGridView_OrderBookAsks.RowHeadersVisible = false;
            this.tableLayoutPanel4.SetRowSpan(this.dataGridView_OrderBookAsks, 2);
            this.dataGridView_OrderBookAsks.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridView_OrderBookAsks.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView_OrderBookAsks.ShowEditingIcon = false;
            this.dataGridView_OrderBookAsks.Size = new System.Drawing.Size(195, 219);
            this.dataGridView_OrderBookAsks.TabIndex = 1;
            this.dataGridView_OrderBookAsks.TabStop = false;
            this.dataGridView_OrderBookAsks.VirtualMode = true;
            this.dataGridView_OrderBookAsks.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_OrderBookAsks_CellClick);
            // 
            // dgv_Asks_Quantity
            // 
            this.dgv_Asks_Quantity.DataPropertyName = "Quantity";
            dataGridViewCellStyle6.Format = "N8";
            this.dgv_Asks_Quantity.DefaultCellStyle = dataGridViewCellStyle6;
            this.dgv_Asks_Quantity.HeaderText = "Quantity";
            this.dgv_Asks_Quantity.Name = "dgv_Asks_Quantity";
            this.dgv_Asks_Quantity.ReadOnly = true;
            // 
            // dgv_Asks_Price
            // 
            this.dgv_Asks_Price.DataPropertyName = "Price";
            dataGridViewCellStyle7.Format = "N8";
            this.dgv_Asks_Price.DefaultCellStyle = dataGridViewCellStyle7;
            this.dgv_Asks_Price.HeaderText = "Price";
            this.dgv_Asks_Price.Name = "dgv_Asks_Price";
            this.dgv_Asks_Price.ReadOnly = true;
            // 
            // askOrderBindingSource
            // 
            this.askOrderBindingSource.DataSource = typeof(Objects.Order);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Controls.Add(this.tabPage6);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(3, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(346, 244);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.button_HideZeros);
            this.tabPage1.Controls.Add(this.dataGridView_Balances);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(338, 218);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Balances";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // button_HideZeros
            // 
            this.button_HideZeros.Location = new System.Drawing.Point(15, 186);
            this.button_HideZeros.Name = "button_HideZeros";
            this.button_HideZeros.Size = new System.Drawing.Size(75, 23);
            this.button_HideZeros.TabIndex = 1;
            this.button_HideZeros.Text = "Show Zeros";
            this.button_HideZeros.UseVisualStyleBackColor = true;
            this.button_HideZeros.Click += new System.EventHandler(this.button_HideZeros_Click);
            // 
            // dataGridView_Balances
            // 
            this.dataGridView_Balances.AllowUserToAddRows = false;
            this.dataGridView_Balances.AllowUserToDeleteRows = false;
            this.dataGridView_Balances.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView_Balances.AutoGenerateColumns = false;
            this.dataGridView_Balances.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView_Balances.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_Balances.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.nameDataGridViewTextBoxColumn,
            this.availableDataGridViewTextBoxColumn,
            this.heldDataGridViewTextBoxColumn,
            this.totalDataGridViewTextBoxColumn});
            this.dataGridView_Balances.DataSource = this.balanceBindingSource;
            this.dataGridView_Balances.Location = new System.Drawing.Point(7, 7);
            this.dataGridView_Balances.MultiSelect = false;
            this.dataGridView_Balances.Name = "dataGridView_Balances";
            this.dataGridView_Balances.ReadOnly = true;
            this.dataGridView_Balances.RowHeadersVisible = false;
            this.dataGridView_Balances.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView_Balances.ShowEditingIcon = false;
            this.dataGridView_Balances.Size = new System.Drawing.Size(325, 173);
            this.dataGridView_Balances.TabIndex = 0;
            this.dataGridView_Balances.TabStop = false;
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "Name";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            this.nameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // availableDataGridViewTextBoxColumn
            // 
            this.availableDataGridViewTextBoxColumn.DataPropertyName = "Available";
            dataGridViewCellStyle8.Format = "N8";
            this.availableDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle8;
            this.availableDataGridViewTextBoxColumn.HeaderText = "Available";
            this.availableDataGridViewTextBoxColumn.Name = "availableDataGridViewTextBoxColumn";
            this.availableDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // heldDataGridViewTextBoxColumn
            // 
            this.heldDataGridViewTextBoxColumn.DataPropertyName = "Held";
            this.heldDataGridViewTextBoxColumn.HeaderText = "Held";
            this.heldDataGridViewTextBoxColumn.Name = "heldDataGridViewTextBoxColumn";
            this.heldDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // totalDataGridViewTextBoxColumn
            // 
            this.totalDataGridViewTextBoxColumn.DataPropertyName = "Total";
            this.totalDataGridViewTextBoxColumn.HeaderText = "Total";
            this.totalDataGridViewTextBoxColumn.Name = "totalDataGridViewTextBoxColumn";
            this.totalDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // balanceBindingSource
            // 
            this.balanceBindingSource.DataSource = typeof(Objects.Balance);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.textBox_CancelOrderId);
            this.tabPage2.Controls.Add(this.button_CancelOrder);
            this.tabPage2.Controls.Add(this.dataGridView_ActiveOrders);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(338, 218);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "ActiveOrders";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // textBox_CancelOrderId
            // 
            this.textBox_CancelOrderId.Enabled = false;
            this.textBox_CancelOrderId.Location = new System.Drawing.Point(96, 187);
            this.textBox_CancelOrderId.MaxLength = 512;
            this.textBox_CancelOrderId.Name = "textBox_CancelOrderId";
            this.textBox_CancelOrderId.ReadOnly = true;
            this.textBox_CancelOrderId.Size = new System.Drawing.Size(229, 20);
            this.textBox_CancelOrderId.TabIndex = 2;
            // 
            // button_CancelOrder
            // 
            this.button_CancelOrder.Location = new System.Drawing.Point(15, 186);
            this.button_CancelOrder.Name = "button_CancelOrder";
            this.button_CancelOrder.Size = new System.Drawing.Size(75, 23);
            this.button_CancelOrder.TabIndex = 1;
            this.button_CancelOrder.Text = "Cancel";
            this.button_CancelOrder.UseVisualStyleBackColor = true;
            this.button_CancelOrder.Click += new System.EventHandler(this.button_CancelOrder_Click);
            // 
            // dataGridView_ActiveOrders
            // 
            this.dataGridView_ActiveOrders.AllowUserToAddRows = false;
            this.dataGridView_ActiveOrders.AllowUserToDeleteRows = false;
            this.dataGridView_ActiveOrders.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView_ActiveOrders.AutoGenerateColumns = false;
            this.dataGridView_ActiveOrders.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView_ActiveOrders.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_ActiveOrders.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.orderIdDataGridViewTextBoxColumn,
            this.priceDataGridViewTextBoxColumn,
            this.quantityDataGridViewTextBoxColumn,
            this.orderTypeDataGridViewTextBoxColumn,
            this.remainingDataGridViewTextBoxColumn,
            this.percentFilledDataGridViewTextBoxColumn});
            this.dataGridView_ActiveOrders.DataSource = this.activeOrderBindingSource;
            this.dataGridView_ActiveOrders.Location = new System.Drawing.Point(7, 7);
            this.dataGridView_ActiveOrders.MultiSelect = false;
            this.dataGridView_ActiveOrders.Name = "dataGridView_ActiveOrders";
            this.dataGridView_ActiveOrders.ReadOnly = true;
            this.dataGridView_ActiveOrders.RowHeadersVisible = false;
            this.dataGridView_ActiveOrders.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView_ActiveOrders.ShowEditingIcon = false;
            this.dataGridView_ActiveOrders.Size = new System.Drawing.Size(325, 173);
            this.dataGridView_ActiveOrders.TabIndex = 0;
            this.dataGridView_ActiveOrders.TabStop = false;
            this.dataGridView_ActiveOrders.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_ActiveOrders_CellClick);
            // 
            // orderIdDataGridViewTextBoxColumn
            // 
            this.orderIdDataGridViewTextBoxColumn.DataPropertyName = "OrderId";
            this.orderIdDataGridViewTextBoxColumn.HeaderText = "OrderId";
            this.orderIdDataGridViewTextBoxColumn.Name = "orderIdDataGridViewTextBoxColumn";
            this.orderIdDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // priceDataGridViewTextBoxColumn
            // 
            this.priceDataGridViewTextBoxColumn.DataPropertyName = "Price";
            this.priceDataGridViewTextBoxColumn.HeaderText = "Price";
            this.priceDataGridViewTextBoxColumn.Name = "priceDataGridViewTextBoxColumn";
            this.priceDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // quantityDataGridViewTextBoxColumn
            // 
            this.quantityDataGridViewTextBoxColumn.DataPropertyName = "Quantity";
            this.quantityDataGridViewTextBoxColumn.HeaderText = "Quantity";
            this.quantityDataGridViewTextBoxColumn.Name = "quantityDataGridViewTextBoxColumn";
            this.quantityDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // orderTypeDataGridViewTextBoxColumn
            // 
            this.orderTypeDataGridViewTextBoxColumn.DataPropertyName = "OrderType";
            this.orderTypeDataGridViewTextBoxColumn.HeaderText = "OrderType";
            this.orderTypeDataGridViewTextBoxColumn.Name = "orderTypeDataGridViewTextBoxColumn";
            this.orderTypeDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // remainingDataGridViewTextBoxColumn
            // 
            this.remainingDataGridViewTextBoxColumn.DataPropertyName = "Remaining";
            this.remainingDataGridViewTextBoxColumn.HeaderText = "Remaining";
            this.remainingDataGridViewTextBoxColumn.Name = "remainingDataGridViewTextBoxColumn";
            this.remainingDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // percentFilledDataGridViewTextBoxColumn
            // 
            this.percentFilledDataGridViewTextBoxColumn.DataPropertyName = "PercentFilled";
            this.percentFilledDataGridViewTextBoxColumn.HeaderText = "PercentFilled";
            this.percentFilledDataGridViewTextBoxColumn.Name = "percentFilledDataGridViewTextBoxColumn";
            this.percentFilledDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // activeOrderBindingSource
            // 
            this.activeOrderBindingSource.DataSource = typeof(Objects.ActiveOrder);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.groupBox_CondOrders);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(338, 218);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "ConditionalOrders";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // groupBox_CondOrders
            // 
            this.groupBox_CondOrders.Controls.Add(this.tableLayoutPanel5);
            this.groupBox_CondOrders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_CondOrders.Location = new System.Drawing.Point(0, 0);
            this.groupBox_CondOrders.Name = "groupBox_CondOrders";
            this.groupBox_CondOrders.Size = new System.Drawing.Size(338, 218);
            this.groupBox_CondOrders.TabIndex = 0;
            this.groupBox_CondOrders.TabStop = false;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 4;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel5.Controls.Add(this.groupBox3, 2, 2);
            this.tableLayoutPanel5.Controls.Add(this.richTextBox_OrderDecrip, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.label1, 0, 2);
            this.tableLayoutPanel5.Controls.Add(this.numericUpDown_CondTrigger, 0, 3);
            this.tableLayoutPanel5.Controls.Add(this.button_CondOrdersCancel, 1, 9);
            this.tableLayoutPanel5.Controls.Add(this.label6, 0, 4);
            this.tableLayoutPanel5.Controls.Add(this.button_CondOrdersSave, 0, 9);
            this.tableLayoutPanel5.Controls.Add(this.numericUpDown_CondOrderPrice, 0, 5);
            this.tableLayoutPanel5.Controls.Add(this.numericUpDown_CondQuant, 0, 7);
            this.tableLayoutPanel5.Controls.Add(this.label8, 0, 6);
            this.tableLayoutPanel5.Controls.Add(this.groupBox4, 2, 5);
            this.tableLayoutPanel5.Controls.Add(this.listBox_CondOrders, 2, 8);
            this.tableLayoutPanel5.Controls.Add(this.label_CondTot, 0, 8);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 10;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(332, 199);
            this.tableLayoutPanel5.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.tableLayoutPanel5.SetColumnSpan(this.groupBox3, 2);
            this.groupBox3.Controls.Add(this.radioButton_CondSell);
            this.groupBox3.Controls.Add(this.radioButton_CondBuy);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(169, 41);
            this.groupBox3.Name = "groupBox3";
            this.tableLayoutPanel5.SetRowSpan(this.groupBox3, 3);
            this.groupBox3.Size = new System.Drawing.Size(160, 51);
            this.groupBox3.TabIndex = 23;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Type";
            // 
            // radioButton_CondSell
            // 
            this.radioButton_CondSell.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.radioButton_CondSell.AutoSize = true;
            this.radioButton_CondSell.Location = new System.Drawing.Point(91, 20);
            this.radioButton_CondSell.Name = "radioButton_CondSell";
            this.radioButton_CondSell.Size = new System.Drawing.Size(42, 17);
            this.radioButton_CondSell.TabIndex = 0;
            this.radioButton_CondSell.TabStop = true;
            this.radioButton_CondSell.Text = "Sell";
            this.radioButton_CondSell.UseVisualStyleBackColor = true;
            this.radioButton_CondSell.CheckedChanged += new System.EventHandler(this.updateCondOrders);
            // 
            // radioButton_CondBuy
            // 
            this.radioButton_CondBuy.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.radioButton_CondBuy.AutoSize = true;
            this.radioButton_CondBuy.Checked = true;
            this.radioButton_CondBuy.Location = new System.Drawing.Point(27, 20);
            this.radioButton_CondBuy.Name = "radioButton_CondBuy";
            this.radioButton_CondBuy.Size = new System.Drawing.Size(43, 17);
            this.radioButton_CondBuy.TabIndex = 0;
            this.radioButton_CondBuy.TabStop = true;
            this.radioButton_CondBuy.Text = "Buy";
            this.radioButton_CondBuy.UseVisualStyleBackColor = true;
            this.radioButton_CondBuy.CheckedChanged += new System.EventHandler(this.updateCondOrders);
            // 
            // richTextBox_OrderDecrip
            // 
            this.tableLayoutPanel5.SetColumnSpan(this.richTextBox_OrderDecrip, 4);
            this.richTextBox_OrderDecrip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox_OrderDecrip.Enabled = false;
            this.richTextBox_OrderDecrip.Location = new System.Drawing.Point(3, 3);
            this.richTextBox_OrderDecrip.Name = "richTextBox_OrderDecrip";
            this.richTextBox_OrderDecrip.ReadOnly = true;
            this.tableLayoutPanel5.SetRowSpan(this.richTextBox_OrderDecrip, 2);
            this.richTextBox_OrderDecrip.Size = new System.Drawing.Size(326, 32);
            this.richTextBox_OrderDecrip.TabIndex = 25;
            this.richTextBox_OrderDecrip.Text = "";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 41);
            this.label1.Margin = new System.Windows.Forms.Padding(3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "Trigger Price";
            this.label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // numericUpDown_CondTrigger
            // 
            this.tableLayoutPanel5.SetColumnSpan(this.numericUpDown_CondTrigger, 2);
            this.numericUpDown_CondTrigger.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numericUpDown_CondTrigger.Location = new System.Drawing.Point(0, 57);
            this.numericUpDown_CondTrigger.Margin = new System.Windows.Forms.Padding(0);
            this.numericUpDown_CondTrigger.Name = "numericUpDown_CondTrigger";
            this.numericUpDown_CondTrigger.Size = new System.Drawing.Size(166, 20);
            this.numericUpDown_CondTrigger.TabIndex = 12;
            this.numericUpDown_CondTrigger.ValueChanged += new System.EventHandler(this.updateCondDescrip);
            // 
            // button_CondOrdersCancel
            // 
            this.button_CondOrdersCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.button_CondOrdersCancel.Enabled = false;
            this.button_CondOrdersCancel.Location = new System.Drawing.Point(86, 174);
            this.button_CondOrdersCancel.Name = "button_CondOrdersCancel";
            this.button_CondOrdersCancel.Size = new System.Drawing.Size(77, 22);
            this.button_CondOrdersCancel.TabIndex = 20;
            this.button_CondOrdersCancel.Text = "Cancel";
            this.button_CondOrdersCancel.UseVisualStyleBackColor = true;
            this.button_CondOrdersCancel.Click += new System.EventHandler(this.button_CondOrdersCancel_Click);
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 79);
            this.label6.Margin = new System.Windows.Forms.Padding(3);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(60, 13);
            this.label6.TabIndex = 17;
            this.label6.Text = "Order Price";
            this.label6.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // button_CondOrdersSave
            // 
            this.button_CondOrdersSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.button_CondOrdersSave.Location = new System.Drawing.Point(3, 174);
            this.button_CondOrdersSave.Name = "button_CondOrdersSave";
            this.button_CondOrdersSave.Size = new System.Drawing.Size(77, 22);
            this.button_CondOrdersSave.TabIndex = 20;
            this.button_CondOrdersSave.Text = "Save";
            this.button_CondOrdersSave.UseVisualStyleBackColor = true;
            this.button_CondOrdersSave.Click += new System.EventHandler(this.button_CondOrdersSave_Click);
            // 
            // numericUpDown_CondOrderPrice
            // 
            this.tableLayoutPanel5.SetColumnSpan(this.numericUpDown_CondOrderPrice, 2);
            this.numericUpDown_CondOrderPrice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numericUpDown_CondOrderPrice.Location = new System.Drawing.Point(0, 95);
            this.numericUpDown_CondOrderPrice.Margin = new System.Windows.Forms.Padding(0);
            this.numericUpDown_CondOrderPrice.Name = "numericUpDown_CondOrderPrice";
            this.numericUpDown_CondOrderPrice.Size = new System.Drawing.Size(166, 20);
            this.numericUpDown_CondOrderPrice.TabIndex = 15;
            this.numericUpDown_CondOrderPrice.ValueChanged += new System.EventHandler(this.updateCondOrders);
            this.numericUpDown_CondOrderPrice.MouseCaptureChanged += new System.EventHandler(this.updateCondOrders);
            // 
            // numericUpDown_CondQuant
            // 
            this.tableLayoutPanel5.SetColumnSpan(this.numericUpDown_CondQuant, 2);
            this.numericUpDown_CondQuant.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numericUpDown_CondQuant.Location = new System.Drawing.Point(0, 133);
            this.numericUpDown_CondQuant.Margin = new System.Windows.Forms.Padding(0);
            this.numericUpDown_CondQuant.Name = "numericUpDown_CondQuant";
            this.numericUpDown_CondQuant.Size = new System.Drawing.Size(166, 20);
            this.numericUpDown_CondQuant.TabIndex = 14;
            this.numericUpDown_CondQuant.ValueChanged += new System.EventHandler(this.updateCondOrders);
            this.numericUpDown_CondQuant.MouseCaptureChanged += new System.EventHandler(this.updateCondOrders);
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 117);
            this.label8.Margin = new System.Windows.Forms.Padding(3);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(46, 13);
            this.label8.TabIndex = 18;
            this.label8.Text = "Quantity";
            this.label8.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // groupBox4
            // 
            this.tableLayoutPanel5.SetColumnSpan(this.groupBox4, 2);
            this.groupBox4.Controls.Add(this.radioButton_PriceBelow);
            this.groupBox4.Controls.Add(this.radioButton_PriceAbove);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(169, 98);
            this.groupBox4.Name = "groupBox4";
            this.tableLayoutPanel5.SetRowSpan(this.groupBox4, 3);
            this.groupBox4.Size = new System.Drawing.Size(160, 51);
            this.groupBox4.TabIndex = 19;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Condition";
            // 
            // radioButton_PriceBelow
            // 
            this.radioButton_PriceBelow.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.radioButton_PriceBelow.AutoSize = true;
            this.radioButton_PriceBelow.Location = new System.Drawing.Point(89, 20);
            this.radioButton_PriceBelow.Name = "radioButton_PriceBelow";
            this.radioButton_PriceBelow.Size = new System.Drawing.Size(65, 17);
            this.radioButton_PriceBelow.TabIndex = 0;
            this.radioButton_PriceBelow.TabStop = true;
            this.radioButton_PriceBelow.Text = "Is Below";
            this.radioButton_PriceBelow.UseVisualStyleBackColor = true;
            this.radioButton_PriceBelow.CheckedChanged += new System.EventHandler(this.updateCondDescrip);
            // 
            // radioButton_PriceAbove
            // 
            this.radioButton_PriceAbove.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.radioButton_PriceAbove.AutoSize = true;
            this.radioButton_PriceAbove.Checked = true;
            this.radioButton_PriceAbove.Location = new System.Drawing.Point(6, 20);
            this.radioButton_PriceAbove.Name = "radioButton_PriceAbove";
            this.radioButton_PriceAbove.Size = new System.Drawing.Size(67, 17);
            this.radioButton_PriceAbove.TabIndex = 0;
            this.radioButton_PriceAbove.TabStop = true;
            this.radioButton_PriceAbove.Text = "Is Above";
            this.radioButton_PriceAbove.UseVisualStyleBackColor = true;
            this.radioButton_PriceAbove.CheckedChanged += new System.EventHandler(this.updateCondDescrip);
            // 
            // listBox_CondOrders
            // 
            this.tableLayoutPanel5.SetColumnSpan(this.listBox_CondOrders, 2);
            this.listBox_CondOrders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox_CondOrders.FormattingEnabled = true;
            this.listBox_CondOrders.Location = new System.Drawing.Point(169, 155);
            this.listBox_CondOrders.Name = "listBox_CondOrders";
            this.tableLayoutPanel5.SetRowSpan(this.listBox_CondOrders, 2);
            this.listBox_CondOrders.Size = new System.Drawing.Size(160, 41);
            this.listBox_CondOrders.TabIndex = 24;
            this.listBox_CondOrders.SelectedIndexChanged += new System.EventHandler(this.listBox_CondOrders_SelectedIndexChanged);
            // 
            // label_CondTot
            // 
            this.label_CondTot.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label_CondTot.AutoSize = true;
            this.tableLayoutPanel5.SetColumnSpan(this.label_CondTot, 2);
            this.label_CondTot.Location = new System.Drawing.Point(3, 155);
            this.label_CondTot.Name = "label_CondTot";
            this.label_CondTot.Size = new System.Drawing.Size(34, 13);
            this.label_CondTot.TabIndex = 26;
            this.label_CondTot.Text = "Total:";
            this.label_CondTot.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // tabPage4
            // 
            this.tabPage4.AutoScroll = true;
            this.tabPage4.Controls.Add(this.label_RatesExchangeName);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(338, 218);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Main Rates";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // label_RatesExchangeName
            // 
            this.label_RatesExchangeName.AutoSize = true;
            this.label_RatesExchangeName.Location = new System.Drawing.Point(7, 7);
            this.label_RatesExchangeName.Name = "label_RatesExchangeName";
            this.label_RatesExchangeName.Size = new System.Drawing.Size(74, 13);
            this.label_RatesExchangeName.TabIndex = 0;
            this.label_RatesExchangeName.Text = "Main Markets:";
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.listView1);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(338, 218);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "Api Status";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.lvC_Name,
            this.lvC_Status,
            this.lvC_Info});
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.Location = new System.Drawing.Point(5, 8);
            this.listView1.Name = "listView1";
            this.listView1.ShowGroups = false;
            this.listView1.Size = new System.Drawing.Size(330, 173);
            this.listView1.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // lvC_Name
            // 
            this.lvC_Name.Text = "Api";
            this.lvC_Name.Width = 42;
            // 
            // lvC_Status
            // 
            this.lvC_Status.Text = "Status";
            this.lvC_Status.Width = 49;
            // 
            // lvC_Info
            // 
            this.lvC_Info.Text = "Info";
            this.lvC_Info.Width = 295;
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.donationForm1);
            this.tabPage6.Location = new System.Drawing.Point(4, 22);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Size = new System.Drawing.Size(338, 218);
            this.tabPage6.TabIndex = 5;
            this.tabPage6.Text = "Donate";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // donationForm1
            // 
            this.donationForm1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.donationForm1.Location = new System.Drawing.Point(0, 0);
            this.donationForm1.Name = "donationForm1";
            this.donationForm1.Size = new System.Drawing.Size(338, 218);
            this.donationForm1.TabIndex = 0;
            // 
            // toolStrip_Settings
            // 
            this.toolStrip_Settings.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip_Settings.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_EditKeys});
            this.toolStrip_Settings.Location = new System.Drawing.Point(3, 0);
            this.toolStrip_Settings.Name = "toolStrip_Settings";
            this.toolStrip_Settings.Size = new System.Drawing.Size(35, 25);
            this.toolStrip_Settings.TabIndex = 0;
            // 
            // toolStripButton_EditKeys
            // 
            this.toolStripButton_EditKeys.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripButton_EditKeys.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_EditKeys.Image = global::Objects.Properties.Resources.key_128;
            this.toolStripButton_EditKeys.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_EditKeys.Name = "toolStripButton_EditKeys";
            this.toolStripButton_EditKeys.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton_EditKeys.Text = "Set Keys";
            this.toolStripButton_EditKeys.ToolTipText = "Set Keys";
            this.toolStripButton_EditKeys.Click += new System.EventHandler(this.button_EditKeys_Click);
            // 
            // toolStrip_Apis
            // 
            this.toolStrip_Apis.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip_Apis.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ApiIndicator_BitCoinCoId,
            this.ApiIndicator_Bittrex,
            this.ApiIndicator_BTCe,
            this.ApiIndicator_Cryptsy,
            this.ApiIndicator_Kraken,
            this.ApiIndicator_Poloniex});
            this.toolStrip_Apis.Location = new System.Drawing.Point(38, 0);
            this.toolStrip_Apis.Name = "toolStrip_Apis";
            this.toolStrip_Apis.Size = new System.Drawing.Size(181, 25);
            this.toolStrip_Apis.TabIndex = 1;
            // 
            // ApiIndicator_Bittrex
            // 
            this.ApiIndicator_Bittrex.BackColor = System.Drawing.Color.Tomato;
            this.ApiIndicator_Bittrex.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ApiIndicator_Bittrex.Image = global::Objects.Properties.Resources.icon_bittrex;
            this.ApiIndicator_Bittrex.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ApiIndicator_Bittrex.Name = "ApiIndicator_Bittrex";
            this.ApiIndicator_Bittrex.Size = new System.Drawing.Size(23, 22);
            this.ApiIndicator_Bittrex.ToolTipText = "Force Update (Bittrex)";
            this.ApiIndicator_Bittrex.Click += new System.EventHandler(this.ApiIndicator_Bittrex_Click);
            // 
            // ApiIndicator_BTCe
            // 
            this.ApiIndicator_BTCe.BackColor = System.Drawing.Color.Tomato;
            this.ApiIndicator_BTCe.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ApiIndicator_BTCe.Image = global::Objects.Properties.Resources.BTCe;
            this.ApiIndicator_BTCe.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ApiIndicator_BTCe.Name = "ApiIndicator_BTCe";
            this.ApiIndicator_BTCe.Size = new System.Drawing.Size(23, 22);
            this.ApiIndicator_BTCe.ToolTipText = "Force Update (BTCe)";
            this.ApiIndicator_BTCe.Click += new System.EventHandler(this.ApiIndicator_BTCe_Click);
            // 
            // ApiIndicator_Cryptsy
            // 
            this.ApiIndicator_Cryptsy.BackColor = System.Drawing.Color.Tomato;
            this.ApiIndicator_Cryptsy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ApiIndicator_Cryptsy.Image = global::Objects.Properties.Resources.Cryptsy;
            this.ApiIndicator_Cryptsy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ApiIndicator_Cryptsy.Name = "ApiIndicator_Cryptsy";
            this.ApiIndicator_Cryptsy.Size = new System.Drawing.Size(23, 22);
            this.ApiIndicator_Cryptsy.ToolTipText = "Force Update (Cryptsy)";
            this.ApiIndicator_Cryptsy.Click += new System.EventHandler(this.ApiIndicator_Cryptsy_Click);
            // 
            // ApiIndicator_Kraken
            // 
            this.ApiIndicator_Kraken.BackColor = System.Drawing.Color.Tomato;
            this.ApiIndicator_Kraken.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ApiIndicator_Kraken.Image = global::Objects.Properties.Resources.Kraken;
            this.ApiIndicator_Kraken.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ApiIndicator_Kraken.Name = "ApiIndicator_Kraken";
            this.ApiIndicator_Kraken.Size = new System.Drawing.Size(23, 22);
            this.ApiIndicator_Kraken.ToolTipText = "Force Update (Kraken)";
            this.ApiIndicator_Kraken.Click += new System.EventHandler(this.ApiIndicator_Kraken_Click);
            // 
            // ApiIndicator_BitCoinCoId
            // 
            this.ApiIndicator_BitCoinCoId.BackColor = System.Drawing.Color.Tomato;
            this.ApiIndicator_BitCoinCoId.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ApiIndicator_BitCoinCoId.Image = global::Objects.Properties.Resources.coId;
            this.ApiIndicator_BitCoinCoId.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ApiIndicator_BitCoinCoId.Name = "ApiIndicator_BitCoinCoId";
            this.ApiIndicator_BitCoinCoId.Size = new System.Drawing.Size(23, 22);
            this.ApiIndicator_BitCoinCoId.ToolTipText = "Force Update (BitcoinCoId)";
            this.ApiIndicator_BitCoinCoId.Click += new System.EventHandler(this.ApiIndicator_BitCoinCoId_Click);
            // 
            // ApiIndicator_Poloniex
            // 
            this.ApiIndicator_Poloniex.BackColor = System.Drawing.Color.Tomato;
            this.ApiIndicator_Poloniex.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ApiIndicator_Poloniex.Image = global::Objects.Properties.Resources.Poloniex;
            this.ApiIndicator_Poloniex.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ApiIndicator_Poloniex.Name = "ApiIndicator_Poloniex";
            this.ApiIndicator_Poloniex.Size = new System.Drawing.Size(23, 22);
            this.ApiIndicator_Poloniex.ToolTipText = "Force Update (Poloniex)";
            this.ApiIndicator_Poloniex.Click += new System.EventHandler(this.ApiIndicator_Poloniex_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1036, 730);
            this.Controls.Add(this.toolStripContainer_Main);
            this.Name = "MainForm";
            this.Text = "SIDTraderLite";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.toolStripContainer_Main.BottomToolStripPanel.ResumeLayout(false);
            this.toolStripContainer_Main.BottomToolStripPanel.PerformLayout();
            this.toolStripContainer_Main.ContentPanel.ResumeLayout(false);
            this.toolStripContainer_Main.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer_Main.TopToolStripPanel.PerformLayout();
            this.toolStripContainer_Main.ResumeLayout(false);
            this.toolStripContainer_Main.PerformLayout();
            this.toolStrip_Status.ResumeLayout(false);
            this.toolStrip_Status.PerformLayout();
            this.tableLayoutPanel_ChartArea.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_TradeRecs)).EndInit();
            this.panel_Main.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_OrderBookBids)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bidOrderBindingSource)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textBox_BuyPrice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBox_SellPrice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBox_BuyQuant)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBox_SellQuant)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_OrderBookAsks)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.askOrderBindingSource)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Balances)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.balanceBindingSource)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_ActiveOrders)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.activeOrderBindingSource)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.groupBox_CondOrders.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_CondTrigger)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_CondOrderPrice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_CondQuant)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.tabPage5.ResumeLayout(false);
            this.tabPage6.ResumeLayout(false);
            this.toolStrip_Settings.ResumeLayout(false);
            this.toolStrip_Settings.PerformLayout();
            this.toolStrip_Apis.ResumeLayout(false);
            this.toolStrip_Apis.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.ToolStripContainer toolStripContainer_Main;
        private System.Windows.Forms.Panel panel_Tickers;
        private System.Windows.Forms.Panel panel_Main;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dataGridView_OrderBookBids;
        private System.Windows.Forms.DataGridView dataGridView_OrderBookAsks;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button button_Buy;
        private System.Windows.Forms.Label label_MarketID;
        private System.Windows.Forms.Button button_Sell;
        private System.Windows.Forms.NumericUpDown textBox_BuyPrice;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown textBox_SellPrice;
        private System.Windows.Forms.NumericUpDown textBox_BuyQuant;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown textBox_SellQuant;
        private System.Windows.Forms.TextBox textBox_BuyFee;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox_SellFee;
        private System.Windows.Forms.TextBox textBox_BuyTotal;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox_SellTotal;
        private System.Windows.Forms.Label label_CurrencyBalance;
        private System.Windows.Forms.Label label_CommodityBalance;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.BindingSource activeOrderBindingSource;
        private System.Windows.Forms.BindingSource bidOrderBindingSource;
        private System.Windows.Forms.BindingSource askOrderBindingSource;
        private System.Windows.Forms.BindingSource balanceBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgv_Asks_Quantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgv_Asks_Price;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgv_Bids_Price;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgv_Bids_Quantity;
        private System.Windows.Forms.ToolStrip toolStrip_Settings;
        private System.Windows.Forms.ToolStrip toolStrip_Status;
        private System.Windows.Forms.ToolStripLabel toolStripLabel_Status;
        private System.Windows.Forms.ToolStripButton toolStripButton_EditKeys;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.DataGridView dataGridView_Balances;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox textBox_CancelOrderId;
        private System.Windows.Forms.Button button_CancelOrder;
        private System.Windows.Forms.DataGridView dataGridView_ActiveOrders;
        private System.Windows.Forms.DataGridViewTextBoxColumn orderIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn priceDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn quantityDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn orderTypeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn remainingDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn percentFilledDataGridViewTextBoxColumn;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn availableDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn heldDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn totalDataGridViewTextBoxColumn;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label_MinPrice;
        private System.Windows.Forms.TextBox textBox_MinPrice;
        private System.Windows.Forms.TextBox textBox_MinVolume;
        private System.Windows.Forms.TextBox textBoxMaxPrice;
        private System.Windows.Forms.TextBox textBox_Decimals;
        private System.Windows.Forms.Label label_MinVolume;
        private System.Windows.Forms.Label label_MaxPrice;
        private System.Windows.Forms.Label label_Decimals;
        private System.Windows.Forms.Label label_Market;
        private System.Windows.Forms.Label label_Spread;
        private System.Windows.Forms.Button button_HideZeros;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel_ChartArea;
        private System.Windows.Forms.Label label_RatesExchangeName;
        private System.Windows.Forms.ToolStrip toolStrip_Apis;
        private System.Windows.Forms.ToolStripButton ApiIndicator_BTCe;
        private System.Windows.Forms.ToolStripButton ApiIndicator_Cryptsy;
        private System.Windows.Forms.ToolStripButton ApiIndicator_Kraken;
        private System.Windows.Forms.ToolStripButton ApiIndicator_BitCoinCoId;
        private System.Windows.Forms.ToolStripButton ApiIndicator_Poloniex;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader lvC_Name;
        private System.Windows.Forms.ColumnHeader lvC_Status;
        private System.Windows.Forms.ColumnHeader lvC_Info;
        private System.Windows.Forms.GroupBox groupBox_CondOrders;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton radioButton_CondSell;
        private System.Windows.Forms.RadioButton radioButton_CondBuy;
        private System.Windows.Forms.ListBox listBox_CondOrders;
        private System.Windows.Forms.Button button_CondOrdersSave;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RadioButton radioButton_PriceBelow;
        private System.Windows.Forms.RadioButton radioButton_PriceAbove;
        private System.Windows.Forms.RichTextBox richTextBox_OrderDecrip;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown numericUpDown_CondQuant;
        private System.Windows.Forms.NumericUpDown numericUpDown_CondOrderPrice;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDown_CondTrigger;
        private System.Windows.Forms.Button button_CondOrdersCancel;
        private System.Windows.Forms.DataGridView dataGridView_TradeRecs;
        private StockCharts.QuickChart quickChart1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_TradeHistory_Time;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_TradeHistory_Price;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_TradeHistory_Quant;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_TradeHistory_Type;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.Label label_CondTot;
        private System.Windows.Forms.TabPage tabPage6;
        private Donate.DonationForm donationForm1;
        private System.Windows.Forms.ToolStripButton ApiIndicator_Bittrex;
    
    }
}