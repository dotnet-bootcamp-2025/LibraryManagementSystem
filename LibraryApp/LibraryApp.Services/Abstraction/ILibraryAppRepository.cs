using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }

}
