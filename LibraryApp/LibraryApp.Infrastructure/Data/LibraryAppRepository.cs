// LibraryApp.Infrastructure.Data/LibraryAppRepository.cs
using System.Linq;
using System.Collections.Generic;
using LibraryApp.Application.Abstractions;
using LibraryApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Infrastructure.Data
{
    public class LibraryAppRepository : ILibraryAppRepository
    {
        private readonly AppDbContext _context;
        private ILibraryAppRepository _libraryAppRepositoryImplementation;

        public LibraryAppRepository(AppDbContext context)
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
            return _context.LibraryItems.AsNoTracking().ToList();
        }

        public LibraryItem? GetLibraryItemById(int id)
        {
            return _context.LibraryItems.Find(id);
        }

        public void UpdateLibraryItem(LibraryItem libraryItem)
        {
            var local = _context.LibraryItems.Local.FirstOrDefault(x => x.Id == libraryItem.Id);
            if (local != null)
            {
                _context.Entry(local).CurrentValues.SetValues(libraryItem);
            }
            else
            {
                _context.LibraryItems.Attach(libraryItem);
                _context.Entry(libraryItem).State = EntityState.Modified;
            }

            _context.SaveChanges();
        }

        public Member? GetMemberById(int id)
        {
            return _context.Members.Find(id);
        }

        public void AddMember(Member member)
        {
            _context.Members.Add(member);
            _context.SaveChanges();
        }

        public void AddBorrowedItem(BorrowedItem borrowedItem)
        {
            _context.BorrowedItems.Add(borrowedItem);
            _context.SaveChanges();
        }

        public BorrowedItem? GetBorrowedItem(int memberId, int itemId)
        {
            return _context.BorrowedItems
                .Include(b => b.Member)
                .AsNoTracking()
                .FirstOrDefault(b => b.MemberId == memberId && b.LibraryItemId == itemId);
        }

        public void RemoveBorrowedItem(BorrowedItem borrowedItem)
        {
            var borrow = _context.BorrowedItems.Find(borrowedItem.Id);
            if (borrow == null)
            {
                borrow= _context.BorrowedItems
                    .FirstOrDefault(b => b.MemberId == borrowedItem.MemberId && b.LibraryItemId == borrowedItem.LibraryItemId);
            }

            if (borrow != null)
            {
                _context.BorrowedItems.Remove(borrow);
                _context.SaveChanges();
            }
        }

        public IEnumerable<Member> GetAllMembers()
        {
            return _context.Members.AsNoTracking().ToList(); //AsNoTracking porque es lectura.
        }

        public IEnumerable<BorrowedItem> GetAllBorrowedItems()
        {
            return _context.BorrowedItems.Include(b => b.LibraryItem)
                .Include(b => b.Member).AsNoTracking().ToList();
        }
    }
}
