using LibraryApp.Application.Abstractions;
using LibraryApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Infrastructure.Data
{
    public class LibraryAppRepository : ILibraryAppRepository
    {
        private readonly AppDBContext _context;

        public LibraryAppRepository(AppDBContext context)
        {
            _context = context;
        }

        public void AddLibraryItem(LibraryItem libraryItem)
        {
            _context.LibraryItems.Add(libraryItem);
            _context.SaveChanges();

        }
        public IEnumerable<LibraryItem> GetAllLibraryItems()
        {
            return _context.LibraryItems.ToList();
        }
        public LibraryItem? GetLibraryItemById(int id)
        {
            return _context.LibraryItems.Find(id);
        }

        public void UpdateLibraryItem(LibraryItem libraryItem)
        {
            _context.LibraryItems.Update(libraryItem);
            _context.SaveChanges();
        }
        public Member? GetMemberById(int id)
        {
            return _context.Members.Find(id);
        }
        public void AddBorrowedItem(BorrowedItem borrowedItem)
        {
            _context.BorrowedItems.Add(borrowedItem);
            _context.SaveChanges();
        }
        public void AddMember(Member member)
        {
            _context.Members.Add(member);
            _context.SaveChanges();
        }
        public BorrowedItem? GetBorrowedItem(int memberId, int itemId)
        {
            return _context.BorrowedItems
                .FirstOrDefault(b => b.MemberId == memberId && b.LibraryItemId == itemId);
        }

        public void RemoveBorrowedItem(BorrowedItem borrowedItem)
        {
            _context.BorrowedItems.Remove(borrowedItem);
            _context.SaveChanges();
        }

        object ILibraryAppRepository.GetBorrowedItem(int memberId, int itemId)
        {
            var borrowedItem = GetBorrowedItem(memberId, itemId);
            if (borrowedItem is null)
            {
                throw new InvalidOperationException($"No borrowed item found for memberId {memberId} and itemId {itemId}.");
            }
            return borrowedItem;
        }

        public void RemoveBorrowedItem(object borrowedItem)
        {
            if (borrowedItem is BorrowedItem item)
            {
                RemoveBorrowedItem(item);
            }
            else
            {
                throw new ArgumentException("Parameter is not of type BorrowedItem.", nameof(borrowedItem));
            }
        }

        public IEnumerable<LibraryItem> FindLibraryItems(string? term)
        {
            if (string.IsNullOrWhiteSpace(term))
                return _context.LibraryItems.AsNoTracking().ToList();

            term = term.Trim().ToLowerInvariant();

            return _context.LibraryItems
                .AsNoTracking()
                .Where(i => i.Title.ToLower().Contains(term) ||
                            (i.Author != null && i.Author.ToLower().Contains(term)) ||
                            (i.Publisher != null && i.Publisher.ToLower().Contains(term)))
                .ToList();
        }

        //public void AddMember(Member member)
        //{
        //    _context.Members.Add(member);
        //    _context.SaveChanges();
        //}
        public IEnumerable<Member> GetAllMembers()
        {
            return _context.Members.AsNoTracking().ToList();
        }
    }
}

