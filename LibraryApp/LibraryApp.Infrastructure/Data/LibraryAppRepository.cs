using LibraryApp.Application.Abstractions;
using LibraryApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Infrastructure.Data
{
    public class LibraryAppRepository : ILibraryAppRepository
    {
        private readonly AppDbContext _context;

        public LibraryAppRepository(AppDbContext context) 
        {
            _context = context;
        }

        #region ADD DATA
        public void AddLibraryItem(LibraryItem libraryItem)
        {
            _context.LibraryItems.Add(libraryItem);
            _context.SaveChanges();
        }

        public void AddMember(Member member)
        {
            _context.Members.Add(member);
            _context.SaveChanges();
        }

        #endregion ADD DATA

        #region GET DATA

        public IEnumerable<LibraryItem> FindItems(string? term)
        {
            if (string.IsNullOrWhiteSpace(term)) return _context.LibraryItems.ToList();
            term = term.Trim().ToLowerInvariant();

            return _context.LibraryItems.
                Where(i => i.Title.ToLowerInvariant().Contains(term));
        }

        public IEnumerable<LibraryItem> GetAllLibraryItems()
        {
            return _context.LibraryItems.ToList();
        }

        public IEnumerable<Member> GetAllMembers()
        {
            return _context.Members.ToList();
        }

        public LibraryItem? GetItemById(int itemId)
        {
            return _context.LibraryItems.
                Find(itemId);
        }

        public Member? GetMemberById(int memberId)
        {
            return _context.Members.
                Find(memberId);
        }

        #endregion GET DATA

        #region UPDATE STATUS

        public void UpdateBorrowedItemStatus(int memberId, int itemId)
        {
            var item = _context.LibraryItems.Find(itemId);
            item!.IsBorrowed = true;

            var borrowedItem = new BorrowedItem
            {
                MemberId = memberId,
                LibraryItemId = itemId
            };

            _context.BorrowedItems.Add(borrowedItem);
            _context.SaveChanges();
        }

        public void UpdateReturnedItemStatus(int memberId, int itemId)
        {
            var item = _context.LibraryItems.Find(itemId);
            item!.IsBorrowed = false;

            var borrowedRecord = _context.BorrowedItems.
                FirstOrDefault(i => i.LibraryItemId == itemId
                && i.MemberId == memberId);

            _context.BorrowedItems.Remove(borrowedRecord!);
            _context.SaveChanges();
        }

        #endregion UPDATE STATUS
    }
}
