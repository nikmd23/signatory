using System.Collections.Generic;
using Signatory.Service;

namespace Signatory.Models
{
    public class UserViewModel
    {
        public GitHubUser User { get; set; }
        public IEnumerable<GitHubRepository> Repositories { get; set; }
        public Pages Pages { get; set; }
    }
}