using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Signatory.Models
{
    public class SignViewModel
    {
        [Display(Name = "Full Name"), Required]
        public string FullName { get; set; }

        [Display(Name = "Email"), Required]
        public string Email { get; set; }

        [Display(Name = "Username"), Required]
        public string Username { get; set; }

        [Display(Name = "Phone Number"), Required]
        public string TelephoneNumber { get; set; }

        [Display(Name = "Street Address"), Required]
        public string Address { get; set; }

        [Display(Name = "Country"), Required]
        public string Country { get; set; }

        [Display(Name = "Date Signed"), Required]
        public DateTime Date { get; set; }

        public string Repo { get; set; }
    }
}