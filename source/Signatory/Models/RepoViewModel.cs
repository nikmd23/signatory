using System.Threading;

namespace Signatory.Models
{
    public class RepoViewModel
    {
        public dynamic User { get; set; }
        public dynamic Collaborators { get; set; }
        public dynamic Repository { get; set; }

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