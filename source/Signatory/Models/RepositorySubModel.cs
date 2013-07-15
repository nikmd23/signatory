namespace Signatory.Models
{
    public class RepositorySubModel
    {
        public RepositorySubModel(dynamic repository)
        {
            Name = repository.name;
        }

        public string Name { get; set; }
    }
}