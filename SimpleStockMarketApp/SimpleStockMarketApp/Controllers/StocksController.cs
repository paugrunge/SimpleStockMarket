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
    public class StocksController : ApiController
    {
        // GET StockMarketApp/stocks
        public IEnumerable<Stock> Get()
        {
            return MemoryStockDao.Instance.GetStocks();
        }

        // GET StockMarketApp/stocks/symbol
        public HttpResponseMessage Get(string symbol)
        {
            try
            {
                var stock = StockHelper.GetStock(symbol);
                return Request.CreateResponse(HttpStatusCode.OK, stock);
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

        [Route("StockMarketApp/Stocks/GetDividendYield/{symbol}/{price}/")]
        public HttpResponseMessage GetDividendYield(string symbol, decimal price)
        {
            try
            {
                var stock = StockHelper.GetStock(symbol);
                var dividend = StockMarketCalculator.Instance.CalculateDividendYield(stock, price);
                return Request.CreateResponse(HttpStatusCode.OK, dividend);
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

        [Route("StockMarketApp/Stocks/GetPERatio/{symbol}/{price}")]
        public HttpResponseMessage GetPERatio(string symbol, decimal price)
        {
            try
            {
                var stock = StockHelper.GetStock(symbol);
                var ratio = StockMarketCalculator.Instance.CalculatePERatio(stock, price);
                return Request.CreateResponse(HttpStatusCode.OK, ratio);
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
    }
}