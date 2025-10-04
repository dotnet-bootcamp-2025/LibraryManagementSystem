using LibraryApp.Domain.Entities;

namespace LibraryApp.Application.Abstractions
{
    public interface ILibraryAppRepository
    {
        IEnumerable<LibraryItem> GetAlLibraryItems();

        IEnumerable<Domain.Entities.Member> GetAllMembers();
        
        void AddLibraryItem(LibraryItem libraryItem);

        void updateLibraryItem(LibraryItem libraryItem);

        LibraryItem? GetLibraryItemById(int id);

        Member? GetMemberById(int id);

        void AddMember(Domain.Entities.Member member);

        void AddBorrowedItem(BorrowedItem borrowedItem);
                
        Domain.Entities.BorrowedItem? GetBorrowedItem(int memberId, int libraryItemId);
                
        void RemoveBorrowedItem(Domain.Entities.BorrowedItem borrowedItem);
               
        void UpdateLibraryItem(LibraryItem libraryItem);
    }
}
