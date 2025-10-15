using LibrartApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp.Application.Abstractions
{
    public interface ILibraryService
    {
        Book AddBook(string title, string author, int pages = 0);
        Magazine AddMagazine(string title, int issueNumber, string publisher);
        bool BorrowItem(int memberId, int itemId, out string message);
        IEnumerable<LibraryItem> FindItems(string? term);
        Member RegisterMember(string name);
        bool ReturnItem(int memberId, int itemId, out string message);
        IEnumerable<LibraryItem> GetAllLibraryItems();
        IEnumerable<Member> GetAllMembers();

    }
}
