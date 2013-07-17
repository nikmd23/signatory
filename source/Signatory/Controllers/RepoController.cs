using System.Collections.Generic;
using Signatory.Data;
using Signatory.Extensions;
using Signatory.Framework;
using Signatory.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Signatory.Controllers
{
    public class RepoController : Controller
    {
        public GitHubService GitHubService { get; set; }
        public DataContext DataContext { get; set; }

        public RepoController(GitHubService gitHubService, DataContext dataContext)
        {
            GitHubService = gitHubService;
            DataContext = dataContext;
        }

        [OutputCache(CacheProfile = "Collaborator")]
        public async Task<ActionResult> Index(string repoOwner, string repoName)
        {
            var user = GitHubService.GetUser(repoOwner);
            var repository = GitHubService.GetRepo(repoOwner, repoName);
            var collaborators = GitHubService.GetCollaborators(repoOwner, repoName);
            var signers = DataContext.Signatures.Where(repoOwner, repoName).ToList();

            var viewModel = new RepoViewModel(await user, await repository, await collaborators, User.Identity, signers);

            // TODO: Move this data caching into a repo wrapper?
            HttpContext.SetCollaborators(viewModel.Collaborators.Select(c => c.Username).ToArray());
            
            return View(viewModel);
        }

        [OutputCache(CacheProfile = "Standard"), Authorize]
        public async Task<ActionResult> Sign(string repoOwner, string repoName)
        {
            var repository = DataContext.Repositories.Where(repoOwner, repoName);
            if (repository == null)
                ViewBag.NotConfigured = true;
            else
                ViewBag.LicenseText = repository.LicenseText;

            var signature = DataContext.Signatures.Where(repoOwner, repoName, User.Identity);
            if (signature != null)
                ViewBag.AlreadySigned = true;

            //TODO: Fix signature when in disabled state

            var user = await GitHubService.GetUser(User.Identity.Name);

            var viewModel = new SignViewModel
                {
                    FullName = user.name, Email = user.email, Username = user.login, Date = DateTime.Now, RepoName = repoName
                };

            return View(viewModel);
        }

        [HttpPost, Authorize]
        public async Task<ActionResult> Sign(SignViewModel model) 
        {
            var repository = DataContext.Repositories.Where(model.Username, model.RepoName);
            ViewBag.LicenseText = repository.LicenseText;

            if (!ModelState.IsValid)
                return View(model);

            if (repository == null) // can't save if the repo doesn't require CLA's
                return new RedirectResult(string.Format("/{0}/{1}/sign/", model.Username, model.RepoName));

            var redirectUrl = string.Format("/{0}/{1}/", model.Username, model.RepoName);

            var user = DataContext.Signatures.Where(model.Username, model.RepoName, User.Identity);
            if (user != null)// redirect user if they have already signed the CLA (CLA's can't be changed
            {
                TempData["userSigned"] = new UserSignedNotification("success", User.Identity.Name);
                return new RedirectResult(redirectUrl);
            } 

            DataContext.Signatures.Add(new Signature
                {
                    Address = model.Address,
                    Country = model.Country,
                    DateSigned = DateTime.Now,
                    Email = model.Email,
                    FullName = model.FullName,
                    RepositoryId = repository.Id,
                    SignatureImage = model.Signature,
                    TelephoneNumber = model.TelephoneNumber,
                    Username = User.Identity.Name
                });

            DataContext.SaveChanges();

            var commitUpdates = await UpdateCommitsFor(repository.Owner, repository.Name, User.Identity.Name, repository.AccessToken);

            Task.WaitAll(commitUpdates.ToArray());
            
            // TODO: Setup/move validation to EF model?
            // TODO: Support ajax submit?
            // TODO: Wrap in a try/catch
            // TODO: Kill output cache

            Response.RemoveOutputCacheItem(redirectUrl);

            TempData["userSigned"] = new UserSignedNotification("success", User.Identity.Name);
            return Redirect(redirectUrl);
        }

        private async Task<IEnumerable<Task<dynamic>>>  UpdateCommitsFor(string repoOwner, string repoName, string committer, string accessToken)
        {
            var pullRequests = await GitHubService.GetPullRequests(repoOwner, repoName);

            var tasks = new List<Task<dynamic>>();

            foreach (var pullRequest in pullRequests)
            {
                if (((string)pullRequest.user.login).Equals(committer, StringComparison.InvariantCultureIgnoreCase))
                {
                    var task = GitHubService.SetCommitStatus(repoOwner, repoName, (string)pullRequest.head.sha, "success",
                                                  committer + " has signed this repository's CLA.",
                                                  string.Format("http://localhost:51692/{0}/{1}/", repoOwner, repoName),
                                                  accessToken);
                }
            }

            return tasks;
        }

        [HttpPost, AuthorizeCollaborator]
        public ActionResult Settings(SettingsViewModel model)
        {
            if (!ModelState.IsValid) 
                return View(model);

            // TODO: Setup/move validation to EF model?
            // TODO: Support ajax submit?
            // TODO: Wrap in a try/catch

            var repository = DataContext.Repositories.Where(model.RepoOwner, model.RepoName);
            if (repository != null)
            {
                repository.AccessToken = model.AccessToken;
                repository.RequireCla = model.RequireCla;
                repository.LicenseText = model.LicenseText.Replace("{repo}", model.RepoName);
            }
            else
            {
                repository = new Repository(
                    owner: model.RepoOwner,
                    name: model.RepoName, 
                    requireCla: model.RequireCla, 
                    accessToken: model.AccessToken,
                    licenseText: model.LicenseText.Replace("{repo}", model.RepoName));
                DataContext.Repositories.Add(repository);
            }

            DataContext.SaveChanges();

            TempData["settingsChanged"] = new SettingsChangedNotification(repository.RequireCla ? "success" : "warning", repository);
            return Redirect(string.Format("/{0}/{1}/", model.RepoOwner, model.RepoName));
        }

        [OutputCache(CacheProfile = "Standard"), AuthorizeCollaborator]
        public ActionResult Settings(string repoOwner, string repoName)
        {
            var mdPath = HttpContext.Server.MapPath("~/Content/apache-cla.md");

            var repository = DataContext.Repositories.Where(repoOwner, repoName);
            var viewModel = SettingsViewModel.From(repository) ?? new SettingsViewModel(mdPath) { RepoOwner = repoOwner, RepoName = repoName };

            return View(viewModel);
        }
    }
}
