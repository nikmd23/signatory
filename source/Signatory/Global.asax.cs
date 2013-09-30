using System;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Signatory.Extensions;
using Signatory.Framework;

namespace Signatory
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            ValueProviderFactories.Factories.Add(new CookieValueProviderFactory());

            MvcHandler.DisableMvcResponseHeader = true;

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
                case "IsRepoCollaborator":
                    var collabs = context.GetCollaborators();

                    if (collabs == null)
                        return false.ToString();

                    var result = collabs.Any(c => c.Equals(User.Identity.Name, StringComparison.InvariantCultureIgnoreCase)).ToString();
                    return result;
                default:
                    return base.GetVaryByCustomString(context, arg);
            }
        }

        protected void Application_PreSendRequestHeaders(object sender, EventArgs e)
        {
            var httpRuntime = sender as HttpApplication;

            if (httpRuntime != null)
                httpRuntime.Context.Response.Headers.Remove("Server");
        }

        protected void Application_BeginRequest(Object sender, EventArgs e)
        {
            switch (Request.Url.Scheme)
            {
                case "https":
                    Response.AddHeader("Strict-Transport-Security", "max-age=3600"); // one hour
                    break;
                case "http":
                    var path = "https://" + Request.Url.Host + Request.Url.PathAndQuery;
                    Response.Status = "301 Moved Permanently";
                    Response.AddHeader("Location", path);
                    break;
            }
        }
    }
}