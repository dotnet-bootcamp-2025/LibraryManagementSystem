using LibraryApp.Domain.Entities;

namespace LibraryApp.Application.Abstractions
{
    public interface ILibraryAppRepository
    {
        IEnumerable<LibraryItem> GetAllLibraryItems();
        IEnumerable<Member> GetAllMembers();
        IEnumerable<LibraryItem> FindItems(string term);

        Member? GetMemberById(int memberId);
        LibraryItem? GetItemById (int itemId);

        void AddLibraryItem(LibraryItem libraryItem);
        void AddMember(Member member);
        void AddBorrowedItem(BorrowedItem borrowedItem);

        void UpdateLibraryItem(LibraryItem libraryItem);
        void UpdateBorrowedItem(BorrowedItem borrowedItem);

        BorrowedItem? GetBorrowedItem(int memberId, int itemId);
    }
}
