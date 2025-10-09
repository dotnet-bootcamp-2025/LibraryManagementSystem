using LibrartApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp.Services
{
    public interface ILibraryService
    {
        IReadOnlyList<LibraryItem> Items { get; }
        IReadOnlyList<Member> Members { get; }

        Book AddBook(string title, string author, int pages = 0);
        Magazine AddMagazine(string title, int issueNumber, string publisher);
        bool BorrowItem(int memberId, int itemId, out string message);
        IEnumerable<LibraryItem> FindItems(string? term);
        Member RegisterMember(string name);
        bool ReturnItem(int memberId, int itemId, out string message);
        void Seed();
    }
}
