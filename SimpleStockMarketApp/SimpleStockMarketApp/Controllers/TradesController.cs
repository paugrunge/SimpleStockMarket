using SimpleStockMarketApp.Bll;
using SimpleStockMarketApp.Controllers.Helpers;
using SimpleStockMarketApp.Dao;
using SimpleStockMarketApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SimpleStockMarketApp.Controllers
{
    public class TradesController : ApiController
    {
        // GET StockMarketApp/trades
        public IEnumerable<Trade> Get()
        {
            return MemoryTradeDao.Instance.GetAllTrades();
        }

        [Route("StockMarketApp/Trades/{minutes}")]
        public IEnumerable<Trade> Get(int minutes)
        {
            var trades =  MemoryTradeDao.Instance.GetAllTrades();
            return trades.Where(t => t.Timestamp >= DateTime.Now.AddMinutes(-minutes)).OrderByDescending(t => t.Timestamp);
        }

        // POST stockmarketapp/trades
        [HttpPost]
        public HttpResponseMessage Post([FromBody]Trade trade)
        {
            try
            {
                trade.Timestamp = DateTime.Now;
                MemoryTradeDao.Instance.AddTrade(trade);
                return Request.CreateResponse(HttpStatusCode.OK, "Trade Added");
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                HttpError err = new HttpError(message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, err);
            }
        }

        [Route("StockMarketApp/Trades/GetVolumeWeightedStockPrice/{symbol}/{minutes}")]
        public HttpResponseMessage GetVolumeWeightedStockPrice(string symbol, int minutes)
        {
            try
            {
                StockSymbol stockSymbol;
                var isValid = Enum.TryParse(symbol.ToUpper(), out stockSymbol);
                if(!isValid)
                    throw new ApplicationException($"Stock  with symbol {symbol} not found");
                var trades = MemoryTradeDao.Instance.GetTrades(stockSymbol, minutes);
                var vwsp = StockMarketCalculator.Instance.CalculateVolumeWeightedStockPrice(trades);
                return Request.CreateResponse(HttpStatusCode.OK, vwsp);
            }
            catch (ApplicationException ex)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    StatusCode = HttpStatusCode.NotFound,
                    ReasonPhrase = ex.Message
                };
                throw new HttpResponseException(resp);
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                HttpError err = new HttpError(message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, err);
            }
        }

        [Route("StockMarketApp/Trades/GetAllShareIndex/{minutes}")]
        public HttpResponseMessage GetAllShareIndex(int minutes)
        {
            try
            {
                var trades = Get(minutes);
                var index = StockMarketCalculator.Instance.CalculateGbceAllShareIndex(trades);
                return Request.CreateResponse(HttpStatusCode.OK, index);
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                HttpError err = new HttpError(message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, err);
            }
        }
    }
}