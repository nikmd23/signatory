using Newtonsoft.Json;
using Signatory.Extensions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace Signatory.Models
{
    public class GitHubService
    {
        public HttpClient HttpClient { get; set; }

        public GitHubService(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        public Task<dynamic> GetUser(string username) 
        {
            var userUri = string.Format("https://api.github.com/users/{0}?client_id={1}&client_secret={2}", username, ConfigurationManager.AppSettings["GitHubKey"], ConfigurationManager.AppSettings["GitHubSecret"]);

            return HttpClient.GetAsync(userUri).ContinueWith(request => {
                request.Result.EnsureSuccessStatusCode();
                return request.Result.Content.ReadAsDynamicJsonObjectAsync();
            }).Unwrap();
        }

        public Task<dynamic> GetRepos(string username) 
        {
            var reposUri = string.Format("https://api.github.com/users/{0}/repos?client_id={1}&client_secret={2}", username, ConfigurationManager.AppSettings["GitHubKey"], ConfigurationManager.AppSettings["GitHubSecret"]);

            return HttpClient.GetAsync(reposUri).ContinueWith(request => {
                request.Result.EnsureSuccessStatusCode();
                return request.Result.Content.ReadAsDynamicJsonAsync();
            }).Unwrap();
        }

        public Task<dynamic> GetRepo(string username, string repo)
        {
            var repoUri = string.Format("https://api.github.com/repos/{0}/{1}?client_id={2}&client_secret={3}", username, repo, ConfigurationManager.AppSettings["GitHubKey"], ConfigurationManager.AppSettings["GitHubSecret"]);

            return HttpClient.GetAsync(repoUri).ContinueWith(request => {
                request.Result.EnsureSuccessStatusCode();
                return request.Result.Content.ReadAsDynamicJsonObjectAsync();
            }).Unwrap();
        }

        public Task<dynamic> GetCollaborators(string username, string repo)
        {
            var collaboratorsUri = string.Format("https://api.github.com/repos/{0}/{1}/collaborators?client_id={2}&client_secret={3}", username, repo, ConfigurationManager.AppSettings["GitHubKey"], ConfigurationManager.AppSettings["GitHubSecret"]);

            return HttpClient.GetAsync(collaboratorsUri).ContinueWith(request => {
                request.Result.EnsureSuccessStatusCode();
                return request.Result.Content.ReadAsDynamicJsonAsync();
            }).Unwrap();
        }

        public Task<dynamic> SetCommitStatus(string username, string repo, string sha, string state, string description, string url, string accessToken)
        {
            var commitStatusUri = string.Format("https://api.github.com/repos/{0}/{1}/statuses/{2}?access_token={3}", username, repo, sha, accessToken);

            var payload = new { state, target_url = url, description };

            return HttpClient.PostAsync(commitStatusUri, new StringContent(JsonConvert.SerializeObject(payload))).ContinueWith(request =>
            {
                request.Result.EnsureSuccessStatusCode();
                return request.Result.Content.ReadAsDynamicJsonObjectAsync();
            }).Unwrap();
        }
    }
}