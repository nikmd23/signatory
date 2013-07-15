using System.ComponentModel.DataAnnotations;
using System.IO;
using Signatory.Data;

namespace Signatory.Models
{
    public class SettingsViewModel
    {
        public static SettingsViewModel From(Repository repository)
        {
            if (repository == null)
                return null;

            return new SettingsViewModel()
                {
                    AccessToken = repository.AccessToken,
                    LicenseText = repository.LicenseText,
                    Repo = repository.Name,
                    RequireCla = repository.RequireCla,
                    Username = repository.Username
                };
        }

        public SettingsViewModel() : this(null)
        {
        }

        public SettingsViewModel(string defaultLicenseTextPath)
        {
            if (defaultLicenseTextPath != null)
            {
                var mdFile = new FileInfo(defaultLicenseTextPath);
                using (var reader = mdFile.OpenText())
                {
                    LicenseText = reader.ReadToEnd();
                }
                RequireCla = false;
            }
        }

        [Required]
        public string Repo { get; set; }

        [Required]
        public string Username { get; set; }

        [Display(Name = "Access Token"), Required] //TODO: Fix so this is only required if RequireCLA = true
        public string AccessToken { get; set; }

        [Display(Name = "License Text"), Required] //TODO: Fix so this is only required if RequireCLA = true
        public string LicenseText { get; set; }

        [Display(Name = "Require a CLA?"), Required]
        public bool RequireCla { get; set; }
    }
}