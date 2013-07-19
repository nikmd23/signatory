using System.Data.Entity;

namespace Signatory.Data
{
    public class DataContext : DbContext
    {
        public IDbSet<Repository> Repositories { get; set; }

        public IDbSet<Signature> Signatures { get; set; }
    }
}