using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SimpleStockMarketApp.Models;
using SimpleStockMarketApp.Dao;

namespace SimpleStockMarketApp.Bll
{
    public class StockMarketCalculator : StockMarketCalculatorBase
    {
        public static readonly StockMarketCalculator Instance = new StockMarketCalculator();
        private StockMarketCalculator()
        {
        }

        public override decimal CalculateDividendYield(Stock stock, decimal price)
        {
            decimal result = -1;
            if (StockType.PREFERRED.Equals(stock.Type))
            {
                if(stock.FixedDividend.HasValue && price != 0)
                    result = (stock.FixedDividend.Value * stock.ParValue) / price;
                    
                return Round(result);
            }
            result = stock.LastDividend / price;
            return Round(result);
        }

        public override decimal CalculateGbceAllShareIndex(IEnumerable<Trade> trades)
        {
            IList<decimal> stockPrices = new List<decimal>();
            var tradeBase = MemoryTradeDao.Instance.GetTradeBase();
            // Calculate volume weighted stock price for all stocks in the system
            foreach (var stockSymbol in tradeBase.Keys)
            {
                var tradesSymbol = trades.Where(t => t.StockSymbol == stockSymbol).ToList();
                var stockPrice = CalculateVolumeWeightedStockPrice(tradesSymbol); // is for all trades in cache?
                if (stockPrice > 0)
                    stockPrices.Add(stockPrice);
            }

            if (stockPrices.Count == 0)
                return 0;

            decimal multiplied = 1;
            foreach (var tradesStockPrice in stockPrices)
            {
                multiplied = multiplied * tradesStockPrice;
            }
            var n = (1d / stockPrices.Count());
            decimal result = (decimal)Math.Pow((double)multiplied, n);
            return Round(result);
        }

        public override decimal CalculatePERatio(Stock stock, decimal price)
        {
            decimal result = -1;
            var dividend = CalculateDividendYield(stock, price);
            if(dividend != 0)
                result = price / dividend;
            return Round(result);
        }

        public override decimal CalculateVolumeWeightedStockPrice(IList<Trade> trades)
        {
            decimal result = 0;
            if (trades.Count == 0)
                return result;

            decimal sumOfPriceQuantity = 0;
            int sumOfQuantity = 0;

            foreach (var trade in trades)
            {
                sumOfPriceQuantity += trade.CashAmount;
                sumOfQuantity += trade.Quantity;
            }
            
            if (sumOfQuantity != 0)
                result = sumOfPriceQuantity / (decimal)sumOfQuantity;
            return Round(result);
        }
    }
}