using System.Web.Mvc;
using System.Web.Routing;
using SimpleAspNetApp.Services;

namespace SimpleAspNetApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            MessagesStorage.LoadFromFile();
        }
    }
}
