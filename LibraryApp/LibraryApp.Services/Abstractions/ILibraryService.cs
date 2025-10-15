using LibraryApp.Domain;

namespace LibraryApp.Application.Abstractions
{
    public interface ILibraryService
    {
        IEnumerable<LibraryItem> GetAllLibraryItems();
        IEnumerable<LibraryItem> FindItems(string term);

        IEnumerable<Member> GetAllMembers();
        (bool Success, Member? Member) GetMemberById(int id, out string message);
        (bool Success, IEnumerable<BorrowedItem> Items) GetMemberBorrowedItems(int memberId, out string message);

        Book AddBook(string title, string author, int pages = 0);
        Magazine AddMagazine(string title, int issueNumber, string publisher);
        Member RegisterMember(string name);

        bool BorrowItem(int memberId, int itemId, out string message);
        bool ReturnItem(int memberId, int itemId, out string message);
    }
}
