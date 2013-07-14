using System.Collections.Generic;

namespace Signatory.Data
{
    public class Repository
    {
        public int Id { get; set; }

        public ICollection<Signature> Signatures { get; set; }

        public string AccessToken { get; set; }
        public bool EnforceCla { get; set; }
        public string LicenseText { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
    }
}