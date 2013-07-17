using System;
using System.Data.Entity;
using System.Linq;
using Signatory.Data;

namespace Signatory.Extensions
{
    public static class DbSetOfRepositoryExtensions
    {
        public static Repository Where(this IDbSet<Repository> repositories, string repoOwner, string repoName)
        {
            return repositories.SingleOrDefault(r => r.Name.Equals(repoName, StringComparison.InvariantCultureIgnoreCase) &&
                                            r.Owner.Equals(repoOwner, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}