using LibraryApp.Domain.Entities;

namespace LibraryApp.Application.Abstractions
{
    public interface ILibraryAppRepository
    {
        IEnumerable<LibraryItem> GetAllLibraryItems();

        IEnumerable<Domain.Entities.Member> GetAllMembers();
        
        void AddLibraryItem(LibraryItem libraryItem);

        void updateLibraryItem(LibraryItem libraryItem);

        LibraryItem? GetLibraryItemById(int id);

        Member? GetMemberById(int id);

        void AddMember(Domain.Entities.Member member);

        void AddBorrowedItem(BorrowedItem borrowedItem);
                
        Domain.Entities.BorrowedItem? GetBorrowedItem(int memberId, int libraryItemId);
                
                     
        void UpdateLibraryItem(LibraryItem libraryItem);
        Domain.Entities.BorrowedItem? GetBorrowedItemByItemId(int libraryItemId);
        void DeactivateBorrowedItem(int itemId);

        void InsertBorrowedItemWithCustomDate(int libraryItemId, int memberId, string dateString);
        string GetDatabasePath();

        List<BorrowedItem> GetBorrowedItemsByMemberId(int memberId);

        int GetActiveBorrowedItemCountByMemberId(int memberId);

    }
}
