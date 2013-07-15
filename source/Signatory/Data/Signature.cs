using System;

namespace Signatory.Data
{
    public class Signature
    {
        public Signature(){} // Required for EF

        public Signature(int repositoryId, string address, string country, DateTime dateSigned, string email, string fullName, string signatureImage, string telephoneNumber, string username)
        {
            RepositoryId = repositoryId;
            Address = address;
            Country = country;
            DateSigned = dateSigned;
            Email = email;
            FullName = fullName;
            SignatureImage = signatureImage;
            TelephoneNumber = telephoneNumber;
            Username = username;
        }

        public int Id { get; set; }

        public int RepositoryId { get; set; }
        public Repository Repository { get; set; }

        public string Address { get; set; }
        public string Country { get; set; }
        public DateTime DateSigned { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string SignatureImage { get; set; }
        public string TelephoneNumber { get; set; }
        public string Username { get; set; }
    }
}