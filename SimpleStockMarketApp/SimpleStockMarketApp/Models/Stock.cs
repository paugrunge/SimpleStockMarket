using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleStockMarketApp.Models
{
    public class Stock
    {
        public long Id { get; set; }
        public StockSymbol Symbol { get; set; }
        public StockType Type { get; set; }
        public decimal LastDividend { get; set; }
        public decimal? FixedDividend { get; set; }
        public decimal ParValue { get; set; }
    }
}