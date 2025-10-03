using LibraryApp.Domain;

namespace LibraryApp.Services
{
    public interface ILibraryService
    {
        IReadOnlyList<LibraryItem> Items { get; }
        IReadOnlyList<Member> Members { get; }

        Book AddBook(string title, string author, int pages = 0);
        Magazine AddMagazine(string title, int issueNumber, string publisher);
        bool BorrowItem(int memberId, int itemId);
        IEnumerable<LibraryItem> FindItems(string? term);
        Member RegisterMember(string name);
        bool ReturnItem(int memberId, int itemId);
        void Seed();
    }
}