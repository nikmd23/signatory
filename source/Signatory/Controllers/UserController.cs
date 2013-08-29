using System.Threading.Tasks;
using System.Web.Mvc;
using Signatory.Models;
using Signatory.Service;

namespace Signatory.Controllers
{
    public class UserController : Controller
    {
        public IGitHubService GitHubService { get; set; }

        public UserController(IGitHubService gitHubService)
	    {
            GitHubService = gitHubService;
	    }

        //[OutputCache(CacheProfile = "Standard")]
        public async Task<ActionResult> Index(string repoOwner, int page = 1)
        {
            var user = GitHubService.GetUser(repoOwner);
            var repositories = await GitHubService.GetRepositories(repoOwner, page);

            return View(new UserViewModel { User = await user, Repositories = repositories.CurrentPage, Pages = repositories.Pages });
        }
    }
}
