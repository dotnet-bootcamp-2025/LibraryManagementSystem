using LibraryApp.Domain;

namespace LibraryApp.Application.Abstractions
{
    public interface ILibraryService
    {
        IEnumerable<LibraryItem> GetAllLibraryItems();
        IEnumerable<Member> GetAllMembers();
        IEnumerable<LibraryItem> FindItems(string term);

        Book AddBook(string title, string author, int pages = 0);
        Magazine AddMagazine(string title, int issueNumber, string publisher);
        Member RegisterMember(string name);

        bool BorrowItem(int memberId, int itemId, out string message);
        bool ReturnItem(int memberId, int itemId, out string message);
    }
}
