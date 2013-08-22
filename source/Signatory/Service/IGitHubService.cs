using System.Collections.Generic;
using System.Threading.Tasks;
using Signatory.Data;

namespace Signatory.Service
{
    public interface IGitHubService
    {
        Task<GitHubUser> GetUser(string repoOwner);

        Task<IEnumerable<GitHubRepository>> GetRepositories(string repoOwner);

        Task<GitHubRepository> GetRepository(string repoOwner, string repoName);

        Task<IEnumerable<GitHubCollaborator>> GetCollaborators(string repoOwner, string repoName);

        Task<bool> SetCommitStatus(Repository repository, string commitSha, bool isSuccess, string description, string detailsUrl);

        Task<IEnumerable<GitHubPullRequest>> GetPullRequests(string repoOwner, string repoName);

        Task<bool> EnableWebHook(Repository repository);
    }
}