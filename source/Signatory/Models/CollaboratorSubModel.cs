namespace Signatory.Models
{
    public class CollaboratorSubModel
    {
        public CollaboratorSubModel(dynamic collaborator)
        {
            Username = collaborator.login;
        }

        public string Username { get; set; }
    }
}