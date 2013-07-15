using System;
using System.Data.Entity;
using System.Linq;
using Signatory.Data;

namespace Signatory.Extensions
{
    public static class DbSetOfRepositoryExtensions
    {
        public static Repository Where(this IDbSet<Repository> repositories, string username, string name)
        {
            return repositories.SingleOrDefault(r => r.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase) &&
                                            r.Username.Equals(username, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}