using LibraryApp.Domain.Entities;

namespace LibraryApp.Application.Abstraction
{
    public interface ILibraryAppRepository
    {
        IEnumerable<LibraryItem> GetAllLibraryItems();

        void AddLibraryItem(LibraryItem item);

        void AddMember(Member member);

        void UpdateLibraryItem(LibraryItem item);

        LibraryItem? GetLibraryItemById(int id);

        Member? GetMemberById(int id);

        void AddBorrowedItem(BorrowedItem borrowedItem);

        void ReturnItem(BorrowedItem borrowedItem);

        IEnumerable<Member> GetAllMembers();

        void UpdateMember(Member member);

        IEnumerable<LibraryItem> FindItems(string searchTerm);

        void RemoveBorrowedItem(BorrowedItem borrowedItem);

        BorrowedItem? GetBorrowedItemByLibraryItemId(int libraryItemId);

        void UpdateBorrowedItem(BorrowedItem borrowedItem);

        IEnumerable<BorrowedItem> GetAllCurrentLoans();

        IEnumerable<BorrowedItem> GetActiveLoansByMemberId(int memberId);

        void SaveChanges();
    }
}