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

        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();

            return RedirectToAction("Index");
        }

        public ActionResult Examples()
        {
            return View();
        }
    }
}
