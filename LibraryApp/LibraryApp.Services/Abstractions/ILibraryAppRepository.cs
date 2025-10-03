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

        void UpdateLibraryItem(LibraryItem libraryItem);
    }
}
