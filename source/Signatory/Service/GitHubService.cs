using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Signatory.Data;
using Signatory.Extensions;

namespace Signatory.Service
{
    public class GitHubService : IGitHubService
    {
        public HttpClient HttpClient { get; set; }

        public GitHubService(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        public async Task<GitHubUser> GetUser(string repoOwner) 
        {
            var userUri = string.Format("https://api.github.com/users/{0}?client_id={1}&client_secret={2}", repoOwner, ConfigurationManager.AppSettings["GitHubKey"], ConfigurationManager.AppSettings["GitHubSecret"]);

            var request = await HttpClient.GetAsync(userUri);
            request.EnsureSuccessStatusCode();
            return await JsonConvert.DeserializeObjectAsync<GitHubUser>(await request.Content.ReadAsStringAsync());
        }

        public async Task<IEnumerable<GitHubRepository>> GetRepositories(string repoOwner) 
        {
            var reposUri = string.Format("https://api.github.com/users/{0}/repos?client_id={1}&client_secret={2}", repoOwner, ConfigurationManager.AppSettings["GitHubKey"], ConfigurationManager.AppSettings["GitHubSecret"]);
            var request = await HttpClient.GetAsync(reposUri);
            request.EnsureSuccessStatusCode();
            return await JsonConvert.DeserializeObjectAsync<IEnumerable<GitHubRepository>>(await request.Content.ReadAsStringAsync());
        }

        public async Task<GitHubRepository> GetRepository(string repoOwner, string repoName)
        {
            var repoUri = string.Format("https://api.github.com/repos/{0}/{1}?client_id={2}&client_secret={3}", repoOwner, repoName, ConfigurationManager.AppSettings["GitHubKey"], ConfigurationManager.AppSettings["GitHubSecret"]);

            var request = await HttpClient.GetAsync(repoUri);
            request.EnsureSuccessStatusCode();
            return await JsonConvert.DeserializeObjectAsync<GitHubRepository>(await request.Content.ReadAsStringAsync());
        }

        public async Task<IEnumerable<GitHubCollaborator>> GetCollaborators(string repoOwner, string repoName)
        {
            var collaboratorsUri = string.Format("https://api.github.com/repos/{0}/{1}/collaborators?client_id={2}&client_secret={3}", repoOwner, repoName, ConfigurationManager.AppSettings["GitHubKey"], ConfigurationManager.AppSettings["GitHubSecret"]);

            var request = await HttpClient.GetAsync(collaboratorsUri);
            request.EnsureSuccessStatusCode();
            return await JsonConvert.DeserializeObjectAsync<IEnumerable<GitHubCollaborator>>(await request.Content.ReadAsStringAsync());
        }

        public async Task<bool> SetCommitStatus(Repository repository, string commitSha, bool isSuccess, string description, string detailsUrl)
        {
            var commitStatusUri = string.Format("https://api.github.com/repos/{0}/{1}/statuses/{2}?access_token={3}", repository.Owner, repository.Name, commitSha, repository.AccessToken);

            var payload = new { status = isSuccess ? "success" : "error", target_url = detailsUrl, description };

            var request = await HttpClient.PostAsync(commitStatusUri, new StringContent(JsonConvert.SerializeObject(payload)));
            return request.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<GitHubPullRequest>> GetPullRequests(string repoOwner, string repoName)
        {
            var pullRequestsUri = string.Format("https://api.github.com/repos/{0}/{1}/pulls?state=open&client_id={2}&client_secret={3}", repoOwner, repoName, ConfigurationManager.AppSettings["GitHubKey"], ConfigurationManager.AppSettings["GitHubSecret"]);

            var request = await HttpClient.GetAsync(pullRequestsUri);
            request.EnsureSuccessStatusCode();
            var json = await request.Content.ReadAsDynamicJsonAsync();
            var result = new List<GitHubPullRequest>();
            foreach (var pullRequest in json)
            {
                result.Add(new GitHubPullRequest{ RequesterUsername = pullRequest.head.user.login, HeadCommitSha = pullRequest.head.sha });
            }

            return result;
        }
    }
}