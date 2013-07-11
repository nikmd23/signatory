using System;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using Signatory.Extensions;
using WorldDomination.Web.Authentication;

namespace Signatory.Controllers
{
    public class UserController : Controller
    {
        [OutputCache(CacheProfile = "2Hours")]
        public async Task<ActionResult> Index(string user)
        {
            var reposUri = string.Format("https://api.github.com/users/{0}/repos?client_id={1}&client_secret={2}", user, ConfigurationManager.AppSettings["GitHubKey"], ConfigurationManager.AppSettings["GitHubSecret"]);
            var userUri = string.Format("https://api.github.com/users/{0}?client_id={1}&client_secret={2}", user, ConfigurationManager.AppSettings["GitHubKey"], ConfigurationManager.AppSettings["GitHubSecret"]);

            var httpClient = new HttpClient();
            var reposRequest = httpClient.GetAsync(reposUri);
            var userRequest = httpClient.GetAsync(userUri);

            Task.WaitAll(new Task[] {reposRequest, userRequest});

            reposRequest.Result.EnsureSuccessStatusCode();
            userRequest.Result.EnsureSuccessStatusCode();

            dynamic repoJson = await reposRequest.Result.Content.ReadAsDynamicJsonAsync();
            // TODO: Handle multiple pages of repos from Link header w/ https://github.com/tavis-software/Link
            dynamic userJson = await userRequest.Result.Content.ReadAsDynamicJsonObjectAsync();

            var tuple = new Tuple<dynamic, dynamic>(userJson, repoJson);

            return View(tuple);
        }
    }
}
