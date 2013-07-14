using Signatory.Framework;
using System;
using System.ComponentModel.DataAnnotations;

namespace Signatory.Models
{
    public class SignViewModel
    {
        [Display(Name = "Full Name"), Required]
        public string FullName { get; set; }

        [Display(Name = "Email"), Required]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Username"), Required]
        public string Username { get; set; }

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
        public string Repo { get; set; }

        [Display(Name = "Signature"), Required]
        [Signature]
        public string Signature { get; set; }
    }
}