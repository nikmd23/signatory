using System.Threading.Tasks;
using System.Web.Mvc;
using Signatory.Models;

namespace Signatory.Controllers
{
    public class UserController : Controller
    {
        public GitHubService GitHubService { get; set; }

        public UserController(GitHubService gitHubService)
	    {
            GitHubService = gitHubService;
	    }

        [OutputCache(CacheProfile = "Standard")]
        public async Task<ActionResult> Index(string repoOwner)
        {
            var user = GitHubService.GetUser(repoOwner);
            var repositories = GitHubService.GetRepos(repoOwner);

            return View(new UserViewModel { User = await user, Repositories = await repositories });
        }
    }
}
