using Signatory.Extensions;
using Signatory.Framework;
using Signatory.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        [OutputCache(CacheProfile = "Collaborator")]
        public async Task<ActionResult> Index(string username, string repo)
        {
            var user = GitHubService.GetUser(username);
            var repository = GitHubService.GetRepo(username, repo);
            var collaborators = await GitHubService.GetCollaborators(username, repo);

            // TODO: Move this data caching into a repo wrapper?
            var collabCache = new List<string>();
            foreach(var collaborator in collaborators)
                collabCache.Add((string)collaborator.login);

            HttpContext.SetCollaborators(collabCache.ToArray());

            return View(new RepoViewModel { User = await user, Repository = await repository, Collaborators = collaborators });
        }

        [OutputCache(CacheProfile = "Standard"), Authorize]
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

        [HttpPost, Authorize]
        public async Task<ActionResult> Sign(SignViewModel model) 
        {
            if (ModelState.IsValid)
                return Content(model.Email);
            else
                return View(model);
        }

        [AuthorizeCollaborator]
        public async Task<ActionResult> Settings(string username, string repo)
        {
            var collaborators = await GitHubService.GetCollaborators(username, repo);

            // TODO: Move this data caching into a repo wrapper?
            var collabCache = new List<string>();
            foreach (var collaborator in collaborators)
                collabCache.Add((string)collaborator.login);

            HttpContext.SetCollaborators(collabCache.ToArray());


            return View(new ConfigureViewModel
                {
                    User = username,
                    Repo = repo,
                    Collaborators = collaborators
                });
        }
    }
}
