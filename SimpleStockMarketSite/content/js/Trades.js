(function (self, $, undefined) {

    var URL_BASE_TRADES = "http://localhost:50581/StockMarketApp/Trades/",
        URL_GET_VOL_WEIGHT_STOCK_PRICE = "GetVolumeWeightedStockPrice/",
        URL_GET_ALL_SHARE_INDEX = "GetAllShareIndex/";
    
    var $stocksList = $("#stocksList"),
        $stocksListCalc = $("#stocksListCalc"),
        $indicator = $("#indicator"),
        $btnAddTrade =  $("#btnAddTrade"),
        $tradedPrice = $("#tradedPrice"),
        $quantity = $("#quantity"),
        $btnStockPrice =  $("#btnStockPrice"),
        $stockPrice = $("#stockPrice"),
        $tradesTable = $("#tradesTable"),
        $btnGbceShareIndex =  $("#btnGbceShareIndex"),
        $gbceShareIndex = $("#gbceShareIndex");
    
    var minutes = 5;

    self.Inicialize = function () {
        StockMarket.LoadStocksCombo($stocksList, true, $stocksListCalc);
        $btnAddTrade.on("click", addTrade);
        $btnStockPrice.on("click", calculateStockPrice);
        $stocksListCalc.on("change", changeStocskListCalc);
        $btnGbceShareIndex.on("click", calculateGbceShareIndex);
        applyMasks();
        cleanTradeForm();
        $stockPrice.val("");
        $gbceShareIndex.val("");
        getTrades();
        setInterval(getTrades, 60000);
    };

    function addTrade() {
        var trade = getFormTrade();
        $.ajax({  
            url: URL_BASE_TRADES,  
            type: 'POST',    
            contentType: "application/json",
            data: JSON.stringify(trade),  
            success: function (data) {  
                StockMarket.ShowDialog("Trade add successfully" , "Trades");
                cleanTradeForm();
            },  
            error: function (xhr, textStatus, errorThrown) {  
                if(errorThrown != "")
                StockMarket.ShowDialog(errorThrown , "Trades");
            else
                StockMarket.ShowDialog("Error while adding the trade" , "Trades");
            }  
        });  
    }

    function getFormTrade(){
        var trade = {};
        trade.StockSymbol = $stocksList.val();
        trade.Indicator =  $indicator.val();
        var price = 0;
        if($tradedPrice.val() != "")
            price = parseFloat($(".js-price").inputmask('unmaskedvalue'));
        trade.TradedPrice = price;
        var quantity = 0;
        if($quantity.val() != "")
            quantity = parseInt($(".js-mask-int").inputmask('unmaskedvalue'));
        trade.Quantity = quantity;
        return trade;
    }
    
    function calculateStockPrice() {
        $stockPrice.val("");
        var stock = $stocksListCalc.val();
        if(stock != ""){
             $.get(URL_BASE_TRADES + URL_GET_VOL_WEIGHT_STOCK_PRICE + stock + "/" + minutes)
            .done(function(data) {
                 $stockPrice.val(data);
            })
            .fail(function(jqXHR, textStatus, errorThrown ) {
                 if(errorThrown != "")
                    StockMarket.ShowDialog(errorThrown , "Trades");
                else
                    StockMarket.ShowDialog("Error while calculating" , "Trades");
            });
        }
    }
    
    function changeStocskListCalc() {
        $stockPrice.val("");
    }
    
    function cleanTradeForm(){
        $stocksList.val("TEA");
        $indicator.val("BUY");
        $tradedPrice.val("");
        $quantity.val("");
    }
    
    function getTrades() {
        $("#tradesTable tbody").empty();
        $.get(URL_BASE_TRADES + minutes)
                .done(function(data) {
                    // Load data
                    data.forEach(function(elem) {
                            tradeAddRow(elem); 
                        });
                })
                .fail(function(jqXHR, textStatus, errorThrown ) {
                     if(errorThrown != "")
                        StockMarket.ShowDialog(errorThrown , "Trades");
                    else
                        StockMarket.ShowDialog("Error while getting trades" , "Trades");
                });
    }
    
    function tradeAddRow(trade) {
        // Check if <tbody> tag exists, add one if not
        if ($("#tradesTable tbody").length == 0) {
        $tradesTable.append("<tbody></tbody>");
        }
        // Append row to <table>
        $("#tradesTable tbody").append(
            tradeBuildTableRow(trade));
    }

    function tradeBuildTableRow(trade) {
        var ret =
            "<tr>" +
            "<td>" + trade.StockSymbol + "</td>" +
            "<td>" + trade.Indicator + "</td>" +
            "<td>" + trade.TradedPrice + "</td>" +
            "<td>" + trade.Quantity + "</td>" +
            "</tr>";
        return ret;
    }
    
    function calculateGbceShareIndex() {
        $gbceShareIndex.val("");
        $.get(URL_BASE_TRADES + URL_GET_ALL_SHARE_INDEX + minutes)
            .done(function(data) {
                 $gbceShareIndex.val(data);
            })
            .fail(function(jqXHR, textStatus, errorThrown ) {
                 if(errorThrown != "")
                    StockMarket.ShowDialog(errorThrown , "Trades");
                else
                    StockMarket.ShowDialog("Error while calculating" , "Trades");
            });
    }
    
    function applyMasks() {
          $(".js-mask-price").inputmask("numeric", {
            radixPoint: ".",
            groupSeparator: "",
            integerDigits: 10,
            digits: 6,
            autoGroup: true,
            prefix: '',
            rightAlign: false,
            enforceDigitsOnBlur: true,
            allowPlus: false,
            allowMinus: false,
            removeMaskOnSubmit: true,
            autoUnmask: true,
            positionCaretOnClick: "none",
        });

          $(".js-mask-int").inputmask("numeric", {
            radixPoint: ".",
            groupSeparator: "",
            integerDigits: 10,
            digits: 0,
            autoGroup: true,
            prefix: '',
            rightAlign: false,
            enforceDigitsOnBlur: true,
            allowPlus: false,
            allowMinus: false,
            removeMaskOnSubmit: true,
            autoUnmask: true,
            positionCaretOnClick: "none"
        });
    }
}(window.StockMarket.Trades = window.StockMarket.Trades || {}, jQuery));

jQuery(function () {
    StockMarket.Trades.Inicialize();
});
