using Newtonsoft.Json;

namespace Signatory.Service
{
    public class GitHubRepository
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("html_url")]
        public string GitHubUrl { get; set; }
    }
}