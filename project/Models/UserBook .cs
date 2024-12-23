namespace project.Models
{
    public class UserBook
    {
        public int CustomerId { get; set; }
        public int BookId { get; set; }
        public Customer Customer { get; set; }
        public Book Book { get; set; }
        public DateTime PurchaseDate { get; set; }
        public int Quantity { get; set; }
    }
}
