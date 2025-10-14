using LibraryApp.Domain.Entities;

namespace LibraryApp.Application.Abstractions
{
    public interface ILibraryAppRepository
    {
        void UpdateLibraryItem(LibraryItem libraryItem);
        LibraryItem? GetLibraryItemById(int id);

        Member? GetMemberById(int id);

        void AddBorrowedItem(BorrowedItem borrowedItem);
        object GetBorrowedItem(int memberId, int itemId);
        void RemoveBorrowedItem(BorrowedItem borrowedItem);
        void RemoveBorrowedItem(object borrowedItem);
        IEnumerable<LibraryItem> FindLibraryItems(string? term);
        IEnumerable<Member> GetAllMembers();
        void AddMember(Member member);
        IEnumerable<Member> GetAllMembersWithBorrowStatus();
        void UpdateMember(Member member);

        void AddLibraryItem(LibraryItem item);
        IEnumerable<LibraryItem> GetAllLibraryItems();

        // New method for registering a member
        void RegisterMember(Member member);

    }
}
