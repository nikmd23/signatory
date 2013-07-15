using System;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using Signatory.Data;

namespace Signatory.Extensions
{
    public static class DbSetOfSignatureExtensions
    {
        public static Signature Where(this IDbSet<Signature> signatures, string repo, IIdentity user)
        {
            return signatures.SingleOrDefault(s => s.Username.Equals(user.Name, StringComparison.InvariantCultureIgnoreCase) &&
                                                   s.Repository.Name.Equals(repo, StringComparison.InvariantCultureIgnoreCase));
        }

        public static IQueryable<Signature> Where(this IDbSet<Signature> signatures, string username, string repo)
        {
            return signatures.Where(s => s.Repository.Username.Equals(username, StringComparison.InvariantCultureIgnoreCase) &&
                                         s.Repository.Name.Equals(repo, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}