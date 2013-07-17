using System.Collections.Generic;

namespace Signatory.Data
{
    public class Repository
    {
        public Repository(){} //Required for EF

        public Repository(string owner, string name, bool requireCla, string accessToken, string licenseText)
        {
            AccessToken = accessToken;
            RequireCla = requireCla;
            LicenseText = licenseText;
            Owner = owner;
            Name = name;
        }

        public int Id { get; set; }

        public ICollection<Signature> Signatures { get; set; }

        public string AccessToken { get; set; }
        public bool RequireCla { get; set; }
        public string LicenseText { get; set; }
        // TODO: Set unique constraint across name+username
        public string Name { get; set; }
        public string Owner { get; set; }
    }
}