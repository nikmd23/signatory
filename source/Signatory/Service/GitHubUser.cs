using Newtonsoft.Json;

namespace Signatory.Service
{
    public class GitHubUser
    {
        [JsonProperty("login")]
        public string Username { get; set; }

        [JsonProperty("gravatar_id")]
        public string GravatarId { get; set; }

        [JsonProperty("html_url")]
        public string GitHubUrl { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("blog")]
        public string BlogUrl { get; set; }

        [JsonProperty("email")]
        public string EmailAddress { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("public_repos")]
        public int RepositoryCount { get; set; }
        
    }
}