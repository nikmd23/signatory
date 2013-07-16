using System;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using Signatory.Data;

namespace Signatory.Extensions
{
    public static class DbSetOfSignatureExtensions
    {
        public static Signature Where(this IDbSet<Signature> signatures, string repoOwner, string repoName, IIdentity user)
        {
            return signatures.Where(repoOwner, repoName, user.Name);
        }

        public static Signature Where(this IDbSet<Signature> signatures, string repoOwner, string repoName, string username)
        {
            return signatures.SingleOrDefault(s => s.Username.Equals(username, StringComparison.InvariantCultureIgnoreCase) &&
                                       s.Repository.Name.Equals(repoName, StringComparison.InvariantCultureIgnoreCase) &&
                                       s.Repository.Username.Equals(repoOwner, StringComparison.InvariantCultureIgnoreCase) );
        }

        public static IQueryable<Signature> Where(this IDbSet<Signature> signatures, string username, string repo)
        {
            return signatures.Where(s => s.Repository.Username.Equals(username, StringComparison.InvariantCultureIgnoreCase) &&
                                         s.Repository.Name.Equals(repo, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}