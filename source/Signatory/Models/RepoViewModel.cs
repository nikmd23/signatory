using Signatory.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using Signatory.Service;

namespace Signatory.Models
{
    public class RepoViewModel
    {
        public RepoViewModel(GitHubUser user, GitHubRepository repository, IEnumerable<GitHubCollaborator> collaborators, IIdentity currentUser, IEnumerable<Signature> signers)
        {
            User = user;
            Repository = repository;
            Collaborators = collaborators;
            CurrentUser = currentUser;
            Signers = signers;
        }

        public GitHubUser User { get; set; }
        public IEnumerable<GitHubCollaborator> Collaborators { get; set; }
        public GitHubRepository Repository { get; set; }
        public IIdentity CurrentUser { get; set; }
        public IEnumerable<Signature> Signers { get; set; }

        public bool CurrentUserIsCollaborator
        {
            get { return Collaborators.Any(collaborator => collaborator.Username.Equals(CurrentUser.Name, StringComparison.InvariantCultureIgnoreCase)); }
        }
    }
}