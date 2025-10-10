using LibraryApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp.Application.Abstractions
{
    public interface ILibraryAppRepository
    {
        IEnumerable<LibraryItem> GetAllLibraryItems();

        void AddLibraryItem(LibraryItem libraryItem);

        void UpdateLibraryItem(LibraryItem libraryItem);

        LibraryItem? GetLibraryItemById(int id);

        Member? GetMemberById(int id);

        void AddBorrowedItem(BorrowedItem borrowedItem);

        void AddMember(Member member);

        IEnumerable<Member> GetAllMembers();

        LibraryItem? GetLibraryItemByIdIgnoringFilters(int id);
    }
}
