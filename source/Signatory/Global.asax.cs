using System;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Signatory
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        public override string GetVaryByCustomString(HttpContext context, string arg) 
        {
            switch (arg) 
            {
                case "IsRepoOwner":
                    var collabs = context.Cache[string.Format("collab:/{0}", Request.Url.LocalPath)] as string[];

                    if (collabs == null)
                        return false.ToString();

                    var result =
                        collabs.Any(c => c.Equals(User.Identity.Name, StringComparison.InvariantCultureIgnoreCase))
                                .ToString();
                    return result;
                default:
                    return base.GetVaryByCustomString(context, arg);
            }
        }
    }
}