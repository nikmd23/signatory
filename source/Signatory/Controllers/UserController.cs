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

        [OutputCache(CacheProfile = "Standard")]
        public async Task<ActionResult> Index(string repoOwner)
        {
            var user = GitHubService.GetUser(repoOwner);
            var repositories = GitHubService.GetRepositories(repoOwner);

            return View(new UserViewModel { User = await user, Repositories = await repositories });
        }
    }
}
