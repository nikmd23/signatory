using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using Signatory.Models;

namespace Signatory.Controllers
{
    public class HookController : Controller
    {
        public GitHubService GitHubService { get; set; }

        public HookController(GitHubService gitHubService)
        {
            GitHubService = gitHubService;
        }

        [HttpPost]
        public async Task<ActionResult> WebHook(string payload)
        {
            dynamic json = JObject.Parse(payload);
            string committer = json.head_commit.committer.username;
            string commitSha = json.head_commit.id;
            string repoName = json.repository.name;
            string repoOwner = json.repository.owner.name;

            var result = await GitHubService.SetCommitStatus(repoOwner, repoName, commitSha, "pending", "This is a smoketest for " + committer, "http://www.google.com/", "8d4de479473149e89a9c5e546068ace7d0096cb0");

            // TODO: Log commit activity

            // Make sure repoOwner/repoName is configured for SLA's and get access_token -> abort if not

            // Check to see if committer has signed the SLA for repoOwner/repoName
            // If so -> update comitSha to Success
            // If not -> update commitSha to Error w/ link to repoOwner/repoName/sign

            // Wrap in try catch
            // Log?

            var output = string.Format("Commit '{0}' by '{1}' to '{2}/{3}'", commitSha, committer, repoOwner, repoName);

            return Content(output);
        }
    }
}
