using System.Collections.Generic;
using Signatory.Data;
using Signatory.Extensions;
using Signatory.Framework;
using Signatory.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Signatory.Service;

namespace Signatory.Controllers
{
    public class RepoController : Controller
    {
        public IGitHubService GitHubService { get; set; }
        public DataContext DataContext { get; set; }

        public RepoController(IGitHubService gitHubService, DataContext dataContext)
        {
            GitHubService = gitHubService;
            DataContext = dataContext;
        }

        //[OutputCache(CacheProfile = "Collaborator")]
        public async Task<ActionResult> Index(string repoOwner, string repoName)
        {
            var user = GitHubService.GetUser(repoOwner);
            var repository = GitHubService.GetRepository(repoOwner, repoName);
            var collaborators = GitHubService.GetCollaborators(repoOwner, repoName);
            var signers = DataContext.Signatures.Where(repoOwner, repoName).ToList();

            var viewModel = new RepoViewModel(await user, await repository, await collaborators, User.Identity, signers);

            // TODO: Move this data caching into a repo wrapper?
            HttpContext.SetCollaborators(viewModel.Collaborators.Select(c => c.Username).ToArray());
            
            return View(viewModel);
        }

        //[OutputCache(CacheProfile = "Standard"), Authorize]
        [Authorize]
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
                    FullName = user.Name, Email = user.EmailAddress, Date = DateTime.Now, RepoOwner = repoOwner, RepoName = repoName
                };

            return View(viewModel);
        }

        [HttpPost, Authorize, ValidateAntiForgeryToken]
        public async Task<ActionResult> Sign(SignViewModel model, string repoOwner, string repoName) 
        {
            var repository = DataContext.Repositories.Where(repoOwner, repoName);
            ViewBag.LicenseText = repository.LicenseText;

            if (!ModelState.IsValid)
                return View(model);

            if (repository == null) // can't save if the repo doesn't require CLA's
                return new RedirectResult(string.Format("/{0}/{1}/sign/", repoOwner, repoName));

            var redirectUrl = string.Format("/{0}/{1}/", repoOwner, repoName);

            var user = DataContext.Signatures.Where(repoOwner, repoName, User.Identity);
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

            var commitUpdates = await UpdateCommitsFor(repository, User.Identity.Name);

            await Task.WhenAll(commitUpdates.ToArray());

            // TODO: Setup/move validation to EF model?
            // TODO: Support ajax submit?
            // TODO: Wrap in a try/catch
            // TODO: Kill output cache

            Response.RemoveOutputCacheItem(redirectUrl);

            TempData["userSigned"] = new UserSignedNotification("success", User.Identity.Name);
            return Redirect(redirectUrl);
        }

        private async Task<IEnumerable<Task<bool>>> UpdateCommitsFor(Repository repository, string committer)
        {
            var pullRequests = await GitHubService.GetPullRequests(repository.Owner, repository.Name);

            var tasks = new List<Task<bool>>();

            foreach (var pullRequest in pullRequests.Where(pr => pr.RequesterUsername.Equals(committer, StringComparison.InvariantCultureIgnoreCase)))
            {
                    tasks.Add(GitHubService.SetCommitStatus(repository, pullRequest.HeadCommitSha, true,
                                                  committer + " has signed this repository's CLA.",
                                                  string.Format("{2}/{0}/{1}/", repository.Owner, repository.Name, Signatory.Settings.Authority)));
            }

            return tasks;
        }

        [HttpPost, AuthorizeCollaborator, ValidateAntiForgeryToken]
        public async Task<ActionResult> Settings(SettingsViewModel model)
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

            await GitHubService.EnableWebHook(repository);

            TempData["settingsChanged"] = new SettingsChangedNotification(repository.RequireCla ? "success" : "warning", repository);
            return Redirect(string.Format("/{0}/{1}/", model.RepoOwner, model.RepoName));
        }

        //[OutputCache(CacheProfile = "Standard"), AuthorizeCollaborator]
        [AuthorizeCollaborator]
        public ActionResult Settings(string repoOwner, string repoName)
        {
            var mdPath = HttpContext.Server.MapPath("~/Content/apache-cla.md");

            var repository = DataContext.Repositories.Where(repoOwner, repoName);
            var viewModel = SettingsViewModel.From(repository) ?? new SettingsViewModel(mdPath) { RepoOwner = repoOwner, RepoName = repoName };

            return View(viewModel);
        }
    }
}
