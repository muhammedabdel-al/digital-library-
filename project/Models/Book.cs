namespace project.Models
{
    public class Book
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }

        // Foreign key to Author
        public int AuthorId { get; set; }
        public Author Author { get; set; }
        public string imgfile { get; set; }
        public ICollection<UserBook> UserBooks { get; set; }
    }
}
