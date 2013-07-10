using System.Web;
using System.Web.Mvc;
using WorldDomination.Web.Authentication.Mvc;

namespace Signatory.Controllers
{
    public class AuthenticationCallbackController : IAuthenticationCallbackProvider
    {
        public ActionResult Process(HttpContextBase context, AuthenticateCallbackData model)
        {
            return new ViewResult
                {
                    ViewName = "AuthenticateCallback",
                    ViewData = new ViewDataDictionary(model)
                };
        }

        public ActionResult OnRedirectToAuthenticationProviderError(HttpContextBase context, string errorMessage)
        {
            throw new System.NotImplementedException("There was an error!");
        }
    }
}