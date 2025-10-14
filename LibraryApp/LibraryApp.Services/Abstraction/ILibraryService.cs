using LibraryApp.Domain;
using LibraryApp.Application.DTOs;

namespace LibraryApp.Application.Abstraction
{
    public interface ILibraryService
    {
        Book AddBook(string title, string author, int pages = 0);

        Magazine AddMagazine(string title, int issueNumber, string publisher);

        bool BorrowItem(int memberId, int itemId, out string message, out string? formattedReturnDate);

        IEnumerable<LibraryItem> FindItems(string? term);

        Member RegisterMember(string name);

        bool ReturnItem(int memberId, int itemId);

        IEnumerable<LibraryItem> GetAllLibraryItems();

        IEnumerable<Member> GetAllMembers();

        IEnumerable<LoanDetailsDto> GetMemberActiveLoans(int memberId);

        bool MemberExists(int memberId);
    }
}