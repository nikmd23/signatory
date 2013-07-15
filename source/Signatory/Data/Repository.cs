using System.Collections.Generic;

namespace Signatory.Data
{
    public class Repository
    {
        public Repository(){} //Required for EF

        public Repository(string username, string name, bool requireCla, string accessToken, string licenseText)
        {
            AccessToken = accessToken;
            RequireCla = requireCla;
            LicenseText = licenseText;
            Username = username;
            Name = name;
        }

        public int Id { get; set; }

        public ICollection<Signature> Signatures { get; set; }

        public string AccessToken { get; set; }
        public bool RequireCla { get; set; }
        public string LicenseText { get; set; }
        // TODO: Set unique constraint across name+username
        public string Name { get; set; }
        public string Username { get; set; }
    }
}