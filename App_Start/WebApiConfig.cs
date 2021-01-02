using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace TRApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Servizi e configurazione dell'API Web

            // Route dell'API Web
            config.MapHttpAttributeRoutes();

            // Gestione CORS
            config.EnableCors();

            config.Routes.MapHttpRoute(
                name: "ActioApi",
                routeTemplate: "TRApi/DropdownValues/{action}/{id}",
                defaults: new { controller = "DropdownValues", id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "TRApi/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

        }
    }
}
