using pbx_lib;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using pbx_web.HelperClasses;

namespace pbx_web
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        public static pbx _pbx { get; set; }


        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            _pbx = pbx_factory.getpbx();

        }
    }
}
