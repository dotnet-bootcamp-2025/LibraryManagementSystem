using LibraryApp.Domain.Entities;

namespace LibraryApp.Application.Abstractions
{
    public interface ILibraryAppRepository
    {
        IEnumerable<LibraryItem> GetAllLibraryItems();
        IEnumerable<Member> GetAllMembers();
        IEnumerable<LibraryItem> FindItems(string? term);

        void AddLibraryItem(LibraryItem libraryItem);
        void AddMember(Member member);
    }
}
