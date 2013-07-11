using System.Web.Mvc;
using System.Web.Routing;
using WorldDomination.Web.Authentication.Mvc;

// ReSharper disable CheckNamespace
namespace Signatory
// ReSharper restore CheckNamespace
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            WorldDominationRouteConfig.RegisterRoutes(routes);

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "User",
                url: "{user}",
                defaults: new { controller = "User", action = "Index" });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional });
        }
    }
}