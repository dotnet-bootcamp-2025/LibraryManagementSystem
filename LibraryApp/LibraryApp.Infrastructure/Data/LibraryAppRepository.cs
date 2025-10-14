using LibraryApp.Application.Abstraction;
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

        public void AddLibraryItem(LibraryItem item)
        {
            _context.LibraryItems.Add(item);
        }

        public void AddMember(Member member)
        {
            _context.Members.Add(member);
        }

        public IEnumerable<LibraryItem> GetAllLibraryItems()
        {
            return _context.LibraryItems.ToList();
        }

        public Member? GetMemberById(int id)
        {
            var member = _context.Members.FirstOrDefault(member => member.Id == id);
            if (member != null)
            {
                _context.Entry(member)
                    .Collection(member => member.BorrowedItems!)
                    .Query()
                    .Include(borrowItem => borrowItem.LibraryItem)
                    .Load();
            }
            return member;
        }

        public LibraryItem? GetLibraryItemById(int id)
        {
            return _context.LibraryItems
                .FirstOrDefault(libraryItem => libraryItem.Id == id);
        }

        public void UpdateLibraryItem(LibraryItem LibraryItem)
        {
        }

        public void UpdateMember(Member member)
        {
        }

        public void AddBorrowedItem(BorrowedItem borrowedItem)
        {
            _context.BorrowedItems.Add(borrowedItem);
        }

        public void ReturnItem(BorrowedItem borrowedItem)
        {
            _context.BorrowedItems.Update(borrowedItem);
        }

        public IEnumerable<Member> GetAllMembers()
        {
            return _context.Members
                .Include(member => member.BorrowedItems!.Where(bi => bi.IsActive))
                .ToList();
        }

        public IEnumerable<LibraryItem> FindItems(string term)
        {
            if (string.IsNullOrWhiteSpace(term))
                return _context.LibraryItems.ToList();

            term = term.Trim();
            return _context.LibraryItems
                .Where(i => EF.Functions.Like(i.Title, $"%{term}%"))
                .ToList();
        }

        public void RemoveBorrowedItem(BorrowedItem borrowedItem)
        {
            _context.BorrowedItems.Remove(borrowedItem);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public BorrowedItem? GetBorrowedItemByLibraryItemId(int libraryItemId)
        {
            return _context.BorrowedItems
                .FirstOrDefault(b => b.LibraryItemId == libraryItemId);
        }

        public void UpdateBorrowedItem(BorrowedItem borrowedItem)
        {
           _context.BorrowedItems.Update(borrowedItem);
        }

        public IEnumerable<BorrowedItem> GetAllCurrentLoans()
        {
           return _context.BorrowedItems
                .Include(borrowedItem => borrowedItem.Member)
                .Where(borrowedItem => borrowedItem.IsActive)
                .ToList();
        }

        public IEnumerable<BorrowedItem> GetActiveLoansByMemberId(int memberId)
        {
            return _context.BorrowedItems
                .Include(borrowedItem => borrowedItem.LibraryItem)
                .Where(borrowedItem => borrowedItem.MemberId == memberId && borrowedItem.IsActive)
                .ToList();
        }
    }
}