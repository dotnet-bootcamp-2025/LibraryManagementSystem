using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryApp.Application.Abstractions;
using LibraryApp.Domain.Entities;

namespace LibraryApp.Infraestructure.Data
{
    public class LibraryAppRepository : ILibraryAppRepository
    {

        private readonly AppDbContext _context;

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
           return _context.LibraryItems.ToList();
        }

        public LibraryItem? GetLibraryItemById(int id)
        {
            return _context.LibraryItems.Find(id);

            //var x = _context.LibraryItems.Where(li => li.IsBorrowed && li.Pages > 100).ToList();
            //return x;
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
        public IEnumerable<Member> GetAllMembers()
        {
           return _context.Members.ToList();
        }
        public void RemoveBorrowedItem(BorrowedItem borrowedItem)
        {
            _context.BorrowedItems.Add(borrowedItem);
            _context.SaveChanges();
        }
    }
}
