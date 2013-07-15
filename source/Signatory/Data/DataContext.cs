using System.Data.Entity;

namespace Signatory.Data
{
    public class DataContext : DbContext
    {
        public DataContext()
        {
            // TODO: Move initialize to web.config and migrations as per pluralight video
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<DataContext>());
        }

        public IDbSet<Repository> Repositories { get; set; }

        public IDbSet<Signature> Signatures { get; set; }
    }
}