using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WorldDomination.Web.Authentication.Mvc;

namespace Signatory.Controllers
{
    public class AuthenticationCallbackController : IAuthenticationCallbackProvider
    {
        public ActionResult Process(HttpContextBase context, AuthenticateCallbackData model)
        {
            if (model.Exception != null)
                throw model.Exception;

            var client = model.AuthenticatedClient;
            var username = client.UserInformation.UserName;

            FormsAuthentication.SetAuthCookie(username, false);

            context.Response.AppendCookie(new HttpCookie("AccessToken", client.AccessToken)
            {
                Secure = !context.IsDebuggingEnabled,
                HttpOnly = true
            });

            var urlHelper = new UrlHelper(((MvcHandler)context.Handler).RequestContext);
            var redirectUrl = string.Format("/{0}/", username);
            var cookie = context.Request.Cookies["returnUrl"];
            if (cookie != null && urlHelper.IsLocalUrl(cookie.Value))
            {
                redirectUrl = cookie.Value;
                cookie.Expires = DateTime.Now.AddDays(-1);
                context.Response.Cookies.Add(cookie);
            } 

            return new RedirectResult(redirectUrl);
        }

        public ActionResult OnRedirectToAuthenticationProviderError(HttpContextBase context, string errorMessage)
        {
            throw new Exception(errorMessage);
        }
    }
}