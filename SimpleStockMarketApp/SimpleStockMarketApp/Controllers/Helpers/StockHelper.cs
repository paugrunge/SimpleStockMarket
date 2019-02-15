using SimpleStockMarketApp.Dao;
using SimpleStockMarketApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleStockMarketApp.Controllers.Helpers
{
    public static class StockHelper
    {
        /// <summary>
        /// Gets a Stock from cache by its symbol
        /// </summary>
        public static Stock GetStock(string symbol)
        {
            try
            {
                StockSymbol stockSymbol = (StockSymbol)Enum.Parse(typeof(StockSymbol), symbol.ToUpper());
                var stock = MemoryStockDao.Instance.GetStock(stockSymbol);
                return stock;
            }
            catch
            {
                throw new ApplicationException($"Stock  with symbol {symbol} not found");
            }

        }

    }
}