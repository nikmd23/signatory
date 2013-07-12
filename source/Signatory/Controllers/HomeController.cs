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

        public ActionResult Examples()
        {
            return View();
        }
    }
}
