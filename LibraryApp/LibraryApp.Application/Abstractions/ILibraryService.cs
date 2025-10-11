using LibraryApp.Domain;
using LibraryApp.Domain.Entities;
using LibraryItem = LibraryApp.Domain.LibraryItem;
using Member = LibraryApp.Domain.Member;

namespace LibraryApp.Application.Abstractions;

public interface ILibraryService
{
    Book AddBook(string title, string author, int pages = 0 );
    Magazine AddMagazine(string title, int issueNumber, string publisher);
    bool BorrowItem(int memberId, int itemId, out string message);
    IEnumerable<LibraryItem> FindItems(string? term);
    Member RegisterMember(string name, DateTime? StartDate, DateTime? EndDate);
    bool ReturnItem(int memberId, int itemId, out string message);
    IEnumerable<LibraryItem> GetAllLibraryItems();
    IEnumerable<Member> GetAllMembers();
    IEnumerable<BorrowedItem>  GetAllBorrowedItems();
    bool MembershipStatus(int memberId, out string message);
    
}