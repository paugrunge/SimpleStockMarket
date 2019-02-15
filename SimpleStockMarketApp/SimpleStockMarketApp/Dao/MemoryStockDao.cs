using SimpleStockMarketApp.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleStockMarketApp.Dao
{
    public class MemoryStockDao
    {
        public static readonly MemoryStockDao Instance = new MemoryStockDao();
        private readonly ConcurrentDictionary<StockSymbol, Stock> stockBase = new ConcurrentDictionary<StockSymbol, Stock>();

        private MemoryStockDao()
        {
            CreateMemoryStock();
        }

        public void AddStock(Stock stock)
        {
            stockBase.TryAdd(stock.Symbol, stock);
        }

        public Stock GetStock(StockSymbol symbol)
        {
            return stockBase[symbol];
        }

        public IList<Stock> GetStocks()
        {
            return stockBase.Values.ToList();
        }

        private void CreateMemoryStock()
        {
            stockBase.TryAdd(StockSymbol.TEA, new Stock()
            {
                Id = 1,
                Symbol = StockSymbol.TEA,
                Type = StockType.COMMON,
                LastDividend = 0,
                ParValue = 100
            });
            stockBase.TryAdd(StockSymbol.POP, new Stock()
            {
                Id = 2,
                Symbol = StockSymbol.POP,
                Type = StockType.COMMON,
                LastDividend = 8,
                ParValue = 100
            });
            stockBase.TryAdd(StockSymbol.ALE, new Stock()
            {
                Id = 3,
                Symbol = StockSymbol.ALE,
                Type = StockType.COMMON,
                LastDividend = 23,
                ParValue = 60
            });
            stockBase.TryAdd(StockSymbol.GIN, new Stock()
            {
                Id = 4,
                Symbol = StockSymbol.GIN,
                Type = StockType.PREFERRED,
                LastDividend = 8,
                FixedDividend = 0.02m,
                ParValue = 100
            });
            stockBase.TryAdd(StockSymbol.JOE, new Stock()
            {
                Id = 5,
                Symbol = StockSymbol.JOE,
                Type = StockType.COMMON,
                LastDividend = 13,
                ParValue = 250
            });
        }
    }
}