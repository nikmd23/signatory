namespace Signatory.Models
{
    public class UserSignedNotification
    {
        public UserSignedNotification(string status, string username)
        {
            Status = status;
            Username = username;
        }

        public string Status { get; set; }
        public string Username { get; set; }
    }
}