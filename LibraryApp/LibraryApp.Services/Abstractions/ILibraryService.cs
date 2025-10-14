using LibraryApp.Domain;
using LibraryApp.Domain.Entities;

namespace LibraryApp.Application.Abstractions
{
    public interface ILibraryService
    {
        Book AddBook(string title, string author, int pages = 0);
        Magazine AddMagazine(string title, int issueNumber, string publisher);
        bool BorrowItem(int memberId, int itemId, out string message);
        IEnumerable<Domain.Entities.LibraryItem> FindItems(string? term);
        Domain.Entities.Member RegisterMember(string name);
        IEnumerable<Domain.Member> Members { get; }
        bool ReturnItem(int memberId, int itemId, out string message);
        //IEnumerable<Domain.LibraryItem> GetAllLibraryItems();

        IEnumerable<Domain.Entities.LibraryItem> GetAllLibraryItems();
        bool ReturnItem(int itemId, out string message);

        List<BorrowedItem> GetBorrowedItemsByMemberId(int memberId);

    }
}
