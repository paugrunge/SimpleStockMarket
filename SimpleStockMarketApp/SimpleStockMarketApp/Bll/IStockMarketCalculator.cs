using SimpleStockMarketApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleStockMarketApp.Bll
{
    public interface IStockMarketCalculator
    {
        /// <summary>
        /// precision - The number of digits to be used for an operation; results are rounded to this precision.
        /// </summary>
        int Precision { get; }

        decimal CalculateDividendYield(Stock stock, decimal price);
        decimal CalculatePERatio(Stock stock, decimal price);
        decimal CalculateVolumeWeightedStockPrice(IList<Trade> trades);
        decimal CalculateGbceAllShareIndex(IEnumerable<Trade> trades);
        decimal Round(decimal value);
    }
}
