using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace WebAPIDemo
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //remove xml formatter.to return web api only json format irrespective of the accept (reponse type) param by requester
            //config.Formatters.Remove(config.Formatters.XmlFormatter);

            //Returning data based upon the accept header, it will return text/html format always
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new System.Net.Http.Headers.MediaTypeHeaderValue("text/html"));

            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "EmployeeV1",
                routeTemplate: "api/v1/employee/{id}",
                defaults: new { controller = "Employee1", id = RouteParameter.Optional }

                );

            config.Routes.MapHttpRoute(
             name: "EmployeeV2",
             routeTemplate: "api/v2/employee/{id}",
             defaults: new { controller = "Employee2", id = RouteParameter.Optional }

             );


            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
