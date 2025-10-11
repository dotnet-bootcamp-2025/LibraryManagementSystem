namespace LibraryApp.Domain.Entities;

public class Member
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public List<BorrowedItem>? BorrowedItems { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    
}