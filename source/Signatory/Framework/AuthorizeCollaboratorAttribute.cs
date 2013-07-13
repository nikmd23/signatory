using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Signatory.Extensions;

namespace Signatory.Framework
{
    public class AuthorizeCollaboratorAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var collabs = httpContext.GetCollaborators();

            if (collabs == null)
                return false;

            var result =
                collabs.Any(c => c.Equals(httpContext.User.Identity.Name, StringComparison.InvariantCultureIgnoreCase));
            return result;
        }
    }
}