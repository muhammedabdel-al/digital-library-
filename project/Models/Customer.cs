namespace project.Models
{
    public class Customer : Person
    {
        public int CashWallet { get; set; }
        public DateTime MembershipDate { get; set; }
        public ICollection<UserBook> PurchasedBooks { get; set; }
    }
}
