namespace project.Models
{
    public class Author :Person
    {

        public ICollection<Book> Books { get; set; } = new List<Book>();

    }
}
