# SimpleStockMarket
A web api and site for a stock market trading in drinks

* SimpleStockMarketApp

Develop in Visual Studio 2017 with .NET Framework 4.6.1 using Web Api. A publish folder is included 
in case of implementation in a ISS Server. Otherwise it should be run in ISS Express provided by Visual Studio. 


* SimpleStockMarketSite

Web site develop in HTML, Bootstrap (v4.2.1) and jQuery (v3.3.1). It consumes the web api by ajax, doing get and 
post requests.

The variables for the base url (URL_BASE_STOCK_MARKET, URL_BASE_STOCKS, URL_BASE_TRADES) should be changed with
their correspondig url for the server where the web api is running.

All data is handled in memory.
