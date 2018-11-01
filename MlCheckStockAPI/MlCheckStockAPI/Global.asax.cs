using CacheManager.Core;
using log4net;
using MlCheckStockAPI.ST_Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;


namespace MlCheckStockAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            string tPath = HttpContext.Current.Server.MapPath("/x");
            var nPath = tPath.IndexOf("x");
            cCNVB.tPath2 = tPath.Substring(0, nPath);

            log4net.Config.XmlConfigurator.Configure();

            cCNVB.oCMConfig = ConfigurationBuilder.BuildConfiguration(settings =>
            {
                settings.WithSystemRuntimeCacheHandle("inProcessCache");

            });
        }
    }
}
