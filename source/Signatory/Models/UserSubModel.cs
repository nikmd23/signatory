namespace Signatory.Models
{
    public class UserSubModel
    {
        public UserSubModel(dynamic user)
        {
            Username = user.login;
            GravatarId = user.gravatar_id;
        }

        public string Username { get; set; }
        public string GravatarId { get; set; }
    }
}