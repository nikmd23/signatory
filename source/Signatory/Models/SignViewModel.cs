using Signatory.Data;
using Signatory.Framework;
using System;
using System.ComponentModel.DataAnnotations;

namespace Signatory.Models
{
    public class SignViewModel
    {
        public static SignViewModel From(Signature signature)
        {
            if (signature == null)
                return null;

            return new SignViewModel
                {
                    Address = signature.Address,
                    Country = signature.Country,
                    Date = signature.DateSigned,
                    Email = signature.Email,
                    FullName = signature.FullName,
                    RepoName = signature.Repository.Name,
                    Signature = signature.SignatureImage,
                    TelephoneNumber = signature.TelephoneNumber,
                    RepoOwner = signature.Repository.Owner
                };
        }

        [Display(Name = "Full Name"), Required]
        public string FullName { get; set; }

        [Display(Name = "Email"), Required]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Phone Number"), Required]
        [Phone]
        public string TelephoneNumber { get; set; }

        [Display(Name = "Street Address"), Required]
        public string Address { get; set; }

        [Display(Name = "Country"), Required]
        public string Country { get; set; }

        [Display(Name = "Date Signed"), Required]
        public DateTime Date { get; set; }

        [Required]
        public string RepoName { get; set; }

        [Required]
        public string RepoOwner { get; set; }

        [Display(Name = "Signature"), Required]
        [SignatureRequired]
        public string Signature { get; set; }
    }
}