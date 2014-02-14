using System.Web.Mvc;
using System.Web.Routing;

// ReSharper disable CheckNamespace
namespace Signatory
// ReSharper restore CheckNamespace
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Hook",
                url: "hook",
                defaults: new { controller = "Hook", action = "WebHook" });

            routes.MapRoute(
                name: "Home",
                url: "Home/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional });

            routes.MapRoute(
                name: "RepoOwner",
                url: "{repoOwner}",
                defaults: new { controller = "User", action = "Index" });

            routes.MapRoute(
                name: "Repo",
                url: "{repoOwner}/{repoName}/{action}",
                defaults: new { controller = "Repo", action = "Index" });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional });
        }
    }
}