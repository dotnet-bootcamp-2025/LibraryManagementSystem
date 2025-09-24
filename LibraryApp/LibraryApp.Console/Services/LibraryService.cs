using LibraryApp.Console.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp.Console.Services
{
    public sealed class LibraryService : ILibraryService
    {
        private readonly List<LibraryItem> _items = new();
        private readonly List<Member> _members = new();
        private int _nextItemId = 1;
        private int _nextMemberId = 1;
        public IReadOnlyList<LibraryItem> Items => _items;
        public IReadOnlyList<Member> Members => _members;
        // Seed fake data for the demo
        public void Seed()
        {
            AddBook("Clean Code", "Robert C. Martin", 464);
            AddBook("The Pragmatic Programmer", "Andrew Hunt", 352);
            AddMagazine("DotNET Weekly", 120, "DevPub");
            AddMagazine("Tech Monthly", 58, "TechPress");
            RegisterMember("Alice");
            RegisterMember("Bob");
        }
        public Book AddBook(string title, string author, int pages = 0)
        {
            var book = new Book(_nextItemId++, title, author, pages);
            _items.Add(book);
            return book;
        }
        public Magazine AddMagazine(string title, int issueNumber, string publisher)
        {
            var mag = new Magazine(_nextItemId++, title, issueNumber, publisher);
            _items.Add(mag);
            return mag;
       
        }
        public Member RegisterMember(string name)
        {
            var member = new Member(_nextMemberId++, name);
            _members.Add(member);
            return member;
        }
        public IEnumerable<LibraryItem> FindItems(string? term)
        {
            if (string.IsNullOrWhiteSpace(term)) return _items;
            term = term.Trim().ToLowerInvariant();
            return _items.Where(i => i.Title.ToLowerInvariant().Contains(term));
        }
        public IEnumerable<LibraryItem> GetItems()
        {
            return _items;
        }

        public bool BorrowItem(int memberId, int itemId, out string message)
        {
            var member = _members.FirstOrDefault(m => m.Id == memberId);
            var item = _items.FirstOrDefault(i => i.Id == itemId);
            if (member is null) { message = "Member not found."; return false; }
            if (item is null) { message = "Item not found."; return false; }
            try
            {
                member.BorrowItem(item);
                message = $"'{item.Title}' borrowed by {member.Name}.";
                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return false;
            }
        }
        public bool ReturnItem(int memberId, int itemId, out string message)
        {
            var member = _members.FirstOrDefault(m => m.Id == memberId);
            var item = _items.FirstOrDefault(i => i.Id == itemId);
            if (member is null) { message = "Member not found."; return false; }
            if (item is null) { message = "Item not found."; return false; }
            try
            {
                member.ReturnItem(item);
                message = $"'{item.Title}' returned by {member.Name}.";
                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return false;
            }
        }
    }
}