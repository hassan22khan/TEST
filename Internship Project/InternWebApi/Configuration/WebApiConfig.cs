using DependencyResolver;
using IData;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using Ninject;
using Ninject.Modules;
using StudentRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using WebApiContrib.IoC.Ninject;

namespace InternWebApi.Configuration
{
    public class WebApiConfig
    {
        public static StandardKernel Register(HttpConfiguration config)
        {
            config.Filters.Add(new AuthorizeAttribute());
            var kernel = new StandardKernel();
            Register(config, kernel);
            return kernel;
        }

        public static void Register(HttpConfiguration config, IKernel kernel)
        {
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            var settings = config.Formatters.JsonFormatter.SerializerSettings;
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            settings.Formatting = Newtonsoft.Json.Formatting.Indented;
         
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

           // EnableCorsAttribute cors = new EnableCorsAttribute("*","*","*");
           // config.EnableCors(cors);
        }
    }
}