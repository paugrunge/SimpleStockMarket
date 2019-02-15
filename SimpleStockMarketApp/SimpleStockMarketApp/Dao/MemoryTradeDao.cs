using SimpleStockMarketApp.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleStockMarketApp.Dao
{
    public class MemoryTradeDao
    {
        public static readonly MemoryTradeDao Instance = new MemoryTradeDao();
        private readonly ConcurrentDictionary<StockSymbol, IList<Trade>> tradeBase = new ConcurrentDictionary<StockSymbol, IList<Trade>>();

        private MemoryTradeDao()
        {
        }

        /// <summary>
        /// Adds a trade to de cache
        /// </summary>
        public void AddTrade(Trade trade)
        {
            var symbol = trade.StockSymbol;
            IList<Trade> trades = new List<Trade>();
            if (tradeBase.ContainsKey(symbol))
            {
                tradeBase[symbol].Add(trade);
                return;
            }
            trades.Add(trade);
            tradeBase.TryAdd(symbol, trades);
        }

        /// <summary>
        /// Gets all trades for a stock by its symbol within the last minutes
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="minutes">Last minutes to consider from timespan</param>
        /// <returns></returns>
        public IList<Trade> GetTrades(StockSymbol symbol, int minutes)
        {
            IList<Trade> trades = new List<Trade>();
            DateTime afterDate = DateTime.Now.AddMinutes(-minutes);
            if (tradeBase.ContainsKey(symbol))
                trades = tradeBase[symbol];
            return trades.Where(t => t.Timestamp >= afterDate).ToList();
        }

        /// <summary>
        /// Gets all trades from cache
        /// </summary>
        /// <returns></returns>
        public IList<Trade> GetAllTrades()
        {
            IList<Trade> result = new List<Trade>();
            foreach (var stockSymbol in tradeBase.Keys)
            {
                var trades = tradeBase[stockSymbol];
                result = result.Concat(trades).ToList();
            }
            return result;
        }

        /// <summary>
        /// Gets the dictionary use as cache
        /// </summary>
        /// <returns></returns>
        public IDictionary<StockSymbol, IList<Trade>> GetTradeBase()
        {
            return tradeBase;
        }
    }
}