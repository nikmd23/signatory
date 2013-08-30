using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using Signatory.Data;
using Signatory.Extensions;
using Signatory.Framework;
using Signatory.Service;

namespace Signatory.Controllers
{
    public class HookController : BaseController
    {
        public DataContext DataContext { get; set; }
        public IGitHubService GitHubService { get; set; }

        public HookController(IGitHubService gitHubService, DataContext dataContext)
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
                var task = await GitHubService.SetCommitStatus(repo, commitSha, false,
                                                         string.Format("This repository requires a signed contributor license agreement. Click details to sign."),
                                                         string.Format("{2}/{0}/{1}/sign", repoOwner, repoName, Settings.Authority));
            }
            else
            {
                var task = await GitHubService.SetCommitStatus(repo, commitSha, true,
                              committer + " has signed this repository's CLA.",
                              string.Format("{2}/{0}/{1}/", repoOwner, repoName, Settings.Authority));
            }

            // Wrap in try catch
            // Log?

            var output = string.Format("Commit '{0}' by '{1}' to '{2}/{3}'", commitSha, committer, repoOwner, repoName);

            return Content(output);
        }
    }
}
