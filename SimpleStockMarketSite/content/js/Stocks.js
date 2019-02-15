(function (self, $, undefined) {

    var URL_BASE_STOCKS = "http://localhost:50581/StockMarketApp/Stocks/",
        URL_GET_DIVIDEND_YIELD = "GetDividendYield/",
        URL_GET_PE_RATIO = "GetPERatio/";
        
    var $stocksList = $("#stocksList"),
        $stockTitle =  $("#stockTitle"),
        $stocksTable = $("#stocksTable"),
        $price = $("#price"),
        $dividendYield = $("#dividendYield"),
        $ratio = $("#ratio"),
        $btnDividendYield = $("#btnDividendYield"),
        $btnRatio = $("#btnRatio");

    self.Inicialize = function () {
            StockMarket.LoadStocksCombo($stocksList, false);
            $stocksList.on("change", changeStocskList);
            $btnDividendYield.on("click", calculateDividendYield);
            $btnRatio.on("click", calculateRatio);
            applyMasks();
    };

    function changeStocskList() {
        cleanScreen();
        var stock = $stocksList.val();
        if(stock != ""){
             $.get(URL_BASE_STOCKS + stock)
                .done(function(data) {
                    // Load data
                    $stockTitle.text(data.Symbol);
                    stockAddRow(data);
                })
                .fail(function(jqXHR, textStatus, errorThrown ) {
                     if(errorThrown != "")
                        StockMarket.ShowDialog(errorThrown , "Stocks");
                    else
                        StockMarket.ShowDialog("Error while getting stock" , "Stocks");
                });
        }
    }

    function stockAddRow(stock) {
        // Check if <tbody> tag exists, add one if not
        if ($("#stocksTable tbody").length == 0) {
        $stocksTable.append("<tbody></tbody>");
        }
        // Append row to <table>
        $("#stocksTable tbody").append(
            stockBuildTableRow(stock));
    }

    function stockBuildTableRow(stock) {
        var fixed = stock.FixedDividend == null ? "" : stock.FixedDividend * 100 + "%";
        var ret =
            "<tr>" +
            "<td>" + stock.Type + "</td>" +
            "<td>" + stock.LastDividend + "</td>" +
            "<td>" +  fixed + "</td>" +
            "<td>" + stock.ParValue + "</td>" +
            "</tr>";
        return ret;
    }

    function cleanScreen() {
        $("#stocksTable tbody").empty();
        $stockTitle.text("");
        $price.val("");
        $dividendYield.val("");
        $ratio.val("");
    }

    function calculateDividendYield() {
        var price = 0;
        if($price.val() != "")
            price = parseFloat($(".js-price").inputmask('unmaskedvalue'));
        
        if(price != 0 && $stockTitle.text() != "") {
            $.get(URL_BASE_STOCKS + URL_GET_DIVIDEND_YIELD + $stockTitle.text() + "/" + price + "/")
            .done(function(data) {
                $dividendYield.val(data);
            })
            .fail(function(jqXHR, textStatus, errorThrown ) {
                StockMarket.ShowDialog(errorThrown , "Dividend Yield");
            });
        }
    }

    function calculateRatio() {
        var price = 0;
        if($price.val() != "")
            price = parseFloat($(".js-mask-price").inputmask('unmaskedvalue'));
        
        if(price != 0 && $stockTitle.text() != "Stock") {
            $.get(URL_BASE_STOCKS + URL_GET_PE_RATIO + $stockTitle.text() + "/" + price + "/")
            .done(function(data) {
                $ratio.val(data);
            })
            .fail(function(jqXHR, textStatus, errorThrown ) {
                StockMarket.ShowDialog(errorThrown , "P/E Ratio");
            });
        }
    }

    function applyMasks() {
          $(".js-mask-price").inputmask("numeric", {
            radixPoint: ".",
            groupSeparator: "",
            integerDigits: 10,
            digits: 6,
            autoGroup: true,
            prefix: '', //Space after $, this will not truncate the first character.
            rightAlign: false,
            enforceDigitsOnBlur: true,
            allowPlus: false,
            allowMinus: false,
            removeMaskOnSubmit: true,
            autoUnmask: true,
            positionCaretOnClick: "none",
        });
    }
}(window.StockMarket.Stocks = window.StockMarket.Stocks || {}, jQuery));

jQuery(function () {
    StockMarket.Stocks.Inicialize();
});
