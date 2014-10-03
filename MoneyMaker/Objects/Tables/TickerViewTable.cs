using System;
using System.Data;

namespace Objects
{
    public class TickerViewTable : DataTable
    {
        #region Overrides
        protected override Type GetRowType()
        {
            return typeof(TickerViewTableRow);
        }
        protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
        {
            return new TickerViewTableRow(builder);
        }
        protected override void OnRowChanged(DataRowChangeEventArgs e)
        {
            base.OnRowChanged(e);
            TickerViewTableRowChangedEventArgs args =
              new TickerViewTableRowChangedEventArgs(e.Action, (TickerViewTableRow)e.Row);
            OnTickerViewTableRowChanged(args);
        }
        protected virtual void OnTickerViewTableRowChanged(TickerViewTableRowChangedEventArgs args)
        {
            if (TickerViewTableRowChanged != null)
            {
                TickerViewTableRowChanged(this, args);
            }
        }
        #endregion

        public delegate void TickerViewTableRowChangedDlgt(TickerViewTable sender, TickerViewTableRowChangedEventArgs args);
        public event TickerViewTableRowChangedDlgt TickerViewTableRowChanged;

        #region Constructor
        public TickerViewTable()
        {
            SetColumns();
        }
        public TickerViewTable(string name)
        {
            TableName = name;
            SetColumns();
        }
        #endregion

        #region Methods
        private void SetColumns()
        {
            DataColumn market = new DataColumn("Market", typeof(string));
            DataColumn Exchange = new DataColumn("Exchange", typeof(ExchangeEnum));
            DataColumn Ask = new DataColumn("Ask", typeof(double));
            DataColumn Bid = new DataColumn("Bid", typeof(double));
            DataColumn LastTradePrice = new DataColumn("LastTradePrice", typeof(double));
            DataColumn LastUpdated = new DataColumn("LastUpdated", typeof(DateTime));
            Columns.Add(market);
            Columns.Add(Exchange);
            Columns.Add(Ask);
            Columns.Add(Bid);
            Columns.Add(LastTradePrice);
            Columns.Add(LastUpdated);
            PrimaryKey = new DataColumn[] { market };
        }
        public TickerViewTableRow this[int idx]
        {
            get { return (TickerViewTableRow)Rows[idx]; }
        }
        public void Add(TickerViewTableRow row)
        {
            Rows.Add(row);
        }
        public void Remove(TickerViewTableRow row)
        {
            Rows.Remove(row);
        }
        public TickerViewTableRow GetNewRow()
        {
            TickerViewTableRow row = (TickerViewTableRow)NewRow();
            return row;
        }
        #endregion
    }
    public class TickerViewTableRow : DataRow
    {
        #region Constructor
        internal TickerViewTableRow(DataRowBuilder builder)
            : base(builder)
        {
            Market = String.Empty;
            Ask = Double.NaN;
            Bid = Double.NaN;
            LastTradePrice = Double.NaN;
        }
        #endregion

        #region Fields
        public ExchangeEnum Exchange
        {
            get { return (ExchangeEnum)base["Exchange"]; }
            set { base["Exchange"] = value; }
        }
        public string Market
        {
            get { return (string)base["Market"]; }
            set { base["Market"] = value; }
        }
        public double Ask
        {
            get { return (double)base["Ask"]; }
            set { base["Ask"] = Convert.ToDouble(value); }
        }
        public double Bid
        {
            get { return (double)base["Bid"]; }
            set { base["Bid"] = Convert.ToDouble(value); }
        }
        public double LastTradePrice
        {
            get { return (double)base["LastTradePrice"]; }
            set { base["LastTradePrice"] = Convert.ToDouble(value); }
        }
        public DateTime LastUpdated
        {
            get { return (DateTime)base["LastUpdated"]; }
            set { base["LastUpdated"] = value; }
        }
        #endregion

        #region Events
        #endregion
    }
    public class TickerViewTableRowChangedEventArgs
    {
        protected DataRowAction action;
        protected TickerViewTableRow row;

        public DataRowAction Action
        {
            get
            {
                return action;
            }
        }
        public TickerViewTableRow Row
        {
            get
            {
                return row;
            }
        }
        public TickerViewTableRowChangedEventArgs(DataRowAction action, TickerViewTableRow row)
        {
            this.action = action;
            this.row = row;
        }
    }
}
