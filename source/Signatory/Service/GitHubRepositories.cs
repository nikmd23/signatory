using System.Collections.Generic;

namespace Signatory.Service
{
    public class GitHubRepositories
    {
        public IEnumerable<GitHubRepository> CurrentPage { get; set; }
        public Pages Pages { get; set; }
    }
}