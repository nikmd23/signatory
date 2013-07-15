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
        public async Task<ActionResult> Index(string username, string repo)
        {
            var user = GitHubService.GetUser(username);
            var repository = GitHubService.GetRepo(username, repo);
            var collaborators = GitHubService.GetCollaborators(username, repo);
            var signers = DataContext.Signatures.Where(username, repo).ToList();

            var viewModel = new RepoViewModel(await user, await repository, await collaborators, User.Identity, signers);

            // TODO: Move this data caching into a repo wrapper?
            HttpContext.SetCollaborators(viewModel.Collaborators.Select(c => c.Username).ToArray());
            
            return View(viewModel);
        }

        [OutputCache(CacheProfile = "Standard"), Authorize]
        public async Task<ActionResult> Sign(string username, string repo)
        {
            var repository = DataContext.Repositories.Where(username, repo);
            if (repository == null)
                ViewBag.NotConfigured = true;
            else
                ViewBag.LicenseText = repository.LicenseText;

            var signature = DataContext.Signatures.Where(repo, User.Identity);
            if (signature != null)
                ViewBag.AlreadySigned = true;

            //TODO: Fix signature when in disabled state

            var user = await GitHubService.GetUser(User.Identity.Name);

            var viewModel = new SignViewModel
                {
                    FullName = user.name, Email = user.email, Username = user.login, Date = DateTime.Now, Repo = repo
                };

            return View(viewModel);
        }

        [HttpPost, Authorize]
        public async Task<ActionResult> Sign(SignViewModel model) 
        {
            var repository = DataContext.Repositories.Where(model.Username, model.Repo);
            ViewBag.LicenseText = repository.LicenseText;

            if (!ModelState.IsValid)
                return View(model);

            if (repository == null) // can't save if the repo doesn't require CLA's
                return new RedirectResult(string.Format("/{0}/{1}/sign/", model.Username, model.Repo));

            var user = DataContext.Signatures.Where(model.Repo, User.Identity);
            if (user != null)// redirect user if they have already signed the CLA (CLA's can't be changed
            {
                TempData["userSigned"] = new UserSignedNotification("success", User.Identity.Name);
                return new RedirectResult(string.Format("/{0}/{1}/", model.Username, model.Repo));
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

            // TODO: update all commits by user to Success commit status
            // TODO: Setup/move validation to EF model?
            // TODO: Support ajax submit?
            // TODO: Wrap in a try/catch

            TempData["userSigned"] = new UserSignedNotification("success", User.Identity.Name);
            return Redirect(string.Format("/{0}/{1}/", model.Username, model.Repo));
        }

        [OutputCache(CacheProfile = "Standard"), AuthorizeCollaborator]
        public ActionResult Settings(string username, string repo)
        {
            var mdPath = HttpContext.Server.MapPath("~/Content/apache-cla.md");

            var repository = DataContext.Repositories.Where(username, repo);
            var viewModel = SettingsViewModel.From(repository) ?? new SettingsViewModel(mdPath) { Username = username, Repo = repo };

            return View(viewModel);
        }

        [HttpPost, AuthorizeCollaborator]
        public ActionResult Settings(SettingsViewModel model)
        {
            if (!ModelState.IsValid) 
                return View(model);

            // TODO: Setup/move validation to EF model?
            // TODO: Support ajax submit?
            // TODO: Wrap in a try/catch

            var repository = DataContext.Repositories.Where(model.Username, model.Repo);
            if (repository != null)
            {
                repository.AccessToken = model.AccessToken;
                repository.RequireCla = model.RequireCla;
                repository.LicenseText = model.LicenseText.Replace("{repo}", model.Repo);
            }
            else
            {
                repository = new Repository(
                    username: model.Username,
                    name: model.Repo, 
                    requireCla: model.RequireCla, 
                    accessToken: model.AccessToken,
                    licenseText: model.LicenseText.Replace("{repo}", model.Repo));
                DataContext.Repositories.Add(repository);
            }

            DataContext.SaveChanges();

            TempData["settingsChanged"] = new SettingsChangedNotification(repository.RequireCla ? "success" : "warning", repository);
            return Redirect(string.Format("/{0}/{1}/", model.Username, model.Repo));
        }
    }
}
