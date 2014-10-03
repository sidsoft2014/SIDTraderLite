using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objects
{
    /// <summary>
    /// Holds all balance details for a single commodity.
    /// Can be used individually, in a collection or as part of an Exchange Object.
    /// </summary>
    public class Balance : INotifyPropertyChanged
    {
        private double? _available;
        private double? _held;

        public virtual Exchange Exchange { get; set; }
        public string Name { get; set; }
        public double Available 
        { 
            get 
            {
                if (_available != null)
                    return (double)_available;
                else return 0;
            } 
            set
            {
                if (_available != value)
                {
                    _available = value;
                    OnPropertyChanged("Available");
                }
            }
        }
        public double Held
        {
            get
            {
                if (_held != null)
                    return (double)_held;
                else return 0;
            }
            set
            {
                if (_held != value)
                {
                    _held = value;
                    OnPropertyChanged("Held");
                }
            }
        }
        public double Total { get { return Available + Held; } }
        public ExchangeEnum ExchangeName { get { return Exchange.Name; } }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string Name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                if (Exchange != null)
                    PropertyChanged(Exchange, new PropertyChangedEventArgs(Name));
                else
                    PropertyChanged(this, new PropertyChangedEventArgs(Name));
            }
        }
    }
}
