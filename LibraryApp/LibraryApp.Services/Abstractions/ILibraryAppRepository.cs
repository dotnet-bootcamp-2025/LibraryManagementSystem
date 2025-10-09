using LibrartApp.Domain.Entities;
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
        IEnumerable<Member> GetAllMembers();
        void AddLibraryItem(LibraryItem libraryItem);
        void UpdateLibraryItem(LibraryItem libraryItem);
        LibraryItem? GetLibraryItem(int id);
        Member? GetMemberById(int id);
        BorrowItem? GetBorrowedItem(int memberId, int libraryItemId);
        void AddBorrowedItem(BorrowItem borrowedItem);
        void ReturnBorrowedItem(int borrowedItemId);
        void AddMember(Member member);
    }

}
