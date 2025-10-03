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

        #region GET DATA

        public IEnumerable<LibraryItem> FindItems(string term)
        {
            return _context.LibraryItems.
                Where(i => i.Title.ToLower().Contains(term))
                .ToList();
        }

        public IEnumerable<LibraryItem> GetAllLibraryItems()
        {
            return _context.LibraryItems.ToList();
        }

        public IEnumerable<Member> GetAllMembers()
        {
            return _context.Members.ToList();
        }

        public BorrowedItem? GetBorrowedItem(int memberId, int itemId)
        {
            return _context.BorrowedItems.
                FirstOrDefault(b => b.MemberId == memberId && b.LibraryItemId == itemId);
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

        #region ADD DATA

        public void AddLibraryItem(LibraryItem libraryItem)
        {
            _context.LibraryItems.Add(libraryItem);
        }

        public void AddMember(Member member)
        {
            _context.Members.Add(member);
        }

        public void AddBorrowedItem(BorrowedItem borrowedItem)
        {
            _context.BorrowedItems.Add(borrowedItem);
        }

        #endregion ADD DATA

        #region UPDATE STATUS

        public void UpdateLibraryItem(LibraryItem libraryItem)
        {
            _context.LibraryItems.Update(libraryItem);
        }
        public void UpdateBorrowedItem(BorrowedItem borrowedItem)
        {
            _context.BorrowedItems.Update(borrowedItem);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        #endregion UPDATE STATUS
    }
}
