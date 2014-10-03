using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Objects;

namespace Toolbox
{
    class Calcs
    {
        /// <summary>
        /// Return How Many Coins Can Be Bought For Specified Percentage
        /// </summary>
        /// <param name="balance">Current balance of coin to buy with</param>
        /// <param name="percent">Percentage to spend</param>
        /// <param name="fee">Buy fee of exchange</param>
        /// <param name="price">Price of currency to buy</param>
        /// <returns></returns>
        public static decimal Buy_HowMany(dynamic balance, dynamic percent, dynamic fee, dynamic price)
        {
            balance = Convert.ToDecimal(balance);
            percent = Convert.ToDecimal(percent);
            price = Convert.ToDecimal(price);

            string f = fee.ToString().Replace(".", "0");
            string f1 = string.Format("1.{0}", f);
            fee = Convert.ToDecimal(f1);
            decimal maxSpend;

            if (percent < 100)
            {
                maxSpend = (balance / 100) * percent;
            }
            else
            {
                maxSpend = balance;
            }

            price = price * fee;
            decimal total = 0;

            if (maxSpend > 0 && price > 0)
                total = Decimal.Round(maxSpend / price, 8, MidpointRounding.AwayFromZero);

            return total;
            
        }

        /// <summary>
        /// Returns Cost Of Buying X Number Of Coins
        /// </summary>
        /// <param name="volume">Volume of coin buying</param>
        /// <param name="fee">Buy fee of exchange</param>
        /// <param name="price">Price of coin to buy</param>
        /// <returns></returns>
        public static decimal Buy_HowMuch(dynamic volume, dynamic fee, dynamic price, out decimal? feeAmount)
        {
            volume = Convert.ToDecimal(volume);
            fee = Convert.ToDecimal(fee);
            price = Convert.ToDecimal(price);

            feeAmount = Math.Round(((volume * price) / 100) * fee, 8);

            string f = fee.ToString().Replace(".", "0");
            string f1 = string.Format("1.{0}", f);
            fee = Convert.ToDecimal(f1);


            price = price * fee;
            decimal total = Decimal.Round(volume * price, 8, MidpointRounding.AwayFromZero);

            return total;
        }
        public static decimal Buy_HowMuch(dynamic volume, dynamic fee, dynamic price)
        {
            volume = Convert.ToDecimal(volume);
            
            string f = string.Format("1.{0}", fee.ToString().Replace(".", "0"));
            fee = Convert.ToDecimal(f);
            price = Convert.ToDecimal(price) * fee;
            decimal total = Decimal.Round(volume * price, 8, MidpointRounding.AwayFromZero);

            return total;
        }

        /// <summary>
        /// Returns Net Value From Selling X Number Of Coins
        /// </summary>
        /// <param name="volume">Volume of coin to sell</param>
        /// <param name="fee">Selling fee on exchange</param>
        /// <param name="price">Price to sell for</param>
        /// <returns></returns>
        public static decimal Sell_HowMuch(dynamic volume, dynamic fee, dynamic price, out decimal? feeTotal)
        {
            volume = Convert.ToDecimal(volume);
            fee = Convert.ToDecimal(fee);
            price = Convert.ToDecimal(price);

            feeTotal = Decimal.Round(((volume * price) / 100) * fee, 8);
            decimal total = Decimal.Round((volume * price) - (decimal)feeTotal, 8, MidpointRounding.AwayFromZero);

            return total;
        }
        public static decimal Sell_HowMuch(dynamic volume, dynamic fee, dynamic price)
        {
            volume = Convert.ToDecimal(volume);
            fee = Convert.ToDecimal(fee);
            price = Convert.ToDecimal(price);

            decimal feeTotal = Decimal.Round(((volume * price) / 100) * fee, 8);
            decimal total = Decimal.Round((volume * price) - (decimal)feeTotal, 8, MidpointRounding.AwayFromZero);

            return total;
        }
        /// <summary>
        /// Checks If A Route Should Return Profit
        /// </summary>
        /// <param name="volume1">Initial Volume To Buy</param>
        /// <param name="price1">Price To Initially Buy At</param>
        /// <param name="price2">Price To Then Sell At</param>
        /// <param name="price3">Price For Third Trade</param>
        /// <param name="finalOrderType">Type of Trade For Third Trade</param>
        /// <returns>Array {[0] = Cost} {[1] = Return} {[2] = Final} {[3] = Profit} </returns>
        public static decimal[] RouteProfitCheck(
            dynamic volume1,
            dynamic buyFee,
            dynamic sellFee,
            dynamic price1,
            dynamic price2,
            dynamic price3,
            OrderType finalOrderType)
        {
            volume1 = Convert.ToDecimal(volume1);
            buyFee = Convert.ToDecimal(buyFee);
            sellFee = Convert.ToDecimal(sellFee);
            price1 = Convert.ToDecimal(price1);
            price2 = Convert.ToDecimal(price2);
            price3 = Convert.ToDecimal(price3);

            decimal cost = Buy_HowMuch(volume1, buyFee, price1);
            decimal ret = Sell_HowMuch(volume1, sellFee, price2);
            decimal final = new decimal();
            if (finalOrderType == OrderType.Bid)
            {
                final = Buy_HowMany(ret, 100, buyFee, price3);
            }
            else if (finalOrderType == OrderType.Ask)
            {
                final = Sell_HowMuch(ret, sellFee, price3);
            }
            decimal profit = Decimal.Round(final - cost, 8, MidpointRounding.AwayFromZero);
            decimal[] answer = new decimal[] { cost, ret, final, profit };
            return answer;
        }

        /// <summary>
        /// Return The Smallest Value For 2 or 3 Variables
        /// </summary>
        /// <param name="AskVol">Value 1</param>
        /// <param name="BidVol">Value 2</param>
        /// <param name="MaxVol">Value 3</param>
        /// <returns></returns>
        public static decimal FindSmallest(dynamic AskVol, dynamic BidVol, dynamic MaxVol)
        {
            AskVol = Convert.ToDecimal(AskVol);
            BidVol = Convert.ToDecimal(BidVol);
            MaxVol = Convert.ToDecimal(MaxVol);

            if (AskVol < BidVol && AskVol <= MaxVol)
            {
                return AskVol;
            }
            else if (BidVol <= AskVol && BidVol <= MaxVol)
            {
                decimal VolOut = Decimal.Round(BidVol / 5, 8, MidpointRounding.AwayFromZero);
                return VolOut;
            }
            else
            {
                return MaxVol;
            }
        }
    }
}
