using LibraryApp.Domain;

namespace LibraryApp.Application.Abstractions
{
    public interface ILibraryService
    {
        Book AddBook(string title, string author, int pages = 0);
        Magazine AddMagazine(string title, int issueNumber, string publisher);
        bool BorrowItem(int memberId, int itemId, out string message);
        IEnumerable<LibraryItem> FindItems(string? term);
        Member RegisterMember(string name);
        IEnumerable<Domain.Member> Members { get; }
        bool ReturnItem(int memberId, int itemId, out string message);
        IEnumerable<LibraryItem> GetAllLibraryItems();

    }
}
