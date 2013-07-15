using Signatory.Data;

namespace Signatory.Models
{
    public class SettingsChangedNotification
    {
        public SettingsChangedNotification(string status, Repository repository)
        {
            Status = status;
            Repo = repository.Name;
            User = repository.Username;
            RequireCla = repository.RequireCla;
        }

        public string Status { get; set; }
        public string Repo { get; set; }
        public string User { get; set; }
        public bool RequireCla { get; set; }
    }
}