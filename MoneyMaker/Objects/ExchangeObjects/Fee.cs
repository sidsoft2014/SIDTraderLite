using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objects
{
    public class Fee
    {
        public Fee()
        {
            this.Quantity = 0D;
            this.FeePercentage = 0D;
        }

        public double Quantity { get; set; }
        public double FeePercentage { get; set; }
        public string MarketId { get; set; }

        public virtual Market Market { get; set; }
    }
}
