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

            return HttpClient.GetAsync(userUri).ContinueWith(request => request.Result.Content.ReadAsDynamicJsonObjectAsync()).Unwrap();
        }

        public Task<dynamic> GetRepos(string username) 
        {
            var reposUri = string.Format("https://api.github.com/users/{0}/repos?client_id={1}&client_secret={2}", username, ConfigurationManager.AppSettings["GitHubKey"], ConfigurationManager.AppSettings["GitHubSecret"]);

            return HttpClient.GetAsync(reposUri).ContinueWith(request => request.Result.Content.ReadAsDynamicJsonAsync()).Unwrap();
        }
    }
}