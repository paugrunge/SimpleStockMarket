using System.Web.Http;
using System.Web.Http.Cors;

namespace SimpleStockMarketApp
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Configuración y servicios de API web
            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors();

            // Rutas de API web
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "StockMarketApp/{controller}/{symbol}",
                defaults: new { symbol = RouteParameter.Optional }
            );
        }
    }
}
