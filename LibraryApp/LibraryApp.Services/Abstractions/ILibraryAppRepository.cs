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
        IEnumerable<LibraryItems> GetAllLibraryItems();
        void AddLibraryItems(LibraryItems libraryItem);
        void UpdateLibraryItem(LibraryItems libraryItem);
        IEnumerable<Member> GetAllMembers();
        LibraryItems? GetLibraryItemById(int id);
        Member? GetMemberById(int id);
        void AddMember(Member member);

        void AddBorrowedItem(BorrowedItem borrowedItem);

        void RemoveBorrowedItem(BorrowedItem borrowedItem);

    }
}
