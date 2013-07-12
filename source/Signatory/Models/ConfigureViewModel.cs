using System.Threading;

namespace Signatory.Models
{
    public class ConfigureViewModel
    {
        public string Repo { get; set; }
        public string User { get; set; }

        public dynamic Collaborators { get; set; }

        public bool CurrentUserIsCollaborator
        {
            get
            {
                foreach (var collaborator in Collaborators)
                {
                    if (collaborator.login == Thread.CurrentPrincipal.Identity.Name)
                        return true;
                }

                return false;
            }
        }
    }
}