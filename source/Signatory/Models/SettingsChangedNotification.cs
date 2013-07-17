using Signatory.Data;

namespace Signatory.Models
{
    public class SettingsChangedNotification
    {
        public SettingsChangedNotification(string status, Repository repository)
        {
            Status = status;
            RepoName = repository.Name;
            RepoOwner = repository.Owner;
            RequireCla = repository.RequireCla;
        }

        public string Status { get; set; }
        public string RepoName { get; set; }
        public string RepoOwner { get; set; }
        public bool RequireCla { get; set; }
    }
}