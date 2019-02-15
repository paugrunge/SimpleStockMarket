(function (self, $, undefined) {

    var URL_BASE_STOCK_MARKET = "http://localhost:50581/StockMarketApp/";
    
    self.Inicialize = function () {

    };

    self.LoadStocksCombo = function($stocksList, isTrade, $stocksListCalc) {
        $.get(URL_BASE_STOCK_MARKET + "Stocks")
            .done(function(data) {
                if(!isTrade){
                    $stocksList.append( 
                        $("<option></option>")
                            .text("Select")
                            .val("")
                    );
                }
                if($stocksListCalc != undefined) {
                     $stocksListCalc.append( 
                        $("<option></option>")
                            .text("Select")
                            .val("")
                    );
                }

                data.forEach(function(elem) {
                     $stocksList.append( 
                        $("<option></option>")
                            .text(elem.Symbol)
                            .val(elem.Symbol)
                    );
                    if($stocksListCalc != undefined) {
                        $stocksListCalc.append( 
                            $("<option></option>")
                                .text(elem.Symbol)
                                .val(elem.Symbol)
                        );
                    }
                });
            })
           .fail(function(jqXHR, textStatus, errorThrown ) {
                if(errorThrown != "")
                    StockMarket.ShowDialog(errorThrown , "Stocks");
                else
                    StockMarket.ShowDialog("Error while getting stocks" , "Stocks");
            });
    };

    self.ShowConfirm = function (message, title) {
         $.confirm({
                    title: title,
                    content: message,
                    columnClass: 'col-md-6 col-md-offset-3  ',
                    type: 'dark',
                    theme: 'material',
                    buttons: {
                    }
                });
        };

    self.ShowDialog = function (message, title) {
        $.dialog({
            title: "GBCE - " + title,
            content: message,
        });
    };

}(window.StockMarket = window.StockMarket || {}, jQuery));

jQuery(function () {

    StockMarket.Inicialize();
});
