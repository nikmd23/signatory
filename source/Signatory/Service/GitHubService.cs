using System;
using System.Collections.Generic;
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
            var userUri = string.Format("https://api.github.com/users/{0}?client_id={1}&client_secret={2}", repoOwner, Settings.GitHubKey, Settings.GitHubSecret);

            var request = await HttpClient.GetAsync(userUri);
            request.EnsureSuccessStatusCode();
            return await JsonConvert.DeserializeObjectAsync<GitHubUser>(await request.Content.ReadAsStringAsync());
        }

        public async Task<GitHubRepositories> GetRepositories(string repoOwner, int page = 1) 
        {
            // TODO: Switch to URI templates
            var reposUri = string.Format("https://api.github.com/users/{0}/repos?client_id={1}&client_secret={2}{3}", repoOwner, Settings.GitHubKey, Settings.GitHubSecret, page > 1 ? string.Format("&page={0}", page) : string.Empty);
            var request = await HttpClient.GetAsync(reposUri);
            request.EnsureSuccessStatusCode();

            return new GitHubRepositories
                {
                    CurrentPage = await JsonConvert.DeserializeObjectAsync<IEnumerable<GitHubRepository>>(await request.Content.ReadAsStringAsync()),
                    Pages = new Pages(request.Headers),
                };
        }

        public async Task<GitHubRepository> GetRepository(string repoOwner, string repoName)
        {
            var repoUri = string.Format("https://api.github.com/repos/{0}/{1}?client_id={2}&client_secret={3}", repoOwner, repoName, Settings.GitHubKey, Settings.GitHubSecret);

            var request = await HttpClient.GetAsync(repoUri);
            request.EnsureSuccessStatusCode();
            return await JsonConvert.DeserializeObjectAsync<GitHubRepository>(await request.Content.ReadAsStringAsync());
        }

        public async Task<IEnumerable<GitHubCollaborator>> GetCollaborators(string repoOwner, string repoName)
        {
            var collaboratorsUri = string.Format("https://api.github.com/repos/{0}/{1}/collaborators?client_id={2}&client_secret={3}", repoOwner, repoName, Settings.GitHubKey, Settings.GitHubSecret);

            var request = await HttpClient.GetAsync(collaboratorsUri);
            request.EnsureSuccessStatusCode();
            return await JsonConvert.DeserializeObjectAsync<IEnumerable<GitHubCollaborator>>(await request.Content.ReadAsStringAsync());
        }

        public async Task<bool> SetCommitStatus(Repository repository, string commitSha, bool isSuccess, string description, string detailsUrl)
        {
            var commitStatusUri = string.Format("https://api.github.com/repos/{0}/{1}/statuses/{2}?access_token={3}", repository.Owner, repository.Name, commitSha, repository.AccessToken);

            var payload = new { state = isSuccess ? "success" : "error", target_url = detailsUrl, description };

            var request = await HttpClient.PostAsync(commitStatusUri, new StringContent(JsonConvert.SerializeObject(payload)));
            return request.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<GitHubPullRequest>> GetPullRequests(string repoOwner, string repoName)
        {
            var pullRequestsUri = string.Format("https://api.github.com/repos/{0}/{1}/pulls?state=open&client_id={2}&client_secret={3}", repoOwner, repoName, Settings.GitHubKey, Settings.GitHubSecret);

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

        public async Task<bool> EnableWebHook(Repository repository)
        {
            var createHook = string.Format("https://api.github.com/repos/{0}/{1}/hooks?access_token={2}", repository.Owner, repository.Name, repository.AccessToken);

            var payload = new { name = "web", active = true, events = new[] { "pull_request" }, config = new { url = Settings.Authority + "hook", content_type = "json", secret = Guid.NewGuid().ToString()} };

            var serializeObject = JsonConvert.SerializeObject(payload);
            var request = await HttpClient.PostAsync(createHook, new StringContent(serializeObject));
            return request.IsSuccessStatusCode;
        }
    }
}