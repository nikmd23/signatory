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
            var session = HttpContext.Current.Session;

            session["token"] = client.AccessToken;
            session["username"] = client.UserInformation.UserName;

            FormsAuthentication.SetAuthCookie(username, true);

            return new RedirectResult("/" + username + "/");
        }

        public ActionResult OnRedirectToAuthenticationProviderError(HttpContextBase context, string errorMessage)
        {
            throw new System.NotImplementedException("There was an error!");
        }
    }
}