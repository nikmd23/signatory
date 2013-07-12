using Signatory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Signatory.Controllers
{
    public class RepoController : Controller
    {
        public GitHubService GitHubService { get; set; }

        public RepoController(GitHubService gitHubService)
        {
            GitHubService = gitHubService;
        }

        [OutputCache(CacheProfile = "2Hours")]
        public async Task<ActionResult> Index(string username, string repo)
        {
            var user = GitHubService.GetUser(username);
            var repository = GitHubService.GetRepo(username, repo);
            var collaborators = GitHubService.GetCollaborators(username, repo);

            return View(new RepoViewModel { User = await user, Repository = await repository, Collaborators = await collaborators });
        }

        public async Task<ActionResult> Sign(string username, string repo)
        {
            var user = await GitHubService.GetUser(username);

            return View(new SignViewModel 
                { 
                    FullName = user.name,
                    Email = user.email,
                    Username = user.login,
                    Date = DateTime.Now,
                    Repo = repo
                });
        }

        [HttpPost]
        public async Task<ActionResult> Sign(SignViewModel model) 
        {
            if (ModelState.IsValid)
                return Content(model.Email);
            else
                return View(model);
        }
    }
}
