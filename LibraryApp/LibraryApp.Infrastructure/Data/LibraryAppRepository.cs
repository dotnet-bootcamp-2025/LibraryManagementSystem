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
            return _context.LibraryItems
                .AsNoTracking()
                .Where(i => i.Active) // only active items
                .ToList();
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

        public IEnumerable<LibraryItem> FindLibraryItems(string? term)
        {
            var query = _context.LibraryItems
            .AsNoTracking()
            .Where(i => i.Active); // exclude soft-deleted items


            if (!string.IsNullOrWhiteSpace(term))
            {
                term = term.Trim().ToLowerInvariant();
                query = query.Where(i =>
                    i.Title.ToLower().Contains(term) ||
                    (i.Author != null && i.Author.ToLower().Contains(term)) ||
                    (i.Publisher != null && i.Publisher.ToLower().Contains(term))
                );
            }

            return query.ToList();

        }

        // ===== Members =====
        public void AddMember(Member member)
        {
            _context.Members.Add(member);
            _context.SaveChanges();
        }
        public IEnumerable<Member> GetAllMembers()
        {
            return _context.Members.AsNoTracking().ToList();
        }
        public Member? GetMemberById(int id)
        {
            return _context.Members
                .Include(m => m.BorrowedItems)
                    .ThenInclude(b => b.LibraryItem)
                .AsNoTracking()
                .FirstOrDefault(m => m.Id == id);
        }
        public IEnumerable<Member> GetAllMembersWithBorrowStatus()
        {
            var members = _context.Members
                            .Include(m => m.BorrowedItems)
                            .AsNoTracking()
                            .ToList();

            return members;

        }

        // ===== Borrowed Items =====

        public void AddBorrowedItem(BorrowedItem borrowedItem)
        {
            if (borrowedItem.BorrowDate == default)
                borrowedItem.BorrowDate = DateTime.Now;

            if (borrowedItem.ExpirationDate == default)
                borrowedItem.ExpirationDate = borrowedItem.BorrowDate.AddDays(3);

            _context.BorrowedItems.Add(borrowedItem);
            _context.SaveChanges();
        }
      
        public BorrowedItem? GetBorrowedItem(int memberId, int itemId)
        {
            return _context.BorrowedItems
                .Include(b => b.LibraryItem)
                .FirstOrDefault(b => b.MemberId == memberId && b.LibraryItemId == itemId);
        }

        public void RemoveBorrowedItem(BorrowedItem borrowedItem)
        {
            _context.BorrowedItems.Remove(borrowedItem);
            _context.SaveChanges();
        }

        // --- Interface overloads for backward compatibility ---
        object ILibraryAppRepository.GetBorrowedItem(int memberId, int itemId)
        {
            var borrowedItem = GetBorrowedItem(memberId, itemId);
            if (borrowedItem is null)
            {
                throw new InvalidOperationException(
                    $"No borrowed item found for memberId {memberId} and itemId {itemId}."
                );
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

        public void UpdateMember(Member member)
        {
            throw new NotImplementedException();
        }

        public void RegisterMember(Member member)
        {
            throw new NotImplementedException();
        }
    }
}

