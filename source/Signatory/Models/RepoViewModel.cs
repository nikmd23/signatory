using Signatory.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

namespace Signatory.Models
{
    public class RepoViewModel
    {
        public RepoViewModel(dynamic user, dynamic repository, dynamic collaborators, IIdentity currentUser, IEnumerable<Signature> signers)
        {
            User = new UserSubModel(user);
            Repository = new RepositorySubModel(repository);

            var collabs = new List<CollaboratorSubModel>();
            foreach (var collaborator in collaborators)
                collabs.Add(new CollaboratorSubModel(collaborator));

            Collaborators = collabs;
            CurrentUser = currentUser;
            Signers = signers;
        }

        public UserSubModel User { get; set; }
        public IEnumerable<CollaboratorSubModel> Collaborators { get; set; }
        public RepositorySubModel Repository { get; set; }
        public IIdentity CurrentUser { get; set; }
        public IEnumerable<Signature> Signers { get; set; }

        public bool CurrentUserIsCollaborator
        {
            get { return Collaborators.Any(collaborator => collaborator.Username.Equals(CurrentUser.Name, StringComparison.InvariantCultureIgnoreCase)); }
        }
    }
}