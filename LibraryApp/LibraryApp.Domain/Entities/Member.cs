namespace LibraryApp.Domain.Entities
{
    public class Member
    {
        public int Id { get; set; }
        public required string Name { get; set; }

        public DateTime MembershipStartDate { get; set; }
        public DateTime MembershipEndDate { get; set; } 
        public List <BorrowedItem>? BorrowedItems { get; set; }

    }
}
