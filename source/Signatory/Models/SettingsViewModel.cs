using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using Signatory.Data;

namespace Signatory.Models
{
    public class SettingsViewModel : IValidatableObject
    {
        private string licenseText;

        public static SettingsViewModel From(Repository repository)
        {
            if (repository == null)
                return null;

            return new SettingsViewModel()
                {
                    AccessToken = repository.AccessToken,
                    LicenseText = repository.LicenseText,
                    RepoName = repository.Name,
                    RequireCla = repository.RequireCla,
                    RepoOwner = repository.Owner
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
        public string RepoName { get; set; }

        [Required]
        public string RepoOwner { get; set; }

        [Display(Name = "Access Token")]
        public string AccessToken { get; set; }

        [Display(Name = "License Text")]
        public string LicenseText {
            get
            {
                if (licenseText == null) return null;
                    
                return licenseText.Replace("{repo}", RepoName);
            }
            set { licenseText = value; }
        }

        [Display(Name = "Require a CLA?"), Required]
        public bool RequireCla { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (RequireCla)
            {
                if (string.IsNullOrWhiteSpace(AccessToken)) yield return new ValidationResult("Cannot find access token.", new []{ "AccessToken" });
                if (string.IsNullOrWhiteSpace(LicenseText)) yield return new ValidationResult("License text cannot be empty.", new[] { "LicenseText" });
            }
        }
    }
}