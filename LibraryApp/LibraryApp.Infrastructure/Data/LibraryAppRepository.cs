using LibrartApp.Domain.Entities;
using LibraryApp.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp.Infrastructure.Data
{
    public class LibraryAppRepository : ILibraryAppRepository
    { 

        private readonly AppDbContext _context;
        public LibraryAppRepository(AppDbContext context)
        {
            _context = context;
        }

        public void AddBorrowedItem(BorrowItem borrowedItem)
        {
            _context.BorrowItems.Add(borrowedItem);
            _context.SaveChanges();
        }

        public void ReturnBorrowedItem(int borrowedItemId)
        {
            var bi = _context.BorrowItems.Find(borrowedItemId);
            _context.BorrowItems.Remove(bi);
            _context.SaveChanges();
        }

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

        public IEnumerable<LibraryItem> GetAllLibraryItems()
        {
            return _context.LibraryItems.ToList();
        }

        public IEnumerable<Member> GetAllMembers()
        {
            return _context.Members.ToList();

            // Unfinished attemp for returning BorrowedItems
            var query = from member in _context.Members
                        join borrow in _context.BorrowItems on member.Id equals borrow.MemberId
                        /*join libItem in _context.LibraryItems on borrow.LibraryItemId equals libItem.Id*/ into borrowedItems
                        select new
                        {
                            Id = member.Id,
                            Name = member.Name,
                            BorrowedItems = (List<BorrowItem>)borrowedItems
                        };

            //foreach (var member in members)
            //{
            //    Console.WriteLine(member.Name);
            //    foreach(var LibItem in member.BorrowedItems)
            //    {
            //        Console.WriteLine($"{LibItem.Id} : {LibItem.Title}");
            //    }
            //}
            var members = (IEnumerable<Member>)query.ToList();
            return members;
        }

        public LibraryItem? GetLibraryItem(int id)
        {
            return _context.LibraryItems.Find(id);

            // NOTA: Forma de traer varios registros
            //var x = _context.LibraryItems.Where(Li => Li.IsBorrowed && Li.Pages > 100).ToList();

            //return x;
        }

        public Member? GetMemberById(int id)
        {
            return _context.Members.Find(id);
        }

        public BorrowItem? GetBorrowedItem(int memberId, int libraryItemId)
        {
            var bi = _context.BorrowItems.Where(bi => bi.MemberId == memberId && bi.LibraryItemId == libraryItemId).FirstOrDefault();
            return bi;
        }

        public void UpdateLibraryItem(LibraryItem libraryItem)
        {
            _context.LibraryItems.Update(libraryItem);
            _context.SaveChanges();
        }
    }
}
