using System;

namespace SimpleStockMarketApp.Models
{
    public class Trade
    {
        public StockSymbol StockSymbol { get; set; }
        public TradeIndicator Indicator { get; set; }
        public decimal TradedPrice { get; set; }
        public int Quantity { get; set; }
        public decimal CashAmount { get { return TradedPrice * Quantity; } }
        public DateTime Timestamp { get; set; }
    }
}