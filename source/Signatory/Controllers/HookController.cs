using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using Signatory.Data;
using Signatory.Extensions;
using Signatory.Models;

namespace Signatory.Controllers
{
    public class HookController : Controller
    {
        public DataContext DataContext { get; set; }
        public GitHubService GitHubService { get; set; }

        public HookController(GitHubService gitHubService, DataContext dataContext)
        {
            GitHubService = gitHubService;
            DataContext = dataContext;
        }

        [HttpPost]
        public async Task<ActionResult> WebHook(string payload)
        {
            dynamic json = JObject.Parse(payload);
            string committer = json.head_commit.committer.username;
            string commitSha = json.head_commit.id;
            string repoName = json.repository.name;
            string repoOwner = json.repository.owner.name;

            // TODO: Log commit activity

            // Make sure repoOwner/repoName is configured for SLA's and get access_token -> abort if not
            var repo = DataContext.Repositories.Where(repoOwner, repoName);
            if (repo == null)
                return Content(string.Format("{0}/{1} is not configured to require CLA's.", repoOwner, repoName));

            var signature = DataContext.Signatures.Where(repoOwner, repoName, committer);
            if (signature == null)
            {
                var task = await GitHubService.SetCommitStatus(repoOwner, repoName, commitSha, "error",
                                                         string.Format(
                                                             "This repository requires a signed contributor license agreement. Click details to sign."),
                                                         string.Format("http://localhost:51692/{0}/{1}/sign", repoOwner,
                                                                       repoName),
                                                         repo.AccessToken);
            }
            else
            {
                var task = await GitHubService.SetCommitStatus(repoOwner, repoName, commitSha, "success",
                              committer + " has signed this repository's CLA.",
                              string.Format("http://localhost:51692/{0}/{1}/", repoOwner, repoName),
                              repo.AccessToken);
            }

            // Wrap in try catch
            // Log?

            var output = string.Format("Commit '{0}' by '{1}' to '{2}/{3}'", commitSha, committer, repoOwner, repoName);

            return Content(output);
        }
    }
}
