using System;
using System.Linq;
using System.Web;

namespace Signatory.Extensions
{
    public static class HttpContextExtensions
    {
        private static string CreateKey(Uri uri)
        {
            var path = string.Join(string.Empty, uri.Segments.Take(3));
            return string.Format("collab:/{0}", path);
        }

        public static string[] GetCollaborators(this HttpContext httpContext)
        {
            return GetCollaborators(new HttpContextWrapper(httpContext));
        }

        public static string[] GetCollaborators(this HttpContextBase httpContext)
        {
            // TODO: Go to GitHub if cache is empty
            return httpContext.Cache[CreateKey(httpContext.Request.Url)] as string[];
        }

        public static void SetCollaborators(this HttpContextBase httpContext, string[] collaborators)
        {
            httpContext.Cache[CreateKey(httpContext.Request.Url)] = collaborators;
        }
    }
}