using InternWebApi.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Optimization;
using DependencyResolver;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Ninject;
using IData;
using IRepository;

namespace InternWebApi
{
    public class Global : System.Web.HttpApplication
    {
        public object WebAPIConfig { get; private set; }

        protected void Application_Start(object sender, EventArgs e)
        {
            var kernel = WebApiConfig.Register(GlobalConfiguration.Configuration);
            ControllerBuilder.Current.SetControllerFactory(new NinjectControllerFactory(kernel));
            GlobalConfiguration.Configuration.EnsureInitialized();
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            
        }
        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}