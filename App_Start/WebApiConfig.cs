using Microsoft.Owin.Security.OAuth;
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
            // Web API configuration and services  
            // Configure Web API to use only bearer token authentication.  
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

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
