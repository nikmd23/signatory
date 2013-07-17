using Newtonsoft.Json;

namespace Signatory.Service
{
    public class GitHubCollaborator
    {
        [JsonProperty("login")]
        public string Username { get; set; }
    }
}