using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SimpleStockMarketApp.Models;

namespace SimpleStockMarketApp.Bll
{
    public abstract class StockMarketCalculatorBase : IStockMarketCalculator
    {
        public int Precision
        {
            get
            {
                return 6;
            }
        }

        public abstract decimal CalculateDividendYield(Stock stock, decimal price);
        public abstract decimal CalculatePERatio(Stock stock, decimal price);
        public abstract decimal CalculateVolumeWeightedStockPrice(IList<Trade> trades);
        public abstract decimal CalculateGbceAllShareIndex(IEnumerable<Trade> trades);

        public decimal Round(decimal value)
        {
            return Math.Round(value, Precision);
        }
    }
}