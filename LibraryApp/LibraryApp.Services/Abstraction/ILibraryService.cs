using LibraryApp.Domain;
using LibraryApp.Domain.Entities;

namespace LibraryApp.Application.Abstraction
{
    public interface ILibraryService
    {
        Book AddBook(string title, string author, int pages = 0);
        Magazine AddMagazine(string title, int issueNumber, string publisher);
        bool BorrowItem(int memberId, int itemId, out string message);
        IEnumerable<Domain.LibraryItem> FindItems(string? term);
        Domain.Member RegisterMember(string name);
        bool ReturnItem(int memberId, int itemId, out string message);
        IEnumerable<Domain.LibraryItem> GetAllLibraryItems();
        IEnumerable<Domain.Member> GetAllMembers();
        BorrowedItem? GetBorrowedItem(int memberId, int itemId);
        BorrowedItem? RemoveBorrowedItem (int memberId, int itemId);

    }
}