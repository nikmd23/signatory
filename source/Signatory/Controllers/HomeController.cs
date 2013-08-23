using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Signatory.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult Navigation()
        {
            return PartialView(User.Identity);
        }

        public ActionResult SignIn(string returnUrl = null)
        {
            if (returnUrl != null && !User.Identity.IsAuthenticated)
                Response.Cookies.Add(new HttpCookie("returnUrl", returnUrl));

            return View();
        }

        [Authorize]
        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();

            return Redirect("/");
        }

        public ActionResult Resources()
        {
            return View();
        }

        public ActionResult Error(string aspxerrorpath)
        {
            var errorMessage = string.Format("We're not quite sure that happened, but requests to '{0}' may be invalid. Please try again in a few minutes.", aspxerrorpath);
            return View(new HandleErrorInfo(new Exception(errorMessage), "Home", "Error"));
        }
    }
}
