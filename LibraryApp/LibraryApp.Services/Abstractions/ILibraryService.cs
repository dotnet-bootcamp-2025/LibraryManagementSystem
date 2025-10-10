using LibraryApp.Domain;

namespace LibraryApp.Application.Abstractions
{
    public interface ILibraryService
    {
        IEnumerable<LibraryItem> GetAllLibraryItems();
        Book AddBook(string title, string author, int pages = 0);
        Magazine AddMagazine(string title, int issueNumber, string publisher);
        bool BorrowItem(int memberId, int itemId, out string message);
        IEnumerable<Domain.Entities.Member> GetAllMembers();
        IEnumerable<LibraryItem> FindItems(string? term);
        Member RegisterMember(string name);
        bool ReturnItem(int memberId, int itemId, out string message); 
        IEnumerable<Domain.Entities.BorrowedItem> GetAllBorrowedItems();

        bool ReturnAllItemsByMember(int memberId, out string message);

    }
}
