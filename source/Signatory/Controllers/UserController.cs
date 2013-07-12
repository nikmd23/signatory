using System;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using Signatory.Extensions;
using Signatory.Models;
using WorldDomination.Web.Authentication;

namespace Signatory.Controllers
{
    public class UserController : Controller
    {
        public GitHubService GitHubService { get; set; }

        public UserController(GitHubService gitHubService)
	    {
            GitHubService = gitHubService;
	    }

        //[OutputCache(CacheProfile = "Standard")]
        public async Task<ActionResult> Index(string username)
        {
            var user = GitHubService.GetUser(username);
            var repositories = GitHubService.GetRepos(username);

            return View(new UserViewModel { User = await user, Repositories = await repositories });
        }
    }
}
